using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxContent : MonoBehaviour
{
    public bool isQuitting = false;
    public GameObject Content;
    public ParticleSystem BoxParticles;
    StoneManager stoneManager;

    private void Start()
    {
        stoneManager = GameObject.Find("LevelManager").GetComponent<StoneManager>();
        isQuitting = false;
    }
    void OnApplicationQuit()
    {
        isQuitting = true;               
    }    
    private void OnDestroy()
    {
        if (!isQuitting && GameObject.Find("GameManager").GetComponent<GameManager>().ReloadScene == false)
        {
            if (Content)
            {                 
                var Obj = Instantiate(Content, transform.position, Quaternion.identity);
                if(Obj.tag == "Gems")
                {
                    stoneManager.StopCoroutine(stoneManager.MoveGems());                   
                    stoneManager.Gems.Add(Obj);
                    stoneManager.StartCoroutine(stoneManager.MoveGems());
                }                
            }
            var tempObj = Instantiate(BoxParticles, transform.position, Quaternion.identity);
            Destroy(tempObj, 0.5f);
        }
    }

}
