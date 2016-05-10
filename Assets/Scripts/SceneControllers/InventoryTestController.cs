using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryTestController : MonoBehaviour
{
	public GameDataObject gameDataObject { get; set; }

	public Text debugText;

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnLevelWasLoaded(int level)
	{
		loadGameData();

	}

	private void loadGameData()
	{
		gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

