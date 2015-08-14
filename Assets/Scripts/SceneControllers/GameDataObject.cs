using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityRPG;


public class GameDataObject : MonoBehaviour
{
    public int battleIndex { get; set; }

    public TreeStore treeStore { get; set; }
    public string testText { get; set; }
    public Dictionary<long, Item> itemDictionary;

    public GameCharacter playerGameCharacter;
    public List<long> playerInventory = new List<long>();

    //Loaded Data
    public GameDataSet gameDataSet { get; set; }

    //Battle Scene Data

    public Dictionary<string, BoardData> BoardDataDictionary { get; set; }
    public AssetLibrary assetLibrary { get; set; }
    public List<GameCharacter> gameCharacterList { get; set; }
    public Board gameBoard { get; set; }


    void Start()
    {
        loadTreeStore();
        this.testText = "Hello World";
        loadItemList();
        loadGameData();
        loadPlayerGameCharacter();
        LoadBoardDataDictionary();

        DontDestroyOnLoad(this);
    }

    private void loadPlayerGameCharacter()
    {
        //load this from a save game.  If we are starting a new game, then get this from the masterlist
        playerGameCharacter = CharacterFactory.getGameCharacterFromGameCharacterData(gameDataSet.gameCharacterDataDictionary[80001], gameDataSet);
    }

    private void loadItemList()
    {
        itemDictionary = ItemFactory.getItemDictionary();
    }

    public void loadGameData()
    {
        gameDataSet = new GameDataSet();

        gameDataSet.usableItemDataDictionary = getDataObjectDictionary("Data/UsableItems", typeof(UsableItemData)).ToDictionary(x => x.Key, x => (UsableItemData)x.Value);
        gameDataSet.weaponDataDictionary = getDataObjectDictionary("Data/Weapons", typeof(WeaponData)).ToDictionary(x => x.Key, x => (WeaponData)x.Value);
        gameDataSet.rangedWeaponDataDictionary = getDataObjectDictionary("Data/RangedWeapons", typeof(RangedWeaponData)).ToDictionary(x => x.Key, x => (RangedWeaponData)x.Value);
        gameDataSet.ammoDataDictionary = getDataObjectDictionary("Data/Ammo", typeof(AmmoData)).ToDictionary(x => x.Key, x => (AmmoData)x.Value);
        gameDataSet.armorDataDictionary = getDataObjectDictionary("Data/Armors", typeof(ArmorData)).ToDictionary(x => x.Key, x => (ArmorData)x.Value);

        gameDataSet.effectDataDictionary = getDataObjectDictionary("Data/Effects", typeof(EffectData)).ToDictionary(x => x.Key, x => (EffectData)x.Value);
        gameDataSet.abilityDataDictionary = getDataObjectDictionary("Data/Abilities", typeof(AbilityData)).ToDictionary(x => x.Key, x => (AbilityData)x.Value);
        gameDataSet.gameCharacterDataDictionary = getDataObjectDictionary("Data/GameCharacters", typeof(GameCharacterData)).ToDictionary(x => x.Key, x => (GameCharacterData)x.Value);
    }

    private Dictionary<long, object> getDataObjectDictionary(string assetName, Type dataType)
    {
        TextAsset manifestTextAsset2 = Resources.Load<TextAsset>(assetName);
        return DataLoader.loadMasterDictionary(manifestTextAsset2.text, dataType);
    }

    private void loadTreeStore()
    {
        TextAsset manifestTextAsset = Resources.Load<TextAsset>("SimpleWorld1/manifestSimple");
        this.treeStore = SimpleTreeParser.LoadTreeStoreFromSimpleManifest(manifestTextAsset.text);
    }

    //Testing - hardcoded battle map
    public void LoadBoardDataDictionary()
    {
        BoardDataDictionary = new Dictionary<string, BoardData>();
        BoardDataDictionary.Add("Board1", new BoardData("Map1", "Dungeon", "Board1", null));
    }

    public void runActions(List<TreeNodeAction> actionList)
    {
        foreach (var action in actionList)
        {
            switch (action.actionType)
            {
                case NodeActionType.AddItem:
                    addItem(action.index);
                    break;
                case NodeActionType.RemoveItem:
                    removeItem(action.index);
                    break;
                default:
                    break;

            }
        }
    }

    public void removeItem(long itemIndex)
    {
        if (itemDictionary.ContainsKey(itemIndex))
        {
            var item = playerInventory.Find(x => x == itemIndex);
            playerInventory.Remove(item);
        }
    }

    public void addItem(long itemIndex)
    {
        if (itemDictionary.ContainsKey(itemIndex))
        {
            playerInventory.Add(itemIndex);
        }

    }


}

