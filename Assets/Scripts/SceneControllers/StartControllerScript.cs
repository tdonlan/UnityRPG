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
        //select the parent tree link:

        gameDataObject.treeStore.SelectTree(1);
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
}
