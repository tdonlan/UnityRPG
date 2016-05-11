using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class WeaponSlotControllerScript : SlotControllerScript
{
	public override bool addItem(DragItemControllerScript dragItem)
	{
		if(dragItem.item is Weapon)
		 {
			return base.addItem (dragItem);
		} else {
			return false;
		}

	}
}

