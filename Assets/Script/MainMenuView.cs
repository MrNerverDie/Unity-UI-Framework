using UnityEngine;
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
    public class MainMenuContext : BaseContext
    {
        public MainMenuContext() : base(UIType.MainMenu)
        {

        }
    }

    public class MainMenuView : BaseView
    {
        public override void OnEnter(BaseContext context)
        {

        }

        public override void OnExit(BaseContext context)
        {

        }

        public override void OnPause(BaseContext context)
        {
            Singleton<UIManager>.Instance.DestroySingleUI(context.ViewType);
        }

        public override void OnResume(BaseContext context)
        {

        }

        public void OKCallBack()
        {
            Singleton<ContextManager>.Instance.Push(new OptionMenuContext());
        }
	}
}
