using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using UnityRPG;

public class TestScript : MonoBehaviour {

    public Canvas canvas { get; set; }
    public GameObject HoverStatsObject { get; set; }
    public bool isStatsDisplay { get; set; }

    GameObject HoverPrefab { get; set; }
    GameObject TextPopupPrefab { get; set; }

    GameObject characterPrefab { get; set; }

    AssetLibrary assetLibrary { get; set; }


    public Text scrollText;
    public ScrollRect scrollRect;

    int particleCounter = 0;

    string testText = "";

  

    private List<TempEffect> tempEffectList { get; set; }

	// Use this for initialization
	void Start () {
       


	}
	
	// Update is called once per frame
    void Update()
    {
        UpdateScrollText();
    }

    private void UpdateTempEffects(float delta)
    {
        for(int i=tempEffectList.Count-1;i>=0;i--)
        {
            tempEffectList[i].Update(delta);
            if(tempEffectList[i].isExpired)
            {
                Destroy(tempEffectList[i].gameObject);
                tempEffectList.RemoveAt(i);
            }
        }
    }


    private void AddEffect(TempEffectType type, float duration, Vector3 pos,Vector3 dest, GameObject gameObject)
    {
        TempEffect te = new TempEffect(type, gameObject, duration, pos,dest);
   
        tempEffectList.Add(te);
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


    
    private void UpdateTextPopup(GameObject textPopup, string text, Color c)
    {
        var textMesh = textPopup.GetComponentInChildren<TextMesh>();
        textMesh.text = text;
        textMesh.color = c;
        var meshRenderer = textPopup.GetComponentInChildren<MeshRenderer>();
        meshRenderer.sortingLayerName = "Foreground";
    }


    public void StartParticles()
    {

        GameObject particle = assetLibrary.getPrefabGameObject( assetLibrary.prefabList[particleCounter].prefabName);
        particleCounter++;
        if (particleCounter >= assetLibrary.prefabList.Count)
        {
            particleCounter = 0;
        }

        var rend = particle.GetComponentInChildren<Renderer>();
        rend.sortingLayerName = "Foreground";

        AddEffect(TempEffectType.Particle, 3, new Vector3(3, 3, -2), new Vector3(3, 3, -2), particle);

    }


    public void StartText()
    {
        var textObj = assetLibrary.getPrefabGameObject("TextPopup");
        UpdateTextPopup(textObj, "2", Color.green);
        AddEffect(TempEffectType.Text, 1, new Vector3(3, 3),new Vector3(3,4), textObj);
    }

    public void StartSprite()
    {

        StartTempSprite(new Vector3(3, 3,-2), "Particles", 48);
    }

    public void StartTempSprite(Vector3 pos, string spritesheetName, int spriteIndex)
    {
        var sprite = assetLibrary.getSprite(spritesheetName, spriteIndex);
        var spriteObj = assetLibrary.getPrefabGameObject("Sprite");

        GameObjectHelper.UpdateSprite(spriteObj, "Sprite", sprite);
        AddEffect(TempEffectType.Sprite, 1, pos, pos, spriteObj);
    }

    private void UpdateScrollText()
    {
        testText += "hello ";
        scrollText.text = testText;

        scrollRect.verticalNormalizedPosition = 0;
    }
}
