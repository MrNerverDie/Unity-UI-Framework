using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Base Animate View
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
	public abstract class AnimateView : BaseView 
    {
        [SerializeField]
        protected Animator _animator;

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

	}
}
