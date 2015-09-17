using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

using UnityRPG;
using UnityEngine.EventSystems;

public class CharacterScreenController : MonoBehaviour {

    Camera mainCamera;

    public GameObject CharacterScreen;
    public GameObject InfoScreen;
    public GameObject EquipmentScreen;


    public GameDataObject gameDataObject;

    public Text CharacterNameText;
    public Image CharacterPortraitImage;

    public Text LevelText;

    public Text StrengthStatText;
    public Text AgilityStatText;
    public Text EnduranceStatText;
    public Text SpiritStatText;

    public Slider HPSlider;
    public Slider XPSlider;

    public Text XPText;
    public Text HPText;

    public Text ACText;
    public Text APText;

    public Text StatPointText;
    public Text TalentPointText;

    public GameObject TalentTagPanel;
    public List<GameObject> TalentDisplayPanelList = new List<GameObject>(); //list of panels to place talent Icons
    public List<GameObject> TalentIconList = new List<GameObject>(); //list of talent Icons after instantiated

    private string selectedTag;
    public List<string> talentTagList = new List<string>();
    public List<TalentTreeDisplayData> talentTreeDisplayDataList;
    public List<TalentTreeDisplayData> sortedTalentTreeDisplayDataList;

    //Prefab
    public GameObject talentTreeIconPrefab;
    public GameObject hoverPopupPrefab;
    public GameObject talentTagPrefab;

    private GameObject hoverPopup;

	// Use this for initialization
	void Start () {

        loadGameData();
        loadTalentTreeData();
        InitPrefabs();
        initScreens();

        UpdateUI();
        UpdateTalentTags();
        UpdateTalentTree();
	}

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void loadTalentTreeData()
    {
        talentTreeDisplayDataList = gameDataObject.getTalentTreeDisplayData();
        talentTagList = talentTreeDisplayDataList.Select(x => x.tag).Distinct().ToList();
        if (selectedTag == null && talentTagList.Count > 0)
        {
                selectedTag = talentTagList[0];
        }
        
        sortedTalentTreeDisplayDataList = talentTreeDisplayDataList.Where(x => x.tag.Equals(selectedTag)).ToList();
    }

    private void InitPrefabs()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>().GetComponent<Camera>();
        hoverPopupPrefab = Resources.Load<GameObject>("PrefabUI/HoverPopupPrefab");
        talentTreeIconPrefab = Resources.Load<GameObject>("PrefabUI/TalentTreePrefab");
        talentTagPrefab = Resources.Load<GameObject>("PrefabUI/TalentTagPrefab");

