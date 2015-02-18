using UnityEngine;
using System.Collections;

public class StartGameScript : MonoBehaviour {


    public string text { get; set; }
    public int battleIndex { get; set; }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadBattle(int battleIndex)
    {

        this.battleIndex = battleIndex;
        Application.LoadLevel("BattleScene");
    }

    public void GameOverScreen()
    {
        Application.LoadLevel("GameOverScene");
    }
}
