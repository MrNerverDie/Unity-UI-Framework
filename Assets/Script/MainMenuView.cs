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
        public Animator _animator;

        public override void OnEnter(BaseContext context)
        {
            _animator.SetTrigger("OnEnter");
        }

        public override void OnExit(BaseContext context)
        {

        }

        public override void OnPause(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
        }

        public override void OnResume(BaseContext context)
        {
            _animator.SetTrigger("OnEnter");
        }

        public void OKCallBack()
        {
            Singleton<ContextManager>.Instance.Push(new OptionMenuContext());
        }
	}
}
