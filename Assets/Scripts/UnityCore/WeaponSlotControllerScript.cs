using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class WeaponSlotControllerScript : SlotControllerScript
{

	public override DragItemControllerScript getItem()
	{
		if (this.dragItem != null) {

			gameDataObject.playerGameCharacter.weapon = null;

			Debug.Log("Removed item " + this.dragItem.gameObject.name + "  from " + gameObject.name);

			DragItemControllerScript tempItem = dragItem;
			this.dragItem = null;
			return tempItem;

		}
		return null;
	}

	public override bool addItem(DragItemControllerScript dragItem)
	{
		if (dragItem.item is Weapon) {
			Debug.Log ("Added item " + dragItem.gameObject.name + "  to " + gameObject.name);

			if (this.dragItem == null) {
				this.dragItem = dragItem;
				this.dragItem.addToSlot (this);

				gameDataObject.playerGameCharacter.weapon = (Weapon)dragItem.item;

				return true;
			} 
		} 

		return false;

	}

}

