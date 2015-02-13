using UnityEngine;
using System.Collections;

public class StartGameScript : MonoBehaviour {

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
}
