using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class DialogControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }

    public long parentTreeLink;
	public DialogTree dialogTree { get; set; }

    public GameObject speakerBox { get; set; }
    public GameObject speakerName { get; set; }
    public GameObject speakerPortrait { get; set; }
    public GameObject responsePanel { get; set; }

    public Text debugText { get; set; }

    public GameObject responseButtonPrefab { get; set; }

    private List<GameObject> responseButtonList = new List<GameObject>();

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
        loadPrefabs();
        LoadTreeStore();

        updateDisplay();
    }

    private void loadPrefabs()
    {
        responsePanel = GameObject.FindGameObjectWithTag("ResponsePanel");
        speakerBox = GameObject.FindGameObjectWithTag("SpeakerText");
        speakerName = GameObject.FindGameObjectWithTag("SpeakerName");
        speakerPortrait = GameObject.FindGameObjectWithTag("SpeakerPortrait");
        responseButtonPrefab = (GameObject)Resources.Load<GameObject>("Prefabs/ResponseButtonPrefab");

        debugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();

    }

    //get the link to the dialog from current zone node content
    //save the parent tree link from tree store
    //switch to the dialog tree
    private void LoadTreeStore()
    {
        //dont select tree, get the tree node from current zone content

        //assuming the parent is a zone type for now
        ZoneTree parentTree = (ZoneTree)gameDataObject.treeStore.getCurrentTree();
        ZoneTreeNode parentTreeNode = (ZoneTreeNode)parentTree.getNode(parentTree.currentIndex);
        long dialogLink = parentTreeNode.content.linkIndex;

        parentTreeLink = gameDataObject.treeStore.currentTreeIndex;

        gameDataObject.treeStore.SelectTree(dialogLink);
        dialogTree = (DialogTree)gameDataObject.treeStore.getCurrentTree();
    }


    public void ClickResponseButton(long linkIndex)
    {
        dialogTree.SelectNode(linkIndex);
        updateDisplay();
    }

    //called when the dialog first loads, or the user clicks a link
    private void updateDisplay()
    {
        DialogTreeNode currentNode = (DialogTreeNode)dialogTree.getNode(dialogTree.currentIndex);
        updateSpeakerBlock(currentNode);
        updateResponseBlock(currentNode);
        updateAction(currentNode);

    }

    private void updateSpeakerBlock(DialogTreeNode currentNode)
    {
        var speakerBoxText = speakerBox.GetComponent<Text>();
        speakerBoxText.text = currentNode.content.text;

        var speakerNameText = speakerName.GetComponent<Text>();
        speakerNameText.text = currentNode.content.speaker;

        var speakerPortraitImg = speakerPortrait.GetComponent<Image>();

        var speakerSprite = Resources.Load<Sprite>("Portraits/" + currentNode.content.portrait);

        speakerPortraitImg.sprite = speakerSprite;

    }

    private void updateAction(DialogTreeNode currentNode)
    {
        if (currentNode.actionList != null)
        {
            gameDataObject.runActions(currentNode.actionList);
        }
    }


    private void updateResponseBlock(DialogTreeNode currentNode)
    {
        clearResponsePanel();
        foreach (var branch in currentNode.getBranchList(dialogTree))
        {
           GameObject responseButton =  createResponseButton(branch);
            responseButtonList.Add(responseButton);
            
        }
        GameObject endDialogButton = createEndDialogButton();
        responseButtonList.Add(endDialogButton);
    }

    private GameObject createResponseButton(TreeBranch branch)
    {
        GameObject responseButton = Instantiate(responseButtonPrefab);

        var responseButtonText = (Text)responseButton.GetComponent<Text>();
        responseButtonText.text = branch.description;

        var button = (Button)responseButton.GetComponent<Button>();
        button.onClick.AddListener(() => ClickResponseButton(branch.linkIndex));

        responseButton.transform.SetParent(responsePanel.transform,false);
        return responseButton;
    }

    private GameObject createEndDialogButton()
    {
        GameObject responseButton = Instantiate(responseButtonPrefab);

        var responseButtonText = (Text)responseButton.GetComponent<Text>();
        responseButtonText.text = "End conversation";

        var button = (Button)responseButton.GetComponent<Button>();
        button.onClick.AddListener(() => EndDialog());
        responseButton.transform.SetParent(responsePanel.transform);
        return responseButton;
    }

    private void clearResponsePanel()
    {
        foreach(var rButton in responseButtonList)
        {
            Destroy(rButton);
        }
        responseButtonList.Clear();

    }

    public void EndDialog()
    {
        
        //reset dialog
        dialogTree.SelectNode(1);

        //switch back to parent tree link
        gameDataObject.treeStore.SelectTree(parentTreeLink);
        
        //go back to the zone view
        Application.LoadLevel((int)UnitySceneIndex.Zone);
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}
}
