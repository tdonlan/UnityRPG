using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class ItemFactory
    {
        public static Item getItem(Random r)
        {
            Item i = new Item() {ID=1,name="Healing Potion",activeEffects=new List<ActiveEffect>(){getActiveEffect(r)},passiveEffects=null,type=ItemType.Potion,
                sheetname="Potions",spriteindex=18
            };

            return i;
        }

        public static UsableItem getHealingPotion(Random r)
        {
            UsableItem i = new UsableItem() {ID=1, name = "Healing Potion", activeEffects = new List<ActiveEffect>() { getActiveEffect(r) }, passiveEffects = null, type = ItemType.Potion, actionPoints=5,uses=1 };
            return i;
        }

        public static ActiveEffect getActiveEffect(Random r)
        {
            return new ActiveEffect() {name="Heal",minAmount=5,maxAmount=10,duration=1,statType=StatType.HitPoints };
        }

        public static PassiveEffect getPassiveEffect(Random r)
        {
            return new PassiveEffect() { name = "Regen", minAmount = 1, maxAmount=1, statType = StatType.HitPoints };
        }

        public static Weapon getLongsword(Random r)
        {
            Weapon w = new Weapon() {ID=2,name="Long Sword",minDamage=5,maxDamage=10, type=ItemType.Weapon,actionPoints=2,activeEffects=null,passiveEffects=null,
                                     sheetname = "Weapons",
                                     spriteindex = 12
            };
            return w;
        }

        public static Weapon getDagger(Random r)
        {
            Weapon w = new Weapon() { ID = 3, name = "Dagger", minDamage = 1, maxDamage = 2, type = ItemType.Weapon, actionPoints = 1, activeEffects = null, passiveEffects = null,
                                      sheetname = "Weapons",
                                      spriteindex = 3
            };
            return w;
        }

        public static Weapon getBattleAxe(Random r)
        {
            ActiveEffect flame = new ActiveEffect() { name = "Flame", duration = 2, maxAmount = 10, minAmount = 5, statType = StatType.Damage };
            Weapon w = new Weapon() { ID = 4, name = "Flaming Battle Axe", minDamage = 10, maxDamage = 20, type = ItemType.Weapon, actionPoints = 5, activeEffects = new List<ActiveEffect>() { flame}, passiveEffects = null,
            sheetname="Weapons",spriteindex=69};
            return w;
        }

        public static RangedWeapon getBow(Random r)
        {
            ActiveEffect stun = new ActiveEffect() { name = "Stun", duration = 2, maxAmount = 0, minAmount = 0, statType = StatType.Stun };
            RangedWeapon w = new RangedWeapon()
            {
                ID = 5,
                name = "Stun Bow",
                minDamage = 5,
                maxDamage = 5,
                type = ItemType.Weapon,
                actionPoints = 5,
                activeEffects = new List<ActiveEffect>() { stun},
                passiveEffects = null,
                ammoType = AmmoType.Arrows,
                range = 15,
                weaponType = WeaponType.TwoHandRanged,
                sheetname = "Weapons",
                spriteindex = 61
            };

            return w;
        }

        public static Ammo getArrow(Random r)
        {
            ActiveEffect poison = new ActiveEffect() { name = "Weak Poison", duration = 3, maxAmount = 2, minAmount = 1, statType = StatType.Damage };
            Ammo a = new Ammo() { ID = 20, activeEffects = new List<ActiveEffect>() { poison}, ammoType = AmmoType.Arrows, bonusDamage = 5, name = "Poison Arrow", passiveEffects = null, type = ItemType.Ammo,
                                  sheetname = "Weapons",
                                  spriteindex = 82
            };
            return a;
        }

        #region usableItems
        public static UsableItem getGrenade(Random r)
        {
            //Get an existing ability, but make it free to use
            Ability grenadeAbility = AbilityHelper.getAbilityFree(AbilityFactory.getFireball());
            UsableItem item = new UsableItem() { ID = 30, name = "Grenade", actionPoints = 5, activeEffects = null, itemAbility = grenadeAbility, passiveEffects = null, type = ItemType.Thrown, uses = 1,
                                                 sheetname = "Weapons",
                                                 spriteindex = 54
            };

            return item;
        }

        public static UsableItem getMissileWand(Random r)
        {
            Ability magicMissile = AbilityHelper.getAbilityFree(AbilityFactory.getMagicMissile());
            UsableItem wand = new UsableItem()
            {
                ID = 6,
                name = "Wand of Magic Missile",
                actionPoints = 5,
                itemAbility = magicMissile,
                passiveEffects = null,
                type = ItemType.Wand,
                activeEffects = null,
                uses = 10,
                sheetname="Wands",
                spriteindex=0
            };

            return wand;
        }
        #endregion

        #region Armor

        public static Armor getChainmail(Random r)
        {
            Armor a = new Armor() { ID = 7, name = "Chain Mail", activeEffects = null, armor = 10, passiveEffects = null, type = ItemType.Armor, armorType = ArmorType.Chest,
                                    sheetname = "Armor",
                                    spriteindex = 45
            };
            return a;
        }

        public static Armor getLeatherChest(Random r)
        {
            Armor a = new Armor() { ID = 8, name = "Leather Chest", activeEffects = null, armor = 5, passiveEffects = null, type = ItemType.Armor, armorType = ArmorType.Chest,
                                    sheetname = "Armor",
                                    spriteindex = 38
            };
            return a;
        }

        public static Armor getGreathelm(Random r)
        {
            Armor a = new Armor() { ID = 9, name = "Great Helm", activeEffects = null, armor = 5, passiveEffects = null, type = ItemType.Armor, armorType = ArmorType.Head,
                                    sheetname = "Armor",
                                    spriteindex = 14
            };
            return a;
        }

        public static Armor getCap(Random r)
        {
            Armor a = new Armor() { ID = 10, name = "Leather Cap", activeEffects = null, armor = 1, passiveEffects = null, type = ItemType.Armor, armorType = ArmorType.Head,
                                    sheetname = "Armor",
                                    spriteindex = 10
            };
            return a;
        }

        public static Armor getRegenRing(Random r)
        {
           
            ActiveEffect regenEffect = new ActiveEffect(){name="Regeneration",minAmount=1,maxAmount=1,statType=StatType.Heal};

            Armor a = new Armor()
            {
                ID = 11,
                name = "Regeneration Ring",
                activeEffects = new List<ActiveEffect>() { regenEffect },
                armor = 0, 
                passiveEffects =null,
                type = ItemType.Armor, armorType = ArmorType.Ring,
                sheetname = "Jewels",
                spriteindex = 23
            };
            return a;
        }

        public static Armor getAttackRing(Random r)
        {
            PassiveEffect attackEffect = new PassiveEffect() { name = "Attack Ring", minAmount = 5, maxAmount = 5, statType = StatType.Attack };

            Armor a = new Armor() { ID = 12, name = "Attack Ring", activeEffects = null, armor = 0, passiveEffects = new List<PassiveEffect>() { attackEffect }, type = ItemType.Armor, armorType = ArmorType.Ring,
                                    sheetname = "Jewels",
                                    spriteindex = 5
            };
            return a;
        }


        #endregion
    }
}
