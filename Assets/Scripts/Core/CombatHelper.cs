using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class CombatHelper
    {
        public static bool Attack(GameCharacter attacker, GameCharacter defender, BattleGame game)
        {
            if(game.r.Next(20) + attacker.attack > defender.ac)
            {

                var tempTile = game.board.getTileFromLocation(defender.x, defender.y);
                game.board.AddTempChar(tempTile, '*');
                game.board.AddTempEffect(tempTile, GameConstants.weaponAttackSpritesheet, GameConstants.weaponAttackSpriteindex);
                
                
                return Hit(attacker, defender, game,null);
            }
            else
            {
                game.battleLog.AddEntry(string.Format("{0} missed {1}.", attacker.name, defender.name));
                return false;
            }
        }

        public static bool RangedAttack(GameCharacter attacker, GameCharacter defender, Tile targetTile, BattleGame game)
        {
            bool retval = false;
            //check for ranged weapon
            if(attacker.weapon is RangedWeapon)
            {
                RangedWeapon w = (RangedWeapon)attacker.weapon;
                Ammo a = (Ammo)ItemHelper.getFirstItemWithID(attacker.inventory,attacker.Ammo.itemID);

                //check we have ammo 
                if(attacker.Ammo.count > 0 && a.ammoType == w.ammoType)
                {

                    List<Tile> tileLOSList = game.board.getBoardLOS(game.ActiveTile, targetTile);

                    //Draw Attack temp path
                    foreach (var t in tileLOSList)
                    {
                        game.board.AddTempChar(t, '*');
                        game.board.AddTempEffect(t, GameConstants.rangedAttackSpritesheet, GameConstants.rangedAttackSpriteindex);
                    }

                    //check LOS
                    //check range
                    if (tileLOSList[tileLOSList.Count - 1] == targetTile )
                    {
                        if (tileLOSList.Count <= w.range)
                        {
                            if (attacker.SpendAP(attacker.weapon.actionPoints))
                            {
                                //check for hit
                                if (game.r.Next(20) + attacker.attack > defender.ac)
                                {
                                    retval = Hit(attacker, defender, game,a);

                                    //remove ammo
                                    attacker.inventory.Remove(a);
                                    attacker.Ammo = ItemHelper.getItemSet(attacker.inventory, a);

                                    retval = true;
                                }
                                else
                                {
                                    game.battleLog.AddEntry(string.Format("{0} missed {1}.", attacker.name, defender.name));
                                }
                            }
                        }
                        else
                        {
                            game.battleLog.AddEntry(string.Format("{0} is out of range.", defender.name));
                        }
                    }
                    else
                    {
                        game.battleLog.AddEntry(string.Format("Unable to hit {0}", defender.name));
                    }
                }
                else
                {
                    game.battleLog.AddEntry(string.Format("{0} requires {1} ammo equipped", w.name,w.ammoType));
                }
            }
            else
            {
                game.battleLog.AddEntry(string.Format("Equip a ranged weapon for ranged attack"));
            }
           
            return retval;
        }

        private static bool Hit(GameCharacter attacker, GameCharacter defender, BattleGame game, Ammo ammo)
        {
            int bonusDamage = 0;
            if(ammo != null)
            {
                bonusDamage = ammo.bonusDamage;

                if (ammo.activeEffects != null)
                {
                    foreach (var ae in ammo.activeEffects)
                    {
                        defender.AddActiveEffect(AbilityHelper.cloneActiveEffect(ae), game);
                    }
                }
            }

            int dmg = game.r.Next(attacker.weapon.minDamage, attacker.weapon.maxDamage) + bonusDamage;

            defender.Damage(dmg, game);

            game.battleLog.AddEntry(string.Format("{0} hit {1} for {2} damage.", attacker.name, defender.name, dmg));

            if(attacker.weapon.activeEffects != null)
            {
                foreach(var ae in attacker.weapon.activeEffects)
                {
                    defender.AddActiveEffect(AbilityHelper.cloneActiveEffect(ae), game);
                }
            }

            

            return true;
        }
    }
}
