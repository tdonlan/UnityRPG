using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityRPG;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using System.Linq;

//For Reference https://github.com/AyARL/UnityGUIExamples/blob/master/EventTrigger/Assets/TriggerSetup.cs

public class EquipmentControllerScript : MonoBehaviour {


    public GameObject CharacterScreen;
    public GameObject InfoScreen;
    public GameObject EquipmentScreen;

    public GameDataObject gameDataObject { get; set; }

    public AssetLibrary assetLibrary { get; set; } 


    List<GameObject> displayEquipList { get; set; }

	// Use this for initialization
	void Start () {

        loadGameData();
        initScreens();

        this.assetLibrary = gameDataObject.assetLibrary;

        LoadCharacterStats();
        ClearCurrentEquip();
	}


    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void initScreens()
    {
        CharacterScreen = GameObject.FindGameObjectWithTag("CharacterScreen");
        InfoScreen = GameObject.FindGameObjectWithTag("PauseMenu");
        EquipmentScreen = this.gameObject;
    }

    public void RefreshEquipment()
    {
        LoadCharacterStats();
        ClearCurrentEquip();
    }


    public void LoadCharacterStats()
    {
        if (gameDataObject.playerGameCharacter != null)
        {
            var ac = gameDataObject.playerGameCharacter;

            var charPanel = GameObject.FindGameObjectWithTag("CharacterPanel");

            UIHelper.UpdateSpriteComponent(charPanel, "CharImage", assetLibrary.getSprite(ac.characterSpritesheetName, ac.characterSpriteIndex));
            UIHelper.UpdateTextComponent(charPanel, "CharNameText", gameDataObject.playerGameCharacter.name);
            UIHelper.UpdateTextComponent(charPanel, "CharStats", gameDataObject.playerGameCharacter.ToString());
            UIHelper.UpdateTextComponent(charPanel, "GoldText", "Gold: " + gameDataObject.playerGameCharacter.money.ToString());
        }
    
    }

	// Update is called once per frame
	void Update () {
	    
	}

