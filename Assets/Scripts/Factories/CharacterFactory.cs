using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class CharacterFactory
    {
        public static GameCharacter getEnemy(Random r)
        {
            GameCharacter retval = new GameCharacter() {name="Goblin",displayChar='G',type=CharacterType.Enemy, ac = 10, attack = 5, totalHP = 50, hp = 50, ap=10, totalAP=10 };
            retval.weapon = ItemFactory.getLongsword(r);
            return retval;
        }

        public static EnemyCharacter getGoblin(Random r)
        {
            EnemyCharacter goblin = new EnemyCharacter() {name="Goblin",displayChar='G',type= CharacterType.Enemy,ac=10,attack =5,totalHP=25,hp=25,totalAP=10,ap=10,enemyType=EnemyType.Warrior };
            goblin.weapon = ItemFactory.getLongsword(r);
            return goblin;
        }

        public static GameCharacter getPlayerCharacter(Random r)
        {
            GameCharacter retval =  new GameCharacter() {name="Warrior",displayChar='@',type=CharacterType.Player, ac = 10, attack = 50, totalHP = 50, hp = 20,ap=10,totalAP=10 };
        
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
                retval.inventory.Add(ItemFactory.getArrow(r));
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
