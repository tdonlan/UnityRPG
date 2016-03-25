using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
        //storeText = storeTextObject.GetComponent<Text>();

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

        var itemList = gameDataObject.playerGameCharacter.inventory.GroupBy(x => x.ID).Select(x=>x.First()).ToList();
    
        foreach (var item in itemList)
        {
            StoreItem tempStoreItem = new StoreItem();
            tempStoreItem.item = item;
            tempStoreItem.price = storeTree.getBuyPrice(item, gameDataObject.gameDataSet);
            tempStoreItem.count = gameDataObject.playerGameCharacter.inventory.Count(x => x.ID == item.ID);
            tempStoreItem.selected = 1;

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
        //storeText.text = storeDisplayText;

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
		SceneManager.LoadScene((int)UnitySceneIndex.Zone);
     
    }

    public void BuyItem(long itemID)
    {

        StoreItem storeItem = storeItemList.Where(x => x.item.ID == itemID).FirstOrDefault();

        long cost = storeItem.price * storeItem.selected;

        if (storeItem != null && gameDataObject.playerGameCharacter.money >= cost)
        {
            gameDataObject.addItem(storeItem.item.ID, storeItem.selected);
            gameDataObject.removeItem(GameConstants.MONEY_INDEX, cost);

            addPlayerItemList(storeItem);
            removeStoreItemList(storeItem);

            updateDisplay();
        }

    }

    public void SellItem(long itemID)
    {
        //check if we have enough gold.
        StoreItem playerItem = playerItemList.Where(x => x.item.ID == itemID).FirstOrDefault();

        long sellCost = playerItem.price * playerItem.selected;
        if (playerItem != null)
        {
            gameDataObject.removeItem(playerItem.item.ID, playerItem.selected);

            removePlayerItemList(playerItem);
            addStoreItemList(playerItem);

            gameDataObject.addItem(GameConstants.MONEY_INDEX, sellCost);

            updateDisplay();
        }
     
    }

    //helper to add storeItems to local player storeitem list
    private void addPlayerItemList(StoreItem storeItem)
    {
        StoreItem matchingItem = playerItemList.Where(x => x.item.ID == storeItem.item.ID).FirstOrDefault();
        if (matchingItem != null)
        {
            matchingItem.count += storeItem.selected;
            matchingItem.selected = 1;
        }
        else
        {
            playerItemList.Add(copyStoreItem( storeItem));
        }
    }

    private void removePlayerItemList(StoreItem storeItem)
    {
        if (storeItem != null)
        {
            if (storeItem.count > storeItem.selected)
            {
                storeItem.count -= storeItem.selected;
                storeItem.selected = 1;
            }
            else
            {
                playerItemList.Remove(storeItem);
            }
        }

    }

    private void addStoreItemList(StoreItem storeItem)
    {
        StoreItem matchingItem = storeItemList.Where(x => x.item.ID == storeItem.item.ID).FirstOrDefault();
        if (matchingItem != null)
        {
            matchingItem.count += storeItem.selected;
            matchingItem.selected = 1;
        }
        else
        {
           
            storeItemList.Add(copyStoreItem(storeItem));
        }
    }

    private void removeStoreItemList(StoreItem storeItem)
    {
        if (storeItem != null)
        {
            if (storeItem.count > storeItem.selected)
            {
                storeItem.count -= storeItem.selected;
                storeItem.selected = 1;
            }
            else
            {
                storeItemList.Remove(storeItem);
            }
        }

    }

    private StoreItem copyStoreItem(StoreItem oldStoreItem)
    {
        StoreItem newStoreItem = new StoreItem()
        {
            item = ItemFactory.getItemFromIndex(oldStoreItem.item.ID, gameDataObject.gameDataSet),
            count = oldStoreItem.selected,
            price = oldStoreItem.price,
            selected = 1

        };
        return newStoreItem;
    }

    public void ItemSelectChange(bool isMore, bool isStore, long itemID )
    {
        StoreItem item = null;
        Text itemCountText = null;
        Text itemPriceText = null;
        if (isStore)
        {
            item = storeItemList.Where(x => x.item.ID == itemID).FirstOrDefault();
            var itemIndex = storeItemList.IndexOf(item);
            var itemObject = storeItemObjectList[itemIndex];
             itemCountText = UIHelper.getGameObjectWithName(itemObject, "ItemCount", typeof(Text)).GetComponent<Text>();
             itemPriceText = UIHelper.getGameObjectWithName(itemObject, "ItemPrice", typeof(Text)).GetComponent<Text>();
        }
        else
        {
            item = playerItemList.Where(x => x.item.ID == itemID).FirstOrDefault();
            var itemIndex = playerItemList.IndexOf(item);
            var itemObject = playerItemObjectList[itemIndex];
            itemCountText = UIHelper.getGameObjectWithName(itemObject, "ItemCount", typeof(Text)).GetComponent<Text>();
            itemPriceText = UIHelper.getGameObjectWithName(itemObject, "ItemPrice", typeof(Text)).GetComponent<Text>();
        }
        if (item != null)
        {
            if (isMore)
            {
                item.selected++;
                if (item.selected > item.count)
                {
                    item.selected = item.count;
                }
            }
            else
            {
                item.selected--;
                if (item.selected <= 0)
                {
                    item.selected = 1;
                }
            }

            if (itemCountText != null)
            {
                itemCountText.text = item.selected + "/" + item.count;
                long newPrice = item.selected * item.price;
                itemPriceText.text = newPrice.ToString();
                if (isStore && newPrice >= gameDataObject.playerGameCharacter.money)
                {
                    itemPriceText.color = Color.red;
                }
            }
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

        Button buyButton = UIHelper.getGameObjectWithName(storeItemObject, "ButtonBuy", typeof(Button)).GetComponent<Button>();

        if (isStore)
        {
            buyButton.onClick.AddListener(() => BuyItem(storeItem.item.ID));
            storeItemObject.transform.SetParent(buyPanel.transform, true);
        }
        else
        {
            buyButton.onClick.AddListener(() => SellItem(storeItem.item.ID));
            UIHelper.UpdateTextComponent(buyButton.gameObject, "Text", "Sell");
            storeItemObject.transform.SetParent(sellPanel.transform, true);
        }

        storeItem.selected = 1;
        if (storeItem.count > 1)
        {
             var itemCountText = UIHelper.getGameObjectWithName(storeItemObject, "ItemCount", typeof(Text)).GetComponent<Text>();
            itemCountText.text = storeItem.selected + "/" + storeItem.count;

            Button lessButton = UIHelper.getGameObjectWithName(storeItemObject, "ButtonCountLess", typeof(Button)).GetComponent<Button>();
            lessButton.onClick.AddListener(() => ItemSelectChange(false, isStore, storeItem.item.ID));
            Button moreButton = UIHelper.getGameObjectWithName(storeItemObject, "ButtonCountMore", typeof(Button)).GetComponent<Button>();
            moreButton.onClick.AddListener(() => ItemSelectChange(true, isStore, storeItem.item.ID));
        }

        return storeItemObject;

    }

    public void addItem()
    {
        updateDisplay();

    }

}
