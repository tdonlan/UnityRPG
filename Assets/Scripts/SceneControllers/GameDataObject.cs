using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityRPG;


public class GameDataObject : MonoBehaviour
{
    public TreeStore treeStore { get; set; }
    public string testText { get; set; }

    public GameCharacter playerGameCharacter;


    //Loaded Data
    public GameDataSet gameDataSet { get; set; }

    //Battle Scene Data

    public AssetLibrary assetLibrary { get; set; }
    public List<GameCharacter> gameCharacterList { get; set; }
    public Board gameBoard { get; set; }


    void Start()
    {
        assetLibrary = new AssetLibrary();
        loadTreeStore();
        this.testText = "Hello World";

        loadGameData();
        loadPlayerGameCharacter();

        DontDestroyOnLoad(this);
    }

    private void loadPlayerGameCharacter()
    {
        //load this from a save game.  If we are starting a new game, then get this from the masterlist
        playerGameCharacter = CharacterFactory.getGameCharacterFromGameCharacterData(gameDataSet.gameCharacterDataDictionary[80001], gameDataSet);
        playerGameCharacter.level = 1;

    }

 

    public void loadGameData()
    {
        gameDataSet = new GameDataSet();
        gameDataSet.itemDataDictionary = getDataObjectDictionary("Data/Items", typeof(ItemData)).ToDictionary(x => x.Key, x => (ItemData)x.Value);
        gameDataSet.usableItemDataDictionary = getDataObjectDictionary("Data/UsableItems", typeof(UsableItemData)).ToDictionary(x => x.Key, x => (UsableItemData)x.Value);
        gameDataSet.weaponDataDictionary = getDataObjectDictionary("Data/Weapons", typeof(WeaponData)).ToDictionary(x => x.Key, x => (WeaponData)x.Value);
        gameDataSet.rangedWeaponDataDictionary = getDataObjectDictionary("Data/RangedWeapons", typeof(RangedWeaponData)).ToDictionary(x => x.Key, x => (RangedWeaponData)x.Value);
        gameDataSet.ammoDataDictionary = getDataObjectDictionary("Data/Ammo", typeof(AmmoData)).ToDictionary(x => x.Key, x => (AmmoData)x.Value);
        gameDataSet.armorDataDictionary = getDataObjectDictionary("Data/Armors", typeof(ArmorData)).ToDictionary(x => x.Key, x => (ArmorData)x.Value);

        gameDataSet.effectDataDictionary = getDataObjectDictionary("Data/Effects", typeof(EffectData)).ToDictionary(x => x.Key, x => (EffectData)x.Value);
        gameDataSet.abilityDataDictionary = getDataObjectDictionary("Data/Abilities", typeof(AbilityData)).ToDictionary(x => x.Key, x => (AbilityData)x.Value);
        gameDataSet.gameCharacterDataDictionary = getDataObjectDictionary("Data/GameCharacters", typeof(GameCharacterData)).ToDictionary(x => x.Key, x => (GameCharacterData)x.Value);

        gameDataSet.talentTreeDataDictionary = getDataObjectDictionary("Data/TalentTree", typeof(TalentTreeData)).ToDictionary(x => x.Key, x => (TalentTreeData)x.Value);
    }

    private Dictionary<long, object> getDataObjectDictionary(string assetName, Type dataType)
    {
        TextAsset manifestTextAsset2 = Resources.Load<TextAsset>(assetName);
        return DataLoader.loadMasterDictionary(manifestTextAsset2.text, dataType);
    }

    private void loadTreeStore()
    {
        TextAsset manifestTextAsset = Resources.Load<TextAsset>("Data/SimpleWorld1/manifestSimple");
        this.treeStore = SimpleTreeParser.LoadTreeStoreFromSimpleManifest(manifestTextAsset.text);
    }

    public void runActions(List<TreeNodeAction> actionList)
    {
        foreach (var action in actionList)
        {
            switch (action.actionType)
            {
                case NodeActionType.AddItem:
                    addItem(action.index, action.count);
                    break;
                case NodeActionType.RemoveItem:
                    removeItem(action.index, action.count);
                    break;
                default:
                    break;
            }
        }
    }

