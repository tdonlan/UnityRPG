using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace UnityRPG
{

    public class Spritesheet
    {
        public string sheetName { get; set; }
    
        public string filename { get; set; }
        public List<Sprite> sprites { get; set; }
    }

    public class Prefab
    {
        public string prefabName { get; set; }
        public string filename { get; set; }
        public float duration { get; set; }
        public float radius { get; set; }

        public Prefab(string name, string filename)
        {
            this.prefabName = name;
            this.filename = filename;

            this.duration = 1;
            this.radius = 0;
        }
    }

        
    public class AssetLibrary
    {
        
        public List<Spritesheet> spritesheetList {get;set;}
        public List<Prefab> prefabList { get; set; }
        public TileSpriteLibrary tileSpriteLibrary { get; set;}

        public Dictionary<string, string> boardLayoutDictionary { get; set; }
   

        public AssetLibrary()
        {
            LoadSpritesheets();
            LoadPrefabs();
            //LoadBoards();

            tileSpriteLibrary = new TileSpriteLibrary();
        }

        private void LoadBoards()
        {
            boardLayoutDictionary = new Dictionary<string, string>();
            boardLayoutDictionary.Add("Map1", Resources.Load<TextAsset>("Data/Map1").text);
        }

        private void LoadPrefabs()
        {
            prefabList = new List<Prefab>();

            //Particles
            prefabList.Add(new Prefab("FireBoom", "Elementals/Prefab/Fire/Boom"));
            prefabList.Add(new Prefab("FireBurst", "Elementals/Prefab/Fire/Fire Burst"));
            prefabList.Add(new Prefab("FireMist", "Elementals/Prefab/Fire/Fire Mist"));
            prefabList.Add(new Prefab("FireSpray", "Elementals/Prefab/Fire/Fire Spray"));
            prefabList.Add(new Prefab("Fire_01", "Elementals/Prefab/Fire/Fire_01"));
            prefabList.Add(new Prefab("Fire_02", "Elementals/Prefab/Fire/Fire_02"));
            prefabList.Add(new Prefab("Fire_03", "Elementals/Prefab/Fire/Fire_03"));
            prefabList.Add(new Prefab("Firewall", "Elementals/Prefab/Fire/Firewall"));
            prefabList.Add(new Prefab("FireMagma", "Elementals/Prefab/Fire/Magma Burst"));
            prefabList.Add(new Prefab("FireEnchant", "Elementals/Prefab/Fire/Flame Enchant"));
            prefabList.Add(new Prefab("HolyBlast", "Elementals/Prefab/Light/Holy Blast"));
            prefabList.Add(new Prefab("HolyShine", "Elementals/Prefab/Light/Holy Shine"));
            prefabList.Add(new Prefab("Lightning", "Elementals/Prefab/Thunder/Lightning"));
            prefabList.Add(new Prefab("LightningSpark", "Elementals/Prefab/Thunder/Lightning Spark"));
            prefabList.Add(new Prefab("Thunder", "Elementals/Prefab/Thunder/Thunder"));
            prefabList.Add(new Prefab("Cyclone", "Elementals/Prefab/Wind/Cyclone"));

            //Text
            prefabList.Add(new Prefab("TextPopup", "PrefabGame/TextPopupPrefab"));

            //sprite
            prefabList.Add(new Prefab("Sprite", "PrefabGame/SpritePrefab"));

        }

        private void LoadSpritesheets()
        {
            //get the manifest from some external file

            spritesheetList = new List<Spritesheet>();
            spritesheetList.Add(getSpritesheet("Tiles", "Sprites/dg_dungeon32"));
            spritesheetList.Add(getSpritesheet("Characters", "Sprites/dg_classm32Edit"));
            spritesheetList.Add(getSpritesheet("Portraits", "Sprites/portraitsEdit"));
            spritesheetList.Add(getSpritesheet("Particles", "Sprites/dg_effects32Edit"));
            spritesheetList.Add(getSpritesheet("Armor", "Sprites/dg_armor32"));
            spritesheetList.Add(getSpritesheet("Weapons", "Sprites/dg_weapons32"));
            spritesheetList.Add(getSpritesheet("Jewels", "Sprites/dg_jewls32"));
            spritesheetList.Add(getSpritesheet("Wands", "Sprites/dg_wands32"));
            spritesheetList.Add(getSpritesheet("Potions", "Sprites/dg_potions32"));
            spritesheetList.Add(getSpritesheet("Dragons", "Sprites/dg_dragon32Edit"));

            spritesheetList.Add(getSpritesheet("Monsters1", "Sprites/monsters1"));
            spritesheetList.Add(getSpritesheet("Monsters2", "Sprites/monsters2"));
            spritesheetList.Add(getSpritesheet("Monsters3", "Sprites/monsters3"));
            spritesheetList.Add(getSpritesheet("Monsters4", "Sprites/monsters4"));
            spritesheetList.Add(getSpritesheet("Monsters5", "Sprites/monsters5"));
            spritesheetList.Add(getSpritesheet("Monsters6", "Sprites/monsters6"));
            spritesheetList.Add(getSpritesheet("Monsters7", "Sprites/monsters7"));

            spritesheetList.Add(getSpritesheet("Blank", "Sprites/blankItem"));
            spritesheetList.Add(getSpritesheet("InitBG1", "Sprites/InitBG1"));
            spritesheetList.Add(getSpritesheet("InitBG2", "Sprites/InitBG2"));
            spritesheetList.Add(getSpritesheet("HighlightTile", "Sprites/highlightTile"));

        }

        private Spritesheet getSpritesheet(string sheetname, string filename)
        {
            Spritesheet retval = new Spritesheet();
            retval.filename = filename;
            retval.sheetName = sheetname;
          
            Sprite[] spriteArray = Resources.LoadAll<Sprite>(filename);

            retval.sprites = spriteArray.ToList();

            return retval;
 
        }

        public Sprite getSprite(string sheetname, int index)
        {
            Spritesheet sheet = (from data in spritesheetList
                                 where data.sheetName == sheetname
                                 select data).FirstOrDefault();

            if (sheet.sprites.Count > index)
            {
                return sheet.sprites[index];
            }
            return null;

        }

        public Prefab getPrefab(string prefabName)
        {
            Prefab pre = (from data in prefabList
                          where data.prefabName == prefabName
                          select data).FirstOrDefault();

            return pre;
        }

        public GameObject getPrefabGameObject(string prefabName)
        {
            var prefab = getPrefab(prefabName);

            return GameObjectHelper.LoadPrefab(prefab.filename);
        }
        
    }


    public class TileSpriteLookup
    {
        public char tileChar { get; set; }
        public string tileName { get; set; }
        public string spritesheetName { get; set; }
        public int spritesheetIndex { get; set; }
        public bool isEmpty { get; set; }
        public TileSpriteType tileSpriteType { get; set; }

        public TileSpriteLookup(char tileChar, string spriteSheet, int index, bool isEmpty, TileSpriteType type)
        {
            this.tileChar = tileChar;
            this.tileName = tileChar.ToString();
            this.spritesheetName = spriteSheet;
            this.spritesheetIndex = index;
            this.isEmpty = isEmpty;
            this.tileSpriteType = type;
        }
    }

    public class TileSpriteLibrary
    {
        public Dictionary<string, Dictionary<char,TileSpriteLookup>> tileSpriteDictionary { get; set; }

        public TileSpriteLibrary()
        {
            tileSpriteDictionary = new Dictionary<string, Dictionary<char, TileSpriteLookup>>();
            LoadTileSpriteLibrary();
        }

        public Dictionary<char,TileSpriteLookup> getTileSpriteDictionary(string name)
        {
            if (tileSpriteDictionary.ContainsKey(name))
            {
                return tileSpriteDictionary[name];
            }
            else
            {
                return null;
            }
        }

        public TileSpriteLookup getTileSpriteLookupFromLibraryAndChar(string libraryName, char c)
        {
            if (tileSpriteDictionary.ContainsKey(libraryName))
            {
                var tileLookupDict = tileSpriteDictionary[libraryName];
                if (tileLookupDict.ContainsKey(c))
                {
                    return tileLookupDict[c];
                }
            }
            return null;
        }

        //Load this from external file / JSON
        private void LoadTileSpriteLibrary()
        {
            tileSpriteDictionary.Add("Dungeon", getDungeonTileSpriteDictionary());
        }

        private Dictionary<char,TileSpriteLookup> getDungeonTileSpriteDictionary()
        {
            Dictionary<char, TileSpriteLookup> tileSpriteDict = new Dictionary<char, TileSpriteLookup>();
            tileSpriteDict.Add('.', new TileSpriteLookup('.', "Tiles", 2, true, TileSpriteType.Floor));
            tileSpriteDict.Add('#',new TileSpriteLookup('#', "Tiles", 27, false, TileSpriteType.Wall));
            tileSpriteDict.Add('P',new TileSpriteLookup('P', "Tiles", 2, true, TileSpriteType.PlayerStart));
            tileSpriteDict.Add('E',new TileSpriteLookup('E', "Tiles", 2, true, TileSpriteType.EnemyStart));

            return tileSpriteDict;

        }
    }


}
