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

        LoadCharacterStats();

	}

    private void LoadCharacterStats()
    {
        var charPanel = GameObject.FindGameObjectWithTag("CharacterPanel");
        UpdateTextComponent(charPanel, "CharNameText", battleGame.ActiveCharacter.name);
        UpdateTextComponent(charPanel, "CharStats", battleGame.ActiveCharacter.ToString());
    
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

    //called to clear out current equiped if nothing in slot
    public void ClearCurrentEquip()
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");
        UpdateTextComponent(currentEquipPanel, "EquipName", "");
        UpdateTextComponent(currentEquipPanel, "EquipType", "");
        UpdateTextComponent(currentEquipPanel, "EquipStats", "");
        UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 0));
    }

    public void LoadCurrentWeapon()
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");
        if (battleGame.ActiveCharacter.weapon != null)
        {
            UpdateTextComponent(currentEquipPanel, "EquipName", battleGame.ActiveCharacter.weapon.name);
            UpdateTextComponent(currentEquipPanel, "EquipType", battleGame.ActiveCharacter.weapon.weaponType.ToString());
            UpdateTextComponent(currentEquipPanel, "EquipStats", battleGame.ActiveCharacter.weapon.ToString());
            UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 3));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadCurrentAmmo()
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");
        if (battleGame.ActiveCharacter.Ammo != null)
        {

            var item = battleGame.ActiveCharacter.getInventoryItembyItemID(battleGame.ActiveCharacter.Ammo.itemID);
            var itemAmmo = (Ammo)item;
            UpdateTextComponent(currentEquipPanel, "EquipName", item.name);
            UpdateTextComponent(currentEquipPanel, "EquipType", itemAmmo.ammoType.ToString());
            UpdateTextComponent(currentEquipPanel, "EquipStats", itemAmmo.ToString());
            UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 3));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadCurrentArmor(ArmorType armorType)
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");

        var armor = battleGame.ActiveCharacter.getArmorInSlot(armorType);
        if (armor != null)
        {
            UpdateTextComponent(currentEquipPanel, "EquipName", armor.name);
            UpdateTextComponent(currentEquipPanel, "EquipType", armorType.ToString());
            UpdateTextComponent(currentEquipPanel, "EquipStats", armorType.ToString());
            UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(SpritesheetType.Items, 3));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadDisplayArmor(int armorType)
    {
       
        LoadDisplayArmor((System.Object)armorType);
    }

    //load the list of display equipment, give a type
    public void LoadDisplayArmor(System.Object armorTypeObj)
    {
      
        ArmorType armorType = (ArmorType)armorTypeObj;

        LoadCurrentArmor((ArmorType)armorType);

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
            AddClickToGameObject(tempObj, SelectArmor, EventTriggerType.PointerClick, a);
            displayEquipList.Add(tempObj);
        }


    }

    public void LoadDisplayWeapon()
    {
        LoadCurrentWeapon();

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
            AddClickToGameObject(tempObj, SelectWeapon, EventTriggerType.PointerClick,w);
            
            tempObj.transform.SetParent(rightEquipPanel.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    public void LoadDisplayAmmo()
    {
        LoadCurrentAmmo();

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
            AddClickToGameObject(tempObj, SelectAmmo, EventTriggerType.PointerClick, a);
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

        DestroyAllChildren(leftEquipPanel.transform);

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
            AddClickToGameObject(tempObj, LoadDisplayAmmo, EventTriggerType.PointerClick);
            tempObj.transform.SetParent(leftEquipPanel.transform, true);

            currentEquipList.Add(tempObj);
        }
        else
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateEmptyGameObject(tempObj, ItemType.Ammo.ToString());
            AddClickToGameObject(tempObj, LoadDisplayAmmo, EventTriggerType.PointerClick);
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


    public void SelectWeapon(System.Object wepObj)
    {
        Weapon w = (Weapon)wepObj;
        battleGame.ActiveCharacter.RemoveWeapon(battleGame.ActiveCharacter.weapon);
        battleGame.ActiveCharacter.EquipWeapon(w);


        LoadCharacterStats();
        LoadDisplayWeapon();
    }

    public void SelectAmmo(System.Object ammoObj)
    {
        Ammo a = (Ammo)ammoObj;

        battleGame.ActiveCharacter.RemoveAmmo();
        battleGame.ActiveCharacter.EquipAmmo(a);


        LoadCharacterStats();
        LoadDisplayAmmo();
    }

    public void SelectArmor(System.Object armorObj)
    {
        Armor armor = (Armor)armorObj;

        battleGame.ActiveCharacter.RemoveArmorInSlot(armor.armorType);
        battleGame.ActiveCharacter.EquipArmor(armor);


        LoadCharacterStats();
        LoadDisplayArmor(armor.armorType);
    }

   
}
