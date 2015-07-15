using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Maintain State For UI Stack
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    public class ContextManager
    {
        private Stack<BaseContext> _contextStack = new Stack<BaseContext>();

        private ContextManager()
        {

        }

        public void Push<T>(T nextContext) where T : BaseContext
        {
            GameObject go = Singleton<UIManager>.Instance.GetSingleUI(nextContext.ViewType);

            if (_contextStack.Count != 0)
            {
                
            }

            BaseView<T> nextView = go.GetComponent<BaseView<T>>();
            nextView.Init(nextContext);
            _contextStack.Push(nextContext);
        }

        public void Pop()
        {

        }
    }
}
