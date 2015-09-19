using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityRPG;
using System.Collections.Generic;

public class PauseMenuScript : MonoBehaviour {


    public GameObject CharacterScreen;
    public CharacterScreenController characterScreenController;
    public GameObject InfoScreen;

    public GameObject EquipmentScreen;
    public EquipmentControllerScript equipmentScreenController;

    public GameDataObject gameDataObject;
    public RectTransform panelRectTransform;
    public Text ZoneInfoText;
    public Text GlobalFlagsText;

    public GameObject InventoryTextGameObject;
    public Text InventoryText;

    //XP Stuff
    public Text TextXP;
    public Text TextLevel;
    public Text TextLevelCounter;
    
	// Use this for initialization
	void Start () {
        initRefs();
        initScreens();
	}

    private void initRefs()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();

        ZoneInfoText = GameObject.FindGameObjectWithTag("PauseZoneInfo").GetComponent<Text>();
        GlobalFlagsText = GameObject.FindGameObjectWithTag("PauseGlobalFlagInfo").GetComponent<Text>();
        InventoryText = InventoryTextGameObject.GetComponent<Text>();

        panelRectTransform = gameObject.GetComponent<RectTransform>();

    }


    private void initScreens()
    {
        CharacterScreen = GameObject.FindGameObjectWithTag("CharacterScreen");
        InfoScreen = this.gameObject;
        EquipmentScreen = GameObject.FindGameObjectWithTag("EquipPanel");


        equipmentScreenController = EquipmentScreen.GetComponent<EquipmentControllerScript>();
        characterScreenController = CharacterScreen.GetComponent<CharacterScreenController>();
    }

	// Update is called once per frame
	void Update () {
        UpdateData();
	}

    public void UpdateData()
    {
        ZoneInfoText.text = getZoneInfo();
        GlobalFlagsText.text = getQuestView();
        InventoryText.text = getInventory();
    }

    private void UpdateXP()
    {
        TextLevel.text = gameDataObject.playerGameCharacter.level.ToString();
        TextXP.text = gameDataObject.playerGameCharacter.xp.ToString() + " / " + gameDataObject.playerGameCharacter.xpToLevel.ToString() ;
    }

    public void CloseMenu()
    {
        panelRectTransform.localPosition = new Vector3(0, 1000, 0);
    }

    private string getZoneInfo()
    {
        string zoneInfo = "Zone Info: ";
        zoneInfo += gameDataObject.treeStore.getCurrentTree().treeName;
        zoneInfo += gameDataObject.treeStore.getCurrentTree().treeType.ToString();
        return zoneInfo;
    }

    private string getGlobalFlags()
    {
        string gfString = "";
        foreach (var flag in gameDataObject.treeStore.globalFlags.globalFlagList)
        {
            gfString += string.Format("{0} : {1} ({2})", flag.name, flag.value, flag.flagType.ToString());
        }
        return gfString;
    }

    private string getQuestView()
    {
        //get quests that are flagged active (we have the flag matching the name of quest tree)
        string output = "";
            

        List<List<string>> questStrLists = gameDataObject.treeStore.getQuestStringLists();

        if (questStrLists.Count > 0)
        {
            foreach(var str in questStrLists[0]){
                output+= str + "\n";
            }
        }

        return output;

    }

    private string getInventory()
    {
        string inventoryStr = "";
        inventoryStr += "Money: " + gameDataObject.playerGameCharacter.money.ToString() + "\n"; 
        foreach (var item in gameDataObject.playerGameCharacter.inventory)
        {
            //Item item = gameDataObject.itemDictionary[l];
            inventoryStr += item.name + "\n";
        }
        return inventoryStr;
    }

    public void getXP()
    {
        gameDataObject.playerGameCharacter.getXP(25);

        UpdateXP();
    }

    public void ShowInfoScreen()
    {
        InfoScreen.transform.localPosition = new Vector3(0, 0, 0);
        CharacterScreen.transform.localPosition = new Vector3(10000, 10000, 0);
        EquipmentScreen.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void ShowEquipmentScreen()
    {
        equipmentScreenController.UpdateUI();

        EquipmentScreen.transform.localPosition = new Vector3(0, 0, 0);
        CharacterScreen.transform.localPosition = new Vector3(10000, 10000, 0);
        InfoScreen.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void ShowCharacterScreen()
    {

        characterScreenController.UpdateUI();

        CharacterScreen.transform.localPosition = new Vector3(0, 0, 0);
        EquipmentScreen.transform.localPosition = new Vector3(10000, 10000, 0);
        InfoScreen.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void CloseScreen()
    {
        gameObject.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void SaveGame()
    {
        SaveGameLoader.SaveGame(gameDataObject.getSaveGameData(),"Save1");
        Debug.Log("Saved to Save1");
    }
}
