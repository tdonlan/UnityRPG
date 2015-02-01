using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleRPG2;
using System;
using UnityEngine.UI;
using Assets;

public class EquipmentTestScript : MonoBehaviour {

    public AssetLibrary assetLibrary { get; set; } 

    public BattleGame battleGame { get; set; }

    List<GameObject> currentEquipList { get; set; }

	// Use this for initialization
	void Start () {
        this.assetLibrary = new AssetLibrary();
        this.battleGame = new BattleGame();

        LoadEquipList();
	}
	
	// Update is called once per frame
	void Update () {
	    
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
            tempObj.transform.SetParent(leftEquipPanel.transform, true);

            currentEquipList.Add(tempObj);
        }
        else
        {
            GameObject tempObj = (GameObject)Instantiate(equipPrefab);
            updateEmptyGameObject(tempObj,ItemType.Weapon.ToString());
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
                tempObj.transform.SetParent(leftEquipPanel.transform, true);

                currentEquipList.Add(tempObj);
            }
            else
            {
                GameObject tempObj = (GameObject)Instantiate(equipPrefab);
                updateEmptyGameObject(tempObj, ((ArmorType)a).ToString());
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
}
