using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class AIAction
    {
        public AIActionType actionType { get; set; }
        public List<BattleAction> battleActionList { get; set; }
        public int cost { get; set; }
        public int weight { get; set; }

    }
}
