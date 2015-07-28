using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MoleMole
{
    public class NextMenuContext : BaseContext
    {
        public NextMenuContext()
            : base(UIType.NextMenu)
        {

        }
    }

    public class NextMenuView : BaseView
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

        public void BackCallBack()
        {
            Singleton<ContextManager>.Instance.Pop();
        }

        public void ChangeLangCallBack()
        {
            if (Singleton<Localization>.Instance.Language == Localization.CHINESE)
            {
                Singleton<Localization>.Instance.Language = Localization.ENGLISH;
            }
            else
            {
                Singleton<Localization>.Instance.Language = Localization.CHINESE;
            }
        }
    }
}

