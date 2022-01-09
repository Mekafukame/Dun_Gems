using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ProfileMenu, PlayMenu, CreateProfileMenu, DeleteConfirmWindow;
    public GameObject gameManager;
    public GameObject[] EmptyProfiles = new GameObject[3];
    public GameObject[] Profiles = new GameObject[3];
    public TMPro.TextMeshProUGUI[] ProfilesLevelText = new TMPro.TextMeshProUGUI[3];
    public TMPro.TextMeshProUGUI[] ProfilesDiffText = new TMPro.TextMeshProUGUI[3];
    public TMPro.TextMeshProUGUI[] ProfilesNickText = new TMPro.TextMeshProUGUI[3];
    public PlayerProfile[] LoadedProfiles = new PlayerProfile[3];    
    public TMPro.TMP_InputField CreateNickname;
    public Image AlertNickname;
    public ToggleGroup DiffGroup;
    public TMPro.TextMeshProUGUI PlayLevelText;
    public TMPro.TextMeshProUGUI PlayDiffText;
    public TMPro.TextMeshProUGUI PlayNickText;
    public GameObject LevelListContent, LvlBttnPrefab;

    public RectTransform snapContent;
    RectTransform snapToButton;
    public List<GameObject> LevelButtons = new List<GameObject>();

    private int createProfileID = 0, deleteProfileID = 0;
    //onPlay button check if there is activeprofile
    public void playBttn()
    {
        PlayerProfile playerProfile = gameManager.GetComponent<GameManager>().activeProfile;        
        if (playerProfile.ProfileName != string.Empty)
        {            
            PlayMenu.SetActive(true);           
            PlayDiffText.text = playerProfile.Difficulty;
            if (playerProfile.Level >= gameManager.GetComponent<GameManager>().Levels.Count)
                PlayLevelText.text = (gameManager.GetComponent<GameManager>().Levels.Count-1).ToString();
            else
                PlayLevelText.text = playerProfile.Level.ToString();
            
            PlayNickText.text = playerProfile.ProfileName;
            snapScroll();
        }
        else
            ProfileMenu.SetActive(true);
    }

    public void snapScroll() 
    {
        if (snapToButton)
        {
            int snapBttnId = int.Parse(snapToButton.name);           
            if (snapBttnId > 3 && snapBttnId < LevelButtons.Count - 3)
            {
                var newpos = new Vector3(0, -snapToButton.rect.position.y * (snapBttnId - 3), 0);                
                snapContent.transform.localPosition = newpos;
            }
            else if(snapBttnId >= LevelButtons.Count - 3)
            {
                var newpos = new Vector3(0, -snapToButton.rect.position.y * (LevelButtons.Count - 7), 0);               
                snapContent.transform.localPosition = newpos;
            }
            else
            {
                snapContent.transform.localPosition = Vector3.zero;
            }
        }
    }
    //delete all level buttons
    public void clearLevelButtons()
    {
        foreach(GameObject obj in LevelButtons)
        {
            Destroy(obj);            
        }
        LevelButtons.Clear();
    }
    //spawn level buttons with level name and level id in name
    public void spawnLevelButtons()
    {
        
        for(int i = 0; i < gameManager.GetComponent<GameManager>().Levels.Count; i++)
        {
            var tempBttn = Instantiate(LvlBttnPrefab,LevelListContent.transform);
            tempBttn.name = i.ToString();
            tempBttn.GetComponentInChildren<Button>().onClick.AddListener(delegate { this.SelectLevel(int.Parse(tempBttn.name)); });
            if(i == 0)
            {
                tempBttn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Tutorial";
            }
            else
                tempBttn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Level " + i;
            if (i > gameManager.GetComponent<GameManager>().activeProfile.Level)
            {
                tempBttn.GetComponentInChildren<Button>().interactable = false;
            }
            else if(i == gameManager.GetComponent<GameManager>().activeProfile.Level)
            {
                snapToButton = tempBttn.GetComponent<RectTransform>();
            }
            LevelButtons.Add(tempBttn);
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        gameManager.GetComponent<GameManager>().ReloadScene = false;        
    }
    //find game manager on scene change and game run
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }
    //load config and menu items on start
    private void Start()
    {        
        gameManager.GetComponent<GameManager>().LoadConfig();
        LoadProfiles();
        spawnLevelButtons();        
    }
    //on level button click set selected level and change scene
    public void SelectLevel(int lvl)
    {
        gameManager.GetComponent<GameManager>().selectedLevel = lvl;
        gameManager.GetComponent<GameManager>().ChangeToGameScene();
    }
    //Load profiles from files
    public void LoadProfiles()
    {
        string path;
        for (int i = 0; i < 3; i++)
        {
            EmptyProfiles[i].SetActive(false);
            Profiles[i].SetActive(false);
            path = Application.persistentDataPath + "/Profile0" + i + ".prof";
            LoadedProfiles[i] = SaveSystem.LoadProfile(path);
            if (LoadedProfiles[i] != null)
            {
                fillProfile(i);
            }
            else
            {
                EmptyProfiles[i].SetActive(true);
            }
        }
    }    
    //fill profiles in main menu
    void fillProfile(int i)
    {
        ProfilesNickText[i].text = LoadedProfiles[i].ProfileName;
        ProfilesDiffText[i].text = LoadedProfiles[i].Difficulty;
        if (LoadedProfiles[i].Level >= gameManager.GetComponent<GameManager>().Levels.Count)
            ProfilesLevelText[i].text = (gameManager.GetComponent<GameManager>().Levels.Count - 1).ToString();
        else
            ProfilesLevelText[i].text = LoadedProfiles[i].Level.ToString();
        Profiles[i].SetActive(true);
    }
    //on back chceck if there is active profile
    public void profileBackBttn()
    {
        if (gameManager.GetComponent<GameManager>().activeProfile.ProfileName != string.Empty)
        {
            PlayMenu.SetActive(true);
            ProfileMenu.SetActive(false);            
        }
        else
            ProfileMenu.SetActive(false);
    }
    //on profile selection set active profile, reset level buttons for this profile and save active profile to config
    public void ChangeProfile(int i)
    {        
        gameManager.GetComponent<GameManager>().activeProfile.Difficulty = LoadedProfiles[i].Difficulty;
        gameManager.GetComponent<GameManager>().activeProfile.ProfileName = LoadedProfiles[i].ProfileName;
        gameManager.GetComponent<GameManager>().activeProfile.Level = LoadedProfiles[i].Level;
        gameManager.GetComponent<GameManager>().selectedProfile = i;
        clearLevelButtons();
        spawnLevelButtons();
        gameManager.GetComponent<GameManager>().saveConfig();        
        playBttn();        
    }
    //on Add profile button open profile creation for this slot
    public void AddProfileBttn(int id)
    {
        ProfileMenu.SetActive(false);
        CreateProfileMenu.SetActive(true);
        CreateNickname.text = "";
        createProfileID = id;       
    }
    //on remove profile display confirm window
    public void RemoveProfileBttn(int id)
    {
        deleteProfileID = id;
        
        DeleteConfirmWindow.SetActive(true);                
    }
    //on confirm delete profile file and reload profiles
    public void DeleteProfile()
    {
        DeleteConfirmWindow.SetActive(false);
        PlayerProfile playerProfile = gameManager.GetComponent<GameManager>().activeProfile;
        if (LoadedProfiles[deleteProfileID].ProfileName == playerProfile.ProfileName)
        {
            playerProfile.ProfileName = string.Empty;
            playerProfile.Level = 0;
            playerProfile.Difficulty = string.Empty;
        }
        string path = Application.persistentDataPath + "/Profile0" + deleteProfileID + ".prof";       
        SaveSystem.DeleteFile(path);
        LoadProfiles();
    }

    //check input for nickname and create new profile / reload profiles || if too short show alert 
    public void CreateProfile()
    {
        if(CreateNickname.text.Length > 0)
        {
            SaveSystem.CreateProfile(1, getDifficulty().name,CreateNickname.text.ToUpper() ,"Profile0" + createProfileID);
            AlertNickname.gameObject.SetActive(false);
            CreateProfileMenu.SetActive(false);
            LoadProfiles();
            ProfileMenu.SetActive(true);
        }
        else
        {
            AlertNickname.gameObject.SetActive(true);
            AlertNickname.CrossFadeAlpha(0, 0, false);
            AlertNickname.CrossFadeAlpha(255, 100f, false);
        }
    }
    //get selected toggle in diff group
    Toggle getDifficulty()
    {
        var toggles = DiffGroup.ActiveToggles();
        foreach (var t in toggles)
            if (t.isOn) return t;  //returns selected toggle
        return null;           // if nothing is selected return null
    }

    //Quit app
    public void ExitBttn()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
