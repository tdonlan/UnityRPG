using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityRPG
{
    public class EffectData
    {
        public long ID { get; set; }
        public string name { get; set; }
        public StatType statType { get; set; }

        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public int duration { get; set; } // -1 for passive effect

        public TempEffectType effectType { get; set; }
        public string effectName { get; set; }
        public int effectIndex { get; set; }
    }

    public class AbilityData
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int ap { get; set; }
        public int uses { get; set; }

        public int range { get; set; }

        public AbilityTargetType targetType { get; set; }
        public TilePatternType tilePatternType { get; set; } //only used for AOE abilities

        public List<long> activeEffects { get; set; }
        public List<long> passiveEffects { get; set; }

        public string sheetname { get; set; }
        public int spriteindex { get; set; }
    }

    public class GameCharacterData
    {
        public long ID { get; set; }

        public string name { get; set; }
        public char displayChar { get; set; }
        public CharacterType type { get; set; }

        public string characterSpritesheetName { get; set; }
        public int characterSpriteIndex { get; set; }

        public string portraitSpritesheetName { get; set; }
        public int portraitSpriteIndex { get; set; }

        private int ac { get; set; }

        public int hp { get; set; }

        private int attack { get; set; }

        public int ap { get; set; }

        public List<long> inventory { get; set; } //list of Item IDs
        public List<long> equippedArmor { get; set; } //list of ArmorIds
        public long weapon { get; set; } //weaponId (Ranged or Melee)

        public List<long> activeEffects { get; set; } //list of EffectIds
        public List<long> passiveEffects { get; set; } //list of EffectIds

        public List<long> abilityList { get; set; } //list of AbilityIds
    }

    public class ItemData
    {
        public long ID { get; set; }
        public string name { get; set; }
        public ItemType type { get; set; }
        public List<long> passiveEffects { get; set; }
        public List<long> activeEffects { get; set; }

        public string sheetname { get; set; }
        public int spriteindex { get; set; }

    }

    public class UsableItemData : ItemData
    {
        public int actionPoints { get; set; }
        public int uses { get; set; }
        public List<long> itemAbility { get; set; } //Reference to Ability ID
    }


    public class WeaponData : ItemData
    {
        public int minDamage { get; set; }
        public int maxDamage { get; set; }
        public int actionPoints { get; set; }
        public WeaponType weaponType { get; set; }
    }

    public class RangedWeaponData : WeaponData
    {
        public int range { get; set; }
        public AmmoType ammoType { get; set; }

    }

    public class AmmoData : ItemData
    {
        public int bonusDamage { get; set; }
        public AmmoType ammoType { get; set; }

    }

    public class ArmorData : ItemData
    {
        public int armor { get; set; }
        public ArmorType armorType { get; set; }

    }

}
