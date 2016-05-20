using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityRPG;

public class DragAndDropScript : MonoBehaviour {
	//All Draggable items
	public List<DragItemControllerScript> draggableItemList = new List<DragItemControllerScript>();
	//All slots
	public List<SlotControllerScript> slotList = new List<SlotControllerScript>();

	public List<SlotControllerScript> inventorySlotList = new List<SlotControllerScript>();
	public Dictionary<ArmorType,EquipmentSlotControllerScript> equipmentDictionary = new Dictionary<ArmorType, EquipmentSlotControllerScript> ();
	public WeaponSlotControllerScript weaponSlot = new WeaponSlotControllerScript(); 
	public AmmoSlotControllerScript ammoSlot = new AmmoSlotControllerScript();
	public List<HotbarSlotControllerScript> hotbarSlotList = new List<HotbarSlotControllerScript>();


	public DragItemControllerScript currentItem = null;
	public SlotControllerScript lastSlot = null;

	public GameObject InventoryPanel;

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

		inventorySlotList = InventoryPanel.GetComponentsInChildren<SlotControllerScript> ().ToList ();

		var armorSlotList = GameObject.FindObjectsOfType<EquipmentSlotControllerScript> ().ToList();
		foreach(var armorSlot in armorSlotList)
		{
			equipmentDictionary.Add (armorSlot.armorType, armorSlot);
		}
		hotbarSlotList = GameObject.FindObjectsOfType<HotbarSlotControllerScript> ().ToList ();
		ammoSlot = GameObject.FindObjectOfType<AmmoSlotControllerScript> ();
		weaponSlot = GameObject.FindObjectOfType<WeaponSlotControllerScript> ();
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
					if(addItemToSlot (slot, currentItem))
					{

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

	private bool addItemToSlot(SlotControllerScript slot, DragItemControllerScript dragItem)
	{
		return slot.addItem (dragItem);
	}

	public void clearEquipment()
	{
		foreach(var slot in inventorySlotList)
		{
			removeItemFromSlot (slot);
		}
		foreach (var slot in equipmentDictionary.Values) {
			removeItemFromSlot (slot);
		}

		removeItemFromSlot (weaponSlot);
		removeItemFromSlot (ammoSlot);
		foreach (var slot in hotbarSlotList) {
			removeItemFromSlot (slot);
		}
	}

	//delete item from slot, and destroy dragItem gameObjec
	private void removeItemFromSlot(SlotControllerScript slot)
	{
		var dragItem = slot.dragItem;
		if (dragItem != null) {
			draggableItemList.Remove (dragItem);
			slot.dragItem = null;
			Destroy (dragItem.gameObject);
		}
	}
}
