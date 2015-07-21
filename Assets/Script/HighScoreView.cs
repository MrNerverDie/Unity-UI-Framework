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
    public class HighScoreContext : BaseContext
    {
        public HighScoreContext()
            : base(UIType.HighScore)
        {

        }
    }

    public class HighScoreView : BaseView
    {
        public Animator _animator;
        public GridScroller _gridScroller;

        public override void OnEnter(BaseContext context)
        {
            //_animator.SetTrigger("OnEnter");
            _gridScroller.Init(OnChange, 100);
        }

        public override void OnExit(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
        }

        public override void OnPause(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
        }

        public override void OnResume(BaseContext context)
        {
            _animator.SetTrigger("OnEnter");
        }

        public void BackCallBack()
        {
            Singleton<ContextManager>.Instance.Pop();
        }

        public void OnChange(Transform trans, int index)
        {
            trans.GetComponent<HighScoreItem>().Init(index);
        }
    }
}
