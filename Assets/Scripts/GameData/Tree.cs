using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityRPG;
  
    public class WorldTree : ITree
    {
        public string treeName { get; set; }
        public long treeIndex { get; set; }
        public long currentIndex { get; set; }
        public TreeType treeType { get; set; }

        public ITreeNode tempTreeNode;

        public Dictionary<long, WorldTreeNode> treeNodeDictionary { get; set; }

        public GlobalFlags globalFlags { get; set; }

        public WorldTree(GlobalFlags globalFlags, TreeType treeType)
        {
            currentIndex = 0;
            treeNodeDictionary = new Dictionary<long, WorldTreeNode>();
            this.globalFlags = globalFlags;
            this.treeType = treeType;
        }

            public ITreeNode getNode(long index)
        {
            if(treeNodeDictionary.ContainsKey(index))
            {
                return treeNodeDictionary[index];
            }
            return null;
        }

        public void SelectNode(long index)
        {
            this.currentIndex = index;

            treeNodeDictionary[currentIndex].SelectNode(this);

        }

        public bool checkNode(long index)
        {
            return treeNodeDictionary.ContainsKey(index);
        }

        public bool validateTreeLinks()
        {
            bool validLinks = true;
            foreach (TreeNode node in treeNodeDictionary.Values)
            {
                foreach (var branch in node.branchList)
                {
                    Console.Write(string.Format("Checking {0} for link {1} ...",branch.description, branch.linkIndex));
                    if (!checkNode(branch.linkIndex))
                    {
                        validLinks = false;
                         Console.Write(" MISSING.\n");
                    }
                    else{
                        Console.Write(" found.\n");
                    }
                }
            }
            return validLinks;
        }

  
    }

    public class ZoneTree : ITree
    {
        public string treeName { get; set; }
        public long treeIndex { get; set; }
        public long currentIndex { get; set; }
        public TreeType treeType { get; set; }

        public Dictionary<long, ZoneTreeNode> treeNodeDictionary { get; set; }

        public GlobalFlags globalFlags { get; set; }

        public ZoneTree(GlobalFlags globalFlags, TreeType treeType)
        {
            currentIndex = 0;
            treeNodeDictionary = new Dictionary<long, ZoneTreeNode>();
            this.globalFlags = globalFlags;
            this.treeType = treeType;
        }

        public ITreeNode getNode(long index)
        {
            if (treeNodeDictionary.ContainsKey(index))
            {
                return treeNodeDictionary[index];
            }
            return null;
        }

        //returns the ZoneTreeNode if its in the root branch list and meets conditions
        public ITreeNode getNodeCheckingRootBranchList(long index)
        {
            var rootNode = treeNodeDictionary[1];
            if (index == 1)
            {
                return rootNode; //always return the root node (entrance to the zone)
            }
            if (rootNode.getBranchList(this).Find(x => x.linkIndex == index) != null)
            {
                return getNode(index);
            }
            return null;
        }

        public void SelectNode(long index)
        {
            this.currentIndex = index;

            treeNodeDictionary[currentIndex].SelectNode(this);

        }

        public bool checkNode(long index)
        {
            return treeNodeDictionary.ContainsKey(index);
        }

        public bool validateTreeLinks()
        {
            bool validLinks = true;
            foreach (TreeNode node in treeNodeDictionary.Values)
            {
                foreach (var branch in node.branchList)
                {
                    Console.Write(string.Format("Checking {0} for link {1} ...", branch.description, branch.linkIndex));
                    if (!checkNode(branch.linkIndex))
                    {
                        validLinks = false;
                        Console.Write(" MISSING.\n");
                    }
                    else
                    {
                        Console.Write(" found.\n");
                    }
                }
            }
            return validLinks;
        }



    }



     public class DialogTree : ITree
     {
          public string treeName { get; set; }
        public long treeIndex { get; set; }
        public long currentIndex { get; set; }
        public TreeType treeType { get; set; }

        public Dictionary<long, DialogTreeNode> treeNodeDictionary { get; set; }

        public GlobalFlags globalFlags {get;set;}

        public DialogTree(GlobalFlags globalFlags, TreeType treeType)
        {
            currentIndex = 0;
            treeNodeDictionary = new Dictionary<long, DialogTreeNode>();
            this.globalFlags = globalFlags;
            this.treeType = treeType;
        }

        public ITreeNode getNode(long index)
        {
            if(treeNodeDictionary.ContainsKey(index))
            {
                return treeNodeDictionary[index];
            }
            return null;
        }

        public void SelectNode(long index)
        {
            this.currentIndex = index;

            treeNodeDictionary[currentIndex].SelectNode(this);

        }

        public bool checkNode(long index)
        {
            return treeNodeDictionary.ContainsKey(index);
        }

        public bool validateTreeLinks()
        {
            bool validLinks = true;
            foreach (TreeNode node in treeNodeDictionary.Values)
            {
                foreach (var branch in node.branchList)
                {
                    Console.Write(string.Format("Checking {0} for link {1} ...", branch.description, branch.linkIndex));
                    if (!checkNode(branch.linkIndex))
                    {
                        validLinks = false;
                        Console.Write(" MISSING.\n");
                    }
                    else
                    {
                        Console.Write(" found.\n");
                    }
                }
            }
            return validLinks;
        }

     }

     public class QuestTree : ITree
     {
         public string treeName { get; set; }
         public long treeIndex { get; set; }
         public long currentIndex { get; set; }
         public TreeType treeType { get; set; }

         public Dictionary<long, QuestTreeNode> treeNodeDictionary { get; set; }

         public GlobalFlags globalFlags { get; set; }

         public QuestTree(GlobalFlags globalFlags, TreeType treeType)
         {
             currentIndex = 0;
             treeNodeDictionary = new Dictionary<long, QuestTreeNode>();
             this.globalFlags = globalFlags;
             this.treeType = treeType;
         }

         public ITreeNode getNode(long index)
         {
             if (treeNodeDictionary.ContainsKey(index))
             {
                 return treeNodeDictionary[index];
             }
             return null;
         }

         public void SelectNode(long index)
         {
             this.currentIndex = index;

             treeNodeDictionary[currentIndex].SelectNode(this);

         }

         public bool checkNode(long index)
         {
             return treeNodeDictionary.ContainsKey(index);
         }

         //return a string list of the quest, given the current global flags 
         //classic tree traversal, checking conditions
         public List<string> getQuestDisplay() { 

             List<string> questStrList = new List<string>();
            if(globalFlags.checkFlag(treeName,"true",CompareType.Equal)){
                QuestTreeNode rootNode = (QuestTreeNode)getNode(1);
                questStrList.Add(rootNode.name); //quest title

                questStrList.AddRange(getQuestDisplayTree());
            }
            return questStrList;
         }

         //-iterate through list first, adding all complete nodes
        //-iterate through again, adding branch names (from complete nodes) pointing to incomplete nodes
         private List<string> getQuestDisplayTree()
         {
             List<string> questStrList = new List<string>();
             foreach (QuestTreeNode node in treeNodeDictionary.Values)
             {
                 if (globalFlags.checkFlag(node.content.flagName, "true", CompareType.Equal))
                 {
                     questStrList.Add("X-" + node.content.description);

                     foreach (var branch in node.branchList)
                     {
                         QuestTreeNode branchNode = (QuestTreeNode)getNode(branch.linkIndex);
                         if (!globalFlags.checkFlag(branchNode.content.flagName, "true", CompareType.Equal))
                         {
                             questStrList.Add("--" + branch.description);
                         }
                     }
                 }
                 
             }
             return questStrList;
         }

        


         public bool validateTreeLinks()
         {
             bool validLinks = true;
             foreach (TreeNode node in treeNodeDictionary.Values)
             {
                 foreach (var branch in node.branchList)
                 {
                     Console.Write(string.Format("Checking {0} for link {1} ...", branch.description, branch.linkIndex));
                     if (!checkNode(branch.linkIndex))
                     {
                         validLinks = false;
                         Console.Write(" MISSING.\n");
                     }
                     else
                     {
                         Console.Write(" found.\n");
                     }
                 }
             }
             return validLinks;
         }
     }

     public class BattleTree : ITree
     {
         public string treeName { get; set; }
         public long treeIndex { get; set; }
         public long currentIndex { get; set; }
         public TreeType treeType { get; set; }

         public Dictionary<long, BattleTreeNode> treeNodeDictionary { get; set; }

         public GlobalFlags globalFlags { get; set; }

         public BattleTree(GlobalFlags globalFlags, TreeType treeType)
         {
             currentIndex = 0;
             treeNodeDictionary = new Dictionary<long, BattleTreeNode>();
             this.globalFlags = globalFlags;
             this.treeType = treeType;
         }

         public ITreeNode getNode(long index)
         {
             if (treeNodeDictionary.ContainsKey(index))
             {
                 return treeNodeDictionary[index];
             }
             return null;
         }

         public void SelectNode(long index)
         {
             this.currentIndex = index;

             treeNodeDictionary[currentIndex].SelectNode(this);

         }


         public bool checkNode(long index)
         {
             return treeNodeDictionary.ContainsKey(index);
         }

         public List<BattleTreeNode> getEnemyNodeList()
         {
             return treeNodeDictionary.Values.Where(x => x.content.nodeType == BattleNodeType.Enemy).ToList();
         }

         public BattleTreeNode getWinNode()
         {
             long winLink = treeNodeDictionary[currentIndex].getBranchList(this).Where(x => x.description.ToLower().Equals("win")).FirstOrDefault().linkIndex;
             var node = treeNodeDictionary[winLink];
             if (node.content.nodeType == BattleNodeType.Win)
             {
                 return node;
             }
             return null;
         }

         public List<Item> getWinItemList(GameDataSet gameDataSet)
         {
             List<Item> itemList = new List<Item>();

             var winNode = getWinNode();
             var itemActionList = winNode.actionList.Where(x => x.actionType == NodeActionType.AddItem);
             foreach (var itemAction in itemActionList)
             {
                 itemList.Add(ItemFactory.getItemFromIndex(itemAction.index, gameDataSet));

             }

             return itemList;

         }

         public long getWinXP()
         {
             var winNode = getWinNode();
             var xpAction = winNode.actionList.Where(x => x.actionType == NodeActionType.AddXP).FirstOrDefault();

             if (xpAction != null)
             {
                 return xpAction.index;
             }
             return 0;
         }


         public bool validateTreeLinks()
         {
             bool validLinks = true;
             foreach (TreeNode node in treeNodeDictionary.Values)
             {
                 foreach (var branch in node.branchList)
                 {
                     Console.Write(string.Format("Checking {0} for link {1} ...", branch.description, branch.linkIndex));
                     if (!checkNode(branch.linkIndex))
                     {
                         validLinks = false;
                         Console.Write(" MISSING.\n");
                     }
                     else
                     {
                         Console.Write(" found.\n");
                     }
                 }
             }
             return validLinks;
         }
     }

     public class InfoTree : ITree
     {
         public string treeName { get; set; }
         public long treeIndex { get; set; }
         public long currentIndex { get; set; }
         public TreeType treeType { get; set; }

         public Dictionary<long, InfoTreeNode> treeNodeDictionary { get; set; }

         public GlobalFlags globalFlags { get; set; }

         public InfoTree(GlobalFlags globalFlags, TreeType treeType)
         {
             currentIndex = 0;
             treeNodeDictionary = new Dictionary<long, InfoTreeNode>();
             this.globalFlags = globalFlags;
             this.treeType = treeType;
         }

         public ITreeNode getNode(long index)
         {
             if (treeNodeDictionary.ContainsKey(index))
             {
                 return treeNodeDictionary[index];
             }
             return null;
         }

         public void SelectNode(long index)
         {
             this.currentIndex = index;

             treeNodeDictionary[currentIndex].SelectNode(this);

         }

         public bool checkNode(long index)
         {
             return treeNodeDictionary.ContainsKey(index);
         }

         public bool validateTreeLinks()
         {
           return true;
         }
     }

     public class CutsceneTree : ITree
     {
         public string treeName { get; set; }
         public long treeIndex { get; set; }
         public long currentIndex { get; set; }
         public TreeType treeType { get; set; }

         public Dictionary<long, CutsceneTreeNode> treeNodeDictionary { get; set; }

         public GlobalFlags globalFlags { get; set; }

         public CutsceneTree(GlobalFlags globalFlags, TreeType treeType)
         {
             currentIndex = 0;
             treeNodeDictionary = new Dictionary<long, CutsceneTreeNode>();
             this.globalFlags = globalFlags;
             this.treeType = treeType;
         }

         public ITreeNode getNode(long index)
         {
             if (treeNodeDictionary.ContainsKey(index))
             {
                 return treeNodeDictionary[index];
             }
             return null;
         }

         public void SelectNode(long index)
         {
             this.currentIndex = index;

             treeNodeDictionary[currentIndex].SelectNode(this);

         }

         public bool checkNode(long index)
         {
             return treeNodeDictionary.ContainsKey(index);
         }

         public bool validateTreeLinks()
         {
             return true;
         }
     }


     public class StoreTree : ITree
     {
         public string treeName { get; set; }
         public long treeIndex { get; set; }
         public long currentIndex { get; set; }
         public TreeType treeType { get; set; }

         public Dictionary<long, StoreTreeNode> treeNodeDictionary { get; set; }

         public GlobalFlags globalFlags { get; set; }

         public StoreTree(GlobalFlags globalFlags, TreeType treeType)
         {
             currentIndex = 0;
             treeNodeDictionary = new Dictionary<long, StoreTreeNode>();
             this.globalFlags = globalFlags;
             this.treeType = treeType;
         }

         public ITreeNode getNode(long index)
         {
             if (treeNodeDictionary.ContainsKey(index))
             {
                 return treeNodeDictionary[index];
             }
             return null;
         }

         public void SelectNode(long index)
         {
             this.currentIndex = index;

             treeNodeDictionary[currentIndex].SelectNode(this);

         }

         public bool checkNode(long index)
         {
             return treeNodeDictionary.ContainsKey(index);
         }

         public bool validateTreeLinks()
         {
             return true;
         }

         //lookup the item in the store list and return the price, otherwise default to .5f
         public long getBuyPrice(Item i, GameDataSet gameDataSet)
         {
             float buyPrice = 0.0f;

             var infoNode = treeNodeDictionary[1];
             List<StoreTreeNode> storeNodeList = new List<StoreTreeNode>();
             foreach (var branch in infoNode.getBranchList(this))
             {
                 var storeNode = (StoreTreeNode)this.getNode(branch.linkIndex);

                 if(storeNode.content.nodeType == StoreNodeType.ItemIndex){
                     if(storeNode.content.linkIndex == i.ID){
                         if(storeNode.content.buyPrice > buyPrice){
                             buyPrice = storeNode.content.buyPrice;
                         }
                     }
                 }
                 else if(storeNode.content.nodeType == StoreNodeType.ItemClass){
                     if (storeNode.content.itemType == i.type && i.price <= storeNode.content.linkIndex)
                     {
                         if (storeNode.content.buyPrice > buyPrice)
                         {
                             buyPrice = storeNode.content.buyPrice;
                         }
                     }
                 }
             }

             if (buyPrice < .5)
             {
                 buyPrice = .5f;
             }

             return (long)Math.Round( i.price * buyPrice);

         }

         //given a gameDataSet, and global flags, return the list of items (and prices) sold
         //dont return items with 0 price (specials, etc)
         public List<StoreItem> getSellList(GameDataSet gameDataSet, Random r)
         {
             List<StoreItem> storeList = new List<StoreItem>();

             var infoNode = treeNodeDictionary[1];
             foreach (var branch in infoNode.getBranchList(this))
             {
                 StoreTreeNode storeNode = (StoreTreeNode)this.getNode(branch.linkIndex);
                 if (storeNode.content.nodeType == StoreNodeType.ItemClass)
                 {
                     storeList.AddRange(getSellItemTypeList(storeNode.content.itemType, storeNode.content.sellPrice, storeNode.content.count, storeNode.content.linkIndex, gameDataSet, r));
                 }
                 else if (storeNode.content.nodeType == StoreNodeType.ItemIndex)
                 {
                     storeList.Add(getSellItem(storeNode.content.linkIndex, storeNode.content.sellPrice, storeNode.content.count, gameDataSet));
                 }
             }
             return storeList;
         }

         private StoreItem getSellItem(long itemID, float priceAdjustment, int count, GameDataSet gameDataSet)
         {
             StoreItem storeItem = null;
             Item i = ItemFactory.getItemFromIndex(itemID, gameDataSet);
             if (i != null)
             {
                 storeItem = new StoreItem();
                 storeItem.item = i;
                 storeItem.count = count;
                 storeItem.price = (long)Math.Round(i.price  * priceAdjustment);
                 storeItem.selected = 1;
                 return storeItem;
             }
             return storeItem;
         }

         //returning a random list of items of type at less than rarityIndex (price)
         private List<StoreItem> getSellItemTypeList(ItemType type, float priceAdjustment, int count, long rarityIndex, GameDataSet gameDataSet, Random r)
         {
             List<StoreItem> storeItemList = new List<StoreItem>();
             var itemCount = r.Next(9)+1; //TODO: Do we want this hardcoded here?
             switch(type){
                 case ItemType.Weapon:
                      List<long> weaponIdList = gameDataSet.weaponDataDictionary.Where(x=>x.Value.price <= rarityIndex).Select(x=>x.Key).OrderBy(x => r.Next()).Take(itemCount).ToList();
                      storeItemList.AddRange(getStoreItemListFromItemIdList(weaponIdList, gameDataSet, count, priceAdjustment));
                     break;
                 case ItemType.Ammo:
                     List<long> ammoIdList = gameDataSet.ammoDataDictionary.Where(x=>x.Value.price <= rarityIndex).Select(x=>x.Key).OrderBy(x => r.Next()).Take(itemCount).ToList();
                     storeItemList.AddRange(getStoreItemListFromItemIdList(ammoIdList, gameDataSet, count, priceAdjustment));
                     break;
                 case ItemType.Armor:
                     List<long> armorIdList = gameDataSet.armorDataDictionary.Where(x => x.Value.price <= rarityIndex).Select(x => x.Key).OrderBy(x => r.Next()).Take(itemCount).ToList();
                     storeItemList.AddRange(getStoreItemListFromItemIdList(armorIdList, gameDataSet, count, priceAdjustment));
                     break;
                 case ItemType.Potion:
                     List<long> potionIdList = gameDataSet.usableItemDataDictionary.Where(x => x.Value.price <= rarityIndex && x.Value.type == ItemType.Potion).Select(x => x.Key).OrderBy(x => r.Next()).Take(itemCount).ToList();
                     storeItemList.AddRange(getStoreItemListFromItemIdList(potionIdList, gameDataSet, count, priceAdjustment));
                     break;
                 case ItemType.Thrown:
                      List<long> thrownIdList = gameDataSet.usableItemDataDictionary.Where(x => x.Value.price <= rarityIndex && x.Value.type == ItemType.Thrown).Select(x => x.Key).OrderBy(x => r.Next()).Take(itemCount).ToList();
                      storeItemList.AddRange(getStoreItemListFromItemIdList(thrownIdList, gameDataSet, count, priceAdjustment));
                     break;
                 case ItemType.Wand:
                      List<long> wandIdList = gameDataSet.usableItemDataDictionary.Where(x => x.Value.price <= rarityIndex && x.Value.type == ItemType.Wand).Select(x => x.Key).OrderBy(x => r.Next()).Take(itemCount).ToList();
                      storeItemList.AddRange(getStoreItemListFromItemIdList(wandIdList, gameDataSet, count, priceAdjustment));
                     break;
                 default:
                     break;
             }

             return storeItemList;
         }

         private List<StoreItem> getStoreItemListFromItemIdList(List<long> itemIdList, GameDataSet gameDataSet, int count, float priceAdjustment)
         {
             List<StoreItem> storeItemList = new List<StoreItem>(); 
             foreach(long id in itemIdList){
                 var item = ItemFactory.getItemFromIndex(id, gameDataSet);
                 if (item.price > 0)
                 {
                     StoreItem tempStoreItem = new StoreItem();
                     tempStoreItem.item = item;
                     tempStoreItem.count = count;
                     tempStoreItem.price = (long)Math.Round(tempStoreItem.item.price * priceAdjustment);
                     tempStoreItem.selected = 1;

                     storeItemList.Add(tempStoreItem);
                 }
              
             }

             return storeItemList;

             
         }
     }


    public class TreeBranchCondition
    {
        public string flagName { get; set; }
        public string value { get; set; }
        public CompareType flagCompareType { get; set; }

        public TreeBranchCondition(string flagName, string value, CompareType compareType)
        {
            this.flagName = flagName;
            this.value = value;
            this.flagCompareType = compareType;
        }
    }

    public class TreeNodeFlagSet
    {
        public string flagName { get; set; }
        public string value { get; set; }
        public FlagType flagType { get; set; }
    }

    public class TreeNodeAction
    {
        public NodeActionType actionType { get; set; }
        public string actionName { get; set; }
        public long index { get; set; }
        public int count { get; set; }
    }

    public class TreeBranch
    {
        public string description { get; set; }
        public long linkIndex { get; set; }
        public List<TreeBranchCondition> conditionList { get; set; }

        public TreeBranch()
        {
            this.conditionList = new List<TreeBranchCondition>();
        }
            
        public TreeBranch(string description, long linkIndex, List<TreeBranchCondition> conditionList)
        {
            this.description = description;
            this.linkIndex = linkIndex;
            this.conditionList = conditionList;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", description, linkIndex);
        }
    }


