using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityRPG;

public class DragAndDropScript : MonoBehaviour {

	public List<DragItemControllerScript> draggableItemList = new List<DragItemControllerScript>();
	public List<SlotControllerScript> slotList = new List<SlotControllerScript>();
	public Dictionary<ArmorType,EquipmentSlotControllerScript> equipmentDictionary = new Dictionary<ArmorType, EquipmentSlotControllerScript> ();

	public DragItemControllerScript currentItem = null;
	public SlotControllerScript lastSlot = null;

	public Camera mainCamera;

	public Text debugText;
	public RectTransform canvasTransform;

	// Use this for initialization
	void Start () {
		addSlots ();
	}

	void addSlots()
	{
		slotList = GameObject.FindObjectsOfType<SlotControllerScript> ().ToList();
		var armorSlotList = GameObject.FindObjectsOfType<EquipmentSlotControllerScript> ().ToList();
		foreach(var armorSlot in armorSlotList)
		{
			equipmentDictionary.Add (armorSlot.armorType, armorSlot);
		}
	}
	
	// Update is called once per frame
	void Update () {

		UpdateMouse ();

		if (currentItem != null) {
			DragCurrentItem ();
		}
	}
		
	private void UpdateMouse()
	{


		if (Input.GetMouseButtonDown (0)) {
			checkGrab ();
		}
		if (Input.GetMouseButtonUp (0)) {
			ReleaseItem ();
		}
	}
		
	private void checkGrab()
	{
		foreach (var slot in slotList) {
			if (slot.boxCollider2D.OverlapPoint (Input.mousePosition)) {
				var slotItem = slot.getItem ();
				if (slotItem != null) {
					this.currentItem = slotItem;
				}
			}
		}

		//Not really needed - we should only grab from slots.
		if (currentItem == null) {
			foreach (var item in draggableItemList) {
				if (item.boxCollider2D.OverlapPoint (Input.mousePosition)) {
					this.currentItem = item;
				}
			}
		}
	}

	private void DragCurrentItem()
	{
		currentItem.gameObject.transform.position = Input.mousePosition;

	}


	private void ReleaseItem()
	{
		if (currentItem != null) {
			bool isSet = false;
			foreach (var slot in slotList) {
				if (slot.boxCollider2D.OverlapPoint (Input.mousePosition)) {
					if (slot.addItem (currentItem)) {
						isSet = true;
					}
					break;
				}
			}

			if (!isSet) {
				currentItem.returnToSlot ();
			}
				
			currentItem = null;
		}
	}
}
