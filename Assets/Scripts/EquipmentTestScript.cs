using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleRPG2;
using System;
using UnityEngine.UI;
using Assets;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using System.Linq;

//For Reference https://github.com/AyARL/UnityGUIExamples/blob/master/EventTrigger/Assets/TriggerSetup.cs

public class EquipmentTestScript : MonoBehaviour {

    public AssetLibrary assetLibrary { get; set; } 

    public BattleGame battleGame { get; set; }

    List<GameObject> currentEquipList { get; set; }


    List<GameObject> displayEquipList { get; set; }

	// Use this for initialization
	void Start () {
        this.assetLibrary = new AssetLibrary();
        this.battleGame = new BattleGame();

        WireTestImage();

        LoadEquipList();
	}

    private void WireTestImage()
    {
        var testImage = GameObject.FindGameObjectWithTag("TestImage");
        var eventTrigger = testImage.AddComponent<EventTrigger>();
        eventTrigger.delegates = new List<EventTrigger.Entry>();


        System.Object o = (System.Object)"Hello World";

        AddEventTrigger(eventTrigger, OnPointerClick, EventTriggerType.PointerClick,o);
        AddEventTrigger(eventTrigger, HoverEnterLeftEquip, EventTriggerType.PointerEnter);
        AddEventTrigger(eventTrigger, HoverExitLeftEquip, EventTriggerType.PointerExit);
    }

    private void AddClickToGameObject(GameObject gameObject, UnityAction action, EventTriggerType triggerType)
    {
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        eventTrigger.delegates = new List<EventTrigger.Entry>();
        AddEventTrigger(eventTrigger, action, triggerType);
    }

