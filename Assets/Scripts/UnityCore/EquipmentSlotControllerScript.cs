using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class EquipmentSlotControllerScript : SlotControllerScript
{
	public ArmorType armorType;


	public override DragItemControllerScript getItem()
	{
		if (this.dragItem != null) {

			gameDataObject.playerGameCharacter.RemoveArmor ((Armor)dragItem.item);

			Debug.Log("Removed item " + this.dragItem.gameObject.name + "  from " + gameObject.name);

			DragItemControllerScript tempItem = dragItem;
			this.dragItem = null;
			return tempItem;
		}
		return null;
	}

	public override bool addItem(DragItemControllerScript dragItem)
	{
		if (dragItem.item is Armor) {
			var a = (Armor)dragItem.item;
			if (a.armorType == armorType) { 
				Debug.Log ("Added item " + dragItem.gameObject.name + "  to " + gameObject.name);

				if (this.dragItem == null) {
					this.dragItem = dragItem;
					this.dragItem.addToSlot (this);

					gameDataObject.playerGameCharacter.EquipArmor (a);

					return true;
				} 
			} 
		}

		return false;

	}
}

