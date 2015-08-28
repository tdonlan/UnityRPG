using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterScreenController : MonoBehaviour {

    public GameDataObject gameDataObject;

    public Text LevelText;

    public Text StrengthStatText;
    public Text AgilityStatText;
    public Text EnduranceStatText;
    public Text SpiritStatText;

    public Text XPText;
    public Text HPText;

    public Text ACText;
    public Text APText;

	// Use this for initialization
	void Start () {

        loadGameData();
        UpdateUI();

	}

    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }
	
	// Update is called once per frame
	void Update () {
      
	}

    private void UpdateUI()
    {

        LevelText.text = gameDataObject.playerGameCharacter.level.ToString();

        StrengthStatText.text = gameDataObject.playerGameCharacter.strength.ToString();
        AgilityStatText.text = gameDataObject.playerGameCharacter.agility.ToString();
        EnduranceStatText.text = gameDataObject.playerGameCharacter.endurance.ToString();
        SpiritStatText.text = gameDataObject.playerGameCharacter.spirit.ToString();

        XPText.text = gameDataObject.playerGameCharacter.xp.ToString();
        HPText.text = gameDataObject.playerGameCharacter.hp.ToString() + "/" + gameDataObject.playerGameCharacter.totalHP.ToString();

        ACText.text = gameDataObject.playerGameCharacter.ac.ToString();
        APText.text = gameDataObject.playerGameCharacter.ap.ToString();

    }

    public void addXP()
    {
        gameDataObject.playerGameCharacter.getXP(25);

        UpdateUI();
    }

    
}
