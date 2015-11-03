using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorldControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; } 

    public WorldTree worldTree { get; set; }

    public GameObject worldButtonPrefab { get; set; }

    public Canvas uiCanvas { get; set; }
    public GameObject navigationPanel { get; set; }
    public GameObject detailZoneName { get; set; }
    public GameObject detailZoneDescription { get; set; }
    public GameObject detailZoneButton { get; set; }

    private List<GameObject> worldNodeList = new List<GameObject>();

	// Use this for initialization
	void Start () {
       
	}

    void OnLevelWasLoaded(int level)
    {
        loadGameData();

        initScene();
    }

    private void initScene()
    {
        initPrefabs();
        loadTree();
        setNavigation();
    }



    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
        
    }

    private void loadTree()
    {

        //int worldIndex = 0; //need to retrieve this from the treeStroe
       // gameDataObject.treeStore.SelectTree(worldIndex);

        worldTree = (WorldTree)gameDataObject.treeStore.getCurrentTree();

    }

    private void initPrefabs()
    {
        uiCanvas = (Canvas)GameObject.FindObjectOfType<Canvas>();
        worldButtonPrefab = (GameObject)Resources.Load<GameObject>("PrefabUI/WorldButtonPrefab");

        navigationPanel = GameObject.FindGameObjectWithTag("WorldNavigationPanel");
        detailZoneName = GameObject.FindGameObjectWithTag("WorldDetailZoneName");
        detailZoneDescription = GameObject.FindGameObjectWithTag("WorldDetailZoneDescription");
        detailZoneButton = GameObject.FindGameObjectWithTag("WorldDetailZoneButton");
    }

    private void setNavigation()
    {
        clearNavigationNodes();

        WorldTreeNode currentNode = (WorldTreeNode)worldTree.getNode(worldTree.currentIndex);

        var worldNode = createNodeButton(currentNode);
        worldNodeList.Add(worldNode);

        foreach(var branch in currentNode.getBranchList(worldTree)){
            worldNode = createNodeButton((WorldTreeNode)worldTree.getNode(branch.linkIndex));
            worldNodeList.Add(worldNode);
        }
    }

    private void clearNavigationNodes()
    {
        foreach (var node in worldNodeList)
        {
            Destroy(node);

        }
        worldNodeList.Clear();
    }

    private GameObject createNodeButton(WorldTreeNode node)
    {
        GameObject worldNodeButton = (GameObject)Instantiate(worldButtonPrefab);
        worldNodeButton.transform.SetParent(uiCanvas.transform, false);

        int index = worldNodeButton.transform.GetSiblingIndex();
        worldNodeButton.transform.SetSiblingIndex(index - 3); //move before other menus in heirarchy

        var rect = worldNodeButton.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(node.content.x, node.content.y);
  
        var worldNodeButtonButt = worldNodeButton.GetComponent<Button>();
        worldNodeButtonButt.onClick.AddListener(() => ClickZoneDetail(node.index));

        return worldNodeButton;
    }

   


    //clicking an icon on the map
    public void ClickZoneDetail(long index)
    {
        worldTree.SelectNode(index);

        WorldTreeNode clickedNode = (WorldTreeNode)worldTree.getNode(index);

        var text = detailZoneName.GetComponent<Text>();
        text.text = clickedNode.content.zoneName;

        var detail = detailZoneDescription.GetComponent<Text>();
        detail.text = clickedNode.content.description;

        var butt = detailZoneButton.GetComponent<Button>();
        butt.onClick.AddListener(()=>ClickEnterZoneButton(clickedNode.content.linkIndex));

        setNavigation();

    }

    //entering the zone / switching scenes
    public void ClickEnterZoneButton(long linkIndex){
        //switch scenes to the zone index
        //Application.LoadLevel(2); //old zone scene
        gameDataObject.treeStore.SelectTree(linkIndex);
        Application.LoadLevel((int)UnitySceneIndex.Zone); //tiled zone scene
    }
	

	// Update is called once per frame
	void Update () {
	
	}
}
