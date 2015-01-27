using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{

    public class Spritesheet
    {
        public SpritesheetType type { get; set; }
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
            spritesheetList.Add(getSpritesheet(SpritesheetType.Tiles, "dg_dungeon32"));
            spritesheetList.Add(getSpritesheet(SpritesheetType.Characters, "dg_classm32Edit"));
            spritesheetList.Add(getSpritesheet(SpritesheetType.Portraits, "portraitsEdit"));
            spritesheetList.Add(getSpritesheet(SpritesheetType.Particles, "dg_effects32Edit"));
        }

        private Spritesheet getSpritesheet(SpritesheetType type, string filename)
        {
            Spritesheet retval = new Spritesheet();
            retval.filename = filename;
            retval.type = type;

            Sprite[] spriteArray = Resources.LoadAll<Sprite>(filename);

            retval.sprites = spriteArray.ToList();

            return retval;
 
        }

        public Sprite getSprite(SpritesheetType type, int index)
        {
            Spritesheet sheet = (from data in spritesheetList
                                 where data.type == type
                                 select data).FirstOrDefault();

            if (sheet.sprites.Count > index)
            {
                return sheet.sprites[index];
            }
            return null;

        }
        
    }
}
