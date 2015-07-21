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
        public GridLayoutGroup _grid;
        public Transform _itemPrefab;

        // private UI elements //
        private ScrollRect _scroller;

        // define //
        public enum Movement
        {
            Horizontal,
            Vertical,
        }

        // public fields //
        public Movement _moveType = Movement.Horizontal;
        public delegate void OnChange(Transform trans, int index);

        // private fields //
        private HashSet<int> _transIndexSet = new HashSet<int>();
        private HashSet<int> _showIndexSet = new HashSet<int>();
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
                return _grid.spacing + _grid.cellSize;
            }
        }

        public void Init(OnChange onChange, int itemCount, GridScrollerPos pos = null)
        {
            Clear();
            InitScroller();
            InitChildren(onChange, itemCount, pos);
            InitGrid();

            _scroller.onValueChanged.AddListener(OnValueChanged);
            _scroller.normalizedPosition = (pos == null) ? Vector2.one : pos.NormalizedPosition;
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
            _grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            _grid.childAlignment = TextAnchor.UpperLeft;
            _grid.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            _grid.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            _grid.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            if (_moveType == Movement.Horizontal)
            {
                _grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_itemCount / _row) * ItemSize.x);
                _grid.startAxis = GridLayoutGroup.Axis.Vertical;
            }
            else
            {
                _grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_itemCount / _col) * ItemSize.y);
                _grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            }
        }

        private void InitChildren(OnChange onChange, int itemCount, GridScrollerPos pos)
        {
            _firstIndex = (pos == null) ? 0 : pos.FirstIndex;
            _onChange = onChange;
            _itemCount = itemCount;
            Vector2 itemSize = _grid.cellSize + _grid.spacing;
            _col = (int)((_scrollerRect.width + _grid.spacing.x) / itemSize.x);
            _row = (int)((_scrollerRect.height + _grid.spacing.y) / itemSize.y);
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
                _onChange(item, i);
                _transIndexSet.Add(item.GetSiblingIndex());
            }


            for (int i = _transCount; i < _itemCount; i++)
            {
                GameObject go = new GameObject("EmptyItem", typeof(RectTransform));
                go.transform.SetParent(_grid.transform, false);
            }
        }   

        private void Clear()
        {
            _transIndexSet.Clear();
            _grid.transform.DestroyChildren();
        }

        public void OnValueChanged(Vector2 normalizedPosition)
        {
            if (_transCount == _itemCount)
            {
                return;
            }

            //Debug.Log(normalizedPosition.y);

            Vector2 scrollerSize = _scroller.GetComponent<RectTransform>().rect.size;
            Vector2 gridSize = _grid.GetComponent<RectTransform>().rect.size;

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
                        SwapSiblingIndex(lhsIter.Current, rhsIter.Current);
                        _onChange(_grid.transform.GetChild(lhsIter.Current), lhsIter.Current);
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

        private void SwapSiblingIndex(int index1, int index2)
        {
            SwapSiblingIndex(_grid.transform.GetChild(index1), _grid.transform.GetChild(index2));
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
