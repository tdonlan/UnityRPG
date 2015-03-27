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

        
    public class AssetLibrary
    {
        
        public List<Spritesheet> spritesheetList {get;set;}
        public TileSpriteLibrary tileSpriteLibrary { get; set; }

        public Dictionary<string, string> boardLayoutDictionary { get; set; }
   

        public AssetLibrary()
        {
            LoadSpritesheets();
            LoadBoards();

            tileSpriteLibrary = new TileSpriteLibrary();
        }

        private void LoadBoards()
        {
            boardLayoutDictionary = new Dictionary<string, string>();
            boardLayoutDictionary.Add("Map1", Resources.Load<TextAsset>("Data/Map1").text);
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
