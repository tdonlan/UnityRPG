using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
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

        public AssetLibrary()
        {
            LoadSpritesheets();
        }

        private void LoadSpritesheets()
        {
            //get the manifest from some external file

            spritesheetList = new List<Spritesheet>();
            spritesheetList.Add(getSpritesheet("Tiles", "dg_dungeon32"));
            spritesheetList.Add(getSpritesheet("Characters", "dg_classm32Edit"));
            spritesheetList.Add(getSpritesheet("Portraits", "portraitsEdit"));
            spritesheetList.Add(getSpritesheet("Particles", "dg_effects32Edit"));
            spritesheetList.Add(getSpritesheet("Armor","dg_armor32"));
            spritesheetList.Add(getSpritesheet("Weapons", "dg_weapons32"));
            spritesheetList.Add(getSpritesheet("Jewels", "dg_jewls32"));
            spritesheetList.Add(getSpritesheet("Wands", "dg_wands32"));
            spritesheetList.Add(getSpritesheet("Potions", "dg_potions32"));
            spritesheetList.Add(getSpritesheet("Blank", "blankItem"));
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
}