    private void AddClickToGameObject(GameObject gameObject, UnityAction<System.Object> action, EventTriggerType triggerType, System.Object eventObject)
    {
           var eventTrigger = gameObject.AddComponent<EventTrigger>();
        eventTrigger.delegates = new List<EventTrigger.Entry>();
        AddEventTrigger(eventTrigger, action, triggerType, eventObject);
    }
  
    
    private void AddEventTrigger(EventTrigger eventTrigger, UnityAction action, EventTriggerType triggerType)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action()); // ignore event data

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        eventTrigger.delegates.Add(entry);
    }
 



    private void AddEventTrigger(EventTrigger eventTrigger, UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action(eventData)); // capture and pass the event data to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        eventTrigger.delegates.Add(entry);
    }

    private void AddEventTrigger(EventTrigger eventTrigger, UnityAction<System.Object> action, EventTriggerType triggerType, System.Object eventObj)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action(eventObj)); // pass additonal argument to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        eventTrigger.delegates.Add(entry);
    }

    private void OnPointerClick(System.Object eventObj)
    {
        //textField.text = "OnPointerClick " + data.selectedObject;
        Debug.Log("OnPointerClick ");
    }


	
	// Update is called once per frame
	void Update () {
	    
	}

    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    //load the list of display equipment, give a type
    private void LoadDisplayArmor(System.Object armorTypeObj)
    {
        ArmorType armorType = (ArmorType)armorTypeObj;

        displayEquipList = new List<GameObject>();
        GameObject equipPrefab = Resources.Load<GameObject>("EquipPrefab");

        GameObject rightEquipPanel = GameObject.FindGameObjectWithTag("EquipRightPanelContent");
        DestroyAllChildren(rightEquipPanel.transform);

        var armorList = from data in battleGame.ActiveCharacter.inventory
                        where data.type == ItemType.Armor
                        select data;

        var armorTypeList = (from Armor data in armorList
                             where data.armorType == armorType
                             select data).ToList();

        foreach (var a in armorTypeList)
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateArmorGameObject(tempObj, a);
            tempObj.transform.SetParent(rightEquipPanel.transform, true);

            displayEquipList.Add(tempObj);
        }


    }

    private void LoadDisplayWeapon()
    {

        displayEquipList = new List<GameObject>();

        GameObject equipPrefab = Resources.Load<GameObject>("EquipPrefab");

        GameObject rightEquipPanel = GameObject.FindGameObjectWithTag("EquipRightPanelContent");

        DestroyAllChildren(rightEquipPanel.transform);

        var weaponList = from data in battleGame.ActiveCharacter.inventory
                        where data.type == ItemType.Weapon
                        select data;

        foreach (var w in weaponList)
        {

            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateWeaponGameObject(tempObj, (Weapon)w);
            tempObj.transform.SetParent(rightEquipPanel.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    private void LoadDisplayAmmo()
    {
        displayEquipList = new List<GameObject>();
        GameObject equipPrefab = Resources.Load<GameObject>("EquipPrefab");

        GameObject rightEquipPanel = GameObject.FindGameObjectWithTag("EquipRightPanelContent");
        DestroyAllChildren(rightEquipPanel.transform);

        var ammoList = from data in battleGame.ActiveCharacter.inventory
                         where data.type == ItemType.Ammo
                         select data;

        foreach (var a in ammoList)
        {
            ItemSet ammoSet = ItemHelper.getItemSet(battleGame.ActiveCharacter.inventory,a);
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateAmmoGameObject(tempObj, ammoSet);
            tempObj.transform.SetParent(rightEquipPanel.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    //load the list of current equipment
    private void LoadEquipList()
    {

        currentEquipList = new List<GameObject>();

        GameObject equipPrefab = Resources.Load<GameObject>("EquipPrefab");

        GameObject leftEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");

        //weapon
        if(battleGame.ActiveCharacter.weapon != null)
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);

            updateWeaponGameObject(tempObj,battleGame.ActiveCharacter.weapon);


            AddClickToGameObject(tempObj, LoadDisplayWeapon, EventTriggerType.PointerClick);

            tempObj.transform.SetParent(leftEquipPanel.transform, true);

            currentEquipList.Add(tempObj);
        }
        else
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
        

            updateEmptyGameObject(tempObj,ItemType.Weapon.ToString());

            AddClickToGameObject(tempObj, LoadDisplayWeapon, EventTriggerType.PointerClick);

            tempObj.transform.SetParent(leftEquipPanel.transform, true);

            currentEquipList.Add(tempObj);
        }

        //ammo
        if(battleGame.ActiveCharacter.Ammo != null)
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateAmmoGameObject(tempObj, battleGame.ActiveCharacter.Ammo);
            tempObj.transform.SetParent(leftEquipPanel.transform, true);

            currentEquipList.Add(tempObj);
        }
        else
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateEmptyGameObject(tempObj, ItemType.Ammo.ToString());
            tempObj.transform.SetParent(leftEquipPanel.transform, true);

            currentEquipList.Add(tempObj);
        }

        foreach (var a in Enum.GetValues(typeof(ArmorType)))
        {
            Armor tempArmor = battleGame.ActiveCharacter.getArmorInSlot((ArmorType)a);

            if (tempArmor != null)
            {
                GameObject tempObj = (GameObject)Instantiate(equipPrefab);
                updateArmorGameObject(tempObj, tempArmor);

                AddClickToGameObject(tempObj, LoadDisplayArmor, EventTriggerType.PointerClick, (System.Object)a);

                tempObj.transform.SetParent(leftEquipPanel.transform, true);

                currentEquipList.Add(tempObj);
            }
            else
            {
                GameObject tempObj = (GameObject)Instantiate(equipPrefab);
                updateEmptyGameObject(tempObj, ((ArmorType)a).ToString());
                AddClickToGameObject(tempObj, LoadDisplayArmor, EventTriggerType.PointerClick, (System.Object)a);

                tempObj.transform.SetParent(leftEquipPanel.transform, true);

                currentEquipList.Add(tempObj);
            }
        }
    }

    private GameObject updateEmptyGameObject(GameObject obj, string type)
    {
        var equipTypePanel = getChildObject(obj, "EquipTypePanel");
        UpdateTextComponent(equipTypePanel, "EquipType", type);
        UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 43));

        UpdateTextComponent(obj, "EquipStats", "Empty");
        return obj;
    }

    private GameObject updateWeaponGameObject(GameObject obj, Weapon wep)
    {

        var equipTypePanel = getChildObject(obj, "EquipTypePanel");
        UpdateTextComponent(equipTypePanel, "EquipType", "Weapon");
        UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 43));

        UpdateTextComponent(obj, "EquipStats", wep.ToString());
        return obj;
    }

    private GameObject updateAmmoGameObject(GameObject obj, ItemSet ammo)
    {
        var equipTypePanel = getChildObject(obj, "EquipTypePanel");
        UpdateTextComponent(equipTypePanel, "EquipType", "Ammo");
        UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 43));

        UpdateTextComponent(obj, "EquipStats", ammo.ToString());
        return obj;
    }

    private GameObject updateArmorGameObject(GameObject obj, Armor armor)
    {
        var equipTypePanel = getChildObject(obj,"EquipTypePanel");
        UpdateTextComponent(equipTypePanel, "EquipType",armor.armorType.ToString());
        UpdateSpriteComponent(equipTypePanel,"EquipImage",assetLibrary.getSprite(SpritesheetType.Items,43));

        UpdateTextComponent(obj, "EquipStats", armor.ToString());


        return obj;
    }

    private GameObject getChildObject(GameObject parent, string name)
    {
        foreach(var comp in parent.GetComponentsInChildren<Transform>())
        {
            if(comp.name == name)
            {
                return comp.gameObject;
            }
        }
        return null;
    }


    private void UpdateTextComponent(GameObject parent, string componentName, string text)
    {
        foreach (var comp in parent.GetComponentsInChildren<Text>())
        {
            if (comp.name == componentName)
            {
                comp.text = text;
            }
        }
    }

    private void UpdateSpriteComponent(GameObject parent, string componentName, Sprite sprite)
    {
        foreach (var comp in parent.GetComponentsInChildren<Image>())
        {
            if (comp.name == componentName)
            {
                comp.sprite = sprite;
            }
        }
    }

    private void updateEventTrigger(GameObject parent)
    {

        var eventTrigger = parent.GetComponentInChildren<EventTrigger>();
    }

    public void HoverEnterLeftEquip()
    {
        var DebugText = GameObject.FindGameObjectWithTag("DebugText");
        var txtComp = DebugText.GetComponent<Text>();
        txtComp.text = "Entered Hover";
    }

    public void HoverExitLeftEquip()
    {

        var DebugText = GameObject.FindGameObjectWithTag("DebugText");
        var txtComp = DebugText.GetComponent<Text>();
        txtComp.text = "Left Hover";

    }

    public void SelectLeftEquip()
    {
        var DebugText = GameObject.FindGameObjectWithTag("DebugText");
        var txtComp = DebugText.GetComponent<Text>();
        txtComp.text = "Clicked Equip";
    }
}