        TalentDisplayPanelList.AddRange( GameObject.FindGameObjectsWithTag("TalentDisplayPanel"));
        TalentDisplayPanelList = TalentDisplayPanelList.OrderBy(x => x.name).ToList();

       
    }

    private void initScreens()
    {
        CharacterScreen = this.gameObject;
        InfoScreen = GameObject.FindGameObjectWithTag("PauseMenu");
        EquipmentScreen = GameObject.FindGameObjectWithTag("EquipPanel");
    }

    public void SelectTalentIcon(object talentTreeDataObject)
    {
        TalentTreeDisplayData tt = (TalentTreeDisplayData)talentTreeDataObject;
        if (tt.unlocked && !tt.owned)
        {
            if (gameDataObject.playerGameCharacter.talentPoints > 0)
            {
                var abilityData = gameDataObject.gameDataSet.abilityDataDictionary[tt.AbilityID];
                gameDataObject.playerGameCharacter.abilityList.Add(AbilityFactory.getAbilityFromAbilityData(abilityData, gameDataObject.gameDataSet.effectDataDictionary));
                gameDataObject.playerGameCharacter.talentPoints--;

                loadTalentTreeData();
                UpdateUI();
                UpdateTalentTree();
            }
        }
    }

    public void SelectTalentTag(string tagName)
    {
        selectedTag = tagName;
        sortTalentTreeDisplayList();
        UpdateTalentTree();
    }


    private void sortTalentTreeDisplayList()
    {
        if (selectedTag != null)
        {
            sortedTalentTreeDisplayDataList = talentTreeDisplayDataList.Where(x => x.tag.Equals(selectedTag)).ToList();
        }
    }

	// Update is called once per frame
	void Update () {
    
	}

    private void UpdateUI()
    {

        var player = gameDataObject.playerGameCharacter;

        CharacterNameText.text = player.name;
        CharacterPortraitImage.sprite = gameDataObject.assetLibrary.getSprite(player.portraitSpritesheetName, player.portraitSpriteIndex);

        LevelText.text = gameDataObject.playerGameCharacter.level.ToString();

        StrengthStatText.text = gameDataObject.playerGameCharacter.strength.ToString();
        AgilityStatText.text = gameDataObject.playerGameCharacter.agility.ToString();
        EnduranceStatText.text = gameDataObject.playerGameCharacter.endurance.ToString();
        SpiritStatText.text = gameDataObject.playerGameCharacter.spirit.ToString();

        XPText.text = gameDataObject.playerGameCharacter.xp.ToString();
        HPText.text = gameDataObject.playerGameCharacter.hp.ToString() + "/" + gameDataObject.playerGameCharacter.totalHP.ToString();

        ACText.text = gameDataObject.playerGameCharacter.ac.ToString();
        APText.text = gameDataObject.playerGameCharacter.ap.ToString();

        HPSlider.value = (float)player.hp / (float)player.totalHP;

        XPSlider.value = player.xpLevelPercent;

        StatPointText.text = player.statPoints.ToString();
        TalentPointText.text = player.talentPoints.ToString();

    }

    private void UpdateTalentTags()
    {
        foreach (string str in talentTagList)
        {

            string tempTag = str;
            
            GameObject talentTagButton = Instantiate(talentTagPrefab);
            UIHelper.UpdateTextComponent(talentTagButton, "Text", tempTag);
            
            Button button = (Button)talentTagButton.GetComponent<Button>();
            button.onClick.AddListener(() => SelectTalentTag(tempTag));

            talentTagButton.transform.SetParent(TalentTagPanel.transform,true);

            if (str.Equals(selectedTag))
            {
                button.image.color = Color.yellow;
            }
            else
            {
                button.image.color = Color.white;
            }
        }
    }

    private void UpdateTalentTree()
    {

        foreach (var icon in TalentIconList)
        {
            Destroy(icon);
        }
        TalentIconList.Clear();

        foreach (TalentTreeDisplayData tt in sortedTalentTreeDisplayDataList)
        {
            var talentTreeIcon = Instantiate(talentTreeIconPrefab);
            TalentIconList.Add(talentTreeIcon);
            UIHelper.UpdateTextComponent(talentTreeIcon, "Text", tt.AbilityName);
            UIHelper.UpdateSpriteComponent(talentTreeIcon, "Image", gameDataObject.assetLibrary.getSprite(tt.SpriteSheetName, tt.SpriteSheetIndex));


            UIHelper.AddClickToGameObject(talentTreeIcon, HoverTalentTree, EventTriggerType.PointerEnter, (object)tt);
            UIHelper.AddClickToGameObject(talentTreeIcon, RemoveHoverTalentTree, EventTriggerType.PointerExit);

            UIHelper.AddClickToGameObject(talentTreeIcon, SelectTalentIcon, EventTriggerType.PointerClick, (object)tt);

            var panelImage = talentTreeIcon.GetComponent<Image>();
            
            if(tt.owned){
                panelImage.color = Color.green;
            }
            else if (tt.unlocked)
            {
                panelImage.color = Color.blue;
            }
            else
            {
                panelImage.color = Color.gray;
            }

            if(tt.tier <=6 && tt.tier >= 1){
             
                talentTreeIcon.transform.SetParent(TalentDisplayPanelList[tt.tier-1].transform);
            }
            else if (tt.tier < 1)
            {
                talentTreeIcon.transform.SetParent(TalentDisplayPanelList[0].transform);
            }
            else
            {
                talentTreeIcon.transform.SetParent(TalentDisplayPanelList[5].transform);
            }
            
        }
    }


    public void addXP()
    {
        gameDataObject.playerGameCharacter.getXP(25);

        UpdateUI();
    }

    public void addStat(int statType)
    {
        var player = gameDataObject.playerGameCharacter;
        if (player.statPoints > 0)
        {
            switch ((StatType)statType)
            {
                case StatType.Strength:
                    gameDataObject.playerGameCharacter.strength++;
                    player.statPoints--;
                    break;
                case StatType.Agility:
                    gameDataObject.playerGameCharacter.agility++;
                    player.statPoints--;
                    break;
                case StatType.Endurance:
                    gameDataObject.playerGameCharacter.endurance++;
                    player.statPoints--;
                    break;
                case StatType.Spirit:
                    gameDataObject.playerGameCharacter.spirit++;
                    player.statPoints--;
                    break;
                default:
                    break;
            }
        }
        UpdateUI();
    }

    //Currently unused - need to keep track of spent points, only refund what we used for this level.
    public void minusStat(int statType)
    {
        switch ((StatType)statType)
        {
            case StatType.Strength:
                 gameDataObject.playerGameCharacter.strength--; 
                break;
            case StatType.Agility:
             gameDataObject.playerGameCharacter.agility--; 
                break;
            case StatType.Endurance:
              gameDataObject.playerGameCharacter.endurance--; 
                break;
            case StatType.Spirit:
              gameDataObject.playerGameCharacter.spirit--; 
                break;
            default:
                break;
        }
        UpdateUI();

    }

    public void HoverTalentTree(object talentTreeDataObject)
    {
        if (hoverPopup == null && talentTreeDataObject != null)
        {
            hoverPopup = Instantiate(hoverPopupPrefab);
            hoverPopup.transform.SetParent(gameObject.transform);

            TalentTreeDisplayData tt = (TalentTreeDisplayData)talentTreeDataObject;
            UIHelper.UpdateTextComponent(hoverPopup, "Name", tt.AbilityName);
            UIHelper.UpdateTextComponent(hoverPopup, "Text1", tt.getDescription());
            if (!tt.unlocked)
            {
                var text2 = hoverPopup.GetComponentsInChildren<Text>()[1];
                text2.color = Color.red;
                text2.text = tt.getRequirements();
               
            }
        }
        hoverPopup.transform.position = new Vector3(Input.mousePosition.x-.1f, Input.mousePosition.y-.1f, 0);
    }

    public void RemoveHoverTalentTree()
    {
        if (hoverPopup != null)
        {
            Destroy(hoverPopup);
            hoverPopup = null;
        }
    }

    public void ShowInfoScreen()
    {
        InfoScreen.transform.localPosition = new Vector3(0, 0, 0);
        CharacterScreen.transform.localPosition = new Vector3(10000, 10000, 0);
        EquipmentScreen.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void ShowEquipmentScreen()
    {
        EquipmentScreen.transform.localPosition = new Vector3(0, 0, 0);
        CharacterScreen.transform.localPosition = new Vector3(10000, 10000, 0);
        InfoScreen.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void ShowCharacterScreen()
    {
        CharacterScreen.transform.localPosition = new Vector3(0, 0, 0);
        EquipmentScreen.transform.localPosition = new Vector3(10000, 10000, 0);
        InfoScreen.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void CloseScreen()
    {
        gameObject.transform.localPosition = new Vector3(10000, 10000, 0);
    }
}
