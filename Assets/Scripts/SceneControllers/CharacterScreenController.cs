using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityRPG;

public class CharacterScreenController : MonoBehaviour {

    Camera mainCamera;

    public GameDataObject gameDataObject;

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

    public List<TalentTreeDisplayData> talentTreeDisplayDataList;

    //Prefab
    public GameObject hoverPopupPrefab;

    private GameObject hoverPopup;

	// Use this for initialization
	void Start () {

        loadGameData();
        InitPrefabs();

        UpdateUI();

	}

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
        talentTreeDisplayDataList = gameDataObject.getTalentTreeDisplayData();
    }

    private void InitPrefabs()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>().GetComponent<Camera>();
        hoverPopupPrefab = Resources.Load<GameObject>("Prefabs/HoverPopupPrefab");
    }

	// Update is called once per frame
	void Update () {
      
	}

    private void UpdateUI()
    {

        var player = gameDataObject.playerGameCharacter;

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

    public void HoverTalentTree(int index)
    {
        if (hoverPopup == null)
        {
            hoverPopup = Instantiate(hoverPopupPrefab);
            hoverPopup.transform.SetParent(gameObject.transform);
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
}
