using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class AIFactory
    {
        public static Dictionary<AIActionType, int> getEnemyActionDictionary(EnemyType enemy)
        {
            Dictionary<AIActionType, int> dict = new Dictionary<AIActionType, int>();
            switch(enemy)
            {
                case EnemyType.Warrior:
                    dict.Add(AIActionType.Attack, 80);
                    dict.Add(AIActionType.Heal, 10);
                    dict.Add(AIActionType.Flee, 5);
                    break;
                case EnemyType.Priest:
                    dict.Add(AIActionType.Attack, 10);
                    dict.Add(AIActionType.Buff, 50);
                    dict.Add(AIActionType.Heal, 75);
                    dict.Add(AIActionType.Nuke, 50);
                    dict.Add(AIActionType.Flee, 25);
                    break;
                case EnemyType.Mage:
                    dict.Add(AIActionType.Attack, 5);
                    dict.Add(AIActionType.Buff, 25);
                    dict.Add(AIActionType.Heal, 20);
                    dict.Add(AIActionType.Nuke, 85);
                    dict.Add(AIActionType.Flee, 50);
                    break;
                case EnemyType.Archer:
                    dict.Add(AIActionType.RangedAttack, 5);
                    dict.Add(AIActionType.Buff, 0);
                    dict.Add(AIActionType.Heal, 20);
                    dict.Add(AIActionType.Nuke, 0);
                    dict.Add(AIActionType.Flee, 50);
                    break;
                default:
                    break;

            }

            return dict;
        }
    }
}
