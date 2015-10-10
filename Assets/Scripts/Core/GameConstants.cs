using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace UnityRPG

{
    public class GameConstants
    {
        public const string weaponAttackSpritesheet = "SpellEffects";
        public const int weaponAttackSpriteindex = 48;

        public const string rangedAttackSpritesheet = "SpellEffects";
        public const int rangedAttackSpriteindex = 112;

        //-----------------------

        public const int maxCharacterUsableItems = 10; //max distinct usable items per character (after stacking)

        //--------------------
        public const long MONEY_INDEX = 20001;

        public const long EFFECTS_MAX_INDEX = 9999;         //1001 - 9999
        public const long ABILITIES_MAX_INDEX = 19999;      //10001 - 19999
        public const long ITEMS_MAX_INDEX = 29999;           //20001 - 29999
        public const long USABLEITEMS_MAX_INDEX = 39999;     //30001 - 19999
        public const long WEAPONS_MAX_INDEX = 49999;         //40001 - 49999
        public const long RANGEDWEAPONS_MAX_INDEX = 59999;   //50001 - 59999
        public const long AMMO_MAX_INDEX = 69999;            //60001 - 69999
        public const long ARMOR_MAX_INDEX = 79999;           //70001 - 79999
        public const long CHARACTERS_MAX_INDEX = 89999;      //80001 - 89999
        public const long TALENT_TREE_MAX_INDEX = 99999;      //90001 - 99999

        //--------------------

    }
}
