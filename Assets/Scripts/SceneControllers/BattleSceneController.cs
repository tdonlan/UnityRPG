using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleSceneController : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }

    public GameObject WinBattlePopup;
    public RectTransform WinBattlePopupRectTransform;

    public long parentTreeLink;
    public BattleTree battleTree { get; set; }

    public Text battleInfoText;
    public Text battleWinText;



	// Use this for initialization
	void Start () {
	
	}
    void OnLevelWasLoaded(int level)
    {
        loadGameData();
        initScene();
    }

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void initScene()
    {
     
        LoadTreeStore();

        WinBattlePopupRectTransform = WinBattlePopup.GetComponent<RectTransform>();
       
    }

    private void LoadTreeStore()
    {
        //dont select tree, get the tree node from current zone content

        //assuming the parent is a zone type for now
        ZoneTree parentTree = (ZoneTree)gameDataObject.treeStore.getCurrentTree();
        ZoneTreeNode parentTreeNode = (ZoneTreeNode)parentTree.getNode(parentTree.currentIndex);
        long dialogLink = parentTreeNode.content.linkIndex;

        parentTreeLink = gameDataObject.treeStore.currentTreeIndex;

        gameDataObject.treeStore.SelectTree(dialogLink);
        battleTree = (BattleTree)gameDataObject.treeStore.getCurrentTree();

    }

    private void updateBattleInfo()
    {
        string battleInfo = "Battle Info - fill in";
        
    }

	
	// Update is called once per frame
	void Update () {
	
	}

    public void WinBattle()
    {
        //Select win node, run actions
        var winNode = battleTree.getWinNode();
        winNode.SelectNode(battleTree);

        gameDataObject.runActions(winNode.actionList);

            //Display the WinBattle Popup
        WinBattlePopupRectTransform.localPosition = new Vector3(0, 0, 0);
        
    }

    public void EndBattle()
    {
        //switch back to parent tree link
        gameDataObject.treeStore.SelectTree(parentTreeLink);

        //go back to the zone view
        Application.LoadLevel((int)UnitySceneIndex.Zone);
    }
}
