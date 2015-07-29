using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MoleMole
{
    public class OptionMenuContext :BaseContext
    {
        public OptionMenuContext() : base(UIType.OptionMenu)
        {

        }
    }

    public class OptionMenuView : AnimateView
    {

        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }

		public void BackCallBack()
        {
            Singleton<ContextManager>.Instance.Pop();
        }

        public void NextCallBack()
        {
            Singleton<ContextManager>.Instance.Push(new NextMenuContext());
        }
    }
}

