using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using UnityRPG;

public class StoreControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }

    public long parentTreeLink;
    public StoreTree storeTree { get; set; }

    private List<StoreItem> storeItemList = new List<StoreItem>();
    private List<StoreItem> playerItemList = new List<StoreItem>();
    
    public GameObject storeTextObject;
    public Text storeText;
    public Text playerMoneyText;

    System.Random r;

    //Prefabs
    private List<GameObject> storeItemObjectList = new List<GameObject>();
    private List<GameObject> playerItemObjectList = new List<GameObject>();
    public GameObject StoreItemPrefab;

    public GameObject buyPanel;
    public GameObject sellPanel;

    private bool wasUpdated = false;


    void OnLevelWasLoaded(int level)
    {
        r = new System.Random();

        loadGameData();
        initScene();
    }

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void initScene()
    {
        loadPrefabs();
        LoadTreeStore();
        LoadStoreList();
        LoadPlayerList();

        updateDisplay();
    }

    private void loadPrefabs()
    {
        storeText = storeTextObject.GetComponent<Text>();

    }

    private void LoadTreeStore()
    {
        //dont select tree, get the tree node from current zone content
        //assuming the parent is a zone type for now
        ZoneTree parentTree = (ZoneTree)gameDataObject.treeStore.getCurrentTree();
        ZoneTreeNode parentTreeNode = (ZoneTreeNode)parentTree.getNode(parentTree.currentIndex);
        long storeLink = parentTreeNode.content.linkIndex;

        parentTreeLink = gameDataObject.treeStore.currentTreeIndex;

        gameDataObject.treeStore.SelectTree(storeLink);
        storeTree = (StoreTree)gameDataObject.treeStore.getCurrentTree();
    }

    private void LoadStoreList()
    {
        storeItemList = storeTree.getSellList(gameDataObject.gameDataSet, r);
    }

    private void LoadPlayerList()
    {
        playerItemList = new List<StoreItem>();

        var itemList = gameDataObject.playerGameCharacter.inventory.Distinct();
    
        foreach (var item in itemList)
        {
            StoreItem tempStoreItem = new StoreItem();
            tempStoreItem.item = item;
            tempStoreItem.price = storeTree.getBuyPrice(item, gameDataObject.gameDataSet);
            tempStoreItem.count = gameDataObject.playerGameCharacter.inventory.Count(x => x.ID == item.ID);

            playerItemList.Add(tempStoreItem);
        }
    }

    private void updateDisplay()
    {
        
        string storeDisplayText = "";
      
        foreach (var item in storeItemList)
        {
            storeDisplayText += item.item.name + " $" + item.price + " ct:" + item.count + "\n";
        }
        storeText.text = storeDisplayText;

        UpdateStore();
        UpdatePlayerInventory();

        playerMoneyText.text = gameDataObject.playerGameCharacter.money + " gold";

    }
	
	// Update is called once per frame
	void Update () {
        //HACK - UI isn't updating when called from levelWasLoaded
        if (!wasUpdated)
        {
            updateDisplay();
            wasUpdated = true;
        }
	}

    public void ExitStore()
    {
        //switch back to parent tree link
        gameDataObject.treeStore.SelectTree(parentTreeLink);

        //go back to the zone view
        Application.LoadLevel((int)UnitySceneIndex.Zone);
    }

    public void BuyItem(long itemID)
    {

        StoreItem storeItem = storeItemList.Where(x => x.item.ID == itemID).FirstOrDefault();

        if (storeItem != null && gameDataObject.playerGameCharacter.money >= storeItem.price)
        {
            gameDataObject.addItem(storeItem.item.ID, 1);
            gameDataObject.removeItem(GameConstants.MONEY_INDEX, storeItem.price);

            playerItemList.Add(storeItem);
            storeItemList.Remove(storeItem);

            updateDisplay();
        }

    }

    public void SellItem(long itemID)
    {
        //check if we have enough gold.
        StoreItem playerItem = playerItemList.Where(x => x.item.ID == itemID).FirstOrDefault();
        if (playerItem != null)
        {
            gameDataObject.removeItem(playerItem.item.ID, 1);
            playerItemList.Remove(playerItem);
            storeItemList.Add(playerItem);

            gameDataObject.addItem(GameConstants.MONEY_INDEX, playerItem.price);

            updateDisplay();
        }
     
    }

    private void UpdatePlayerInventory(){
        foreach (var playerItemObject in playerItemObjectList)
        {
            Destroy(playerItemObject);
        }
        playerItemObjectList.Clear();

        foreach (var item in playerItemList)
        {
            playerItemObjectList.Add(UpdateStoreItem(item, false));
        }
    }

    private void UpdateStore(){
        //clear store list.  re-add all items in store
       
        foreach (var storeItemObject in storeItemObjectList)
        {
            Destroy(storeItemObject);
        }
        storeItemObjectList.Clear();
     

        foreach (var item in storeItemList)
        {
            storeItemObjectList.Add(UpdateStoreItem(item,true));
        }
    }


    private GameObject UpdateStoreItem(StoreItem storeItem, bool isStore) {
        GameObject storeItemObject = (GameObject)Instantiate(StoreItemPrefab);

        var itemSprite = gameDataObject.assetLibrary.getSprite(storeItem.item.sheetname,storeItem.item.spriteindex);

        UIHelper.UpdateSpriteComponent(storeItemObject, "ItemImg", itemSprite);
        UIHelper.UpdateTextComponent(storeItemObject, "ItemName", storeItem.item.name);
        UIHelper.UpdateTextComponent(storeItemObject, "ItemStats", storeItem.item.ToString());

        UIHelper.UpdateTextComponent(storeItemObject, "ItemPrice", storeItem.price.ToString());
        var priceText = UIHelper.getGameObjectWithName(storeItemObject, "ItemPrice", typeof(Text)).GetComponent<Text>();
        if (isStore && storeItem.price > gameDataObject.playerGameCharacter.money)
        {
            priceText.color = Color.red;
        }

        Button storeItemButton = storeItemObject.GetComponentInChildren<Button>();

        if (isStore)
        {
            storeItemButton.onClick.AddListener(() => BuyItem(storeItem.item.ID));
            storeItemObject.transform.SetParent(buyPanel.transform, true);
        }
        else
        {
            storeItemButton.onClick.AddListener(() => SellItem(storeItem.item.ID));
            UIHelper.UpdateTextComponent(storeItemButton.gameObject, "Text", "Sell");
            storeItemObject.transform.SetParent(sellPanel.transform, true);
        }
      
        return storeItemObject;

    }

    public void addItem()
    {
        updateDisplay();

    }

}
