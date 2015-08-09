using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    #region Node

    public abstract class TreeNode
    {
        public long index { get; set; }
        public string name { get; set; }

        public List<TreeBranch> branchList { get; set; }

        public List<TreeNodeFlagSet> flagSetList { get; set; }

        public List<TreeNodeAction> actionList { get; set; }

        public void SelectNode(ITree t)
        {
            if (flagSetList != null)
            {
                foreach (var flag in flagSetList)
                {
                    t.globalFlags.addFlag(flag.flagName, flag.flagType, flag.value);
                }
            }
        }

        public List<TreeBranch> getBranchList(ITree t)
        {

            List<TreeBranch> branchList = new List<TreeBranch>();
            foreach (var tb in this.branchList)
            {
                var branchInclude = true;
                //check conditions on branch
                if (tb.conditionList != null)
                {
                    foreach (var cond in tb.conditionList)
                    {
                        if (!t.globalFlags.checkFlag(cond.flagName, cond.value, cond.flagCompareType))
                        {
                            branchInclude = false;
                        }
                    }
                }

                if (branchInclude)
                {
                    branchList.Add(tb);
                }
            }

            return branchList;
        }

        public List<string> getBranchListDisplay(ITree t)
        {
            List<string> strList = new List<string>();
            int count = 1;
            foreach (var tb in branchList)
            {
                var branchInclude = true;
                //check conditions on branch
                if (tb.conditionList != null)
                {
                    foreach (var cond in tb.conditionList)
                    {
                        if (!t.globalFlags.checkFlag(cond.flagName, cond.value, cond.flagCompareType))
                        {
                            branchInclude = false;
                        }
                    }
                }

                if (branchInclude)
                {
                    strList.Add(string.Format("-->{0}. {1}", count, tb.ToString()));
                    count++;
                }
            }

            return strList;
        }

        //given the index of the selected index, return the new branch index
        public long getBranchIndex(int selected)
        {
            selected--;
            if (selected > -1 && selected < branchList.Count)
            {
                return branchList[selected].linkIndex;
            }
            return -1;
        }
    }

    public class WorldTreeNode : TreeNode, ITreeNode
    {
        public WorldNodeContent content { get; set; }

        public WorldTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, WorldNodeContent content)
        {
            this.content = content;
            this.index = index;
            this.name = name;

            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
            return retval;
        }

    }

    public class ZoneTreeNode : TreeNode, ITreeNode
    {
        public ZoneNodeContent content { get; set; }

        public ZoneTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, ZoneNodeContent content)
        {
            this.content = content;
            this.index = index;
            this.name = name;

            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);

            return retval;

        }
    }

    public class DialogTreeNode : TreeNode, ITreeNode
    {
        public DialogNodeContent content { get; set; }

        public DialogTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, DialogNodeContent content)
        {
            this.content = content;
            this.index = index;
            this.name = name;

            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
            return retval;
        }

    }

    public class QuestTreeNode : TreeNode, ITreeNode
    {
        public QuestNodeContent content { get; set; }

        public QuestTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, QuestNodeContent content)
        {
            this.content = content;
            this.index = index;
            this.name = name;

            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
            return retval;
        }

    }

    public class BattleTreeNode : TreeNode, ITreeNode
    {
        public BattleNodeContent content { get; set; }

        public BattleTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, BattleNodeContent content)
        {
            this.content = content;
            this.index = index;
            this.name = name;

            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
            return retval;
        }

    }

    public class InfoTreeNode : TreeNode, ITreeNode
    {
        public InfoNodeContent content { get; set; }

        public InfoTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, InfoNodeContent content)
        {
            this.content = content;
            this.index = index;
            this.name = name;

            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
            return retval;
        }

    }

    #endregion


    #region NodeContent

    public class WorldNodeContent : ITreeNodeContent
    {
        public long linkIndex { get; set; }
        public string zoneName { get; set; }
        public string image { get; set; }
        public string avatar { get; set; }
        public string description { get; set; }
        public int x { get; set; }
        public int y { get; set; }

    }

    public class ZoneNodeContent : ITreeNodeContent
    {
        public ZoneNodeType nodeType { get; set; }
        public string nodeName { get; set; }
        public long linkIndex { get; set; }
        public string icon { get; set; }
        public string description { get;set;}
        public int x {get;set;}
        public int y{get;set;}
    }

    public class DialogNodeContent : ITreeNodeContent
    {
        public string portrait { get; set; }
        public string speaker { get; set; }
        public string text { get; set; }
        public long linkIndex { get; set; }

    }

    public class QuestNodeContent : ITreeNodeContent
    {
        public string flagName { get; set; }
        public string description { get; set; }
    }

    public class BattleNodeContent : ITreeNodeContent
    {
        public BattleNodeType nodeType { get; set; }
        public string nodeName { get; set; }
        public long linkIndex { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public int x { get;set; }
        public int y { get; set; }
        public int count { get; set; }
    }

    public class InfoNodeContent : ITreeNodeContent
    {
        public InfoNodeType nodeType { get; set; }
        public string icon { get; set; }
        public string nodeName { get; set; }
        public string text { get; set; }
        public long linkIndex { get; set; }
    }

#endregion


