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
            Singleton<ContextManager>.Create();
        }

	}
}
