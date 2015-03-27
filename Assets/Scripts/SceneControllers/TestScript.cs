using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class TestScript : MonoBehaviour {

    public Canvas canvas { get; set; }
    public GameObject HoverStatsObject { get; set; }
    public bool isStatsDisplay { get; set; }

    GameObject HoverPrefab { get; set; }
    GameObject TextPopupPrefab { get; set; }

    GameObject characterPrefab { get; set; }

    GameObject tempText { get; set; }
    public float tempTextTimer { get; set; }

	// Use this for initialization
	void Start () {
        HoverPrefab = Resources.Load<GameObject>("HoverPrefab");
        characterPrefab = Resources.Load<GameObject>("CharacterPrefab");
        TextPopupPrefab = Resources.Load<GameObject>("Prefab/TextPopupPrefab");

        isStatsDisplay = false;

        canvas = GameObject.FindObjectOfType<Canvas>();

        //LoadCharacter();

        //LoadPanel();

        //AddCharacterCollider();


	}
	
	// Update is called once per frame
	void Update () {
        tempTextTimer -= Time.deltaTime;
        if(tempTextTimer <=0)
        {
            Destroy(tempText);
        }
	}

    private void LoadPanel()
    {

        var testPanel = GameObject.FindGameObjectWithTag("TestPanel");
        var testPanelEventTrigger = testPanel.GetComponentInChildren<EventTrigger>();
        UIHelper.AddEventTrigger(testPanelEventTrigger, DisplayDebugText, EventTriggerType.PointerEnter);
    }

    private void LoadCharacter()
    {
        GameObject characterObject = (GameObject)Instantiate(characterPrefab);

        var characterPanel = UIHelper.getGameObjectWithName(characterObject, "PanelOverlay", typeof(RectTransform));

        //var characterEventTrigger = characterPanel.AddComponent<EventTrigger>();

        var characterEventTrigger = characterObject.GetComponentInChildren<EventTrigger>();
        UIHelper.AddEventTrigger(characterEventTrigger, DisplayDebugText, EventTriggerType.PointerEnter);
        


        characterObject.transform.position = new Vector3(2.5f, 2.5f, -1);
    }

    /*
    private void AddCharacterCollider()
    {
        var characterObject = GameObject.FindGameObjectWithTag("CharacterObject");
        var colliderComp = characterObject.GetComponentInChildren<BoxCollider2D>();
        
    }*/

    private void OnMouseOver()
    { }


    public void DisplayDebugText()
    {
        var debugText = GameObject.FindGameObjectWithTag("DebugText");
        var text = debugText.GetComponentInChildren<Text>();

        text.text = "Hovering " + DateTime.Now.ToString();
        
    }


    public void DisplayHoverStats()
    {
        if(!isStatsDisplay)
        {
            isStatsDisplay = true;
            HoverStatsObject = (GameObject)Instantiate(HoverPrefab);
            HoverStatsObject.transform.SetParent(canvas.transform,true);

            //UIHelper.MoveUIObject(HoverStatsObject, new Vector3(-200, -200,0));

            UIHelper.MoveUIObject(HoverStatsObject, getMouseViewportCoords() + new Vector3(0,100,0));
            //HoverStatsObject.transform.position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
            
        }
    }

    private Vector3 getMouseViewportCoords()
    {
           var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        var camera = mainCamera.GetComponent<Camera>();

        return camera.ScreenToViewportPoint(Input.mousePosition);
    }

    public void HideHoverStats()
    {
        Destroy(HoverStatsObject);
        isStatsDisplay = false;
    }


    public void PopupText()
    {
        tempTextTimer = 1f;
        tempText = (GameObject)Instantiate(TextPopupPrefab);
       UpdateTextPopup(tempText, new Vector3(2.5f, 2.5f, 0), "Popup Text!");


    }

    private void UpdateTextPopup(GameObject textPopup, Vector3 pos, string text)
    {
        textPopup.transform.position = pos;

        var textMesh = textPopup.GetComponentInChildren<TextMesh>();
        textMesh.text = text;


    }
}
