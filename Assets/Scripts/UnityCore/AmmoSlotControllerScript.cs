using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class AmmoSlotControllerScript : SlotControllerScript
{
	public override DragItemControllerScript getItem()
	{
		if (this.dragItem != null) {

			removeAmmo ((Ammo)dragItem.item);

			Debug.Log("Removed item " + this.dragItem.gameObject.name + "  from " + gameObject.name);

			DragItemControllerScript tempItem = dragItem;
			this.dragItem = null;
			return tempItem;
		}
		return null;
	}


	public override bool addItem(DragItemControllerScript dragItem)
	{
		if (dragItem.item is Ammo) {
			var a = (Ammo)dragItem.item;
			if (this.dragItem == null) {
				Debug.Log ("Added item " + dragItem.gameObject.name + "  to " + gameObject.name);
				this.dragItem = dragItem;
				this.dragItem.addToSlot (this);

				addAmmo (a);

				return true;
			}
		}
		return false;
	}

	private void removeAmmo(Ammo a)
	{
		gameDataObject.getSelectedCharacter ().RemoveAmmo ();
	}

	private void addAmmo(Ammo a)
	{
		gameDataObject.getSelectedCharacter ().EquipAmmo (a);
	}
}

