using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    [Serializable]
    public class PassiveEffect
    {
        public string name { get; set; }
        public StatType statType { get; set; }
        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public string sheetname { get; set; }
        public int spriteindex { get; set; }

        public override string ToString()
        {
            return string.Format("{0} (Passive): {1} {2}-{3}", name, statType.ToString(), minAmount,maxAmount);
        }

    }

    [Serializable]
    public class ActiveEffect
    {
        public string name { get; set; }
        public StatType statType { get; set; }

        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public int duration { get; set; }

        public TempEffectType effectType { get; set; }
        public string effectName { get; set; }
        public int effectIndex { get; set; }
             
        public ActiveEffect()
        {
            effectType = TempEffectType.Sprite;
                
        }
        
        public override string ToString()
        {
            return string.Format("{0} (Active): {1} {2}-{3} for {4} turns", name, statType.ToString(), minAmount, maxAmount, duration);
        }
    }

}
