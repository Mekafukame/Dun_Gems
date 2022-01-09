using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{

    public GameObject[] Stones = new GameObject[3];
    public float Cooldown = 5f;
    public LayerMask Layers;
    StoneManager stoneManager;

    // Start is called before the first frame update
    void Start()
    {
        stoneManager = GameObject.Find("LevelManager").GetComponent<StoneManager>();
        StartCoroutine(Timer());        
    }
    void CheckTile()
    {
        if (Physics2D.OverlapCircle(transform.position, .4f, Layers))
        {
            var tempLayer = Physics2D.OverlapCircle(transform.position, .2f, Layers);
            if(tempLayer.tag == "Player" || tempLayer.tag == "Enemie" || tempLayer.tag == "Teleport")
            {
                SpawnStone();
            }
        }
        else
        {
            SpawnStone();
        }
    }
    void SpawnStone()
    {
        int i = Mathf.RoundToInt(Random.Range(0f, 2f));
        var tempObj = Instantiate(Stones[i], transform.position, Quaternion.identity);
        tempObj.transform.localScale = new Vector3(0,0,0);
        tempObj.GetComponent<LightStone>().isFalling = true;
        tempObj.GetComponent<LightStone>().isScaling = true;
        tempObj.GetComponent<CircleCollider2D>().enabled = false;
        stoneManager.LightStones.Add(tempObj);
        
    }
    IEnumerator Timer()
    {
        CheckTile();
        yield return new WaitForSeconds(Cooldown);
        StartCoroutine(Timer());
    }
}
