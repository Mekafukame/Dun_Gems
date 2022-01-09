using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GUI_Manager : MonoBehaviour
{

    public TMPro.TMP_Text[] Keys = new TMPro.TMP_Text[6];
    public TMPro.TMP_Text Bombs;
    public TMPro.TMP_Text Gems;
    public TMPro.TMP_Text Time;
    public TMPro.TMP_Text Level;
    LevelSettings Settings;
    public PlayerInv Inventory;
    // Start is called before the first frame update
    void Start()
    {
        Settings = GameObject.Find("LevelManager").GetComponent<LevelSettings>();
        Level.text = Settings.LevelName;
        Time.text = Settings.LevelTime.ToString();        
        StartCoroutine(TimeCountdown());
        updateGUI();        
    }        
    
    public void updateGUI()
    {               
        Bombs.text = Inventory.numOfBombs.ToString();
        if (Settings.GemsReq > Inventory.numOfPoints)
        {
            Gems.text = (Settings.GemsReq - Inventory.numOfPoints).ToString();
        }
        else
            Gems.text = "0";
        for (int i = 0; i < Keys.Length; i++)
        {
            Keys[i].text = Inventory.Keys[i].ToString();
        }
    }
    public void ToMainMenu()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().ChangeToMainMenu();
    }
    IEnumerator TimeCountdown()
    {
        yield return new WaitForSeconds(1);
        if (Inventory)
        {
            Settings.LevelTime--;
            Time.text = Settings.LevelTime.ToString();
            if (Settings.LevelTime > 0)
                StartCoroutine(TimeCountdown());
            else
                Settings.EndOfTime();
        }
    }
    
}
