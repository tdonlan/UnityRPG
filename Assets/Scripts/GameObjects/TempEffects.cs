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
        public Vector3 destination { get; set; }
     
        public GameObject gameObject { get; set; }

        public TempEffect(TempEffectType type, GameObject gameObject, float duration, Vector3 position, Vector3 destination)
        {
            this.gameObject = gameObject;
        
            this.effectType = type;
            this.duration = duration;
            this.lifeTimer = 0;
            this.position = position;
            this.destination = destination;
            this.isExpired = false;

            gameObject.transform.position = position;
        }

        public void Update(float delta)
        {
            lifeTimer += delta;
            UpdatePosition(delta);
            if(lifeTimer >= duration)
            {
                isExpired = true;
            }
        }

        private void UpdatePosition(float delta)
        {
            float ratio = lifeTimer / duration;
            float newX = Mathf.Lerp(position.x, destination.x, ratio);
            float newY = Mathf.Lerp(position.y,destination.y,ratio);

            gameObject.transform.position = new Vector3(newX, newY, position.z);
 
        }

    }

    public class ParticleTempEffect : TempEffect
    {
        public string particleSetName { get; set; }

        public ParticleTempEffect(TempEffectType type, GameObject gameObject, float duration, Vector3 position, Vector3 destination, string particleSetName)
            : base (type,gameObject, duration,position,destination)
        {
            this.particleSetName = particleSetName;
          
        }
    }

    public class SpriteTempEffect : TempEffect
    {
        public string spriteSetName { get; set; }
        public int spriteSetIndex { get; set; }

        public SpriteTempEffect(TempEffectType type, GameObject gameObject, float duration, Vector3 position, Vector3 destination, string particleSetName, string spriteSetName, int spriteSetIndex)
            : base(type, gameObject, duration, position, destination)
        {
            this.spriteSetName = spriteSetName;
            this.spriteSetIndex = spriteSetIndex;
        }

    }

    public class TextTempEffect : TempEffect
    {
        public string text { get; set; }

        public TextTempEffect(TempEffectType type, GameObject gameObject, float duration, Vector3 position, Vector3 destination, string particleSetName, string text)
            : base(type, gameObject, duration, position, destination)
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

        public void Update(BattleSceneControllerScript gameController, float delta)
        {
            foreach(var te in TempEffectList)
            {
                te.Update(delta);
            }
        }

    }
}
