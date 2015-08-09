using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace UnityRPG
{
    public class UIHelper
    {
        public static void UpdateTextComponent(GameObject parent, string componentName, string text)
        {
            foreach (var comp in parent.GetComponentsInChildren<Text>())
            {
                if (comp.name == componentName)
                {
                    comp.text = text;
                }
            }
        }
    }
}
