using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour {

    BattleSceneControllerScript gameControllerScript { get; set; }
	// Use this for initialization
	void Start () {
        this.gameControllerScript = GameObject.FindObjectOfType<BattleSceneControllerScript>();

        LoadBattleLog();
	}

    private void LoadBattleLog()
    {
        var battleLogContent = GameObject.FindGameObjectWithTag("BattleLogContent");
        UIHelper.UpdateTextComponent(battleLogContent, "BattleLogContent", gameControllerScript.battleGame.battleLog.PrintLog(0));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Restart()
    {
        Destroy(this.gameControllerScript);
		SceneManager.LoadScene ("StartScene");
       
    }
}
