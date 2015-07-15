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
    public class MainMenuContext : BaseContext
    {
        public MainMenuContext() : base(UIType.MainMenu)
        {

        }
    }

    public class MainMenuView : BaseView<MainMenuContext>
    {

        public override void Init(MainMenuContext context)
        {
            base.Init(context);
        }

	}
}
