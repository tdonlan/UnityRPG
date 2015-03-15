using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class CharacterColliderScript : MonoBehaviour {

    public GameControllerScript gameControllerScript;
	// Use this for initialization
	void Start () {
        getGameControllerScript();
	}

    private void getGameControllerScript()
    {
        if(this.gameControllerScript == null)
        {
            var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            this.gameControllerScript = gameControllerObject.GetComponentInChildren<GameControllerScript>();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseOver()
    {
        gameControllerScript.UpdateCharacterHover();


    }

    public void OnMouseExit()
    {
        gameControllerScript.ClearCharacterHover();
    }
}
