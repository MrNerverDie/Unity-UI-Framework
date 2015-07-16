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

    public class OptionMenuView : BaseView
    {
        public override void OnEnter(BaseContext context)
        {

        }

        public override void OnExit(BaseContext context)
        {
            Singleton<UIManager>.Instance.DestroySingleUI(context.ViewType);
        }

        public virtual void OnPause(BaseContext context)
        {

        }

        public virtual void OnResume(BaseContext context)
        {

        }

		public void BackCallBack()
        {
            Singleton<ContextManager>.Instance.Pop();
        }
    }
}

