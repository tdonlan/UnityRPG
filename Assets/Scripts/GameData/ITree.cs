using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



    public interface ITree
    {
        string treeName { get; set; }
        TreeType treeType { get; set; }
        GlobalFlags globalFlags { get; set; }

        ITreeNode getNode(long index);
        void SelectNode(long index);
        bool checkNode(long index);
        bool validateTreeLinks();
    }

    public interface ITreeNode
    {
         long index { get; set; }
         List<TreeBranch> branchList { get; set; }
         List<TreeNodeFlagSet> flagSetList { get; set; }
         List<TreeNodeAction> actionList { get; set; }

          void SelectNode(ITree t);
          List<TreeBranch> getBranchList(ITree t);
          List<string> getBranchListDisplay(ITree t);
          long getBranchIndex(int selected);
    }

    public interface ITreeNodeContent
    {

    }


