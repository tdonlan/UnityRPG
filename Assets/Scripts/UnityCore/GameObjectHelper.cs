using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using System;
using UnityEngine.UI;
using UnityRPG;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Linq;


    public class GameObjectHelper
    {

        public static void UpdateSprite(GameObject parentObject, string componentName, Sprite sprite)
        {
            foreach (var comp in parentObject.GetComponentsInChildren<SpriteRenderer>())
            {
                if (comp.name == componentName)
                {
                    comp.sprite = sprite;
                }
            }

        }

        public static void UpdateSpriteColor(GameObject parentObject, string componentName, Color c)
        {
            foreach (var comp in parentObject.GetComponentsInChildren<SpriteRenderer>())
            {
                if (comp.name == componentName)
                {
                    comp.color = c;
                }
            }

        }

        public static GameObject LoadPrefab(string prefabFileName)
        {
           return (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>(prefabFileName));

        }

       

    }

