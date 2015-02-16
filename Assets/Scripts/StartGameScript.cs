using UnityEngine;
using System.Collections;

public class StartGameScript : MonoBehaviour {


    public string text { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadBattle()
    {
        Application.LoadLevel("BattleScene");
    }

    public void GameOverScreen()
    {
        Application.LoadLevel("GameOverScene");
    }
}
