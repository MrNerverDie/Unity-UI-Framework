using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    public class GridScrollerPos
    {
        public int FirstIndex { get; private set; }
        public Vector2 NormalizedPosition { get; private set; }

        public GridScrollerPos(int firstIndex, Vector2 normalizedPosition)
        {
            FirstIndex = firstIndex;
            NormalizedPosition = normalizedPosition;
        }
    }

    [RequireComponent(typeof(ScrollRect))]
	public class GridScroller : MonoBehaviour {

        // public UI elements //
        public Transform _itemPrefab;

        // private UI elements //
        private ScrollRect _scroller;
        private RectTransform _grid;

        // define //
        public enum Movement
        {
            Horizontal,
            Vertical,
        }

        // public fields //
        public Movement _moveType = Movement.Horizontal;
        public Vector2 _cellSize = Vector3.zero;
        public Vector2 _spacing = Vector3.zero;
        public delegate void OnChange(Transform trans, int index);

        // private fields //
        private HashSet<int> _transIndexSet = new HashSet<int>();
        private HashSet<int> _showIndexSet = new HashSet<int>();
        private Dictionary<int, RectTransform> _transDict = new Dictionary<int, RectTransform>();
        private OnChange _onChange;
        private int _firstIndex = 0;
        private int _itemCount = 0;
        private int _transCount = 0;
        private int _col = 0;
        private int _row = 0;
        private Rect _scrollerRect;
        private bool _hasChanged = false;

        public Vector2 ItemSize
        {
            get
            {
                return _spacing + _cellSize;
            }
        }

        public void Init(OnChange onChange, int itemCount, GridScrollerPos pos = null)
        {
            Clear();
            InitScroller();
            InitGrid();
            InitChildren(onChange, itemCount, pos);

            _scroller.onValueChanged.AddListener(OnValueChanged);
            _scroller.normalizedPosition = (pos == null) ? Vector2.one : pos.NormalizedPosition;

            if (_moveType == Movement.Horizontal)
            {
                _grid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_itemCount / _row) * ItemSize.x);
            }
            else
            {
                _grid.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_itemCount / _col) * ItemSize.y);
            }
        }

        private void InitScroller()
        {
            // Init Scroller //
            _scroller = GetComponent<ScrollRect>();
            _scrollerRect = _scroller.GetComponent<RectTransform>().rect;

            if (_moveType == Movement.Horizontal)
            {
                _scroller.vertical = false;
                _scroller.horizontal = true;
            }
            else
            {
                _scroller.vertical = true;
                _scroller.horizontal = false;
            }
        }

        private void InitGrid()
        {
            _grid = new GameObject("Grid", typeof(RectTransform)).GetComponent<RectTransform>();
            _scroller.content = _grid;
            _grid.SetParent(_scroller.transform, false);
            _grid.anchorMin = new Vector2(0f, 1f);
            _grid.anchorMax = new Vector2(1f, 1f);
            _grid.pivot = new Vector2(0.5f, 1f);
            // set left right to zero //
        }

        private void InitChildren(OnChange onChange, int itemCount, GridScrollerPos pos)
        {
            _firstIndex = (pos == null) ? 0 : pos.FirstIndex;
            _onChange = onChange;
            _itemCount = itemCount;
            _col = (int)((_scrollerRect.width + _spacing.x) / ItemSize.x);
            _row = (int)((_scrollerRect.height + _spacing.y) / ItemSize.y);
            if (_moveType == Movement.Horizontal)
            {
                _col += 2;
            }
            else
            {
                _row += 2;
            }
            _transCount = _col * _row;

            if (_transCount > _itemCount)
            {
                _transCount = _itemCount;
            }

            for (int i = 0; i < _transCount; i++)
            {
                Transform item = _grid.transform.AddChildFromPrefab(_itemPrefab, i.ToString());
                InitChild(item.GetComponent<RectTransform>(), i);
                _onChange(item, i);
                _transIndexSet.Add(i);
                _transDict.Add(i, item.GetComponent<RectTransform>());
            }
        }
   
        private void InitChild(RectTransform rectTrans, int index)
        {
            rectTrans.anchorMax = new Vector2(0, 1);
            rectTrans.anchorMin = new Vector2(0, 1);
            rectTrans.pivot = new Vector2(0, 1);
            rectTrans.sizeDelta = _cellSize;
            rectTrans.anchoredPosition = IndexToPosition(index);
        }

        private void Clear()
        {
            _transIndexSet.Clear();
            _transDict.Clear();
            _showIndexSet.Clear();
            if (_grid != null)
            {
                _grid.DestroyChildren();                
            }
        }

        public void OnValueChanged(Vector2 normalizedPosition)
        {
            if (_transCount == _itemCount)
            {
                return;
            }

            //Debug.Log(normalizedPosition.y);

            Vector2 scrollerSize = _scroller.GetComponent<RectTransform>().rect.size;
            Vector2 gridSize = _grid.rect.size;

            if (_moveType == Movement.Horizontal)
            {

            }
            else
            {
                float scrollLength = _grid.GetComponent<RectTransform>().anchoredPosition.y;
                int scrollRow = (int)(scrollLength / ItemSize.y);
                int startSiblingIndex = scrollRow * _col;

                _showIndexSet.Clear();
                for (int i = 0; i < _transCount; i++)
                {
                    if ((i + startSiblingIndex) < _itemCount && i > 0)
                    {
                        _showIndexSet.Add(i + startSiblingIndex);
                    }
                }

                if (_showIndexSet.SetEquals(_transIndexSet))
                {
                    return;
                }
                else
                {
                    IEnumerator<int> lhsIter = _showIndexSet.Except<int>(_transIndexSet).GetEnumerator();
                    IEnumerator<int> rhsIter = _transIndexSet.Except<int>(_showIndexSet).GetEnumerator();

                    while (lhsIter.MoveNext() && rhsIter.MoveNext())
                    {
                        ChangeToIndex(rhsIter.Current, lhsIter.Current);
                        _onChange(_transDict[lhsIter.Current], lhsIter.Current);
                    }

                    HashSet<int> tempSet = _transIndexSet;
                    _transIndexSet = _showIndexSet;
                    _showIndexSet = tempSet;
                }

                //if (Mathf.Abs(progress - normalizedPosition.y) > ((float)_col / _itemCount))
                //{
                //    if (progress > normalizedPosition.y && _firstIndex + _transCount < _itemCount)
                //    {
                //        for (int i = 0; i < _col; i++)
                //        {
                //            SwapSiblingIndex(_firstIndex, _firstIndex + _transCount);
                //            _onChange(_grid.transform.GetChild(_firstIndex + _transCount), _firstIndex + _transCount);
                //            _firstIndex++;
                //        };
                //    }
                //    else if (progress < normalizedPosition.y && _firstIndex > 0)
                //    {
                //        for (int i = 0; i < _col; i++)
                //        {
                //            SwapSiblingIndex(_firstIndex + _transCount - 1, _firstIndex - 1);
                //            _onChange(_grid.transform.GetChild(_firstIndex - 1), _firstIndex - 1);
                //            _firstIndex--;
                //        }
                //    }
                //}
            }
        }

        //private void ShiftToFirst()
        //{
        //    for (int i = 0; i < _col; i++)
        //    {
        //        _firstIndex--;
        //        Transform childTrans = _transList.Last.Value;
        //        childTrans.SetAsFirstSibling();
        //        _transList.RemoveLast();
        //        _transList.AddFirst(childTrans);
        //        if (_firstIndex > 0)
        //        {
        //            _firstIndex--;
        //            childTrans.gameObject.SetActive(true);
        //            _onChange(childTrans, _firstIndex);
        //        }
        //        else
        //        {
        //            childTrans.gameObject.SetActive(false);
        //        }
        //    }
        //}

        //private void ShiftToLast()
        //{
        //    for (int i = 0; i < _col && (_firstIndex + _transCount) < _itemCount ; i++)
        //    {
        //        Transform childTrans = _transList.First.Value;
        //        childTrans.SetSiblingIndex(_firstIndex + _transCount);
        //        _transList.RemoveFirst();
        //        _transList.AddLast(childTrans);
        //        _firstIndex++;
        //        childTrans.gameObject.SetActive(true);
        //        _onChange(childTrans, _firstIndex + _transCount - 1);
        //    }
        //}

        private void ChangeToIndex(int from, int to)
        {
            RectTransform rectTrans = _transDict[from];
            rectTrans.anchoredPosition = IndexToPosition(to);
            _transDict.Remove(from);
            _transDict.Add(to, rectTrans);
        }

        private Vector2 IndexToPosition(int index)
        {
            if (_moveType == Movement.Horizontal)
	        {
                return Vector2.zero;
	        }else
	        {
                return new Vector2(ItemSize.x * (index % _col), -ItemSize.y * (index / _col));
	        }
        }

        private void SwapSiblingIndex(Transform trans1, Transform trans2)
        {
            int trans1Index = trans1.GetSiblingIndex();
            int trans2Index = trans2.GetSiblingIndex();
            trans1.SetSiblingIndex(trans2Index);
            trans2.SetSiblingIndex(trans1Index);
        }

        public GridScrollerPos GetCurPos()
        {
            return new GridScrollerPos(_firstIndex, _scroller.normalizedPosition);
        }

        
	}
}
