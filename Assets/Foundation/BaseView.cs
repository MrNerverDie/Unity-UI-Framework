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
	public abstract class BaseView<T> : MonoBehaviour where T : BaseContext
    {
        public T Context { get; private set; }

        public virtual void Init(T context)
        {
            Context = context;
        }
	}
}
