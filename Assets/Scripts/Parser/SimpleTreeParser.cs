using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEditor;
using UnityEngine;


    public class SimpleTreeParser
    {

        //Load the tree store from a simple file list (not json)
        public static TreeStore LoadTreeStoreFromSimpleManifest(string manifestSimple)
        {

            TreeStore ts = new TreeStore();
            try
            {
                string[] lineArray = manifestSimple.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var line in lineArray.ToList<String>())
                {
                    string[] treeArray = line.Split(';');

                    TextAsset treeText = Resources.Load<TextAsset>(treeArray[0]);

                    ITree tempTree = SimpleTreeParser.getTreeFromString(treeText.text, (TreeType)Int32.Parse(treeArray[1]), ts.globalFlags);
                    tempTree.treeName = treeArray[2];
                    ts.treeDictionary.Add(Int32.Parse(treeArray[3]), tempTree);
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message + ex.StackTrace;
                return null;
            }

            return ts;
        }

        public static ITree getTreeFromString(string data, TreeType treeType, GlobalFlags gf)
        {
            ITree t = null;
            List<ITreeNode> treeNodeList = null;
            switch (treeType)
            {
                case TreeType.World:
                    WorldTree worldTree = new WorldTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    worldTree.treeNodeDictionary = getWorldTreeNodeFromList(treeNodeList);
                    worldTree.currentIndex = treeNodeList[0].index;
                    t = worldTree;
                    break;
                case TreeType.Zone:
                    ZoneTree zoneTree = new ZoneTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    zoneTree.treeNodeDictionary = getZoneTreeNodeFromList(treeNodeList);
                    zoneTree.currentIndex = treeNodeList[0].index;
                    t = zoneTree;
                    break;
                case TreeType.Dialog:
                    DialogTree dialogTree = new DialogTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    dialogTree.treeNodeDictionary = getDialogTreeNodeFromList(treeNodeList);
                    dialogTree.currentIndex = treeNodeList[0].index;
                    t = dialogTree;
                    break;
                case TreeType.Quest:
                    QuestTree questTree = new QuestTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    questTree.treeNodeDictionary = getQuestTreeNodeFromList(treeNodeList);
                    questTree.currentIndex = treeNodeList[0].index;
                    t = questTree;
                    break;
                case TreeType.Battle:
                    BattleTree battleTree = new BattleTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    battleTree.treeNodeDictionary = getBattleTreeNodeFromList(treeNodeList);
                    battleTree.currentIndex = treeNodeList[0].index;
                    t = battleTree;
                    break;
                case TreeType.Info:
                    InfoTree infoTree = new InfoTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    infoTree.treeNodeDictionary = getInfoTreeNodeFromList(treeNodeList);
                    infoTree.currentIndex = treeNodeList[0].index;
                    t = infoTree;
                    break;
                case TreeType.Store:
                    StoreTree storeTree = new StoreTree(gf, treeType);
                    treeNodeList = getTreeNodeListFromString(data, treeType);
                    storeTree.treeNodeDictionary = getStoreTreeNodeFromList(treeNodeList);
                    storeTree.currentIndex = treeNodeList[0].index;
                    t = storeTree;
                    break;
                default:
                    break;
            }
            return t;
        }

        //DEPRECATED
        public static ITree getTreeFromFile(string path,TreeType treeType,  GlobalFlags gf)
        {
            ITree t = null;
            List<ITreeNode> treeNodeList = null;
            switch (treeType)
            {
                case TreeType.World:
                    WorldTree worldTree = new WorldTree(gf, treeType);
                        treeNodeList = getTreeNodeListFromFile(path,treeType);
                       worldTree.treeNodeDictionary = getWorldTreeNodeFromList(treeNodeList);
                       worldTree.currentIndex = treeNodeList[0].index;
                    t = worldTree;
                    break;
                case TreeType.Zone:
                    ZoneTree zoneTree = new ZoneTree(gf, treeType);
                       treeNodeList = getTreeNodeListFromFile(path,treeType);
                       zoneTree.treeNodeDictionary = getZoneTreeNodeFromList(treeNodeList);
                       zoneTree.currentIndex = treeNodeList[0].index;
                       t = zoneTree;
                    break;
                case TreeType.Dialog:
                    DialogTree dialogTree = new DialogTree(gf, treeType);
                        treeNodeList = getTreeNodeListFromFile(path,treeType);
                        dialogTree.treeNodeDictionary = getDialogTreeNodeFromList(treeNodeList);
                        dialogTree.currentIndex = treeNodeList[0].index;
                        t = dialogTree;
                    break;
                case TreeType.Quest:
                    QuestTree questTree = new QuestTree(gf, treeType);
                        treeNodeList = getTreeNodeListFromFile(path,treeType);
                        questTree.treeNodeDictionary = getQuestTreeNodeFromList(treeNodeList);
                        questTree.currentIndex = treeNodeList[0].index;
                        t = questTree;
                    break;
                default:
                    break;
            }
            return t;
        }

        private static List<ITreeNode> getTreeNodeListFromString(string data, TreeType treeType)
        {
            List<ITreeNode> treeNodeList = new List<ITreeNode>();


            var nodeList = ParseHelper.getSplitList(data, "----");
            foreach (var node in nodeList)
            {
                treeNodeList.Add(getTreeNode(node, treeType));
            }

            return treeNodeList;
        }

        //DEPRECATED
        private static List<ITreeNode> getTreeNodeListFromFile(string path, TreeType treeType)
        {
            List<ITreeNode> treeNodeList = new List<ITreeNode>();

            string fileTxt = File.ReadAllText(path);
            var nodeList = ParseHelper.getSplitList(fileTxt, "----");
            foreach(var node in nodeList)
            {
                treeNodeList.Add(getTreeNode(node,treeType));
            }

            return treeNodeList;

        }

        private static ITreeNode getTreeNode(string str, TreeType treeType)
        {
            var nodePartList = ParseHelper.getSplitList(str, "--");
            var nodeDataStr = nodePartList[0];
            var linkDataStr = nodePartList[1];

            ITreeNode td = getTreeNodeFromDataStr(nodeDataStr, treeType);
            td.branchList = getTreeBranchListFromDataStr(linkDataStr);

            return td;

        }

        private static ITreeNode getTreeNodeFromDataStr(string nodeDataStr, TreeType treeType)
        {
            var dataList = ParseHelper.getSplitList(nodeDataStr, Environment.NewLine);

            ITreeNode node = null;
            switch(treeType)
            {
                case TreeType.World:
                    
                    var worldTreeNode =  new WorldTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (WorldNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    node = worldTreeNode;
                    break; 
                case TreeType.Zone:
                    var zoneTreeNode = new ZoneTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (ZoneNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    node = zoneTreeNode;
                    break; 
                case TreeType.Dialog:
                    var dialogTreeNode = new DialogTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (DialogNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                node = dialogTreeNode;
                    break;
                case TreeType.Quest:
                    var questTreeNode = new QuestTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (QuestNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    node = questTreeNode;
                    break;
                case TreeType.Battle:
                    var battleTreeNode = new BattleTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (BattleNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    node = battleTreeNode;
                    break;
                case TreeType.Info:
                     var infoTreeNode = new InfoTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (InfoNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                   node = infoTreeNode;
                    break;
                case TreeType.Store:
                    var storeTreeNode = new StoreTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (StoreNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    node = storeTreeNode;
                    break;
                default: break;
            }

            if (dataList.Count > 3)
            {
                node.flagSetList = getFlagSetFromDataStr(dataList[3]);
            }
            if (dataList.Count > 4)
            {
                node.actionList = getTreeNodeActionListFromDataStr(dataList[4]);
            }
         
            return node;
        }

        private static ITreeNodeContent getTreeNodeContentFromStr(string contentStr, TreeType treeType)
        {
            var contentList = ParseHelper.getSplitList(contentStr,";");
            switch(treeType)
            {
                case TreeType.World:
                    return new WorldNodeContent() {linkIndex=Int64.Parse(contentList[0]), zoneName=contentList[1], description=contentList[2],avatar=contentList[3],x=Int32.Parse(contentList[4]),y=Int32.Parse(contentList[5]) };
                case TreeType.Zone:
                    return new ZoneNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = getZoneNodeTypeFromStr(contentList[2]), description = contentList[3], icon = contentList[4], x = Int32.Parse(contentList[5]), y = Int32.Parse(contentList[6]) };
                case TreeType.Dialog:
                    return new DialogNodeContent() {linkIndex = Int64.Parse(contentList[0]), speaker=contentList[1],portrait=contentList[2], text=contentList[3] };
                case TreeType.Quest:
                    return new QuestNodeContent() { flagName = contentList[0], description = contentList[1] };
                case TreeType.Battle:
                    return getBattleNodeContentFromStr(contentStr);
                case TreeType.Info:
                    return getInfoNodeContentFromStr(contentStr);
                case TreeType.Store:
                    return getStoreNodeContentFromStr(contentStr);
                default:
                    return null;
            }
        }

        //Parsing of BattleNodeContent, helper for getTreeNodeContentFromStr
        private static BattleNodeContent getBattleNodeContentFromStr(string contentStr)
        {
              var contentList = ParseHelper.getSplitList(contentStr,";");
             BattleNodeType battleNodeType = getBattleNodeTypeFromStr(contentList[2]);
             switch (battleNodeType)
              {
                  case BattleNodeType.Info:
                      return new BattleNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = battleNodeType, description = contentList[3], icon=contentList[4] };
                  case BattleNodeType.Enemy:
                      return new BattleNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = battleNodeType, description = contentList[3], icon = contentList[4], x = Int32.Parse(contentList[5]), y = Int32.Parse(contentList[6]) };
                  case BattleNodeType.Loot:
                      return new BattleNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = battleNodeType, description = contentList[3], icon = contentList[4], count = Int32.Parse(contentList[5]) };
                  case BattleNodeType.Win:
                      return new BattleNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = battleNodeType, description = contentList[3], icon = contentList[4] };
               
                 default: return null;
              }
        }

        private static InfoNodeContent getInfoNodeContentFromStr(string contentStr)
        {
            var contentList = ParseHelper.getSplitList(contentStr, ";");
            InfoNodeType infoNodeType = getInfoNodeTypeFromStr(contentList[2]);
            switch (infoNodeType)
            {
                case InfoNodeType.Info:
                    return new InfoNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = infoNodeType, icon = contentList[3], text = contentList[4] };
                default: return null;
            }
        }

        public static StoreNodeContent getStoreNodeContentFromStr(string contentStr)
        {
            var contentList = ParseHelper.getSplitList(contentStr, ";");
            StoreNodeType storeNodeType = getStoreNodeTypeFromStr(contentList[1]);
            switch (storeNodeType)
            {
                case StoreNodeType.Info:
                    return new StoreNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeType = storeNodeType, storeName = contentList[2], storePortrait = contentList[3], storeDialog = contentList[4]};
                case StoreNodeType.ItemClass:
                    return new StoreNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeType = storeNodeType, itemType = getItemTypeFromStr(contentList[2]), buyPrice = float.Parse(contentList[3]), sellPrice = float.Parse(contentList[4]), count = Int32.Parse(contentList[5]) };
                case StoreNodeType.ItemIndex:
                    return new StoreNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeType = storeNodeType, itemType = getItemTypeFromStr(contentList[2]), buyPrice = float.Parse(contentList[3]), sellPrice = float.Parse(contentList[4]), count = Int32.Parse(contentList[5]) };
                default: return null;
            }
        }

        private static ZoneNodeType getZoneNodeTypeFromStr(string zoneTypeStr)
        {
            return (from data in Enum.GetValues(typeof(ZoneNodeType)).Cast<ZoneNodeType>().ToList()
                            where data.ToString() == zoneTypeStr
                            select data).FirstOrDefault();
        }

        private static BattleNodeType getBattleNodeTypeFromStr(string battleTypeStr)
        {
            return (from data in Enum.GetValues(typeof(BattleNodeType)).Cast<BattleNodeType>().ToList()
                    where data.ToString() == battleTypeStr
                    select data).FirstOrDefault();
        }

        private static InfoNodeType getInfoNodeTypeFromStr(string infoTypeStr)
        {
            return (from data in Enum.GetValues(typeof(InfoNodeType)).Cast<InfoNodeType>().ToList()
                    where data.ToString() == infoTypeStr
                    select data).FirstOrDefault();
        }

        private static StoreNodeType getStoreNodeTypeFromStr(string storeTypeStr)
        {
            return (from data in Enum.GetValues(typeof(StoreNodeType)).Cast<StoreNodeType>().ToList()
                    where data.ToString() == storeTypeStr
                    select data).FirstOrDefault();
        }

        private static ItemType getItemTypeFromStr(string itemtypeStr)
        {
            return (from data in Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList()
                    where data.ToString() == itemtypeStr
                    select data).FirstOrDefault();
        }

        public static NodeActionType getNodeActionTypeFromStr(string nodeActionStr)
        {
            return (from data in Enum.GetValues(typeof(NodeActionType)).Cast<NodeActionType>().ToList()
                    where data.ToString() == nodeActionStr
                    select data).FirstOrDefault();
        }

        //Format: {<nodeAction>;<index>;<action name>;<action count>},{action1},{action3}
        private static List<TreeNodeAction> getTreeNodeActionListFromDataStr(string actionListStr)
        {
            List<TreeNodeAction> actionList = new List<TreeNodeAction>();
            List<string> actionSplitList = actionListStr.Split(',').ToList();
            foreach (var actionStr in actionSplitList)
            {
                var fieldSplitList = ParseHelper.getSplitListInBlock(actionStr, ";", "{", "}");
                if(fieldSplitList.Count > 2){
                    TreeNodeAction tempAction = new TreeNodeAction() { actionType = getNodeActionTypeFromStr(fieldSplitList[0]), index = Int64.Parse(fieldSplitList[1]), actionName = fieldSplitList[2], count = Int32.Parse(fieldSplitList[3]) };

                    actionList.Add(tempAction);
                }
               
            }

            return actionList;
        }

        //Defaulting to a list of bool flags
        private static List<TreeNodeFlagSet> getFlagSetFromDataStr(string flagSetStr)
        {
            List<TreeNodeFlagSet> flagSetList = new List<TreeNodeFlagSet>();

            var flagList = ParseHelper.getSplitListInBlock(flagSetStr,";","{","}");
            foreach(var flag in flagList)
            {
                flagSetList.Add(new TreeNodeFlagSet(){flagName=flag,flagType=FlagType.boolFlag,value="true"});
            }
            return flagSetList;
        }

        private static List<TreeBranch> getTreeBranchListFromDataStr(string linkDataStr)
        {
            List<TreeBranch> branchList = new List<TreeBranch>();

            var linkList = ParseHelper.getSplitList(linkDataStr, Environment.NewLine);
            foreach(var link in linkList)
            {
                TreeBranch tb = new TreeBranch();
                var linkDataList = ParseHelper.getSplitList(link, ":");
                tb.linkIndex = Int64.Parse(linkDataList[0]);
                tb.description = ParseHelper.removeBlock(linkDataList[1],"{","}");

                tb.conditionList = getTreeBranchConditionList(linkDataList[1]);

                branchList.Add(tb);
            }

            return branchList;
        }

        private static List<TreeBranchCondition> getTreeBranchConditionList(string linkStr)
        {
            List<TreeBranchCondition> branchCondList = new List<TreeBranchCondition>();
            var conditionList = ParseHelper.getSplitListInBlock(linkStr,";", "{", "}");
            foreach( var cond in conditionList)
            {
                if (cond[0].Equals('!'))
                {
                    string fixedCond = cond.Remove(0, 1);
                    branchCondList.Add(new TreeBranchCondition(fixedCond, "true", CompareType.NotEqual));
                }
                else
                {
                    branchCondList.Add(new TreeBranchCondition(cond, "true", CompareType.Equal));
                }
              
            }

            return branchCondList;

        }


        public static Dictionary<long, WorldTreeNode> getWorldTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, WorldTreeNode> treeNodeDict = new Dictionary<long, WorldTreeNode>();
            foreach (var node in treeNodeList)
            {
                WorldTreeNode wNode = (WorldTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, ZoneTreeNode> getZoneTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, ZoneTreeNode> treeNodeDict = new Dictionary<long, ZoneTreeNode>();
            foreach (var node in treeNodeList)
            {
                ZoneTreeNode wNode = (ZoneTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, DialogTreeNode> getDialogTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, DialogTreeNode> treeNodeDict = new Dictionary<long, DialogTreeNode>();
            foreach (var node in treeNodeList)
            {
                DialogTreeNode wNode = (DialogTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, QuestTreeNode> getQuestTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, QuestTreeNode> treeNodeDict = new Dictionary<long, QuestTreeNode>();
            foreach (var node in treeNodeList)
            {
                QuestTreeNode wNode = (QuestTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, BattleTreeNode> getBattleTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, BattleTreeNode> treeNodeDict = new Dictionary<long, BattleTreeNode>();
            foreach (var node in treeNodeList)
            {
                BattleTreeNode wNode = (BattleTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, InfoTreeNode> getInfoTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, InfoTreeNode> treeNodeDict = new Dictionary<long, InfoTreeNode>();
            foreach (var node in treeNodeList)
            {
                InfoTreeNode wNode = (InfoTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, StoreTreeNode> getStoreTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, StoreTreeNode> treeNodeDict = new Dictionary<long, StoreTreeNode>();
            foreach (var node in treeNodeList)
            {
                StoreTreeNode wNode = (StoreTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }
    }

