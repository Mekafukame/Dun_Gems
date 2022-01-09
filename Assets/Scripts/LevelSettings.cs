using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{

    public int GemsReq;
    public int GemsEasy, GemsNormal, GemsHard;
    public int LevelTime;
    public string LevelName;
    [HideInInspector]
    public int Score = 0;
    GameManager gameManager;

    private void Start()
    {        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager)
        {
            
            if (gameManager.activeProfile.Difficulty == "Easy")
            {
                GemsReq = GemsEasy;
            }
            else if (gameManager.activeProfile.Difficulty == "Normal")
            {
                GemsReq = GemsNormal;
            }
            else if (gameManager.activeProfile.Difficulty == "Hard")
            {
                GemsReq = GemsHard;
            }
            else
            {
                GemsReq = GemsNormal;
            }
            gameManager.ReloadScene = false;
        }
        else
        {
            GemsReq = GemsNormal;
        }
        GameObject.Find("Player").GetComponent<PlayerInv>().Player_GUI.updateGUI();
        GameObject.Find("Exit Doors").GetComponent<ExitDoorController>().updatePoints();
        gameManager.LevelManager = gameObject;
    }
   
    public void EndOfTime()
    {
        GetComponent<AudioSource>().Play();
        GameObject.Find("GameManager").GetComponent<GameManager>().LevelFail("The time is over!");        
    }
}
