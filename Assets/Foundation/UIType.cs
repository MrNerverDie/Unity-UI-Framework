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
	public class UIType {

        private string _path;

        public string Path { get; private set; }

        private string _name;

        public string Name { get; private set; }

        public UIType(string path, string name)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('/'));
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }

        public static readonly UIType MainMenu = new UIType("UIMainMenu", "UIMainMenu");
	}
}
