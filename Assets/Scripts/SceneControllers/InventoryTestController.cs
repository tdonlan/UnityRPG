using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityRPG;

public class InventoryTestController : MonoBehaviour
{
	public GameDataObject gameDataObject { get; set; }

	public Text debugText;

	public DragAndDropScript dragAndDropScript;

	//prefabs
	private GameObject draggableItemPrefab;
	private GameObject itemInfoPrefab;

	private GameObject itemInfoPopup;


	private bool isInventoryLoaded = false;

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnLevelWasLoaded(int level)
	{
		loadPrefabs ();
		loadGameData();
		initScene ();
	}

	private void loadPrefabs()
	{
		draggableItemPrefab = Resources.Load<GameObject> ("PrefabUI/DragItemPrefab");
		itemInfoPrefab = Resources.Load<GameObject> ("PrefabUI/ItemInfoPrefab");
	}

	private void loadGameData()
	{
		gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
	}

	private void initScene()
	{
		dragAndDropScript = GameObject.FindObjectOfType<DragAndDropScript> ();
	}

	private void loadInventory()
	{
		var inventoryList = gameDataObject.playerGameCharacter.inventory;

		int slotCounter = 0;
		foreach (var i in inventoryList) {
			
			debugText.text += i.name;

			var dragItem = initDraggableItem (i);
			if (slotCounter < dragAndDropScript.slotList.Count) {
				var slot = dragAndDropScript.slotList [slotCounter];

				var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();

				dragAndDropScript.draggableItemList.Add (dragItemScript);
				slot.addItem (dragItemScript);
				slotCounter++;
			}

		}

	}

	private void loadEquipment()
	{
		var armorList = gameDataObject.playerGameCharacter.equippedArmor;
		foreach (var armor in armorList) {
			var dragItem = initDraggableItem ((Item)armor);
			var slot = dragAndDropScript.equipmentDictionary [armor.armorType];
			var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();

			dragAndDropScript.draggableItemList.Add (dragItemScript);
			slot.addItem (dragItemScript);

		}
	
	}

	private GameObject initDraggableItem(Item i)
	{
		var dragItem = Instantiate (draggableItemPrefab);
		dragItem.transform.parent = dragAndDropScript.canvasTransform;
		var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();
		dragItemScript.item = i;
		var dragItemSprite = dragItem.GetComponent<Image> ();
		dragItemSprite.sprite = gameDataObject.assetLibrary.getSprite (i.sheetname, i.spriteindex);

		return dragItem;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isInventoryLoaded) {
			loadInventory ();
			loadEquipment ();
			isInventoryLoaded = true;
		}

		checkMouseOver ();
		checkRightClick ();
	}

	//TODO: inefficient to instantiate every frame?
	private void checkMouseOver()
	{
		Destroy (itemInfoPopup);
		//only display popup if not dragging.
		if (dragAndDropScript.currentItem == null) {
			foreach (var dragItem in dragAndDropScript.draggableItemList) {
				if (dragItem.boxCollider2D.OverlapPoint (Input.mousePosition)) {
					initItemInfoPopup (dragItem);
				}
			}
		}

	}

	private void checkRightClick()
	{
		if (Input.GetMouseButtonDown (1)) {
			foreach (var slot in dragAndDropScript.slotList) {
				if (slot.boxCollider2D.OverlapPoint (Input.mousePosition)) {
					var slotItem = slot.getItem ();
					if (slotItem != null) {
						switch (slotItem.item.type) {
						case ItemType.Armor:
							rightClickArmor (slot, slotItem);
							break;
						default:
							break;
						}
					}
				}

			}

		}
	}

	private void rightClickArmor(SlotControllerScript slot, DragItemControllerScript dragItem)
	{
		if (slot is EquipmentSlotControllerScript) {
			//do nothing if we're already an equipment slot
			return;
		} else {
			Armor a = (Armor)dragItem.item;
			var equipSlot = dragAndDropScript.equipmentDictionary [a.armorType];
			var curEquipItem = equipSlot.getItem ();
			if (curEquipItem != null) {
				//switch items
				slot.addItem (curEquipItem);
			}
			equipSlot.addItem (dragItem);
		}
	}

	private void rightClickWeapon()
	{

	}

	private void rightClickUsableItem()
	{

	}

	private void initItemInfoPopup(DragItemControllerScript dragItem)
	{
		itemInfoPopup = Instantiate (itemInfoPrefab);
		itemInfoPopup.transform.parent = dragAndDropScript.canvasTransform;
		itemInfoPopup.transform.position = Input.mousePosition;

		var itemInfoText = itemInfoPopup.GetComponentInChildren<Text> ();
		itemInfoText.text = dragItem.item.ToString ();

	}
}

