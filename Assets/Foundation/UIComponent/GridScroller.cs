using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Grid Scroller For High Performance
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    [RequireComponent(typeof(ScrollRect))]
	public class GridScroller : MonoBehaviour {

        // public UI elements //
        [SerializeField]
        private Transform _itemPrefab;
        [SerializeField]
        private GridLayoutGroup _grid;

        // private UI elements //
        private ScrollRect _scroller;

        // define //
        public enum Movement
        {
            Horizontal,
            Vertical,
        }

        // public fields //
        [SerializeField]
        private Movement _moveType = Movement.Horizontal;
        
        public delegate void OnChange(Transform trans, int index);

        // private fields //
        private HashSet<int> _transIndexSet = new HashSet<int>();
        private HashSet<int> _showIndexSet = new HashSet<int>();
        private Dictionary<int, RectTransform> _transDict = new Dictionary<int, RectTransform>();
        private OnChange _onChange;
        private int _itemCount = 0;
        private int _transCount = 0;
        private int _col = 0;
        private int _row = 0;
        private Rect _scrollerRect;
        private bool _hasChanged = false;
        private Vector2 _cellSize = Vector3.zero;
        private Vector2 _spacing = Vector3.zero;

        public Vector2 ItemSize
        {
            get
            {
                return _spacing + _cellSize;
            }
        }

        #region Public Methods

        public void Init(OnChange onChange, int itemCount, Vector2? normalizedPosition = null)
        {
            Clear();
            InitScroller();
            InitGrid();
            InitChildren(onChange, itemCount);
            InitTransform(normalizedPosition);
        }

        public void RefreshCurrent()
        {
            foreach (int index  in _transIndexSet)
            {
                if (_onChange != null)
                {
                    _onChange(_transDict[index], index);
                }
            }
        }

        #endregion

        #region Init

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
            _cellSize = _grid.GetComponent<GridLayoutGroup>().cellSize;
            _spacing = _grid.GetComponent<GridLayoutGroup>().spacing;
            _grid.GetComponent<GridLayoutGroup>().enabled = false;

        }

        private void InitChildren(OnChange onChange, int itemCount)
        {
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
        private void InitTransform(Vector2? normalizedPosition = null)
        {

            if (_moveType == Movement.Horizontal)
            {
                _grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((_itemCount + 1) / _row) * ItemSize.x);
            }
            else
            {
                _grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((_itemCount + 1) / _col) * ItemSize.y);
            }
            _scroller.onValueChanged.AddListener(OnValueChanged);
            _scroller.normalizedPosition = (normalizedPosition.HasValue) ? normalizedPosition.Value : new Vector2(0, 1);
        }

        private void Clear()
        {
            _transIndexSet.Clear();
            _transDict.Clear();
            _showIndexSet.Clear();
            if (_grid != null)
            {
                _grid.GetComponent<RectTransform>().DestroyChildren();
            }
        }

        #endregion

        #region OnValueChanged

        public void OnValueChanged(Vector2 normalizedPosition)
        {
            if (_transCount == _itemCount)
            {
                return;
            }

            Vector2 scrollerSize = _scroller.GetComponent<RectTransform>().rect.size;
            Vector2 gridSize = _grid.GetComponent<RectTransform>().rect.size;

            int startIndex = 0;
            if (_moveType == Movement.Horizontal)
            {
                float scrollLength = - _grid.GetComponent<RectTransform>().anchoredPosition.x;
                int scrollCol = (int)(scrollLength / ItemSize.x);
                startIndex = scrollCol * _row;
            }
            else
            {
                float scrollLength = _grid.GetComponent<RectTransform>().anchoredPosition.y;
                int scrollRow = (int)(scrollLength / ItemSize.y);
                startIndex = scrollRow * _col;
            }

            SwapIndex(startIndex);
        }

        private void SwapIndex(int startIndex)
        {
            _showIndexSet.Clear();
            for (int i = 0; i < _transCount; i++)
            {
                if ((i + startIndex) < _itemCount && (i + startIndex) >= 0)
                {
                    _showIndexSet.Add(i + startIndex);
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
        }

        private void ChangeToIndex(int from, int to)
        {
            //Debug.Log(from + " | " + to);
            RectTransform rectTrans = _transDict[from];
            rectTrans.anchoredPosition = IndexToPosition(to);
            _transDict.Remove(from);
            _transDict.Add(to, rectTrans);
        }

        private Vector2 IndexToPosition(int index)
        {
            if (_moveType == Movement.Horizontal)
	        {
                return new Vector2(ItemSize.x * (index / _row), -ItemSize.y * (index % _row));
	        }else
	        {
                return new Vector2(ItemSize.x * (index % _col), -ItemSize.y * (index / _col));
	        }
        }

        #endregion

    }
}
