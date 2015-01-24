using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class AbilityFactory
    {

        public static Ability getFireball()
        {
            ActiveEffect fireballEffect = new ActiveEffect() { name = "Fireball", duration = 1, minAmount = 100,maxAmount=200, statType = StatType.Damage };

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
                passiveEffects=null
            };

            return fireball;
        }

        public static Ability getMagicMissile()
        {
            ActiveEffect missileEffect = new ActiveEffect(){name ="Magic Missile",duration=1,minAmount=1,maxAmount=6,statType=StatType.Damage};
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
                passiveEffects = null
            };

            return magicMissile;
        }

        public static Ability getHeal()
        {
            ActiveEffect healEffect = new ActiveEffect() { name = "Heal", duration = 1, minAmount = 5,maxAmount=10, statType = StatType.Heal };
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
                passiveEffects = null
            };

            return heal;
        }

        public static Ability getTeleport()
        {
            ActiveEffect teleportEffect = new ActiveEffect() { name = "Teleport", duration = 1, minAmount = 0,maxAmount=0, statType = StatType.Teleport };
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
                passiveEffects = null
            };

            return teleport;
        }

        public static Ability getKnockback()
        {
            ActiveEffect knockbackEffect = new ActiveEffect() { name = "Knockback", duration = 1, minAmount = 1,maxAmount=1, statType = StatType.Knockback };
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

            };

            return knockback;


        }

        public static Ability getCharge()
        {
            ActiveEffect chargeEffect = new ActiveEffect() { name = "Charge", duration = 1, minAmount = 5,maxAmount=5, statType = StatType.Teleport };
            ActiveEffect knockbackEffect = new ActiveEffect() { name = "Knockback", duration = 1, minAmount = 1,maxAmount=1, statType = StatType.Knockback };
            ActiveEffect damageEffect = new ActiveEffect() { name = "ChargeDamage", duration = 1, minAmount = 10,maxAmount=20, statType = StatType.Damage };

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
            };

            return charge;
        }

        public static Ability getGrenade()
        {
            ActiveEffect explodeEffect = new ActiveEffect() { name = "Explode", duration = 1, minAmount = 2,maxAmount=3, statType = StatType.Explode };
            ActiveEffect fireEffect = new ActiveEffect() { name = "Fire", duration = 1, minAmount = 5,maxAmount=10, statType = StatType.Damage };

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
                statType = StatType.ActionPoints
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
                passiveEffects=null

            };

            return haste;
        }

        public static Ability getShield()
        {
            ActiveEffect shieldEffect = new ActiveEffect() { name = "Shield", duration = 2, minAmount = 10, maxAmount = 10, statType = StatType.HitPoints };
            ActiveEffect healEffect = new ActiveEffect() { name = "Heal", duration = 1, minAmount = 999, maxAmount = 999, statType = StatType.Heal };
            Ability shield = new Ability() {name = "Shield",description="Summon a temporary shield to protect you"
            ,activeEffects = new List<ActiveEffect>(){shieldEffect,healEffect},
            passiveEffects=null,
            ap=5,range=1,targetType=AbilityTargetType.Self,tilePatternType=TilePatternType.Single,uses=1};

            return shield;
        }

        public static Ability getRage()
        {
            ActiveEffect rageEffect = new ActiveEffect() { name = "Rage", duration = 2, minAmount = 5, maxAmount = 5, statType = StatType.Attack };

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
                tilePatternType = TilePatternType.Single
            };

            return rage;
        }

        public static Ability getWeb()
        {
            ActiveEffect webEffect = new ActiveEffect() { name = "Web", duration = 99, minAmount = 0, maxAmount = 0, statType = StatType.Stuck };
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
                tilePatternType = TilePatternType.NineSquare
            };

            return web;
        }

        public static Ability getDispellMagic()
        {
            ActiveEffect dispellEffect = new ActiveEffect() { name = "Dispell", duration = 1, minAmount = 1, maxAmount = 1, statType = StatType.Dispell };
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
            };

            return dispell;
        }

        public static Ability getSlow()
        {
            ActiveEffect slowEffect = new ActiveEffect() { name="Slow",duration=2,minAmount=-5,maxAmount=-5,statType = StatType.ActionPoints};

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
                uses = 5
            };

            return slow;
        }

        public static Ability getStun()
        {
            ActiveEffect stunEffect = new ActiveEffect() { name = "Stun", duration = 5, minAmount = 0, maxAmount = 0, statType = StatType.Stun };
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
                uses = 5
            };
            return stun;
        }

        public static Ability getPoison()
        {
            ActiveEffect poisonEffect = new ActiveEffect() { name = "Poison", duration = 99, minAmount = 10, maxAmount = 10, statType = StatType.Damage };
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
                uses = 99
            };

            return poison;
        }

    }
}
