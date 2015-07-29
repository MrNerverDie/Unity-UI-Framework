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

    public class NextMenuView : AnimateView
    {

        public Animator _animator;

        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
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

