using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTriggers : MonoBehaviour
{
    Tilemap DirtTileMap;
    public Transform PlayerMovePoint;
    public ParticleSystem dirtParticles;
    public ParticleSystem BloodParticles;

    bool isQuitting = false;

    // Start is called before the first frame update
    void Start()
    {
        DirtTileMap = GameObject.Find("Tilemap Dirt").GetComponent<Tilemap>();
        isQuitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if player move to dirt
        if (collision.tag == "Dirt")
        {
            DirtTileMap.SetTile(DirtTileMap.WorldToCell(PlayerMovePoint.position), null);
            var tmpObj = Instantiate(dirtParticles, PlayerMovePoint.position, Quaternion.identity);
            tmpObj.Play();
            Destroy(tmpObj.gameObject, 1f);
        }
        //check if player move to something interactional
        else if (collision.tag == "Interactions")
        {
            string Tilename = collision.gameObject.GetComponent<Tilemap>().GetTile(collision.gameObject.GetComponent<Tilemap>().WorldToCell(Vector3Int.FloorToInt(PlayerMovePoint.position))).name;            
            //check if it is a lava
            if (Tilename.Contains("Lava"))
            {                
                Destroy(gameObject);
            }
            //check if it is a damaged dirt
            else if(Tilename.Contains("Damaged Dirt"))
            {                
                collision.GetComponent<Tilemap>().SetTile(DirtTileMap.WorldToCell(PlayerMovePoint.position), null);
                var tmpObj = Instantiate(dirtParticles, PlayerMovePoint.position, Quaternion.identity);
                tmpObj.Play();
                Destroy(tmpObj.gameObject, 1f);
            }        
        }
        else if(collision.tag == "Exit")
        {
            collision.GetComponent<AudioSource>().Play();
            GameObject.Find("GameManager").GetComponent<GameManager>().LevelCompleted();            
        }
        
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }    
    private void OnDestroy()
    {
        if (!isQuitting && GameObject.Find("GameManager").GetComponent<GameManager>().ReloadScene == false)
        {
            var tmpObj = Instantiate(BloodParticles, transform.position, Quaternion.identity);
            Destroy(tmpObj.gameObject, 60f);
            GameObject.Find("GameManager").GetComponent<GameManager>().LevelFail("You are dead!");
        }
    }
}
