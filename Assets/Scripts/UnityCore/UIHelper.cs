using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using System;
using UnityEngine.UI;
using UnityRPG;
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

        public static void UpdateSpriteColor(GameObject parent, string componentName, Color c)
        {
            foreach (var comp in parent.GetComponentsInChildren<Image>())
            {
                if (comp.name == componentName)
                {
                    comp.color = c;
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

        public static GameObject getGameObjectWithName(GameObject parent, string name, Type componentType)
        {
            foreach (var comp in parent.GetComponentsInChildren(componentType))
            {
                if (comp.name == name)
                {
                    return comp.gameObject;
                }
            }
            return null;
        }

        public static void AddClickToGameObject(GameObject gameObject, UnityAction action, EventTriggerType triggerType)
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            AddEventTrigger(eventTrigger, action, triggerType);
        }

        public static void AddClickToGameObject(GameObject gameObject, UnityAction<System.Object> action, EventTriggerType triggerType, System.Object eventObject)
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
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
            eventTrigger.triggers.Add(entry);
        }


        public static void AddEventTrigger(EventTrigger eventTrigger, UnityAction<BaseEventData> action, EventTriggerType triggerType)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventData)); // capture and pass the event data to the listener

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.triggers.Add(entry);
        }

        public static void AddEventTrigger(EventTrigger eventTrigger, UnityAction<System.Object> action, EventTriggerType triggerType, System.Object eventObj)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventObj)); // pass additonal argument to the listener

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.triggers.Add(entry);
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


        public static Button getButton(GameObject parent, string name)
        {
            var buttons = parent.GetComponentsInChildren<Button>();
            foreach (var b in buttons)
            {
                if (b.name == name)
                {
                    return b;
                }
            }
            return null;
        }

        //SHOULD BE DEPRECATED
        public static void SetAllButtons(bool flag)
        {
            var canvas = GameObject.FindGameObjectWithTag("FrontCanvas");
            getButton(canvas, "EndTurnButton").interactable = flag;
            getButton(canvas, "MoveButton").interactable = flag;
            getButton(canvas, "AttackButton").interactable = flag;
            getButton(canvas, "AbilitiesButton").interactable = flag;
            getButton(canvas, "ItemButton").interactable = flag;
            //getButton(canvas, "EquipmentButton").interactable = flag;
        }

        //DEPRECATE
        public static void SetButton(string buttonName, bool flag)
        {
            var canvas = GameObject.FindGameObjectWithTag("FrontCanvas");
            getButton(canvas, buttonName).interactable = flag;
           
        }

        public static void SetAllButtons(List<GameObject> buttonObjects, bool flag)
        {
            foreach (var b in buttonObjects)
            {
                SetButton(b, flag);
            }
        }

        public static void SetButton(GameObject buttonObject, bool flag)
        {
            var button = buttonObject.GetComponent<Button>();
            button.interactable = flag;
        }

        public static void MoveUIObject(GameObject uiObject, Vector3 newPos)
        {
            var uiRectTransform = uiObject.GetComponent<RectTransform>();
            uiRectTransform.localPosition = newPos;
        }

    }

