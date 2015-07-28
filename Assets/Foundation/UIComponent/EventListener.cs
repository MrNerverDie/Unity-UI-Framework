using UnityEngine;
using UnityEngine.EventSystems;
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
	public class EventListener : EventTrigger {

        private const float CLICK_INTERVAL_TIME = 0.2f; //const click interval time
        private float _onDowntime = 0f;

        public delegate void PointerEventDelegate(GameObject go);
        public delegate void BaseEventDelegate(GameObject go);
        public delegate void AxisEventDelegate(GameObject go);

        public BaseEventDelegate onDeselect = null;
        public PointerEventDelegate onDrag = null;
        public PointerEventDelegate onDrop = null;
        public AxisEventDelegate onMove = null;
        public PointerEventDelegate onClick = null;
        public PointerEventDelegate onDown = null;
        public PointerEventDelegate onEnter = null;
        public PointerEventDelegate onExit = null;
        public PointerEventDelegate onUp = null;
        public PointerEventDelegate onScroll = null;
        public BaseEventDelegate onSelect = null;
        public BaseEventDelegate onUpdateSelect = null;

        public static EventListener Get(Transform go)
        {
            return Get(go.gameObject);
        }

        public static EventListener Get(MonoBehaviour go)
        {
            return Get(go.gameObject);
        }

        public static EventListener Get(GameObject go)
        {
            EventListener listener = go.GetComponent<EventListener>();
            if (listener == null)
            {
                go.AddComponent<EventListener>();
            }
            return listener;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (onDeselect != null) onDeselect(gameObject);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null) onDrag(gameObject);
        }

        public override void OnDrop(PointerEventData eventData)
        {
            if (onDrop != null) onDrop(gameObject);
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (onMove != null) onMove(gameObject);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            //if (Time.realtimeSinceStartup - this._onDowntime > CLICK_INTERVAL_TIME)
            //{
            //    return;
            //}
            if (onClick != null) onClick(gameObject);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            this._onDowntime = Time.realtimeSinceStartup;
            if (onDown != null) onDown(gameObject);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
        }

        public override void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null) onScroll(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }

	}
}