    //called to clear out current equiped if nothing in slot
    public void ClearCurrentEquip()
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");
        UIHelper.UpdateTextComponent(currentEquipPanel, "EquipName", "");
        UIHelper.UpdateTextComponent(currentEquipPanel, "EquipType", "");
        UIHelper.UpdateTextComponent(currentEquipPanel, "EquipStats", "");
        UIHelper.UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite("Blank", 0));
    }

    public void LoadCurrentWeapon()
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");


        if (gameDataObject.playerGameCharacter.weapon != null)
        {

            var wep = gameDataObject.playerGameCharacter.weapon;
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipName", wep.name);
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipType", wep.weaponType.ToString());
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipStats", wep.ToString());
            UIHelper.UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(wep.sheetname, wep.spriteindex));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadCurrentAmmo()
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");
        if (gameDataObject.playerGameCharacter.Ammo != null)
        {

            var item = gameDataObject.playerGameCharacter.getInventoryItembyItemID(gameDataObject.playerGameCharacter.Ammo.itemID);
            var itemAmmo = (Ammo)item;
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipName", item.name);
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipType", itemAmmo.ammoType.ToString());
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipStats", itemAmmo.ToString());
            UIHelper.UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(itemAmmo.sheetname,itemAmmo.spriteindex));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadCurrentArmor(ArmorType armorType)
    {
        var currentEquipPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");

        var armor = gameDataObject.playerGameCharacter.getArmorInSlot(armorType);
        if (armor != null)
        {
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipName", armor.name);
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipType", armorType.ToString());
            UIHelper.UpdateTextComponent(currentEquipPanel, "EquipStats", armorType.ToString());
            UIHelper.UpdateSpriteComponent(currentEquipPanel, "EquipImage", assetLibrary.getSprite(armor.sheetname,armor.spriteindex));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    //load the list of display equipment, give a type
    public void LoadDisplayArmor(int armorType)
    {

        LoadCurrentArmor((ArmorType)armorType);

        displayEquipList = new List<GameObject>();
        GameObject equipPrefab = Resources.Load<GameObject>("PrefabUI/EquipPrefab");

        GameObject rightEquipPanel = GameObject.FindGameObjectWithTag("EquipRightPanelContent");
        UIHelper.DestroyAllChildren(rightEquipPanel.transform);

        var armorList = from data in gameDataObject.playerGameCharacter.inventory
                        where data.type == ItemType.Armor
                        select data;

        var armorTypeList = (from Armor data in armorList
                             where data.armorType == (ArmorType)armorType
                             select data).ToList();

        foreach (var a in armorTypeList)
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateArmorGameObject(tempObj, a);
            tempObj.transform.SetParent(rightEquipPanel.transform, true);
            UIHelper.AddClickToGameObject(tempObj, SelectArmor, EventTriggerType.PointerClick, a);
            displayEquipList.Add(tempObj);
        }

    }

    public void LoadDisplayWeapon()
    {
        LoadCurrentWeapon();

        displayEquipList = new List<GameObject>();

        GameObject equipPrefab = Resources.Load<GameObject>("PrefabUI/EquipPrefab");

        GameObject rightEquipPanel = GameObject.FindGameObjectWithTag("EquipRightPanelContent");

        UIHelper.DestroyAllChildren(rightEquipPanel.transform);

        var weaponList = from data in gameDataObject.playerGameCharacter.inventory
                        where data.type == ItemType.Weapon
                        select data;

        foreach (var w in weaponList)
        {

            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateWeaponGameObject(tempObj, (Weapon)w);
            UIHelper.AddClickToGameObject(tempObj, SelectWeapon, EventTriggerType.PointerClick, w);
            
            tempObj.transform.SetParent(rightEquipPanel.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    public void LoadDisplayAmmo()
    {
        LoadCurrentAmmo();

        displayEquipList = new List<GameObject>();
        GameObject equipPrefab = Resources.Load<GameObject>("PrefabUI/EquipPrefab");

        GameObject rightEquipPanel = GameObject.FindGameObjectWithTag("EquipRightPanelContent");
        UIHelper.DestroyAllChildren(rightEquipPanel.transform);

        var ammoList = from data in gameDataObject.playerGameCharacter.inventory
                         where data.type == ItemType.Ammo
                         select data;

        foreach (var a in ammoList)
        {
            ItemSet ammoSet = ItemHelper.getItemSet(gameDataObject.playerGameCharacter.inventory, a);
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateAmmoGameObject(tempObj, ammoSet);
            UIHelper.AddClickToGameObject(tempObj, SelectAmmo, EventTriggerType.PointerClick, a);
            tempObj.transform.SetParent(rightEquipPanel.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    private GameObject updateEmptyGameObject(GameObject obj, string type)
    {
        var equipTypePanel = UIHelper.getChildObject(obj, "EquipTypePanel");
        UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", type);
        UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite("Blank",0));

        UIHelper.UpdateTextComponent(obj, "EquipStats", "Empty");
        return obj;
    }

    private GameObject updateWeaponGameObject(GameObject obj, Weapon wep)
    {

        var equipTypePanel = UIHelper.getChildObject(obj, "EquipTypePanel");
        UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", "Weapon");
        UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(wep.sheetname,wep.spriteindex));

        UIHelper.UpdateTextComponent(obj, "EquipStats", wep.ToString());
        return obj;
    }

    private GameObject updateAmmoGameObject(GameObject obj, ItemSet ammo)
    {
        var equipTypePanel = UIHelper.getChildObject(obj, "EquipTypePanel");
        UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", "Ammo");
        UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(ammo.sheetname,ammo.spriteindex));

        UIHelper.UpdateTextComponent(obj, "EquipStats", ammo.ToString());
        return obj;
    }

    private GameObject updateArmorGameObject(GameObject obj, Armor armor)
    {
        var equipTypePanel = UIHelper.getChildObject(obj, "EquipTypePanel");
        UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", armor.armorType.ToString());
        UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(armor.sheetname,armor.spriteindex));

        UIHelper.UpdateTextComponent(obj, "EquipStats", armor.ToString());


        return obj;
    }

    public void SelectWeapon(System.Object wepObj)
    {
        Weapon w = (Weapon)wepObj;
        gameDataObject.playerGameCharacter.RemoveWeapon(gameDataObject.playerGameCharacter.weapon);
        gameDataObject.playerGameCharacter.EquipWeapon(w);


        LoadCharacterStats();
        LoadDisplayWeapon();
    }

    public void SelectAmmo(System.Object ammoObj)
    {
        Ammo a = (Ammo)ammoObj;

        gameDataObject.playerGameCharacter.RemoveAmmo();
        gameDataObject.playerGameCharacter.EquipAmmo(a);

        LoadCharacterStats();
        LoadDisplayAmmo();
    }

    public void SelectArmor(System.Object armorObj)
    {
        Armor armor = (Armor)armorObj;

        gameDataObject.playerGameCharacter.RemoveArmorInSlot(armor.armorType);
        gameDataObject.playerGameCharacter.EquipArmor(armor);


        LoadCharacterStats();
        LoadDisplayArmor((int)armor.armorType);
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
