using UnityEngine;
using UnityEngine.UI;
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
        public Movement _moveType = Movement.Vertical;
        public delegate void OnChange(Transform trans, int index);

        // private fields //
        private Queue<Transform> _transQueue = new Queue<Transform>();
        private OnChange _onChange;
        private int _firstIndex = 0;
        private int _itemCount = 0;
        private int _transCount = 0;
        private int _col = 0;
        private int _row = 0;
        private Rect _gridRect;

        public void Init(OnChange onChange, int itemCount, GridScrollerPos pos = null)
        {
            Clear();
            InitScroller();
            InitGrid();

            // Init Children //
            //_firstIndex = (pos == null) ? 0 : pos.FirstIndex;
            //_onChange = onChange;
            //for (int i = 0; i < _itemCount; i++)
            //{
            //    Transform item = _grid.transform.AddChild(_itemPrefab);
            //    bool isActive = onChange(item, _firstIndex + i);
            //    _itemQueue.Enqueue(item);
            //    item.gameObject.SetActive(isActive);
            //}
            //_scroller.normalizedPosition = (pos == null) ? Vector2.zero : pos.NormalizedPosition;

            //_transCount = 

        }

        private void InitScroller()
        {
            // Init Scroller //
            _scroller = GetComponent<ScrollRect>();
            _scroller.onValueChanged.AddListener(OnValueChanged);

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
            _grid.startAxis = (_moveType == Movement.Horizontal) ? GridLayoutGroup.Axis.Vertical : GridLayoutGroup.Axis.Horizontal;
            _grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            _grid.childAlignment = TextAnchor.UpperLeft;
            _gridRect = _grid.GetComponent<RectTransform>().rect;
        }

        private void Clear()
        {
            _transQueue.Clear();
            _grid.transform.DestroyChildren();
        }

        public void OnValueChanged(Vector2 normalizedPosition)
        {
            for (int i = 0; i < _grid.transform.childCount; i++)
            {
                Transform childTrans = _grid.transform.GetChild(i);
                RectTransform childRect = childTrans.GetComponent<RectTransform>();
                if (!IsItemInScroller(childRect))
                {
                    _firstIndex++;
                    
                }
            }


        }

        private bool IsItemInScroller(RectTransform rectTrans)
        {
            Rect itemRect = rectTrans.rect;
            Rect gridRect = _grid.GetComponent<RectTransform>().rect;
            Rect scrollerRect = new Rect(0, 0, _scroller.GetComponent<RectTransform>().sizeDelta.x, _scroller.GetComponent<RectTransform>().sizeDelta.y);

            Rect itemRectInScroller = new Rect(itemRect.x + gridRect.x, itemRect.y + gridRect.y, itemRect.width, itemRect.height);
            return scrollerRect.Overlaps(itemRectInScroller);
        }

        public GridScrollerPos GetCurPos()
        {
            return new GridScrollerPos(_firstIndex, _scroller.normalizedPosition);
        }

        
	}
}
