using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }

    public long parentTreeLink;
    public StoreTree storeTree { get; set; }

    public GameObject storeTextObject;
    public Text storeText;

    System.Random r;



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

    private void updateDisplay()
    {
        string storeDisplayText = "";
        var storeSellList = storeTree.getSellList(gameDataObject.gameDataSet, r);
        foreach (var item in storeSellList)
        {
            storeDisplayText += item.item.name + " $" + item.price + " ct:" + item.count + "\n";
        }
        storeText.text = storeDisplayText;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ExitStore()
    {
        //switch back to parent tree link
        gameDataObject.treeStore.SelectTree(parentTreeLink);

        //go back to the zone view
        Application.LoadLevel((int)UnitySceneIndex.Zone);
    }
}
