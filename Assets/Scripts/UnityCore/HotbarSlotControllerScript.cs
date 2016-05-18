using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityRPG;

public class HotbarSlotControllerScript : SlotControllerScript
{
	public override DragItemControllerScript getItem()
	{
		if (this.dragItem != null) {

			removeHotbarItem (dragItem.item);

			Debug.Log("Removed item " + this.dragItem.gameObject.name + "  from " + gameObject.name);

			DragItemControllerScript tempItem = dragItem;
			this.dragItem = null;
			return tempItem;
		}
		return null;
	}


	public override bool addItem(DragItemControllerScript dragItem)
	{
		if (dragItem.item is UsableItem) {
			Debug.Log ("Added item " + dragItem.gameObject.name + "  to " + gameObject.name);

			if (this.dragItem == null) {
				this.dragItem = dragItem;
				this.dragItem.addToSlot (this);

				addHotbarItem (dragItem.item);

				return true;
			} 
		}

		return false;
	}

	private void removeHotbarItem(Item i)
	{
		gameDataObject.getSelectedCharacter ().removeUsableItem (i);
	}

	private void addHotbarItem(Item i)
	{
		gameDataObject.getSelectedCharacter ().addUsableItem (i);

	}
}

