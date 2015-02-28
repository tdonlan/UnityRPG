using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

    public Canvas canvas { get; set; }
    public GameObject HoverStatsObject { get; set; }
    public bool isStatsDisplay { get; set; }

    GameObject HoverPrefab { get; set; }
    

	// Use this for initialization
	void Start () {
        HoverPrefab = Resources.Load<GameObject>("HoverPrefab");
        isStatsDisplay = false;

        canvas = GameObject.FindObjectOfType<Canvas>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayHoverStats()
    {
        if(!isStatsDisplay)
        {
            isStatsDisplay = true;
            HoverStatsObject = (GameObject)Instantiate(HoverPrefab);
            HoverStatsObject.transform.SetParent(canvas.transform,true);

            //UIHelper.MoveUIObject(HoverStatsObject, new Vector3(-200, -200,0));

            UIHelper.MoveUIObject(HoverStatsObject, getMouseViewportCoords() + new Vector3(0,100,0));
            //HoverStatsObject.transform.position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
            
        }
    }

    private Vector3 getMouseViewportCoords()
    {
           var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        var camera = mainCamera.GetComponent<Camera>();

        return camera.ScreenToViewportPoint(Input.mousePosition);
    }

    public void HideHoverStats()
    {
        Destroy(HoverStatsObject);
        isStatsDisplay = false;
    }
}
