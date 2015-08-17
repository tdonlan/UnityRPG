using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityRPG;
using System.Collections.Generic;

public class PauseMenuScript : MonoBehaviour {

    public GameDataObject gameDataObject;
    public RectTransform panelRectTransform;
    public Text ZoneInfoText;
    public Text GlobalFlagsText;

    public GameObject InventoryTextGameObject;
    public Text InventoryText;

	// Use this for initialization
	void Start () {
        initRefs();
	}

    private void initRefs()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();

        ZoneInfoText = GameObject.FindGameObjectWithTag("PauseZoneInfo").GetComponent<Text>();
        GlobalFlagsText = GameObject.FindGameObjectWithTag("PauseGlobalFlagInfo").GetComponent<Text>();
        InventoryText = InventoryTextGameObject.GetComponent<Text>();

        panelRectTransform = gameObject.GetComponent<RectTransform>();

    }

	// Update is called once per frame
	void Update () {
        UpdateData();
	}

    public void UpdateData()
    {
        ZoneInfoText.text = getZoneInfo();
        //GlobalFlagsText.text = getGlobalFlags();
        GlobalFlagsText.text = getQuestView();
        InventoryText.text = getInventory();
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
        foreach (var item in gameDataObject.playerGameCharacter.inventory)
        {
            //Item item = gameDataObject.itemDictionary[l];
            inventoryStr += item.name + "\n";
        }
        return inventoryStr;
    }
}
