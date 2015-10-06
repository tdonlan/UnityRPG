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
    public CharacterScreenController characterScreenController;

    public GameObject InfoScreen;
    public PauseMenuScript pauseScreenController;

    public GameObject EquipmentScreen;
    public GameObject RightPanelContent;
    public GameObject EquipLeftPanel;
    public GameObject EquipLeftPanelContent;
    public GameObject EquipLeftItemPanel;
    public GameObject CharacterPanel;

    public GameObject usableItemStatPanel;


    private GameObject EquipPrefab;

    public Text ItemTypeText;


    public GameDataObject gameDataObject { get; set; }

    public AssetLibrary assetLibrary { get; set; }

    public GameCharacter curGameCharacter;

    List<GameObject> displayEquipList { get; set; }

    private int usableItemSlot = -1;


	// Use this for initialization
	void Start () {

        loadGameData();

        curGameCharacter = gameDataObject.playerGameCharacter;

        initScreens();
        InitPrefabs();

        this.assetLibrary = gameDataObject.assetLibrary;

        LoadCharacterStats();
        ClearCurrentEquip();
	}


    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void InitPrefabs()
    {
       
        EquipPrefab = Resources.Load<GameObject>("PrefabUI/EquipPrefab");

        CharacterPanel = GameObject.FindGameObjectWithTag("CharacterPanel");

        EquipLeftPanel = GameObject.FindGameObjectWithTag("EquipLeftPanel");
        EquipLeftPanelContent = UIHelper.getChildObject(EquipLeftPanel, "CurrentEquipStatPanel");
        
        EquipLeftItemPanel = GameObject.FindGameObjectWithTag("EquipLeftItemPanel");
        usableItemStatPanel = UIHelper.getChildObject(EquipLeftItemPanel, "SelectedItemStats");


       // EquipLeftPanelContent = EquipLeftPanel.GetComponentsInChildren<GameObject>().Where(x=>x.name.Equals("CurrentEquipStatPanel")).FirstOrDefault();
        RightPanelContent = GameObject.FindGameObjectWithTag("EquipRightPanelContent");
        ItemTypeText = GameObject.FindGameObjectWithTag("EquipItemTypeText").GetComponent<Text>();
    }

    private void initScreens()
    {
        CharacterScreen = GameObject.FindGameObjectWithTag("CharacterScreen");
        InfoScreen = GameObject.FindGameObjectWithTag("PauseMenu");
        EquipmentScreen = this.gameObject;

        characterScreenController = CharacterScreen.GetComponent<CharacterScreenController>();
        pauseScreenController = InfoScreen.GetComponent<PauseMenuScript>();
    }

    public void RefreshEquipment()
    {
        LoadCharacterStats();
        ClearCurrentEquip();
    }

    public void RefreshItems()
    {
        LoadCharacterUsableItems();
        LoadDisplayItems();
        UpdateSelectedItemSlot(usableItemSlot);
    }

    public void LoadCharacterStats()
    {
        if (curGameCharacter != null)
        {

            UIHelper.UpdateSpriteComponent(CharacterPanel, "CharImage", assetLibrary.getSprite(curGameCharacter.characterSpritesheetName, curGameCharacter.characterSpriteIndex));
            UIHelper.UpdateTextComponent(CharacterPanel, "CharNameText", curGameCharacter.name);
            UIHelper.UpdateTextComponent(CharacterPanel, "CharStats", curGameCharacter.ToString());
            UIHelper.UpdateTextComponent(CharacterPanel, "GoldText", "Gold: " + gameDataObject.playerGameCharacter.money.ToString()); //only the player stores money
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
            //CloseScreen();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ShowCharacterScreen();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            //CloseScreen();
        }
    }

    public void UpdateUI()
    {
        curGameCharacter = gameDataObject.getSelectedCharacter();
        if (curGameCharacter == null)
        {
            curGameCharacter = gameDataObject.playerGameCharacter;
        }

        RefreshEquipment();
        RefreshItems();
    }

    //called to clear out current equiped if nothing in slot
    public void ClearCurrentEquip()
    {
      
        UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipName", "");
        UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipType", "");
        UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipStats", "");
        UIHelper.UpdateSpriteComponent(EquipLeftPanelContent, "EquipImage", assetLibrary.getSprite("Blank", 0));
    }

    public void LoadCurrentWeapon()
    {

        ItemTypeText.text = "Weapons";

        if (curGameCharacter.weapon != null)
        {

            var wep = curGameCharacter.weapon;
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipName", wep.name);
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipType", wep.weaponType.ToString());
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipStats", wep.ToString());
            UIHelper.UpdateSpriteComponent(EquipLeftPanelContent, "EquipImage", assetLibrary.getSprite(wep.sheetname, wep.spriteindex));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadCurrentAmmo()
    {

        

        if (curGameCharacter.Ammo != null)
        {


            var item = curGameCharacter.getInventoryItembyItemID(curGameCharacter.Ammo.itemID);
            var itemAmmo = (Ammo)item;
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipName", item.name);
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipType", itemAmmo.ammoType.ToString());
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipStats", itemAmmo.ToString());
            UIHelper.UpdateSpriteComponent(EquipLeftPanelContent, "EquipImage", assetLibrary.getSprite(itemAmmo.sheetname, itemAmmo.spriteindex));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    public void LoadCurrentArmor(ArmorType armorType)
    {
       
        var armor = curGameCharacter.getArmorInSlot(armorType);
        if (armor != null)
        {
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipName", armor.name);
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipType", armorType.ToString());
            UIHelper.UpdateTextComponent(EquipLeftPanelContent, "EquipStats", armorType.ToString());
            UIHelper.UpdateSpriteComponent(EquipLeftPanelContent, "EquipImage", assetLibrary.getSprite(armor.sheetname, armor.spriteindex));
        }
        else
        {
            ClearCurrentEquip();
        }
    }

    //load the list of display equipment, give a type
    public void LoadDisplayArmor(int armorType)
    {
        ItemTypeText.text = ((ArmorType)armorType).ToString();
        LoadCurrentArmor((ArmorType)armorType);

        displayEquipList = new List<GameObject>();
        UIHelper.DestroyAllChildren(RightPanelContent.transform);

        var armorList = from data in gameDataObject.playerGameCharacter.inventory
                        where data.type == ItemType.Armor
                        select data;

        var armorTypeList = (from Armor data in armorList
                             where data.armorType == (ArmorType)armorType
                             select data).ToList();

        foreach (var a in armorTypeList)
        {
            GameObject tempObj = (GameObject)Instantiate(EquipPrefab);
            updateArmorGameObject(tempObj, a);
            tempObj.transform.SetParent(RightPanelContent.transform, true);
            UIHelper.AddClickToGameObject(tempObj, SelectArmor, EventTriggerType.PointerClick, a);
            displayEquipList.Add(tempObj);
        }

    }

    public void LoadDisplayWeapon()
    {

        ItemTypeText.text = "Weapons";

        LoadCurrentWeapon();

        displayEquipList = new List<GameObject>();

        UIHelper.DestroyAllChildren(RightPanelContent.transform);

        var weaponList = from data in gameDataObject.playerGameCharacter.inventory
                        where data.type == ItemType.Weapon
                        select data;

        foreach (var w in weaponList)
        {

            GameObject tempObj = (GameObject)Instantiate(EquipPrefab);
            updateWeaponGameObject(tempObj, (Weapon)w);
            UIHelper.AddClickToGameObject(tempObj, SelectWeapon, EventTriggerType.PointerClick, w);

            tempObj.transform.SetParent(RightPanelContent.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    public void LoadDisplayAmmo()
    {
        ItemTypeText.text = "Ammo";
        LoadCurrentAmmo();

        displayEquipList = new List<GameObject>();

        UIHelper.DestroyAllChildren(RightPanelContent.transform);

        var ammoList = from data in curGameCharacter.inventory
                         where data.type == ItemType.Ammo
                         select data;

        foreach (var a in ammoList)
        {
            ItemSet ammoSet = ItemHelper.getItemSet(curGameCharacter.inventory, a);
            GameObject tempObj = (GameObject)Instantiate(EquipPrefab);
            updateAmmoGameObject(tempObj, ammoSet);
            UIHelper.AddClickToGameObject(tempObj, SelectAmmo, EventTriggerType.PointerClick, a);
            tempObj.transform.SetParent(RightPanelContent.transform, true);

            displayEquipList.Add(tempObj);
        }
    }

    public void LoadDisplayItems()
    {

        ItemTypeText.text = "Items";
        displayEquipList = new List<GameObject>();


        UIHelper.DestroyAllChildren(RightPanelContent.transform);

        var itemList = from data in gameDataObject.playerGameCharacter.inventory
                       where data.type == ItemType.Potion || data.type == ItemType.Quest || data.type == ItemType.Thrown || data.type == ItemType.Wand
                       select data;

        var distinctItemList = itemList.Distinct().ToList();

        foreach (var i in distinctItemList)
        {
            GameObject tempObj = (GameObject)Instantiate(EquipPrefab);
            int count = itemList.Count(x => x.name == i.name);
            updateItemGameobject(tempObj, i, count);
            UIHelper.AddClickToGameObject(tempObj, SelectUsableItem, EventTriggerType.PointerClick, i);
            tempObj.transform.SetParent(RightPanelContent.transform, true);

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

    private GameObject updateItemGameobject(GameObject obj, Item i, int count)
    {
        var equipTypePanel = UIHelper.getChildObject(obj, "EquipTypePanel");
        string itemName = string.Format("{0} ({1})",i.name,count);
        UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", itemName);
        UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(i.sheetname, i.spriteindex));

        UIHelper.UpdateTextComponent(obj, "EquipStats", i.ToString());
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

        curGameCharacter.RemoveWeapon(curGameCharacter.weapon);
        curGameCharacter.EquipWeapon(w);


        LoadCharacterStats();
        LoadDisplayWeapon();
    }

    public void SelectAmmo(System.Object ammoObj)
    {
        Ammo a = (Ammo)ammoObj;

        curGameCharacter.RemoveAmmo();
        curGameCharacter.EquipAmmo(a);

        LoadCharacterStats();
        LoadDisplayAmmo();
    }

    public void SelectArmor(System.Object armorObj)
    {
        Armor armor = (Armor)armorObj;

        curGameCharacter.RemoveArmorInSlot(armor.armorType);
        curGameCharacter.EquipArmor(armor);

        LoadCharacterStats();
        LoadDisplayArmor((int)armor.armorType);
    }

    public void ShowEquipment()
    {
        UIHelper.DestroyAllChildren(RightPanelContent.transform);
        ItemTypeText.text = "Equipment";
        EquipLeftPanel.transform.localPosition = new Vector3(-297, -164.6f, 0);
        EquipLeftItemPanel.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void ShowInventory()
    {
        //hide equipment buttons
        LoadDisplayItems();
        ItemTypeText.text = "Items";
        EquipLeftItemPanel.transform.localPosition = new Vector3(-297, -164.6f, 0);
        EquipLeftPanel.transform.localPosition = new Vector3(10000, 10000, 0);
        LoadCharacterUsableItems();
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
        characterScreenController.UpdateUI();
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
                if (partyIndex >= gameDataObject.partyList.Count)
                {
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

    //----------------------

    public void LoadCharacterUsableItems()
    {
        var itemSlotPanel = UIHelper.getChildObject(EquipLeftItemPanel, "ItemSlotPanel");
        GameObject itemSlot = null;

        for (int i = 0; i < 10; i++)
        {
            itemSlot = UIHelper.getChildObject(itemSlotPanel, "Item" + i);
            var itemSpriteObj = UIHelper.getChildObject(itemSlot, "Image");
            itemSpriteObj.GetComponent<Image>().sprite = gameDataObject.assetLibrary.getSprite("Blank", 0);
        }

        var itemList = curGameCharacter.usableItemList;
        if (itemList.Count > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemSlot = UIHelper.getChildObject(itemSlotPanel, "Item" + i);
                var itemSpriteObj = UIHelper.getChildObject(itemSlot, "Image");
                itemSpriteObj.GetComponent<Image>().sprite = gameDataObject.assetLibrary.getSprite(itemList[i].sheetname, itemList[i].spriteindex);
            }
        }

    }

    private void UpdateSelectedItemSlot(int slot)
    {

        GameObject itemSlotPanel = UIHelper.getChildObject(EquipLeftItemPanel, "ItemSlotPanel");
        for (int i = 0; i < 10; i++)
        {
            Color c = Color.white;
            if (i == slot)
            {
                c = Color.yellow;
            }

            UIHelper.UpdateSpriteColor(itemSlotPanel, "Item" + i, c);
        }


        //Display Blank Item as default
        GameObject equipTypePanel = UIHelper.getChildObject(usableItemStatPanel, "EquipTypePanel");
        UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", "");
        UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite("Blank", 0));
        UIHelper.UpdateTextComponent(usableItemStatPanel, "EquipStats", "");


        var itemList = curGameCharacter.usableItemList;

        if (slot > -1 && itemList.Count > 0 && itemList.Count > slot)
        {
            var item = itemList[slot];
            if (item != null)
            {
                equipTypePanel = UIHelper.getChildObject(usableItemStatPanel, "EquipTypePanel");
                UIHelper.UpdateTextComponent(equipTypePanel, "EquipType", item.name);
                UIHelper.UpdateSpriteComponent(equipTypePanel, "EquipImage", assetLibrary.getSprite(item.sheetname, item.spriteindex));

                UIHelper.UpdateTextComponent(usableItemStatPanel, "EquipStats", item.ToString());
            }
        }
    }

    public void SelectUsableItemSlot(int slot)
    {
        if (usableItemSlot == slot)
        {
            usableItemSlot = -1;
        }
        else
        {
            usableItemSlot = slot;
        }

        UpdateSelectedItemSlot(usableItemSlot);

    }

    public void RemoveUsableItem()
    {

        var itemList = curGameCharacter.usableItemList;
        if (usableItemSlot > -1 && itemList.Count > 0 && itemList.Count > usableItemSlot)
        {
            var item = itemList[usableItemSlot];

            curGameCharacter.removeUsableItem(item);

            RefreshItems();
        }
    }

    public void SelectUsableItem(System.Object itemObj)
    {

        Item i = (Item)itemObj;
        //remove the item in the current slot
        RemoveUsableItem();
        //refresh the item ui
        curGameCharacter.addUsableItem(i);

        RefreshItems();
    }


}
