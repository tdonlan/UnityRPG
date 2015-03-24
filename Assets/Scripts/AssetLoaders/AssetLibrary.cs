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
        public List<string> boardStringList { get; set; }

        public AssetLibrary()
        {
            LoadSpritesheets();
            LoadBoards();

            tileSpriteLibrary = new TileSpriteLibrary();
        }

        private void LoadBoards()
        {
            boardStringList = new List<string>();
            boardStringList.Add(Resources.Load<TextAsset>("Data/Map1").text);
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
        public Dictionary<string, List<TileSpriteLookup>> tileSpriteDictionary { get; set; }

        public TileSpriteLibrary()
        {
            tileSpriteDictionary = new Dictionary<string, List<TileSpriteLookup>>();
            LoadTileSpriteLibrary();
        }

        public List<TileSpriteLookup> getTileSpriteList(string name)
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

        //Load this from external file / JSON
        private void LoadTileSpriteLibrary()
        {
            tileSpriteDictionary.Add("Dungeon", getDungeonTileSpriteList());
        }

        private List<TileSpriteLookup> getDungeonTileSpriteList()
        {
            List<TileSpriteLookup> tileSpriteList = new List<TileSpriteLookup>();
            tileSpriteList.Add(new TileSpriteLookup('.', "Tiles", 2, true, TileSpriteType.Floor));
            tileSpriteList.Add(new TileSpriteLookup('#', "Tiles", 27, false, TileSpriteType.Wall));
            tileSpriteList.Add(new TileSpriteLookup('P', "Tiles", 2, true, TileSpriteType.PlayerStart));
            tileSpriteList.Add(new TileSpriteLookup('E', "Tiles", 2, true, TileSpriteType.EnemyStart));

            return tileSpriteList;

        }
    }


}
