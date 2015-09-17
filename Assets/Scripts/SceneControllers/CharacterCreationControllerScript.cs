using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityRPG;

public class CharacterCreationControllerScript : MonoBehaviour {

    public GameDataObject gameDataObject { get; set; }

    private List<int> portraitIndexList;
    private int curPortraitIndex=0;
    private List<int> spriteIndexList;
    private int curSpriteIndex=0;
    private int maxStatPoints;
    private int usedStatPoints;

    private int str;
    private int end;
    private int agi;
    private int spi;

    //-------------

    public Text statPointText;
    public Text StrText;
    public Text EndText;
    public Text AgiText;
    public Text SpiText;

    public Image SpriteImage;
    public Image PortraitImage;

    public Text CharName;

	// Use this for initialization
	void Start () {

        loadGameData();

	    InitData();
        InitPrefabs();

        UpdateUI();
	}
    private void InitData()
    {
        portraitIndexList = new List<int>() { 0, 1, 2, 3, 7, 8, 17, 27 };
        spriteIndexList = new List<int>() { 0, 3, 4, 6, 22, 31 };
        maxStatPoints = 5;
        usedStatPoints = 0;
        str = 1;
        end = 1;
        agi = 1;
        spi = 1;
    }


    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void InitPrefabs()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void UpdateUI()
    {
        var spriteIndex = spriteIndexList[curSpriteIndex];
        SpriteImage.sprite =  gameDataObject.assetLibrary.getSprite("Characters", spriteIndex);


        var portraitIndex = portraitIndexList[curPortraitIndex];
        PortraitImage.sprite = gameDataObject.assetLibrary.getSprite("Portraits", portraitIndex);

        var curStatPoints = maxStatPoints - usedStatPoints;
        statPointText.text = curStatPoints + " pts";
        StrText.text = str.ToString();
        EndText.text = end.ToString();
        AgiText.text = agi.ToString();
        SpiText.text = spi.ToString();

    }

    public void PrevCharacterSprite()
    {
        curSpriteIndex--;
        if (curSpriteIndex < 0)
        {
            curSpriteIndex = spriteIndexList.Count - 1;
        }

        UpdateUI();
    }

    public void NextCharacterSprite() {
        curSpriteIndex++;
        if (curSpriteIndex >= spriteIndexList.Count)
        {
            curSpriteIndex = 0;
        }
        UpdateUI();
    }

    public void PrevCharacterPortrait()
    {
        curPortraitIndex--;
        if (curPortraitIndex < 0)
        {
            curPortraitIndex = portraitIndexList.Count - 1;
        }
        UpdateUI();
    }

    public void NextCharacterPortrait()
    {
        curPortraitIndex++;
        if (curPortraitIndex >= portraitIndexList.Count)
        {
            curPortraitIndex = 0;
        }
        UpdateUI();
    }

    /*
     * str - 0
     * end - 1
     * agi - 2
     * spi - 3
     */
    public void PlusStat(int index)
    {
        var curStatPoints = maxStatPoints - usedStatPoints;
        if (curStatPoints > 0)
        {
            switch (index)
            {
                case 0:
                    str++;
                    usedStatPoints++;
                    break;
                case 1:
                    end++;
                    usedStatPoints++;
                    break;
                case 2:
                    agi++;
                    usedStatPoints++;
                    break;
                case 3:
                    spi++;
                    usedStatPoints++;
                    break;
                default: break;
            }
            UpdateUI();
        }

    }

    public void MinusStat(int index)
    {
        var curStatPoints = maxStatPoints - usedStatPoints;
        if (curStatPoints <= maxStatPoints)
        {
            switch (index)
            {
                case 0:
                    if (str > 1)
                    {
                        str--;
                        usedStatPoints--;
                    }
                  
                    break;
                case 1:
                    if (end > 1)
                    {
                        end--;
                        usedStatPoints--;
                    }
                
                    break;
                case 2:
                    if (agi > 1)
                    {
                        agi--;
                        usedStatPoints--;
                    }
                    
                    break;
                case 3:
                    if (spi > 1)
                    {
                        spi--;
                        usedStatPoints--;
                    }
             
                    break;
                default: break;
            }
            UpdateUI();
        }
    }

    public void CreateCharacter()
    {
        //verify we have a name and used stat points
        if (CharName.text.Length > 0 && usedStatPoints == maxStatPoints)
        {
            GameCharacterData playerData = gameDataObject.gameDataSet.gameCharacterDataDictionary[80001]; //hardcoded to player data, store this somewhere?
            playerData.name = CharName.text;
            
            playerData.portraitSpriteIndex = portraitIndexList[curPortraitIndex];
            playerData.characterSpriteIndex = spriteIndexList[curSpriteIndex];
            playerData.strength = str;
            playerData.endurance = end;
            playerData.agility = agi;
            playerData.spirit = spi;

            //confirm?

            gameDataObject.playerGameCharacter = CharacterFactory.getGameCharacterFromGameCharacterData(playerData, gameDataObject.gameDataSet);

            Application.LoadLevel((int)UnitySceneIndex.World);

        }
    }
} 
