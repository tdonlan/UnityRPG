using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class AbilityFactory
    {


        #region DataFactory


        public static PassiveEffect getPassiveEffectFromEffectData(EffectData data)
        {
            PassiveEffect pe = new PassiveEffect()
            {
                statType = data.statType,
                maxAmount = data.maxAmount,
                minAmount = data.minAmount,
                name = data.name,
                sheetname = data.effectName,
                spriteindex = data.effectIndex
            };
            return pe;
        }

        public static ActiveEffect getActiveEffectFromEffectData(EffectData data)
        {
            return new ActiveEffect()
            {
                duration = data.duration,
                effectIndex = data.effectIndex,
                effectName = data.effectName,
                effectType = data.effectType,
                maxAmount = data.maxAmount,
                minAmount = data.minAmount,
                name = data.name,
                statType = data.statType
            };
        }

        public static Ability getAbilityFromAbilityData(AbilityData data, Dictionary<long, EffectData> effectDataDictionary)
        {
            Ability ability = new Ability(){
                ap = data.ap,
                description = data.description,
                ID = data.ID,
                name = data.name,
                range = data.range,
                sheetname = data.sheetname,
                spriteindex = data.spriteindex,
                targetType = data.targetType,
                tilePatternType = data.tilePatternType,
                uses = data.uses
            };

            if (data.activeEffects.Count > 0)
            {
                List<ActiveEffect> aeList = new List<ActiveEffect>(); 

                foreach (long l in data.activeEffects)
                {
                    if(effectDataDictionary.ContainsKey(l)){
                        aeList.Add(getActiveEffectFromEffectData(effectDataDictionary[l]));
                    }
                   
                }
                ability.activeEffects = aeList;
            }

            if (data.passiveEffects.Count > 0)
            {
                List<PassiveEffect> peList = new List<PassiveEffect>();

                foreach (long l in data.passiveEffects)
                {
                    if (effectDataDictionary.ContainsKey(l))
                    {
                        peList.Add(getPassiveEffectFromEffectData(effectDataDictionary[l]));
                    }

                }
                ability.passiveEffects = peList;
            }

            return ability;
        }

        #endregion

    }
}
