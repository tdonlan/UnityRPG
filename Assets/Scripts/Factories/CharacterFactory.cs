using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class CharacterFactory
    {

        private static EnemyCharacter getEnemyFromGameCharacter(GameCharacter gameChar, EnemyType enemyType)
        {
            EnemyCharacter enemyChar = new EnemyCharacter()
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


        public static GameCharacter getEnemy(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Goblin",
                displayChar = 'G',
                type = CharacterType.Enemy,
                ac = 10,
                attack = 5,
                totalHP = 50,
                hp = 50,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 85,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex=98
            };
            retval.weapon = ItemFactory.getLongsword(r);
            return retval;
        }

        public static EnemyCharacter getGoblin(Random r)
        {
            EnemyCharacter goblin = new EnemyCharacter()
            {
                name = "Goblin",
                displayChar = 'G',
                type = CharacterType.Enemy,
                ac = 10,
                attack = 5,
                totalHP = 25,
                hp = 25,
                totalAP = 10,
                ap = 10,
                enemyType = EnemyType.Warrior,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 85,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 98
            };
            goblin.weapon = ItemFactory.getLongsword(r);
            return goblin;
        }

        public static EnemyCharacter getEnemyWarrior(Random r)
        {
            return getEnemyFromGameCharacter(getWarrior(r), EnemyType.Warrior);
        }

        public static EnemyCharacter getEnemyMage(Random r)
        {
            return getEnemyFromGameCharacter(getMage(r), EnemyType.Warrior);
        }

        public static EnemyCharacter getEnemyPriest(Random r)
        {
            return getEnemyFromGameCharacter(getPriest(r), EnemyType.Warrior);
        }


        public static EnemyCharacter getEnemyArcher(Random r)
        {
            return getEnemyFromGameCharacter(getArcher(r), EnemyType.Warrior);
        }

        public static EnemyCharacter getDragon(Random r)
        {
            EnemyCharacter dragon = new EnemyCharacter()
            {
                name = "Dragon",
                displayChar = 'D',
                type = CharacterType.Enemy,
                ac = 20,
                attack = 25,
                totalHP = 200,
                hp = 200,
                totalAP = 15,
                ap = 15,
                enemyType = EnemyType.Warrior,
                characterSpritesheetName = "Dragons",
                characterSpriteIndex = 32,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 152
            };

            List<string> abilityList = new List<string>(){"Fireball"};
            dragon.abilityList.AddRange(AbilityFactory.getAbilityListFromStrList(abilityList));

            dragon.weapon = ItemFactory.getDragonClaw(r);

            return dragon;
        }
             
        public static GameCharacter getWarrior(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Warrior",
                displayChar = '@',
                type = CharacterType.Player,
                ac = 10,
                attack = 50,
                totalHP = 50,
                hp = 20,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 13,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 0
            };

            List<string> itemList = new List<string>() { "HealingPotion", "HealingPotion", "Chainmail", "Greathelm", "BattleAxe", "AttackRing" };
            retval.inventory.AddRange(ItemFactory.getItemListFromStrList(itemList,r));
            retval.weapon = ItemFactory.getLongsword(r);

            List<string> abilityList = new List<string>() { "Knockback", "Rage", "Charge", "Stun" };
            retval.abilityList.AddRange(AbilityFactory.getAbilityListFromStrList(abilityList));

            return retval;
        }

        public static GameCharacter getRogue(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Rogue",
                displayChar = '@',
                type = CharacterType.Player,
                ac = 10,
                attack = 10,
                totalHP = 25,
                hp = 25,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 22,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 8
            };

            List<string> itemList = new List<string>() { "HealingPotion", "LeatherChest", "Cap", "AttackRing" };
            retval.inventory.AddRange(ItemFactory.getItemListFromStrList(itemList, r));
            retval.weapon = ItemFactory.getDagger(r);

            List<string> abilityList = new List<string>() { "Teleport", "Haste", "Stun", "Poison" };
            retval.abilityList.AddRange(AbilityFactory.getAbilityListFromStrList(abilityList));

            return retval;
        }

        public static GameCharacter getMage(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Mage",
                displayChar = '@',
                type = CharacterType.Player,
                ac = 10,
                attack = 10,
                totalHP = 10,
                hp = 10,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 1,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 40
            };

            List<string> itemList = new List<string>() { "MissileWand","Dagger","HealingPotion" };
            retval.inventory.AddRange(ItemFactory.getItemListFromStrList(itemList, r));

            List<string> abilityList = new List<string>() { "Teleport", "Fireball", "MagicMissile", "Shield","Web","DispellMagic","Slow" };
            retval.abilityList.AddRange(AbilityFactory.getAbilityListFromStrList(abilityList));

            return retval;
        }

        public static GameCharacter getPriest(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Priest",
                displayChar = '@',
                type = CharacterType.Player,
                ac = 10,
                attack = 10,
                totalHP = 30,
                hp = 30,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 44,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 44
            };

            List<string> itemList = new List<string>() { "HealingPotion", "HealingPotion", "Grenade", "Chainmail", "Greathelm", "RegenRing" };
            retval.inventory.AddRange(ItemFactory.getItemListFromStrList(itemList, r));
            retval.weapon = ItemFactory.getMace(r);

            List<string> abilityList = new List<string>() { "Heal","GroupHeal","Knockback","Charge","Shield","Stun","Slow" };
            retval.abilityList.AddRange(AbilityFactory.getAbilityListFromStrList(abilityList));

            return retval;
        }

        public static GameCharacter getArcher(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Archer",
                displayChar = '@',
                type = CharacterType.Player,
                ac = 10,
                attack = 10,
                totalHP = 25,
                hp = 25,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 38,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 21
            };

            List<string> itemList = new List<string>() { "HealingPotion", "LeatherChest","Cap","AttackRing"};
            retval.inventory.AddRange(ItemFactory.getItemListFromStrList(itemList, r));
            for (int i = 0; i < 10;i++ )
            {
                retval.inventory.Add(ItemFactory.getPoisonArrow(r));
            }
            retval.weapon = ItemFactory.getBow(r);

            List<string> abilityList = new List<string>() { "Web","Slow","Stun"};
            retval.abilityList.AddRange(AbilityFactory.getAbilityListFromStrList(abilityList));

            return retval;
        }


        public static GameCharacter getPlayerCharacter(Random r)
        {
            GameCharacter retval = new GameCharacter()
            {
                name = "Warrior",
                displayChar = '@',
                type = CharacterType.Player,
                ac = 10,
                attack = 50,
                totalHP = 50,
                hp = 20,
                ap = 10,
                totalAP = 10,
                characterSpritesheetName = "Characters",
                characterSpriteIndex = 13,
                portraitSpritesheetName = "Portraits",
                portraitSpriteIndex = 0
            };
        
            retval.inventory.Add(ItemFactory.getHealingPotion(r));
            retval.inventory.Add(ItemFactory.getHealingPotion(r));
            retval.inventory.Add(ItemFactory.getHealingPotion(r));
            retval.inventory.Add(ItemFactory.getHealingPotion(r));
            retval.inventory.Add(ItemFactory.getHealingPotion(r));

            //Weapons
            retval.weapon = ItemFactory.getLongsword(r);

            retval.inventory.Add(ItemFactory.getDagger(r));
            retval.inventory.Add(ItemFactory.getBattleAxe(r));
            retval.inventory.Add(ItemFactory.getBow(r));

            //Armor
            retval.inventory.Add(ItemFactory.getLeatherChest(r));
            retval.inventory.Add(ItemFactory.getChainmail(r));
            retval.inventory.Add(ItemFactory.getGreathelm(r));
            retval.inventory.Add(ItemFactory.getCap(r));
            retval.inventory.Add(ItemFactory.getAttackRing(r));
            retval.inventory.Add(ItemFactory.getRegenRing(r));

            //other
            retval.inventory.Add(ItemFactory.getGrenade(r));
            retval.inventory.Add(ItemFactory.getMissileWand(r));

            for(int i=0;i<5;i++)
            {
                retval.inventory.Add(ItemFactory.getPoisonArrow(r));
            }

            //equipping ammo
            //retval.Ammo = ItemHelper.getItemSet(retval.inventory, ItemFactory.getArrow(r));
          

            //Abilities
            /*
 
          
            retval.abilityList.Add(AbilityFactory.getKnockback());
            retval.abilityList.Add(AbilityFactory.getCharge());
            retval.abilityList.Add(AbilityFactory.getGrenade());
            retval.abilityList.Add(AbilityFactory.getShield());
            retval.abilityList.Add(AbilityFactory.getRage());
            */
            retval.abilityList.Add(AbilityFactory.getFireball());
            retval.abilityList.Add(AbilityFactory.getWeb());
            retval.abilityList.Add(AbilityFactory.getDispellMagic());
            retval.abilityList.Add(AbilityFactory.getStun());
            retval.abilityList.Add(AbilityFactory.getTeleport());
            retval.abilityList.Add(AbilityFactory.getPoison());
           

            return retval;
        }

    }
}
