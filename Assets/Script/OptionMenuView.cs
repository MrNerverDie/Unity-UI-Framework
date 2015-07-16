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

        public virtual void OnPause(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
        }

        public virtual void OnResume(BaseContext context)
        {
            _animator.SetTrigger("OnEnter");
        }

		public void BackCallBack()
        {
            Singleton<ContextManager>.Instance.Pop();
        }
    }
}

