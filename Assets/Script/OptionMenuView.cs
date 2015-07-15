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
		public void BackCallBack()
        {
            Singleton<ContextManager>.Instance.Pop();
        }
    }
}

