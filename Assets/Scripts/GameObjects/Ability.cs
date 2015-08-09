using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class Ability
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int ap { get; set; }
        public int uses { get; set; }

        public int range { get; set; }

        public AbilityTargetType targetType { get; set; }
        public TilePatternType tilePatternType { get; set; } //only used for AOE abilities

        public List<ActiveEffect> activeEffects { get; set; }
        public List<PassiveEffect> passiveEffects { get; set; }

        public string sheetname { get; set; }
        public int spriteindex { get; set; }



        public Ability()
        {
            
        }


        public override string ToString()
        {
            return string.Format("{0}: ap:{1} uses:{2} | {3}", name, ap, uses, description);
        }

        public bool spendUses(int uses)
        {
            if(this.uses >= uses)
            {
                this.uses -= uses;
                return true;
            }
            return false;
        }

        //Better abstraction for these?

        public bool canUseSelf()
        {
            switch(targetType)
            {
                case AbilityTargetType.Self:
                    return true;
                case AbilityTargetType.SingleFriend:
                    return true;
                case AbilityTargetType.SingleFoe:
                    return false;
                case AbilityTargetType.AllFriends:
                    return true;
                case AbilityTargetType.AllFoes:

                    return false;
                case AbilityTargetType.PointEmpty:
                    return false;
                case AbilityTargetType.PointTarget:
                    return true;
                case AbilityTargetType.LOSEmpty:
                    return false;
                case AbilityTargetType.LOSTarget:
                    return true;// ??
                default:
                    return false;
            }
        }

        public bool canUseAlly()
        {
            switch (targetType)
            {
                case AbilityTargetType.Self:
                    return false;
                case AbilityTargetType.SingleFriend:
                    return true;
                case AbilityTargetType.SingleFoe:
                    return false;
                case AbilityTargetType.AllFriends:
                    return true;
                case AbilityTargetType.AllFoes:

                    return false;
                case AbilityTargetType.PointEmpty:
                    return false;
                case AbilityTargetType.PointTarget:
                    return true;
                case AbilityTargetType.LOSEmpty:
                    return false;
                case AbilityTargetType.LOSTarget:
                    return true;// ??
                default:
                    return false;
            }
        }

        public bool canUseEnemy()
        {
            switch (targetType)
            {
                case AbilityTargetType.Self:
                    return false;
                case AbilityTargetType.SingleFriend:
                    return false;
                case AbilityTargetType.SingleFoe:
                    return true;
                case AbilityTargetType.AllFriends:
                    return false;
                case AbilityTargetType.AllFoes:

                    return true;
                case AbilityTargetType.PointEmpty:
                    return false;
                case AbilityTargetType.PointTarget:
                    return true;
                case AbilityTargetType.LOSEmpty:
                    return false;
                case AbilityTargetType.LOSTarget:
                    return true;// ??
                default:
                    return false;
            }
        }
    }
}
