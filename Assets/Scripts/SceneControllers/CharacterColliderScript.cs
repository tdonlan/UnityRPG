using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class CharacterColliderScript : MonoBehaviour {

    public BattleSceneControllerScript gameControllerScript;
	// Use this for initialization
	void Start () {
        getGameControllerScript();
	}

    private void getGameControllerScript()
    {
        if(this.gameControllerScript == null)
        {
            //var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            //this.gameControllerScript = gameControllerObject.GetComponentInChildren<BattleSceneControllerScript>();

            this.gameControllerScript = GameObject.FindObjectOfType<BattleSceneControllerScript>().GetComponent<BattleSceneControllerScript>();
   
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnMouseOver()
    {
        //gameControllerScript.SelectCharacter();

    }

    public void OnMouseExit()
    {
        //gameControllerScript.DeselectCharacter();
    }
}
