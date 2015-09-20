using UnityEngine;
using System.Collections;

public class PauseButtonScript : MonoBehaviour {

    public GameDataObject gameDataObject;
    public GameObject pauseMenuObject;

    public GameObject CharacterScreen;
    public CharacterScreenController characterScreenController;

    public GameObject EquipmentScreen;
    public EquipmentControllerScript equipmentScreenController;

    public RectTransform pauseMenuRectTransform;

	// Use this for initialization
	void Start () {
        loadGameData();
        initRefs();
	}


    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void initRefs()
    {
        pauseMenuObject = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenuRectTransform = pauseMenuObject.GetComponent<RectTransform>();

        CharacterScreen = GameObject.FindGameObjectWithTag("CharacterScreen");
        EquipmentScreen = GameObject.FindGameObjectWithTag("EquipPanel");

        equipmentScreenController = EquipmentScreen.GetComponent<EquipmentControllerScript>();
        characterScreenController = CharacterScreen.GetComponent<CharacterScreenController>();
    }

    public void DisplayEquipMenu()
    {
        equipmentScreenController.UpdateUI();
        EquipmentScreen.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DisplayCharacterMenu()
    {
        characterScreenController.UpdateUI();
        CharacterScreen.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DisplayMenu()
    {
        gameDataObject.isPaused = true;

        pauseMenuRectTransform.localPosition = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (!gameDataObject.isPaused)
        {
            UpdateInput();
        }
	}

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            gameDataObject.isPaused = true;
            DisplayEquipMenu();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
             gameDataObject.isPaused = true;
            DisplayCharacterMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameDataObject.isPaused = true;
            DisplayMenu();
        }
    }
}
