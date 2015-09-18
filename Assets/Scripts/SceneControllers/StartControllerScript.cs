using UnityEngine;
using System.Collections;

public class StartControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }
    public TreeStore treeStore { get; set; }

    public string testString = "HelloWorld";

	// Use this for initialization
	void Start () {
        loadRefs();
	}

    private void loadRefs()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    public void LoadBattle(int battleIndex)
    {
        int treeIndex = 1;
        int nodeIndex = 1;
        //select the parent tree link:
        switch (battleIndex)
        {
            case 1:
                nodeIndex = 3;
                break;
            case 2:
                treeIndex = 3;
                nodeIndex = 3;
                break;
            case 3:
                treeIndex = 4;
                nodeIndex = 2;
                break;
            default:
                break;
        }

        gameDataObject.treeStore.SelectTree(treeIndex);
        ZoneTree curTree = (ZoneTree)gameDataObject.treeStore.getTree(gameDataObject.treeStore.currentTreeIndex);
        curTree.SelectNode(nodeIndex);
        Application.LoadLevel((int)UnitySceneIndex.Battle);
    }

    public void EnterWorld()
    {
        Application.LoadLevel((int)UnitySceneIndex.World);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Testing
    public void LoadCharacterScreen()
    {
        Application.LoadLevel((int)UnitySceneIndex.CharacterScreen);
    }

    public void CreateCharacter()
    {
        Application.LoadLevel((int)UnitySceneIndex.CharacterCreationScreen);
    }

    public void LoadGame()
    {
        SaveGameData loadedGameData = SaveGameLoader.LoadGame("Save1");
        this.gameDataObject.playerGameCharacter = loadedGameData.playerGameCharacter;
        this.gameDataObject.partyList = loadedGameData.partyList;
        this.gameDataObject.treeStore.globalFlags = loadedGameData.globalFlags;
        this.gameDataObject.treeStore.SelectTree(loadedGameData.treeLink);

        ITree curTree = this.gameDataObject.treeStore.getCurrentTree();
        switch (curTree.treeType)
        {
            case TreeType.World:
                Application.LoadLevel((int)UnitySceneIndex.World);
                break;
            case TreeType.Zone:
                Application.LoadLevel((int)UnitySceneIndex.Zone);
                break;
        }
    }
}
