using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class WeaponSlotControllerScript : SlotControllerScript
{

	public override DragItemControllerScript getItem()
	{
		if (this.dragItem != null) {

			removeWeapon ((Weapon)dragItem.item);

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

				addWeapon ((Weapon)dragItem.item);

				return true;
			} 
		} 

		return false;

	}

	private void addWeapon(Weapon w)
	{
		gameDataObject.getSelectedCharacter ().EquipWeapon (w);
	}

	private void removeWeapon(Weapon w)
	{
		gameDataObject.getSelectedCharacter ().RemoveWeapon (w);
	}

}

