using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartGame : MonoBehaviour
{

    public void Restart()
    {

        var tempScene = SceneManager.GetActiveScene().name;
        GameObject.Find("GameManager").GetComponent<GameManager>().GemsLeft = 0;
        GameObject.Find("GameManager").GetComponent<GameManager>().ReloadScene = true;              
        Time.timeScale = 1;
        SceneManager.LoadScene(tempScene); // loads current scene
        
    }

}