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

        public Animator _animator;

        public override void OnEnter(BaseContext context)
        {
            _animator.SetTrigger("OnEnter");
        }

        public override void OnExit(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
        }

        public override void OnPause(BaseContext context)
        {
            _animator.SetTrigger("OnPause");
        }

        public override void OnResume(BaseContext context)
        {
            _animator.SetTrigger("OnResume");
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

