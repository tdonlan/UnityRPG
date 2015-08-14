using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class ItemFactory
    {

        //return an item, given an index of an unknown type (for inventory, etc)
        //should we use a lookup for index range, to know what list to use?
        public static Item getItemFromIndex(long index, GameDataSet gameDataSet)
        {
            if (index <= GameConstants.ITEMS_MAX_INDEX)
            {
                return getItemFromItemData(gameDataSet.itemDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
            }
            else if (index <= GameConstants.USABLEITEMS_MAX_INDEX)
            {
                return getUsableItemFromData(gameDataSet.usableItemDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
            }
            else if (index <= GameConstants.WEAPONS_MAX_INDEX)
            {
                return getWeaponFromWeaponData(gameDataSet.weaponDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
            }
            else if (index <= GameConstants.RANGEDWEAPONS_MAX_INDEX)
            {
                return getRangedWeaponFromRangedWeaponData(gameDataSet.rangedWeaponDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
            }
            else if (index <= GameConstants.AMMO_MAX_INDEX)
            {
                return getAmmoFromAmmoData(gameDataSet.ammoDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
            }
            else if (index <= GameConstants.ARMOR_MAX_INDEX)
            {
                return getArmorFromArmorData(gameDataSet.armorDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
            }
            else
            {
                return null;
            }
        }

        public static Item getItemFromItemData(ItemData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Item i = new Item()
            {
                ID = data.ID,
                name = data.name,
                sheetname = data.sheetname,
                spriteindex = data.spriteindex,
                type = data.type
            };

            if (data.activeEffects.Count > 0)
            {
                List<ActiveEffect> aeList = new List<ActiveEffect>();

                foreach (long l in data.activeEffects)
                {
                    if (effectDataDictionary.ContainsKey(l))
                    {
                        aeList.Add(AbilityFactory.getActiveEffectFromEffectData(effectDataDictionary[l]));
                    }

                }
                i.activeEffects = aeList;
            }

            if (data.passiveEffects.Count > 0)
            {
                List<PassiveEffect> peList = new List<PassiveEffect>();

                foreach (long l in data.passiveEffects)
                {
                    if (effectDataDictionary.ContainsKey(l))
                    {
                        peList.Add(AbilityFactory.getPassiveEffectFromEffectData(effectDataDictionary[l]));
                    }

                }
                i.passiveEffects = peList;
            }

            return i;
        }

        public static UsableItem getUsableItemFromData(UsableItemData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Item i = getItemFromItemData(data, abilityDataDictionary, effectDataDictionary);
            UsableItem ui = new UsableItem()
            {

                activeEffects = i.activeEffects,
                ID = i.ID,
                name = i.name,
                passiveEffects = i.passiveEffects,
                sheetname = i.sheetname,
                spriteindex = i.spriteindex,
                type = i.type,
                uses = data.uses,
                actionPoints = data.actionPoints,

            };


            if (data.itemAbility.Count > 0)
            {
                List<Ability> abilityList = new List<Ability>();

                foreach (long l in data.itemAbility)
                {
                    if (abilityDataDictionary.ContainsKey(l))
                    {
                        abilityList.Add(AbilityFactory.getAbilityFromAbilityData(abilityDataDictionary[l], effectDataDictionary));
                    }

                }
                ui.itemAbility = abilityList.FirstOrDefault(); //hack?  can we have multiple abilities on usable items?
            }
            return ui;
        }

        public static Weapon getWeaponFromWeaponData(WeaponData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Item i = getItemFromItemData(data, abilityDataDictionary, effectDataDictionary);

            Weapon w = new Weapon()
            {
                actionPoints = data.actionPoints,
                activeEffects = i.activeEffects,
                ID = i.ID,
                maxDamage = data.maxDamage,
                minDamage = data.minDamage,
                name = i.name,
                passiveEffects = i.passiveEffects,
                sheetname = i.sheetname,
                spriteindex = i.spriteindex,
                type = i.type,
                weaponType = data.weaponType

            };
          

            return w;
        }
        
        public static RangedWeapon getRangedWeaponFromRangedWeaponData(RangedWeaponData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Weapon w = getWeaponFromWeaponData(data, abilityDataDictionary, effectDataDictionary);
            RangedWeapon rw = new RangedWeapon()
            {
                ID = w.ID,
                name = w.name,
                actionPoints = w.actionPoints,
                activeEffects = w.activeEffects,
                maxDamage = w.maxDamage,
                minDamage = w.minDamage,
                passiveEffects = w.passiveEffects,
                sheetname = w.sheetname,
                spriteindex = w.spriteindex,
                type = w.type,
                weaponType = w.weaponType,

                range = data.range,
                ammoType = data.ammoType
            };

            return rw;
        }

        public static Ammo getAmmoFromAmmoData(AmmoData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Item i = getItemFromItemData(data, abilityDataDictionary, effectDataDictionary);
            Ammo a = new Ammo()
            {
                activeEffects = i.activeEffects,
                ammoType = data.ammoType,
                bonusDamage = data.bonusDamage,
                ID = i.ID,
                name = i.name,
                passiveEffects = i.passiveEffects,
                sheetname = i.sheetname,
                spriteindex = i.spriteindex,
                type = i.type
            };
            

            return a;
        }

        public static Armor getArmorFromArmorData(ArmorData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Item i = getItemFromItemData(data, abilityDataDictionary, effectDataDictionary);

            Armor a = new Armor()
            {
                activeEffects = i.activeEffects,
                armor = data.armor,
                armorType = data.armorType,
                ID = i.ID,
                name = i.name,
                passiveEffects = i.passiveEffects,
                sheetname = i.sheetname,
                spriteindex = i.spriteindex,
                type = i.type
            };
           
            return a;
        }


        #region DialogTestStuff
        //DIALOG TEST STUFF
        //---------------------

        public static int itemCounter = 1;
        public static Item getItem(string name)
        {
            return new Item() { ID = itemCounter++, name = name };
        }

        public static Dictionary<long, Item> getItemDictionary()
        {
            Dictionary<long, Item> itemDictionary = new Dictionary<long, Item>();
            List<string> itemNameList = new List<string>() { "Gold", "Gold Tooth", "Old Key", "Red Gem" };
            foreach (var s in itemNameList)
            {
                Item tempItem = ItemFactory.getItem(s);
                itemDictionary.Add(tempItem.ID, tempItem);
            }

            return itemDictionary;
        }

        #endregion


        //---------------------

        #region OldFactory

        public static List<Item> getItemListFromStrList(List<string> strList, Random r)
        {
            List<Item> retvalList = new List<Item>();
            foreach(var str in strList)
            {
                switch (str)
                {
                    case "HealingPotion": retvalList.Add((Item)getHealingPotion(r)); break;

                    case "Longsword": retvalList.Add((Item)getLongsword(r)); break;

                    case "Dagger": retvalList.Add((Item)getDagger(r)); break;
                    case "BattleAxe": retvalList.Add((Item)getBattleAxe(r)); break;
                    case "Mace": retvalList.Add((Item)getMace(r)); break;
                    case "Bow": retvalList.Add((Item)getBow(r)); break;
                    case "PoisonArrow": retvalList.Add((Item)getPoisonArrow(r)); break;
                    case "Grenade": retvalList.Add((Item)getGrenade(r)); break;
                    case "MissileWand": retvalList.Add((Item)getMissileWand(r)); break;
                    case "Chainmail": retvalList.Add((Item)getChainmail(r)); break;
                    case "LeatherChest": retvalList.Add((Item)getLeatherChest(r)); break;
                    case "Greathelm": retvalList.Add((Item)getGreathelm(r)); break;
                    case "Cap": retvalList.Add((Item)getCap(r)); break;
                    case "RegenRing": retvalList.Add((Item)getRegenRing(r)); break;
                    case "AttackRing": retvalList.Add((Item)getAttackRing(r)); break;

                    default:
                        break;

                }
            }
            return retvalList;
        }


        public static UsableItem getHealingPotion(Random r)
        {
            ActiveEffect heal = new ActiveEffect()
            {
                name = "Heal",
                duration = 1,
                minAmount = 10,
                maxAmount = 10,
                statType = StatType.Heal,
                effectName = "Potions",
                effectIndex = 18

            };
            UsableItem i = new UsableItem() { ID = 1, name = "Healing Potion", activeEffects = new List<ActiveEffect>() { heal }, passiveEffects = null, type = ItemType.Potion, actionPoints = 5, uses = 1, sheetname = "Potions", spriteindex = 18 };
            return i;
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

        public static Weapon getMace(Random r)
        {
            Weapon w = new Weapon()
            {
                ID = 101,
                name = "Holy Mace",
                minDamage = 2,
                maxDamage = 12,
                type = ItemType.Weapon,
                actionPoints = 5,
                activeEffects = null,
                passiveEffects = null,
                sheetname = "Weapons",
                spriteindex = 24
            };
            return w;
        }

        public static Weapon getDragonClaw(Random r)
        {
            Weapon w = new Weapon()
            {
                ID = 102,
                name = "Dragon Claw",
                minDamage = 20,
                maxDamage = 50,
                type = ItemType.Weapon,
                actionPoints = 10,
                activeEffects = null,
                passiveEffects = null,
                sheetname = "Weapons",
                spriteindex = 17

            };
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

        public static Ammo getPoisonArrow(Random r)
        {
            ActiveEffect poison = new ActiveEffect() { name = "Weak Poison", duration = 3, maxAmount = 2, minAmount = 1, statType = StatType.Damage };
            Ammo a = new Ammo() { ID = 20, activeEffects = new List<ActiveEffect>() { poison}, ammoType = AmmoType.Arrows, bonusDamage = 5, name = "Poison Arrow", passiveEffects = null, type = ItemType.Ammo,
                                  sheetname = "Weapons",
                                  spriteindex = 82
            };
            return a;
        }


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
