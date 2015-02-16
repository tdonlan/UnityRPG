using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour {

    GameControllerScript gameControllerScript { get; set; }
	// Use this for initialization
	void Start () {
       // this.gameControllerScript = GameObject.FindObjectOfType<GameControllerScript>();

        //var canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        //UIHelper.UpdateTextComponent(canvas, "BattleLogText", gameControllerScript.battleGame.battleLog.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Restart()
    {
        Application.LoadLevel("StartScene");
    }
}
