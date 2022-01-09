using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        var gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnLevel(gamemanager.Levels[gamemanager.selectedLevel]);        
    }
    void spawnLevel(GameObject level)
    {
        Instantiate(level);        
    }
     
   
}
