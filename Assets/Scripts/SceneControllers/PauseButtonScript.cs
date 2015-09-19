using UnityEngine;
using System.Collections;

public class PauseButtonScript : MonoBehaviour {

    public GameDataObject gameDataObject;
    public GameObject pauseMenuObject;
    public RectTransform pauseMenuRectTransform;

	// Use this for initialization
	void Start () {
        loadGameData();
        initRefs();
	}


    private void loadGameData()
    {
        gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
    }

    private void initRefs()
    {
        pauseMenuObject = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenuRectTransform = pauseMenuObject.GetComponent<RectTransform>();
    }

    public void DisplayMenu()
    {
        gameDataObject.isPaused = true;

        pauseMenuRectTransform.localPosition = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
