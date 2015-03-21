using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class AbilityFactory
    {

        public static List<Ability> getAbilityListFromStrList(List<string> strList)
        {
            List<Ability> retvalList = new List<Ability>();
            foreach(var str in strList)
            {
                switch(str)
                {
                    case "Fireball": retvalList.Add(getFireball()); break;
                    case "MagicMissile": retvalList.Add(getMagicMissile()); break;
                    case "Heal": retvalList.Add(getHeal()); break;
                    case "GroupHeal": retvalList.Add(getGroupHeal()); break;
                    case "Teleport": retvalList.Add(getTeleport()); break;
                    case "Knockback": retvalList.Add(getKnockback()); break;
                    case "Charge": retvalList.Add(getCharge()); break;
                    case "Grenade": retvalList.Add(getGrenade()); break;
                    case "Haste": retvalList.Add(getHaste()); break;
                    case "Shield": retvalList.Add(getShield()); break;
                    case "Rage": retvalList.Add(getRage()); break;
                    case "Web": retvalList.Add(getWeb()); break;
                    case "DispellMagic": retvalList.Add(getDispellMagic()); break;
                    case "Slow": retvalList.Add(getSlow()); break;
                    case "Stun": retvalList.Add(getStun()); break;
                    case "Poison": retvalList.Add(getPoison()); break;

                    default:
                        break;
                        
                }
            }
            return retvalList;
        }

        public static Ability getFireball()
        {
            ActiveEffect fireballEffect = new ActiveEffect() { name = "Fireball", duration = 1, minAmount = 100,maxAmount=200, statType = StatType.Damage,
            sheetname="Particles",spriteindex=68};

            Ability fireball = new Ability()
            {
                name = "Fireball",
                description = "Send a large ball of flame into a crowd of foes",
                ap = 5,
                range = 20,
                uses = 5,
                targetType = AbilityTargetType.LOSTarget,
                tilePatternType = TilePatternType.NineSquare,
                activeEffects = new List<ActiveEffect>() {fireballEffect },
                passiveEffects=null,
                sheetname="Particles",
                spriteindex=68
            };

            return fireball;
        }

        public static Ability getMagicMissile()
        {
            ActiveEffect missileEffect = new ActiveEffect() { name = "Magic Missile", duration = 1, minAmount = 1, maxAmount = 6, statType = StatType.Damage, sheetname = "Particles", spriteindex = 33 };
            Ability magicMissile = new Ability()
            {
                name= "Magic Missile",
                description = "Shoot three magic missiles to a target",
                ap =5,
                range = 20,
                uses = 5,
                targetType = AbilityTargetType.LOSTarget,
                tilePatternType = TilePatternType.Single,
                activeEffects = new List<ActiveEffect>(){missileEffect,missileEffect,missileEffect},
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 33
            };

            return magicMissile;
        }

        public static Ability getHeal()
        {
            ActiveEffect healEffect = new ActiveEffect() { name = "Heal", duration = 1, minAmount = 5, maxAmount = 10, statType = StatType.Heal, sheetname = "Particles", spriteindex = 97 };
            Ability heal = new Ability()
            {
                name = "Heal",
                description = "Heal Self",
                ap = 5,
                range = 1,
                uses = 1,
                targetType = AbilityTargetType.Self,
                tilePatternType = TilePatternType.Single,
                activeEffects = new List<ActiveEffect>() { healEffect },
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 97
            };

            return heal;
        }


        public static Ability getGroupHeal()
        {
            ActiveEffect healEffect = new ActiveEffect() { name = "Group Heal", duration = 1, minAmount = 10, maxAmount = 20, statType = StatType.Heal, sheetname = "Particles", spriteindex = 97 };
            Ability groupHeal = new Ability()
            {
                name = "Group Heal",
                description = "Heal all Allies",
                ap = 10,
                range = 20,
                uses = 1,
                targetType = AbilityTargetType.AllFriends,
                tilePatternType = TilePatternType.Single,
                activeEffects = new List<ActiveEffect>() { healEffect },
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 97

            };
            return groupHeal;
        }


        public static Ability getTeleport()
        {
            ActiveEffect teleportEffect = new ActiveEffect() { name = "Teleport", duration = 1, minAmount = 0, maxAmount = 0, statType = StatType.Teleport, sheetname = "Particles", spriteindex = 104 };
            Ability teleport = new Ability()
            {
                name = "Teleport",
                description = "Teleport to a selected location on the map",
                ap = 10,
                range = 10,
                uses = 10,
                targetType = AbilityTargetType.PointEmpty,
                tilePatternType = TilePatternType.Single,
                activeEffects = new List<ActiveEffect>() { teleportEffect},
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 104
            };

            return teleport;
        }

        public static Ability getKnockback()
        {
            ActiveEffect knockbackEffect = new ActiveEffect() { name = "Knockback", duration = 1, minAmount = 1, maxAmount = 1, statType = StatType.Knockback, sheetname = "Particles", spriteindex = 56 };
            Ability knockback = new Ability()
            {
                name = "Knockback",
                description = "Knockback the target 1 tile",
                ap = 5,
                range = 1,
                uses = 1,
                targetType = AbilityTargetType.SingleFoe,
                tilePatternType = TilePatternType.Single,
                activeEffects = new List<ActiveEffect>() { knockbackEffect },
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 56

            };

            return knockback;


        }

        public static Ability getCharge()
        {
            ActiveEffect chargeEffect = new ActiveEffect() { name = "Charge", duration = 1, minAmount = 5, maxAmount = 5, statType = StatType.Teleport,sheetname = "Particles", spriteindex = 56 };
            ActiveEffect knockbackEffect = new ActiveEffect() { name = "Knockback", duration = 1, minAmount = 1, maxAmount = 1, statType = StatType.Knockback,sheetname = "Particles", spriteindex = 56 };
            ActiveEffect damageEffect = new ActiveEffect() { name = "ChargeDamage", duration = 1, minAmount = 10, maxAmount = 20, statType = StatType.Damage, sheetname = "Particles", spriteindex = 102 };

            Ability charge = new Ability()
            {
                name = "Charge",
                description = "Charge in a straight line towards an enemy, causing damage",
                ap = 5,
                range = 5,
                uses = 1,
                targetType = AbilityTargetType.LOSTarget,
                tilePatternType = TilePatternType.Single,
                activeEffects = new List<ActiveEffect>() { knockbackEffect, chargeEffect, damageEffect },
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 56
            };

            return charge;
        }

        public static Ability getGrenade()
        {
            ActiveEffect explodeEffect = new ActiveEffect() { name = "Explode", duration = 1, minAmount = 2, maxAmount = 3, statType = StatType.Explode, sheetname = "Particles", spriteindex = 55 };
            ActiveEffect fireEffect = new ActiveEffect() { name = "Fire", duration = 1, minAmount = 5, maxAmount = 10, statType = StatType.Damage, sheetname = "Particles", spriteindex = 68 };

            Ability grenade = new Ability()
            {
                name = "Grenade",
                description = "A grenade explodes, blasting back enemies and causing fire damage",
                ap = 5,
                range = 10,
                uses = 1,
                targetType = AbilityTargetType.LOSTarget,
                tilePatternType = TilePatternType.NineSquare,
                activeEffects = new List<ActiveEffect>() { fireEffect, explodeEffect },
                passiveEffects =null,
                sheetname = "Particles",
                spriteindex = 68

            };

            return grenade;
        }

        public static Ability getHaste()
        {
            ActiveEffect hasteEffect = new ActiveEffect()
            {
                name = "Haste",
                duration = 3,
                minAmount = 5,
                maxAmount = 5,
                statType = StatType.ActionPoints,
                sheetname = "Particles",
                spriteindex = 27
            };

            Ability haste = new Ability()
            {
                name = "Haste",
                description = "Addition Action Points per turn",
                ap = 1,
                range = 1,
                targetType = AbilityTargetType.SingleFriend,
                tilePatternType = TilePatternType.Single,
                uses = 1,
                activeEffects = new List<ActiveEffect>() { hasteEffect},
                passiveEffects = null,
                sheetname = "Particles",
                spriteindex = 27

            };

            return haste;
        }

        public static Ability getShield()
        {
            ActiveEffect shieldEffect = new ActiveEffect() { name = "Shield", duration = 2, minAmount = 10, maxAmount = 10, statType = StatType.HitPoints, sheetname = "Particles", spriteindex = 98 };
            ActiveEffect healEffect = new ActiveEffect() { name = "Heal", duration = 1, minAmount = 999, maxAmount = 999, statType = StatType.Heal, sheetname = "Particles", spriteindex = 97 };

            Ability shield = new Ability()
            {
                name = "Shield",
                description = "Summon a temporary shield to protect you" ,
                activeEffects = new List<ActiveEffect>() { shieldEffect, healEffect },
                passiveEffects = null,
                ap = 5,
                range = 1,
                targetType = AbilityTargetType.Self,
                tilePatternType = TilePatternType.Single,
                uses = 1,
                sheetname="Particles",spriteindex=98
            };

            return shield;
        }

        public static Ability getRage()
        {
            ActiveEffect rageEffect = new ActiveEffect() { name = "Rage", duration = 2, minAmount = 5, maxAmount = 5, statType = StatType.Attack, sheetname = "Particles", spriteindex = 102 };

            Ability rage = new Ability()
            {
                name = "Rage",
                description = "Attack with a fury",
                uses = 1,
                ap = 5,
                activeEffects = new List<ActiveEffect>() { rageEffect },
                passiveEffects = null,
                range = 1,
                targetType = AbilityTargetType.Self,
                tilePatternType = TilePatternType.Single,
                sheetname = "Particles",
                spriteindex = 102
            };

            return rage;
        }

        public static Ability getWeb()
        {
            ActiveEffect webEffect = new ActiveEffect() { name = "Web", duration = 99, minAmount = 0, maxAmount = 0, statType = StatType.Stuck, sheetname = "Particles", spriteindex = 94 };
            Ability web = new Ability()
            {
                name = "Web",
                description = "Send sticky web to trap a target and prevent movement",
                uses = 5,
                ap = 5,
                activeEffects = new List<ActiveEffect>() { webEffect },
                passiveEffects = null,
                range = 5,
                targetType = AbilityTargetType.LOSTarget,
                tilePatternType = TilePatternType.NineSquare,
                sheetname = "Particles",
                spriteindex = 94
            };

            return web;
        }

        public static Ability getDispellMagic()
        {
            ActiveEffect dispellEffect = new ActiveEffect() { name = "Dispell", duration = 1, minAmount = 1, maxAmount = 1, statType = StatType.Dispell, sheetname = "Particles", spriteindex = 107 };
            Ability dispell = new Ability()
            {
                name = "Dispell",
                description = "Remove 1 active effect from target",
                uses = 10,
                ap = 5,
                activeEffects = new List<ActiveEffect>() { dispellEffect },
                passiveEffects = null,
                range = 5,
                targetType = AbilityTargetType.PointTarget,
                tilePatternType = TilePatternType.Single,
                sheetname = "Particles",
                spriteindex = 107
            };

            return dispell;
        }

        public static Ability getSlow()
        {
            ActiveEffect slowEffect = new ActiveEffect() { name = "Slow", duration = 2, minAmount = -5, maxAmount = -5, statType = StatType.ActionPoints, sheetname = "Particles", spriteindex = 100 };

            Ability slow = new Ability()
            {
                name = "Slow",
                description = "Cause slow by reducing Action Points",
                activeEffects = new List<ActiveEffect>() { slowEffect },
                ap = 5,
                passiveEffects = null,
                range = 5,
                targetType = AbilityTargetType.SingleFoe,
                tilePatternType = TilePatternType.Single,
                uses = 5,
                sheetname = "Particles",
                spriteindex = 100
            };

            return slow;
        }

        public static Ability getStun()
        {
            ActiveEffect stunEffect = new ActiveEffect() { name = "Stun", duration = 5, minAmount = 0, maxAmount = 0, statType = StatType.Stun, sheetname = "Particles", spriteindex = 58 };
            Ability stun = new Ability()
            {
                name = "Stun",
                description = "Prohibit any action by the character",
                activeEffects = new List<ActiveEffect>() { stunEffect },
                ap = 5,
                passiveEffects = null,
                range = 5,
                targetType = AbilityTargetType.SingleFoe,
                tilePatternType = TilePatternType.Single,
                uses = 5,
                sheetname = "Particles",
                spriteindex = 58
            };
            return stun;
        }

        public static Ability getPoison()
        {
            ActiveEffect poisonEffect = new ActiveEffect() { name = "Poison", duration = 99, minAmount = 10, maxAmount = 10, statType = StatType.Damage, sheetname = "Particles", spriteindex = 49 };
            Ability poison = new Ability()
            {
                name = "Poison",
                description = "Dangerous poison damages every round",
                activeEffects = new List<ActiveEffect>() { poisonEffect },
                ap = 1,
                ID = 99,
                passiveEffects = null
                ,
                range = 1,
                targetType = AbilityTargetType.PointTarget,
                tilePatternType = TilePatternType.Single,
                uses = 99,
                sheetname = "Particles",
                spriteindex = 49
            };

            return poison;
        }

    }
}
