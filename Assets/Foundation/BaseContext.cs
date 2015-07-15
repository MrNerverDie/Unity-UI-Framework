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
	public class BaseContext 
    {

        public UIType ViewType { get; private set; }

        public GameObject View { get; private set; }

        public BaseContext(UIType viewType)
        {
            ViewType = viewType;
        }

        public void SetupUI<T>() where T : BaseContext
        {
            View = Singleton<UIManager>.Instance.GetSingleUI(ViewType);
            View.Init<T>(this);
        }
	}
}
