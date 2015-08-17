using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using UnityRPG;
using UnityEngine.EventSystems;

public class ZoneControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }

    private GameObject pauseButtonPrefab;
    private GameObject pauseMenuPrefab;
    private RectTransform canvasRectTransform;

    public GameObject TreeInfoPanel;
    private RectTransform treeInfoPanelRectTransform;

    public GameObject tileMapPrefab;
    public GameObject tileMapObject;

    public GameObject tileSelectPrefab;
    public GameObject tileSelectSprite;

    public TileMapData tileMapData;

    public ZoneTree zoneTree { get; set; }
    private ZoneTreeNode currentNode;
    private long currentNodeIndex;


    public string debugTextString;
    public Text debugText;
    public Text panelText;
    public Button panelButton;

    public GameObject player;
    public PlayerControllerScript playerScript;

    public GameObject spritePrefab;

    public List<GameObject> objectSpriteList = new List<GameObject>();

    Camera mainCamera;

    Point mouseTilePoint;
    List<Point> movePath = new List<Point>();

    public float moveTimer;
    public float moveTime = .05f;



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
        initPrefabs();
        loadTree();
        loadTileMap();
        
        loadTileMapData();
        loadTileSprites();
        setPlayerStart();
    }

    private void loadTree()
    {
        zoneTree = (ZoneTree)gameDataObject.treeStore.getCurrentTree();
    }

    private void initPrefabs()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>().GetComponent<Camera>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControllerScript>();
        debugText = GameObject.FindGameObjectWithTag("debugText").GetComponent<Text>();
        panelText = GameObject.FindGameObjectWithTag("PanelText").GetComponent<Text>();
        panelButton = GameObject.FindGameObjectWithTag("PanelButton").GetComponent<Button>();
        
        tileSelectPrefab = Resources.Load<GameObject>("Prefabs/TileSelectPrefab");

        spritePrefab = Resources.Load<GameObject>("Prefabs/SpritePrefab");

        pauseButtonPrefab = Resources.Load<GameObject>("Prefabs/PauseButtonPrefab");
        pauseMenuPrefab = Resources.Load<GameObject>("Prefabs/PauseMenuPrefab");
        canvasRectTransform = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();

        treeInfoPanelRectTransform = TreeInfoPanel.GetComponent<RectTransform>();
    }

    private void loadTileMap()
    {
        //get name of prefab of this map - match same name of tree?
        tileMapPrefab = Resources.Load<GameObject>(zoneTree.treeName);
        tileMapObject = (GameObject)Instantiate(tileMapPrefab);
        tileMapObject.tag = "tileMap";
    }

    private void loadTileMapData()
    {
        tileMapData = new TileMapData(tileMapObject);
  
    }

    private void loadTileSprites()
    {
        //clear all the object sprites
        foreach (var sprite in objectSpriteList)
        {
            Destroy(sprite);
        }
        objectSpriteList.Clear();

        //only load sprites that match zone flags
        for (int i = 0; i < tileMapData.objectBounds.Count; i++)
        {
            ZoneTreeNode node = (ZoneTreeNode)zoneTree.getNodeCheckingRootBranchList(i+1);
            if (node != null)
            {
                objectSpriteList.Add(loadTileSprite(node.content.icon, tileMapData.objectBounds[i].center));
            }
        }
    }

    private GameObject loadTileSprite(string spriteName, Vector3 pos)
    {
        GameObject spriteObject = null;
        var spriteResource = Resources.Load<Sprite>("ZoneImage/"+spriteName);
        if(spriteResource != null){
               spriteObject = Instantiate(spritePrefab);
              spriteObject.transform.position = pos;
            var spriteObjectSprite = spriteObject.GetComponent<SpriteRenderer>();
            spriteObjectSprite.sprite = spriteResource;
        }
        return spriteObject;

    }

    private void setPlayerStart()
    {
        player.transform.position = tileMapData.getSpawnPoint((int)zoneTree.currentIndex-1).center;
    }
    
	
	// Update is called once per frame
	void Update () {
	    //check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                UpdateMouseClick();
            }
          
        }
        UpdateMove();

	}

    private void UpdateMove()
    {
        if (movePath.Count > 0)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                Vector3 newPos = getWorldPosFromTilePoint(new Point(movePath[0].x, -movePath[0].y));
                playerScript.Move(newPos);
                movePath.RemoveAt(0);
                moveTimer = moveTime;
            }
        }
    }

    private void UpdateMouseClick()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePos);
        mouseTilePoint = getTileLocationFromVectorPos(mouseWorldPosition);
        if (mouseTilePoint != null)
        {
            Bounds mouseTileBounds = getTileBounds(mouseTilePoint.x, mouseTilePoint.y);
            AddTileSelectSprite(getWorldPosFromTilePoint(mouseTilePoint));

            if (!(tileMapData.checkCollision(mouseTileBounds)))
            {
              
                Point playerPointPos = getTileLocationFromVectorPos(player.transform.position);
                movePath = tileMapData.getPath(playerPointPos.x , -playerPointPos.y , mouseTilePoint.x, -mouseTilePoint.y);
            }
        }
    }

    private void AddTileSelectSprite(Vector3 pos)
    {
        Destroy(tileSelectSprite);
        tileSelectSprite = Instantiate(tileSelectPrefab);
        tileSelectSprite.transform.position = new Vector3(pos.x,pos.y,-5);
        setDebugText(pos.ToString());
    }

    private Bounds getTileBounds(int x, int y)
    {
        Vector3 center = new UnityEngine.Vector3(x,y,0);
        Vector3 size = new UnityEngine.Vector3(Tile.TILE_SIZE,Tile.TILE_SIZE);
        Bounds b = new UnityEngine.Bounds(center, size);
        return b;
    }

    private Point getTileLocationFromVectorPos(Vector3 pos)
    {

        int x = Mathf.RoundToInt(pos.x / Tile.TILE_SIZE);
        int y = Mathf.RoundToInt(pos.y / Tile.TILE_SIZE);
        
        Point retval = null;

        if (x >= 0 && x <= tileMapData.zoneTileArray.GetLength(0) && y <= 0 && y >= -tileMapData.zoneTileArray.GetLength(1))
        {
            retval = new Point() { x = (int)x, y = (int)y };
        }
        return retval;
    }

    private Vector3 getWorldPosFromTilePoint(Point p)
    {
        return new Vector3(p.x * Tile.TILE_SIZE, p.y * Tile.TILE_SIZE, 0);
    }
    

    public void setDebugText(string text)
    {
        this.debugText.text = text;
    }


    public void checkPlayerObjectCollision(Bounds playerBounds)
    {
        if (tileMapData != null)
        {
            currentNodeIndex = tileMapData.checkObjectCollision(playerBounds) + 1;
            if (currentNodeIndex > 0)
            {
                currentNode = (ZoneTreeNode)zoneTree.getNodeCheckingRootBranchList(currentNodeIndex);
                if (currentNode != null)
                {
                    //Enter battles automatically
                    if (currentNode.content.nodeType == ZoneNodeType.Battle)
                    {
                        ZoneNodeButtonClick();
                    }
  
                    panelText.text = currentNode.content.nodeName + " " + currentNode.content.description;
                    panelButton.enabled = true;
                }
                else
                {
                    panelText.text = "";
                    panelButton.enabled = false;
                }
            }
            else
            {
                panelText.text = "";
                panelButton.enabled = false;
            }

        }
       
    }

    
    public void ZoneNodeButtonClick()
    {
        zoneTree.SelectNode(currentNodeIndex);
        if (currentNode != null)
        {

            switch (currentNode.content.nodeType)
            {
                case ZoneNodeType.Link:
                    ClickLinkNode(currentNode.content.linkIndex);
                    break;
                case ZoneNodeType.Dialog:
                    ClickDialogNode(currentNode.content.linkIndex);
                    break;
                case ZoneNodeType.Battle:
                    ClickBattleNode(currentNode.content.linkIndex);
                    break;
                case ZoneNodeType.Info:
                    ClickInfoNode(currentNode.content.linkIndex);
                    break;
                default:
                    break;

            }
        }

    }

    private void ClickInfoNode(long linkIndex)
    {
        //update the TreeInfoPanel
        InfoTree curInfoTree = (InfoTree)gameDataObject.treeStore.getTree(linkIndex);

        TreeInfoControllerScript treeInfoScript = TreeInfoPanel.GetComponent<TreeInfoControllerScript>();
        treeInfoScript.UpdateInfo(gameDataObject, curInfoTree);


        treeInfoPanelRectTransform.localPosition = new UnityEngine.Vector3(0, 0, 0);

        loadTileSprites(); //update object sprites
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

    private void ClickDialogNode(long dialogIndex)
    {
        Application.LoadLevel((int)UnitySceneIndex.Dialog);
    }

    private void ClickBattleNode(long battleIndex)
    {
        Application.LoadLevel((int)UnitySceneIndex.Battle);
    }



}
