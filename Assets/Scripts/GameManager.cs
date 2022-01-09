using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public PlayerProfile activeProfile = null;
    public AudioMixer mainMixer;
    public bool ReloadScene = false;
    public GameObject WinScreen, FailScreen, GameCopleteScreen;
    public TMPro.TextMeshProUGUI GemsLeftText, ScoreText, FailText;
    public List<GameObject> Levels = new List<GameObject>();
    public int selectedLevel;
    public int selectedProfile;
    public GameObject LevelManager = null;
    public int GemsLeft = 0;
    // Start is called before the first frame update

    //Load variables from config
    public void LoadConfig()
    {
        Config tempCfg = SaveSystem.LoadConfig();
        if (tempCfg != null)
        {
            mainMixer.SetFloat("SoundVolume", tempCfg.SoundVolume);
            mainMixer.SetFloat("MusicVolume", tempCfg.MusicVolume);
            activeProfile = tempCfg.LastProfile;
            selectedProfile = tempCfg.LastProfileID;           
        }
    }
    //save variables to config
    public void saveConfig()
    {
        float musicVolume, soundVolume;
        mainMixer.GetFloat("MusicVolume", out musicVolume);
        mainMixer.GetFloat("SoundVolume", out soundVolume);
        SaveSystem.SaveConfig(musicVolume, soundVolume, activeProfile, selectedProfile);       
    }
    //On level complete check for progress and save progress, calculate score and display WinScreen
    public void LevelCompleted()
    {
        Time.timeScale = 0;
        if (activeProfile.Level <= selectedLevel)
        {
            activeProfile.Level++;
            string path = "Profile0" + selectedProfile;
            SaveSystem.CreateProfile(activeProfile.Level, activeProfile.Difficulty, activeProfile.ProfileName, path);            
            saveConfig();
        }

        WinScreen.SetActive(true);         
               
        var gemlist = LevelManager.GetComponent<StoneManager>().Gems;
        foreach(GameObject obj in gemlist)
        {
            if (obj != null)
            {
                GemsLeft++;
            }
        }        
        GemsLeftText.text = "";
        for (int i = 0; i < 9 - GemsLeft.ToString().Length; i++)
        {
            GemsLeftText.text += "0";            
        }
        GemsLeftText.text += GemsLeft;
        int score = LevelManager.GetComponent<LevelSettings>().Score;
        score += Mathf.RoundToInt(LevelManager.GetComponent<LevelSettings>().LevelTime) * 10;
        score -= GemsLeft * 100;
        if(activeProfile.Difficulty == "Easy")
        {
            score /= 2;
        }
        else if(activeProfile.Difficulty == "Hard")
        {
            score *= 2;
        }
        ScoreText.text = "";
        for (int i = 0; i < 9 - score.ToString().Length; i++)
        {
            ScoreText.text += "0";
        }
        ScoreText.text += score.ToString();
    }
    //on next button go to next level
    public void nextLevel()
    {        
        if (Levels.Count - 1 == selectedLevel)
        {
            Debug.Log("complete");
            WinScreen.SetActive(false);
            GameCopleteScreen.SetActive(true);

        }
        else { 
            selectedLevel++;
            WinScreen.SetActive(false);
            GetComponent<RestartGame>().Restart();
        }        
    }
    //on level fail show failWindow after 1 second
    public void LevelFail(string message)
    {
        
        StartCoroutine(LevelFailTime(message));        
}
    IEnumerator LevelFailTime(string message)
    {
        yield return new WaitForSeconds(1);
        FailScreen.SetActive(true);        
        FailText.text = message;             
        Time.timeScale = 0;
    }    
    //save config on appQuit
    private void OnApplicationQuit()
    {
        saveConfig();
    }
    //change gameManager to global gameobject and start menu
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        ChangeToMainMenu();
    }
    //change scene to levels
    public void ChangeToGameScene()
    {
        GemsLeft = 0;
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }
    //change scene to menu
    public void ChangeToMainMenu()
    {
        GemsLeft = 0;
        ReloadScene = true;
        SceneManager.LoadScene("MainMenuScene");
        Time.timeScale = 1;

    }
}
