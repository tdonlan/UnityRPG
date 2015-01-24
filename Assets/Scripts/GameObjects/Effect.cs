using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class PassiveEffect
    {
        public string name { get; set; }
        public StatType statType { get; set; }
        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public override string ToString()
        {
            return string.Format("{0} (Passive): {1} {2}-{3}", name, statType.ToString(), minAmount,maxAmount);
        }

    }

    public class ActiveEffect
    {
        public string name { get; set; }
        public StatType statType { get; set; }

        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public int duration { get; set; }


        public override string ToString()
        {
            return string.Format("{0} (Active): {1} {2}-{3} for {4} turns", name, statType.ToString(), minAmount, maxAmount, duration);
        }
    }

}
