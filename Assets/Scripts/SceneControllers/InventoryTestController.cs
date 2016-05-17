using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityRPG;

public class InventoryTestController : MonoBehaviour
{
	public GameDataObject gameDataObject { get; set; }
	public GameCharacter selectedGameCharacter;
	public int selectedCharIndex = 0;

	public Text debugText;

	public DragAndDropScript dragAndDropScript;

	//prefabs
	private GameObject draggableItemPrefab;
	private GameObject itemInfoPrefab;

	//Game Objects
	private GameObject itemInfoPopup;

	private bool inventoryShowing = false;
	public GameObject InventoryPanel;

	private bool equipmentShowing = false;
	public GameObject EquipmentPanel;

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
		draggableItemPrefab = Resources.Load<GameObject> ("PrefabUI/InventoryUI/DragItemPrefab");
		itemInfoPrefab = Resources.Load<GameObject> ("PrefabUI/InventoryUI/ItemInfoPrefab");
	}

	private void loadGameData()
	{
		gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
		gameDataObject.SelectCharacter (gameDataObject.playerGameCharacter);
		selectedGameCharacter = gameDataObject.getSelectedCharacter ();
	}

	private void initScene()
	{
		dragAndDropScript = GameObject.FindObjectOfType<DragAndDropScript> ();
	}

	public void resetAllEquipment()
	{
		dragAndDropScript.clearEquipment ();
		loadEquipment ();
		loadWeapon ();
	}

	//Inventory shared by all chars, just load once.
	private void loadInventory()
	{
		var inventoryList = gameDataObject.playerGameCharacter.inventory;

		int slotCounter = 0;
		foreach (var i in inventoryList) {
			
			//debugText.text += i.name;

			var dragItem = initDraggableItem (i);
			if (slotCounter < dragAndDropScript.slotList.Count) {
				var slot = dragAndDropScript.slotList [slotCounter];

				var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();

				dragAndDropScript.draggableItemList.Add (dragItemScript);
				slot.putItem (dragItemScript);
				slotCounter++;
			}

		}

	}

	private void loadEquipment()
	{
		//Load all armor from current selected character
		var armorList = gameDataObject.getSelectedCharacter().equippedArmor;
		foreach (var armor in armorList) {
			var dragItem = initDraggableItem ((Item)armor);
			var slot = dragAndDropScript.equipmentDictionary [armor.armorType];
			var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();

			dragAndDropScript.draggableItemList.Add (dragItemScript);
			slot.putItem (dragItemScript);
		}
	}

	private void loadWeapon()
	{
		//load weapon from current selected character
		if (gameDataObject.getSelectedCharacter().weapon != null) {
			var dragItem = initDraggableItem ((Item)gameDataObject.getSelectedCharacter().weapon);
			var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();
			dragAndDropScript.draggableItemList.Add (dragItemScript);
			dragAndDropScript.weaponSlot.putItem (dragItemScript);
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
			loadWeapon ();
			isInventoryLoaded = true;
		}

		UpdateKeys ();
		checkMouseOver ();
		checkRightClick ();

		//UpdateStats ();
	}

	//move somewhere else?
	private void UpdateKeys()
	{
		if (Input.GetKeyDown (KeyCode.I)) {
			if (inventoryShowing) {
				hideInventoryPanel ();
			} else {
				showInventoryPanel ();
			}
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			if (equipmentShowing) {
				hideEquipmentPanel ();
			} else {
				showEquipmentPanel ();
			}

		}


	}

	private void UpdateStats()
	{
		//debugText.text =gameDataObject.getSelectedCharacter().name + "\n" +   gameDataObject.getSelectedCharacter().ToString ();

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
					var slotItem = slot.dragItem;
					if (slotItem != null) {
						switch (slotItem.item.type) {
						case ItemType.Armor:
							rightClickArmor (slot);
							break;
						case ItemType.Weapon:
							rightClickWeapon (slot);
							break;
						default:
							break;
						}
					}
				}

			}

		}
	}

	private void rightClickArmor(SlotControllerScript slot)
	{
		if (slot is EquipmentSlotControllerScript) {
			Debug.Log ("right clicked equip slot");
			//do nothing if we're already an equipment slot
			return;
		} else {
			var dragItem = slot.getItem ();
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

	private void rightClickWeapon(SlotControllerScript slot)
	{
		if (slot is WeaponSlotControllerScript) {
			//do nothing if we're already an weapon slot
			return;
		} else {
			var dragItem = slot.getItem ();
			var weaponSlot = dragAndDropScript.weaponSlot;
			var curWeapon = weaponSlot.getItem ();
			if (curWeapon != null) {
				//switch items
				slot.addItem (curWeapon);
			}
			weaponSlot.addItem (dragItem);
		}
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
		itemInfoText.text =  dragItem.item.ToString () +  "\n" + dragItem.lastSlot.name;

	}

	public void toggleCharacter()
	{
		Debug.Log ("toggling characters");
		selectedCharIndex++;
		if (selectedCharIndex > gameDataObject.partyList.Count) {
			selectedCharIndex = 0;
		}

		if (selectedCharIndex == 0)
		{
			selectedGameCharacter = gameDataObject.playerGameCharacter;
		}
		else if (gameDataObject.partyList.Count > 0 && selectedCharIndex <= gameDataObject.partyList.Count)
		{
			selectedGameCharacter =  gameDataObject.partyList[selectedCharIndex - 1];
		}

		gameDataObject.SelectCharacter (selectedGameCharacter);

		Debug.Log ("Selected " + selectedGameCharacter.name);
	
		dragAndDropScript.clearEquipment ();
		loadEquipment ();
		loadWeapon ();
	
	}

	public void showInventoryPanel()
	{
		inventoryShowing = true;
		InventoryPanel.transform.position = GameConfig.InventoryPanelLocation;
	}

	public void hideInventoryPanel()
	{
		inventoryShowing = false;
		InventoryPanel.transform.position = GameConfig.OffscreenLocation;
	}

	public void showEquipmentPanel()
	{
		equipmentShowing = true;
		EquipmentPanel.transform.position = GameConfig.EquipmentPanelLocation;
	}

	public void hideEquipmentPanel()
	{
		equipmentShowing = false;
		EquipmentPanel.transform.position = GameConfig.OffscreenLocation;
	}
}

