using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotControllerScript : MonoBehaviour
{

	public DragItemControllerScript dragItem;
	public BoxCollider2D boxCollider2D;

	public Text slotText;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.boxCollider2D == null) {
			this.boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
		}

		if (dragItem != null) {
			dragItem.transform.position = gameObject.transform.position;
		}

	}

	private void updateSlotText()
	{
		if(dragItem!= null)
		{
			slotText.text = dragItem.name;
		}
		else{
			slotText.text = "Empty";
		}
	}
		
	public DragItemControllerScript getItem()
	{
		if (this.dragItem != null) {
			Debug.Log("Removed item " + this.dragItem.gameObject.name + "  from " + gameObject.name);

			DragItemControllerScript tempItem = dragItem;
			this.dragItem = null;
			return tempItem;

		}
		return null;
	}

	public bool addItem(DragItemControllerScript dragItem)
	{
		Debug.Log ("Added item " + dragItem.gameObject.name + "  to " + gameObject.name);

		if (this.dragItem == null) {
			this.dragItem = dragItem;
			this.dragItem.addToSlot (this);
			return true;
		} else {
			return false;
		}
	}

}

