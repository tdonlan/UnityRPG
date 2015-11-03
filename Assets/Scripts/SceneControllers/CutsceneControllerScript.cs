using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }


    public CutsceneTree cutsceneTree { get; set; }

    //UI Refs
    public Text cutsceneText;
    public Image bgImage;

	// Use this for initialization
	void Start () {
	
	}

    void OnLevelWasLoaded(int level)
    {
        loadGameData();
        LoadTreeStore();
        UpdateUI();
    }

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void LoadTreeStore()
    {
        cutsceneTree = (CutsceneTree)gameDataObject.treeStore.getCurrentTree();
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ClickNextNode();
         
        }
	}

    private void ClickNextNode()
    {
        CutsceneTreeNode curNode = (CutsceneTreeNode)cutsceneTree.getNode(cutsceneTree.currentIndex);
        if(curNode.branchList.Count > 0) //go to next node
        {
               long linkIndex = curNode.branchList[0].linkIndex;
               cutsceneTree.SelectNode(linkIndex);
               UpdateUI();
        }
        else //leave the cutscene
        {
            long linkindex = curNode.content.linkIndex;
            switch (curNode.content.nodeType)
            {
                case ZoneNodeType.Battle:
                    ClickBattleNode(linkindex);
                    break;
                case ZoneNodeType.Cutscene:
                    ClickCutsceneNode(linkindex);
                    break; 
                case ZoneNodeType.Link:
                    ClickLinkNode(linkindex);
                    break;
                default:
                    break;
            }
        }
    }

    private void ClickLinkNode(long linkIndex)
    {
        gameDataObject.treeStore.SelectTree(linkIndex);
        if (gameDataObject.treeStore.getCurrentTree() is WorldTree)
        {
            Application.LoadLevel((int)UnitySceneIndex.World);
        }
        else
        {
            Application.LoadLevel((int)UnitySceneIndex.Zone);
        }
    }

    private void ClickBattleNode(long battleIndex)
    {
        Application.LoadLevel((int)UnitySceneIndex.Battle);
    }

    private void ClickCutsceneNode(long index)
    {
        Application.LoadLevel((int)UnitySceneIndex.Cutscene);
    }


    private void UpdateUI()
    {
        CutsceneTreeNode curNode = (CutsceneTreeNode)cutsceneTree.getNode(cutsceneTree.currentIndex);
        cutsceneText.text = curNode.content.text;
        bgImage.sprite = gameDataObject.assetLibrary.getSprite(curNode.content.sheetName, curNode.content.spriteIndex);
    }
}
