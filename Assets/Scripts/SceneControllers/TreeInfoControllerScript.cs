using UnityEngine;
using System.Collections;
using UnityRPG;

public class TreeInfoControllerScript : MonoBehaviour {


    public GameObject TreeInfoPanel;
    private RectTransform panelRectTransform;

	// Use this for initialization
	void Start () {
        initRefs();
	}

    private void initRefs()
    {
        panelRectTransform = TreeInfoPanel.GetComponent<RectTransform>();
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateInfo(GameDataObject gameObject, InfoTree infoTree)
    {
        if (infoTree != null)
        {
            InfoTreeNode infoNode = (InfoTreeNode)infoTree.getNode(infoTree.currentIndex);
            infoNode.SelectNode(infoTree);
            UIHelper.UpdateTextComponent(TreeInfoPanel, "TreeInfoTitle", infoNode.content.nodeName);
            UIHelper.UpdateTextComponent(TreeInfoPanel, "TreeInfoText", infoNode.content.text);

            gameObject.runActions(infoNode.actionList);
        }

    }

    public void ClosePanel()
    {
        panelRectTransform.localPosition = new Vector3(2000, 2000, 0);
    }
}
