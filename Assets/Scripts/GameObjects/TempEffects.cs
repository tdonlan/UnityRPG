using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace UnityRPG
{

    public class TempEffect
    {
        public TempEffectType effectType { get; set; }
        public float duration { get; set; }
        public float lifeTimer { get; set; }
        public bool isExpired { get; set; }
        public Vector3 position { get; set; }

        public GameObject gameObject { get; set; }

        public TempEffect(TempEffectType type, float duration, Vector3 position)
        {
            this.effectType = type;
            this.duration = duration;
            this.lifeTimer = 0;
            this.position = position;
            this.isExpired = false;
        }

        public void Update(float delta)
        {
            lifeTimer += delta;
            if(lifeTimer >= duration)
            {
                isExpired = true;
            }
        }

    }

    public class ParticleTempEffect : TempEffect
    {
        public string particleSetName { get; set; }

        public ParticleTempEffect(TempEffectType type, float duration, Vector3 position, string particleSetName)
            : base (type,duration,position)
        {
            this.particleSetName = particleSetName;
          
        }
    }

    public class SpriteTempEffect : TempEffect
    {
        public string spriteSetName { get; set; }
        public int spriteSetIndex { get; set; }

        public SpriteTempEffect(TempEffectType type, float duration, Vector3 position, string particleSetName, string spriteSetName, int spriteSetIndex)
            : base(type,duration,position)
        {
            this.spriteSetName = spriteSetName;
            this.spriteSetIndex = spriteSetIndex;
        }

    }

    public class TextTempEffect : TempEffect
    {
        public string text { get; set; }

        public TextTempEffect(TempEffectType type, float duration, Vector3 position, string particleSetName, string text) 
            : base (type,duration,position)
        {
            this.text = text;
        }

    }

    public class TempEffects
    {
        public List<TempEffect> TempEffectList { get; set; }

        public TempEffects()
        {
            this.TempEffectList = new List<TempEffect>();
        }

        public void Update(GameControllerScript gameController, float delta)
        {
            foreach(var te in TempEffectList)
            {
                te.Update(delta);
            }
        }

    }
}
