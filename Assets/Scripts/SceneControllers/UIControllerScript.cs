using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityRPG;

using UnityRPG;


public class UIControllerScript : MonoBehaviour {

    public GameObject gameControllerObject;
    public GameControllerScript gameController {get;set;}

    public Canvas FrontCanvas { get; set; }
    public GameObject InitiativePanel { get; set; }

    private GameObject InitPrefab {get;set;}

    private bool updated = false;

    

	// Use this for initialization
	void Start () {
        LoadGameController();
        LoadPrefabs();
       // LoadInitiative();
        
	}


    private void LoadGameController()
    {
        gameController = gameControllerObject.GetComponent<GameControllerScript>();
    }

    private void LoadPrefabs()
    {
        InitPrefab = Resources.Load<GameObject>("BattleInitiativePanelPrefab");
        InitiativePanel = GameObject.FindGameObjectWithTag("InitiativePanel");

    }

	// Update is called once per frame
	void Update () {
	    if(!updated)
        {
            
            LoadInitiative();
            updated = true;
        }
	}

    public void UpdateInitiativePanel()
    {
        
        //clear current Panel
        for (int i = InitiativePanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(InitiativePanel.transform.GetChild(i));
        }

        LoadInitiative();
    }

    private void LoadInitiative()
    {
        if (gameController.battleGame != null)
        {
            foreach (var character in gameController.battleGame.characterList)
            {
                GameObject charPortrait = (GameObject)Instantiate(InitPrefab);
                charPortrait = updateCharPortrait(charPortrait, character);

                charPortrait.transform.SetParent(InitiativePanel.transform, true);
            }
        }
       
    }

    private GameObject updateCharPortrait(GameObject charPortrait, GameCharacter character)
    {

        if(character == gameController.battleGame.ActiveCharacter)
        {
            var panelImg = charPortrait.GetComponent<Image>();
            panelImg.color = new Color(.8f, .8f, 0, .5f);

        }

        UpdateSpriteComponent(charPortrait, "PortraitImage", gameController.assetLibrary.getSprite("Portraits", 0));

        UpdateTextComponent(charPortrait, "CharacterName", character.name.ToString());
        UpdateTextComponent(charPortrait, "CharacterStats", string.Format("{0}/{1}",character.hp,character.totalHP));
       
        return charPortrait;
    }

    private void UpdateTextComponent(GameObject parent, string componentName,  string text)
    {
        foreach (var comp in parent.GetComponentsInChildren<Text>())
        {
            if(comp.name == componentName)
            {
                comp.text = text;
            }
        }
    }

    private void UpdateSpriteComponent(GameObject parent, string componentName, Sprite sprite)
    {
        foreach (var comp in parent.GetComponentsInChildren<Image>())
        {
            if(comp.name == componentName)
            {
                comp.sprite = sprite;
            }
        }
    }

    public void EndTurnAction()
    {
        gameController.PlayerEndTurn();
    }
   
}
