using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using System;
using UnityEngine.UI;
using Assets;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using System.Linq;


    public class UIHelper
    {
        public static void UpdateTextComponent(GameObject parent, string componentName, string text)
        {
            foreach (var comp in parent.GetComponentsInChildren<Text>())
            {
                if (comp.name == componentName)
                {
                    comp.text = text;
                }
            }
        }

        public static void UpdateSpriteComponent(GameObject parent, string componentName, Sprite sprite)
        {
            foreach (var comp in parent.GetComponentsInChildren<Image>())
            {
                if (comp.name == componentName)
                {
                    comp.sprite = sprite;
                }
            }
        }

        public static void UpdateSliderValue(GameObject parent, string componentName, float val)
        {
            foreach (var comp in parent.GetComponentsInChildren<Slider>())
            {
                if (comp.name == componentName)
                {
                    comp.value = val;
                }
            }
        }

        public static void AddClickToGameObject(GameObject gameObject, UnityAction action, EventTriggerType triggerType)
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.delegates = new List<EventTrigger.Entry>();
            AddEventTrigger(eventTrigger, action, triggerType);
        }

        public static void AddClickToGameObject(GameObject gameObject, UnityAction<System.Object> action, EventTriggerType triggerType, System.Object eventObject)
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.delegates = new List<EventTrigger.Entry>();
            AddEventTrigger(eventTrigger, action, triggerType, eventObject);
        }


        public static void AddEventTrigger(EventTrigger eventTrigger, UnityAction action, EventTriggerType triggerType)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action()); // ignore event data

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.delegates.Add(entry);
        }


        public static void AddEventTrigger(EventTrigger eventTrigger, UnityAction<BaseEventData> action, EventTriggerType triggerType)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventData)); // capture and pass the event data to the listener

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.delegates.Add(entry);
        }

        public static void AddEventTrigger(EventTrigger eventTrigger, UnityAction<System.Object> action, EventTriggerType triggerType, System.Object eventObj)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventObj)); // pass additonal argument to the listener

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.delegates.Add(entry);
        }


        public static void DestroyAllChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
            }
        }

        public static GameObject getChildObject(GameObject parent, string name)
        {
            foreach (var comp in parent.GetComponentsInChildren<Transform>())
            {
                if (comp.name == name)
                {
                    return comp.gameObject;
                }
            }
            return null;
        }

    }

