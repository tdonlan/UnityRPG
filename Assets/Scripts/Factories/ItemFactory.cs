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
                if (gameDataSet.itemDataDictionary.ContainsKey(index))
                {
                    return getItemFromItemData(gameDataSet.itemDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
                }
               
            }
            else if (index <= GameConstants.USABLEITEMS_MAX_INDEX)
            {
                if (gameDataSet.usableItemDataDictionary.ContainsKey(index))
                {
                    return getUsableItemFromData(gameDataSet.usableItemDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
                }
            }
            else if (index <= GameConstants.WEAPONS_MAX_INDEX)
            {
                if (gameDataSet.weaponDataDictionary.ContainsKey(index))
                {
                    return getWeaponFromWeaponData(gameDataSet.weaponDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
                }
            }
            else if (index <= GameConstants.RANGEDWEAPONS_MAX_INDEX)
            {
                if (gameDataSet.rangedWeaponDataDictionary.ContainsKey(index))
                {
                    return getRangedWeaponFromRangedWeaponData(gameDataSet.rangedWeaponDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
                }
            }
            else if (index <= GameConstants.AMMO_MAX_INDEX)
            {
                if (gameDataSet.ammoDataDictionary.ContainsKey(index))
                {
                    return getAmmoFromAmmoData(gameDataSet.ammoDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
                }
            }
            else if (index <= GameConstants.ARMOR_MAX_INDEX)
            {
                if (gameDataSet.armorDataDictionary.ContainsKey(index))
                {
                    return getArmorFromArmorData(gameDataSet.armorDataDictionary[index], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary);
                }
            }
            
                return null;
            
        }

        public static Item getItemFromItemData(ItemData data, Dictionary<long, AbilityData> abilityDataDictionary, Dictionary<long, EffectData> effectDataDictionary)
        {
            Item i = new Item()
            {
                ID = data.ID,
                name = data.name,
                sheetname = data.sheetname,
                spriteindex = data.spriteindex,
                type = data.type,
                price = data.price
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
                price = data.price

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
                actionPoints = data.AP,
                activeEffects = i.activeEffects,
                ID = i.ID,
                maxDamage = data.maxDamage,
                minDamage = data.minDamage,
                name = i.name,
                passiveEffects = i.passiveEffects,
                sheetname = i.sheetname,
                spriteindex = i.spriteindex,
                type = i.type,
                weaponType = data.weaponType,
                price = data.price

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
                ammoType = data.ammoType,
                price = data.price
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
                type = i.type,
                price = data.price
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
                type = i.type,
                price = data.price
            };
           
            return a;
        }

    }
}