    public void removeItem(long itemIndex, long count)
    {
        var item = ItemFactory.getItemFromIndex(itemIndex, gameDataSet);
        if (item != null)
        {
            if (item.type == ItemType.Money)
            {
                if (playerGameCharacter.money >= count)
                {
                    playerGameCharacter.money -= count;
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var inventoryItem = playerGameCharacter.inventory.Where(x => x.ID == itemIndex).FirstOrDefault();
                    if (inventoryItem != null)
                    {
                        playerGameCharacter.inventory.Remove(inventoryItem); 
                    }
                }
            }
        }
    }

    public void addItem(long itemIndex, long count)
    {
        var item = ItemFactory.getItemFromIndex(itemIndex, gameDataSet);
        if (item != null)
        {
            if (item.type == ItemType.Money)
            {
                playerGameCharacter.money += count;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    playerGameCharacter.inventory.Add(item);
                }
            }
        }
    }

    //TODO: pass in the character to use to construct player ownership
    public List<TalentTreeDisplayData> getTalentTreeDisplayData()
    {
        List<TalentTreeDisplayData> talentTreeDisplayList = new List<TalentTreeDisplayData>();

        foreach (var talentData in gameDataSet.talentTreeDataDictionary.Values) {
            var abilityData = gameDataSet.abilityDataDictionary[talentData.AbilityID];
            bool playerOwned = (playerGameCharacter.abilityList.Where(x => x.ID == abilityData.ID).Count() > 0 )? true : false;
            TalentTreeDisplayData tempTT = new TalentTreeDisplayData() { 
                ID = talentData.ID,
                AbilityID = abilityData.ID,
                AbilityDescription = abilityData.description,
                AbilityName = abilityData.name,
                SpriteSheetName = abilityData.sheetname,
                SpriteSheetIndex = abilityData.spriteindex,
                AP = abilityData.ap,
                levelReq = talentData.levelReq,
                owned = playerOwned,
                unlocked = getPlayerUnlocked(playerGameCharacter,talentData.levelReq,talentData.abilityReqs),
                range = abilityData.range,
                tag = talentData.tag,
                targetType = abilityData.targetType,
                tier = talentData.tier,
                uses = abilityData.uses,
                tilePatternType = abilityData.tilePatternType,
                abilityReqNameList = getAbilityNameList(talentData.abilityReqs),
                effectDescriptionList = getEffectsDescriptionList(abilityData.passiveEffects,abilityData.activeEffects)
            };

            talentTreeDisplayList.Add(tempTT);
        
        }
        return talentTreeDisplayList;
    }

    private bool getPlayerUnlocked(GameCharacter playerCharacter, int levelReq, List<long> abilityReq)
    {
        bool retval = true;
        if (playerCharacter.level < levelReq)
        {
            retval = false;
        }

        foreach (var abilityID in abilityReq)
        {
            if (playerCharacter.abilityList.Where(x => x.ID == abilityID).Count() == 0)
            {
                retval = false;
            }
        }

        return retval;
    }

    private List<string> getAbilityNameList(List<long> abilityIDList)
    {
        List<string> abilityNameList = new List<string>();
        foreach (var id in abilityIDList)
        {
            var abilityData = gameDataSet.abilityDataDictionary[id];
            if(abilityData != null){
                abilityNameList.Add(abilityData.name);
            }
        }
        return abilityNameList;
    }

    private List<string> getEffectsDescriptionList(List<long> passiveEffects, List<long> activeEffects)
    {
        List<string> effectsDescriptionList = new List<string>();
        foreach (var ID in passiveEffects)
        {
            EffectData efData = gameDataSet.effectDataDictionary[ID];
            if (efData != null)
            {
               
                string tempStr = string.Format("{0}: {1}-{2}",efData.statType.ToString(),efData.minAmount.ToString(),efData.maxAmount.ToString());
            }
        }

        foreach (var ID in activeEffects)
        {
            EffectData efData = gameDataSet.effectDataDictionary[ID];
            if (efData != null)
            {
                string tempStr = string.Format("{0}: {1}-{2} for {3} rnds.", efData.statType.ToString(), efData.minAmount.ToString(), efData.maxAmount.ToString(),efData.duration.ToString());
            }
        }

         return effectsDescriptionList;
    }
}

