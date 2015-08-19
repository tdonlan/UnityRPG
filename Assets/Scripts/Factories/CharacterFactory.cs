using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityRPG
{
    public class CharacterFactory
    {

        public static GameCharacter getGameCharacterFromGameCharacterData(GameCharacterData data, GameDataSet gameDataSet)
        {
            GameCharacter character = new GameCharacter()
            {
                ac = data.ac,
                ap = data.ap,
                attack = data.attack,
                characterSpriteIndex = data.characterSpriteIndex,
                characterSpritesheetName = data.characterSpritesheetName,
                hp = data.hp,
                displayChar = data.displayChar,
                name = data.name,
                portraitSpriteIndex = data.portraitSpriteIndex,
                portraitSpritesheetName = data.portraitSpritesheetName,
                totalAP = data.ap,
                totalHP = data.hp,
                type = data.type,
                x = 0,
                y = 0

            };

            if (data.abilityList.Count > 0)
            {
                List<Ability> abilityList = new List<Ability>();
                foreach (var l in data.abilityList)
                {
                    if(gameDataSet.abilityDataDictionary.ContainsKey(l)){
                         abilityList.Add(AbilityFactory.getAbilityFromAbilityData(gameDataSet.abilityDataDictionary[l],gameDataSet.effectDataDictionary));
                    }
                   
                }
                character.abilityList = abilityList;
            }

            if(data.inventory.Count > 0){
                List<Item> itemList = new List<Item>();
                foreach (var i in data.inventory)
                {
                    Item tempItem = ItemFactory.getItemFromIndex(i, gameDataSet);
                    if (tempItem != null)
                    {
                        itemList.Add(tempItem);
                    }
                    
                }

                character.inventory = itemList;
            }

            if (data.equippedArmor.Count > 0)
            {
                List<Armor> armorList = new List<Armor>();
                foreach (var a in data.equippedArmor)
                {
                    if(gameDataSet.armorDataDictionary.ContainsKey(a)){
                        armorList.Add(ItemFactory.getArmorFromArmorData(gameDataSet.armorDataDictionary[a], gameDataSet.abilityDataDictionary, gameDataSet.effectDataDictionary));
                    }
                  
                }
                character.equippedArmor = armorList;
            }

            if (data.weapon > 0)
            {
                Weapon w = (Weapon)ItemFactory.getItemFromIndex(data.weapon, gameDataSet);
                character.weapon = w;
            }

            if (data.activeEffects.Count > 0)
            {
                List<ActiveEffect> aeList = new List<ActiveEffect>();

                foreach (long l in data.activeEffects)
                {
                    if (gameDataSet.effectDataDictionary.ContainsKey(l))
                    {
                        aeList.Add(AbilityFactory.getActiveEffectFromEffectData(gameDataSet.effectDataDictionary[l]));
                    }
                }
                character.activeEffects = aeList;
            }

            if (data.passiveEffects.Count > 0)
            {
                List<PassiveEffect> peList = new List<PassiveEffect>();

                foreach (long l in data.passiveEffects)
                {
                    if (gameDataSet.effectDataDictionary.ContainsKey(l))
                    {
                        peList.Add(AbilityFactory.getPassiveEffectFromEffectData(gameDataSet.effectDataDictionary[l]));
                    }

                }
                character.passiveEffects = peList;
            }


            if (data.type == CharacterType.Enemy)
            {
                character = getEnemyFromGameCharacter(character, data.enemyType);
            }

            return character;
            
        }

        private static EnemyCharacter getEnemyFromGameCharacter(GameCharacter gameChar, EnemyType enemyType)
        {
            EnemyCharacter enemyChar = new EnemyCharacter(enemyType)
            {
                abilityList = gameChar.abilityList,
                ac = gameChar.ac,
                activeEffects = gameChar.activeEffects,
                Ammo = gameChar.Ammo,
                ap = gameChar.ap,
                attack = gameChar.attack,
                characterSpriteIndex = gameChar.characterSpriteIndex,
                characterSpritesheetName = gameChar.characterSpritesheetName,
                displayChar = gameChar.displayChar,
                enemyType = enemyType,
                equippedArmor = gameChar.equippedArmor,
                hp = gameChar.hp,
                inventory = gameChar.inventory,
                name = gameChar.name,
                passiveEffects = gameChar.passiveEffects,
                portraitSpriteIndex = gameChar.portraitSpriteIndex,
                portraitSpritesheetName = gameChar.portraitSpritesheetName,
                totalAP = gameChar.totalAP,
                totalHP = gameChar.totalHP,
                type = CharacterType.Enemy,
                weapon = gameChar.weapon,
                x = gameChar.x,
                y = gameChar.y
            };

            return enemyChar;
        }


    }
}
