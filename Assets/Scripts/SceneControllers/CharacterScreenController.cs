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
    public PauseMenuScript pauseScreenController;

    public GameObject EquipmentScreen;
    public EquipmentControllerScript equipmentScreenController;


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

    private GameCharacter curGameCharacter;

	// Use this for initialization
	void Start () {

        loadGameData();
        InitPrefabs();
        initScreens();

        curGameCharacter = gameDataObject.playerGameCharacter;

        UpdateUI();
        UpdateTalentTags();
	}

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void loadTalentTreeData()
    {
        talentTreeDisplayDataList = gameDataObject.getTalentTreeDisplayData(curGameCharacter);
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

        equipmentScreenController = EquipmentScreen.GetComponent<EquipmentControllerScript>();
        pauseScreenController = InfoScreen.GetComponent<PauseMenuScript>();
    }

    public void SelectTalentIcon(object talentTreeDataObject)
    {
        TalentTreeDisplayData tt = (TalentTreeDisplayData)talentTreeDataObject;
        if (tt.unlocked && !tt.owned)
        {
            if (curGameCharacter.talentPoints > 0)
            {
                var abilityData = gameDataObject.gameDataSet.abilityDataDictionary[tt.AbilityID];
                curGameCharacter.abilityList.Add(AbilityFactory.getAbilityFromAbilityData(abilityData, gameDataObject.gameDataSet.effectDataDictionary));
                curGameCharacter.talentPoints--;

                UpdateUI();
            }
        }
    }

    public void SelectTalentTag(string tagName)
    {
        selectedTag = tagName;
        sortTalentTreeDisplayList();
        UpdateUI();
        //UpdateTalentTags();
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
        if (gameDataObject.isPaused)
        {
            UpdateInput();
        }
	}

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowEquipmentScreen();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //CloseScreen();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
           // CloseScreen();
        }
    }

    public void UpdateUI()
    {

        curGameCharacter = gameDataObject.getSelectedCharacter();
        if (curGameCharacter == null)
        {
            curGameCharacter = gameDataObject.playerGameCharacter;
        }

        loadTalentTreeData();
        UpdateTalentTree();

        CharacterNameText.text = curGameCharacter.name;
        CharacterPortraitImage.sprite = gameDataObject.assetLibrary.getSprite(curGameCharacter.portraitSpritesheetName, curGameCharacter.portraitSpriteIndex);

        LevelText.text = curGameCharacter.level.ToString();

        StrengthStatText.text = curGameCharacter.strength.ToString();
        AgilityStatText.text = curGameCharacter.agility.ToString();
        EnduranceStatText.text = curGameCharacter.endurance.ToString();
        SpiritStatText.text = curGameCharacter.spirit.ToString();

        XPText.text = curGameCharacter.xp.ToString();
        HPText.text = curGameCharacter.hp.ToString() + "/" + curGameCharacter.totalHP.ToString();

        ACText.text = curGameCharacter.ac.ToString();
        APText.text = curGameCharacter.ap.ToString();

        HPSlider.value = (float)curGameCharacter.hp / (float)curGameCharacter.totalHP;

        XPSlider.value = curGameCharacter.xpLevelPercent;

        StatPointText.text = curGameCharacter.statPoints.ToString();
        TalentPointText.text = curGameCharacter.talentPoints.ToString();

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
        curGameCharacter.getXP(25);

        UpdateUI();
    }

    public void addStat(int statType)
    {

        if (curGameCharacter.statPoints > 0)
        {
            switch ((StatType)statType)
            {
                case StatType.Strength:
                    curGameCharacter.strength++;
                    curGameCharacter.statPoints--;
                    break;
                case StatType.Agility:
                    curGameCharacter.agility++;
                    curGameCharacter.statPoints--;
                    break;
                case StatType.Endurance:
                    curGameCharacter.endurance++;
                    curGameCharacter.statPoints--;
                    break;
                case StatType.Spirit:
                    curGameCharacter.spirit++;
                    curGameCharacter.statPoints--;
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
                curGameCharacter.strength--; 
                break;
            case StatType.Agility:
                curGameCharacter.agility--; 
                break;
            case StatType.Endurance:
                curGameCharacter.endurance--; 
                break;
            case StatType.Spirit:
                curGameCharacter.spirit--; 
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
                var text2 = hoverPopup.GetComponentsInChildren<Text>()[2];
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
        equipmentScreenController.UpdateUI();
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
        gameDataObject.isPaused = false;
        gameObject.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void NextGameCharacter()
    {
        if (gameDataObject.partyList.Count > 0)
        {
            if (curGameCharacter.Equals(gameDataObject.playerGameCharacter))
            {
                curGameCharacter = gameDataObject.partyList[0];
            }
            else
            {
                var partyIndex = gameDataObject.partyList.IndexOf(curGameCharacter);
                partyIndex++;
                if (partyIndex >= gameDataObject.partyList.Count) {
                    curGameCharacter = gameDataObject.playerGameCharacter;
                
                }
                else
                {
                    curGameCharacter = gameDataObject.partyList[partyIndex];
                }
            }
            gameDataObject.SelectCharacter(curGameCharacter);
            UpdateUI();
        }
      
    }
}
