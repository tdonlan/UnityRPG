using UnityEngine;
using System.Collections;

using UnityRPG;

public class DragItemControllerScript: MonoBehaviour
{
	public BoxCollider2D boxCollider2D;

	SlotControllerScript lastSlot;

	public Item item; 

	// Use this for initialization
	void Start ()
	{
		this.lastSlot = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.boxCollider2D == null) {
			this.boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
		}
	}

		
	public void addToSlot(SlotControllerScript slot)
	{
		this.lastSlot = slot;
	}

	public void returnToSlot()
	{
		if (lastSlot != null) {
			lastSlot.addItem (this);
		}
	}

}

