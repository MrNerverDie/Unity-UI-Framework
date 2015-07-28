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
	public class GameRoot : MonoBehaviour {

        public void Start()
        {
            Singleton<UIManager>.Create();
            Singleton<ContextManager>.Create();
            Singleton<Localization>.Create();
        }

	}
}
