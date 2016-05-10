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

		//debugText.text = JsonUtility.ToJson (inventoryList);

		int slotCounter = 0;
		foreach (var i in inventoryList) {
			debugText.text += i.name;
			var dragItem = initDraggableItem (i);
			if (slotCounter < dragAndDropScript.slotList.Count) {
				var slot = dragAndDropScript.slotList [slotCounter];

				var dragItemScript = dragItem.GetComponent<DragItemControllerScript> ();
				slot.addItem (dragItemScript);
				//dragItemScript.addToSlot (slot);
				slotCounter++;
			}


			
		}

	}



	private GameObject initDraggableItem(Item i)
	{
		var dragItem = Instantiate (draggableItemPrefab);
		dragItem.transform.parent = dragAndDropScript.canvasTransform;
		var dragItemSprite = dragItem.GetComponent<Image> ();
		dragItemSprite.sprite = gameDataObject.assetLibrary.getSprite (i.sheetname, i.spriteindex);

		return dragItem;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isInventoryLoaded) {
			loadInventory ();
			isInventoryLoaded = true;
		}
	}
}

