using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class EquipmentSlotControllerScript : SlotControllerScript
{
	public ArmorType armorType;

	public override bool addItem(DragItemControllerScript dragItem)
	{
		var a = (Armor)dragItem.item;
		if (a.armorType == armorType) {
			return base.addItem (dragItem);
		} else {
			return false;
		}

	}
}

