using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityRPG;


public class GameDataObject : MonoBehaviour
    {
        public TreeStore treeStore { get; set; }
        public string testText { get; set; }
        public Dictionary<long, Item> itemDictionary;

        public List<long> playerInventory = new List<long>();

        void Start()
        {
            loadTreeStore();
            this.testText = "Hello World";
            loadItemList();

            DontDestroyOnLoad(this);

        }

    private void loadItemList(){
        itemDictionary = ItemFactory.getItemDictionary();
    }

        private void loadTreeStore()
        {
            TextAsset manifestTextAsset = Resources.Load<TextAsset>("SimpleWorld1/manifestSimple");
            this.treeStore = SimpleTreeParser.LoadTreeStoreFromSimpleManifest(manifestTextAsset.text);
        }

        public void runActions(List<TreeNodeAction> actionList)
        {
            foreach (var action in actionList)
            {
                switch (action.actionType)
                {
                    case NodeActionType.AddItem:
                        addItem(action.index);
                        break;
                    case NodeActionType.RemoveItem:
                        removeItem(action.index);
                        break;
                    default:
                        break;
                         
                }
            }
        }

        public void removeItem(long itemIndex)
        {
            if (itemDictionary.ContainsKey(itemIndex))
            {
                var item = playerInventory.Find(x => x == itemIndex);
                playerInventory.Remove(item);
            }
        }

        public void addItem(long itemIndex)
        {
            if(itemDictionary.ContainsKey(itemIndex)){
                playerInventory.Add(itemIndex);
            }
           
        }


    }

