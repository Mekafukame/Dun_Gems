using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombExplosion : MonoBehaviour
{
    public GameObject AnimationObj;
    public Transform StoneMovePoint;

    public LayerMask stopMovement;
    public LayerMask otherTilemaps;
    public LayerMask collectibles;
    public LayerMask Enemies;

    public bool Exploded = false;
    Vector3[] ExploPos = new Vector3[8];
    // Start is called before the first frame update
    void Start()
    {
        updateExploPos();
    }       
    public void updateExploPos()
    {
        //explo pos for explostone
        if (StoneMovePoint) { 
        ExploPos[0] = new Vector3(StoneMovePoint.position.x + 1, StoneMovePoint.position.y - 1, 0f);
        ExploPos[1] = new Vector3(StoneMovePoint.position.x -1, StoneMovePoint.position.y, 0f);
        ExploPos[2] = new Vector3(StoneMovePoint.position.x -1, StoneMovePoint.position.y -1, 0f);
        ExploPos[3] = new Vector3(StoneMovePoint.position.x -1, StoneMovePoint.position.y +1, 0f);
        ExploPos[4] = new Vector3(StoneMovePoint.position.x , StoneMovePoint.position.y +1, 0f);
        ExploPos[5] = new Vector3(StoneMovePoint.position.x, StoneMovePoint.position.y -1, 0f);
        ExploPos[6] = new Vector3(StoneMovePoint.position.x +1, StoneMovePoint.position.y, 0f);
        ExploPos[7] = new Vector3(StoneMovePoint.position.x +1, StoneMovePoint.position.y +1, 0f);
        }
        //explo pos for bomb
        else
        {
            ExploPos[0] = new Vector3(transform.position.x + 1, transform.position.y - 1, 0f);
            ExploPos[1] = new Vector3(transform.position.x - 1, transform.position.y, 0f);
            ExploPos[2] = new Vector3(transform.position.x - 1, transform.position.y - 1, 0f);
            ExploPos[3] = new Vector3(transform.position.x - 1, transform.position.y + 1, 0f);
            ExploPos[4] = new Vector3(transform.position.x, transform.position.y + 1, 0f);
            ExploPos[5] = new Vector3(transform.position.x, transform.position.y - 1, 0f);
            ExploPos[6] = new Vector3(transform.position.x + 1, transform.position.y, 0f);
            ExploPos[7] = new Vector3(transform.position.x + 1, transform.position.y + 1, 0f);
        }
    }
    //spawn explosion prefab
    void spawnExplosion(Vector3 pos)
    {
        var tmpObj = Instantiate(AnimationObj, pos, Quaternion.identity);
        Destroy(tmpObj, 0.5f);
    }
    //Delete tile
    void DestroyTile(Tilemap tileMap, Vector3 pos)
    {
        tileMap.SetTile(tileMap.WorldToCell(pos), null);       
    }

    public void Explode()
    {
        if (!Exploded) { 
            Exploded = true;
            string tagName = string.Empty;
            //spawn explosion on gameObject pos
            if (StoneMovePoint)
                spawnExplosion(StoneMovePoint.position);
            else
            {
                spawnExplosion(transform.position);
                                
                var tempObj = GameObject.Find("Player");                
                if (tempObj.transform.position == transform.position)
                {
                    Destroy(tempObj.gameObject);
                }
                
            }
            for (int i = 0; i < ExploPos.Length; i++)
            {
                //check if it is solid wall
                if (!Physics2D.OverlapCircle(ExploPos[i], .2f, stopMovement)){
                    tagName = string.Empty;
                    //check if it is a othertilemap object
                    if (Physics2D.OverlapCircle(ExploPos[i], .2f, otherTilemaps))
                    {
                        tagName = Physics2D.OverlapCircle(ExploPos[i], .2f, otherTilemaps).tag;
                    }
                    //check if it collectible object
                    else if (Physics2D.OverlapCircle(ExploPos[i], .2f, collectibles))
                    {
                        tagName = Physics2D.OverlapCircle(ExploPos[i], .2f, collectibles).tag;
                    }
                    else if (Physics2D.OverlapCircle(ExploPos[i], .2f, Enemies))
                    {
                        tagName = Physics2D.OverlapCircle(ExploPos[i], .2f, Enemies).tag;
                    }
                    if (tagName != string.Empty)
                    {
                        //check if it is dirt - destroy tile
                        if (tagName == "Dirt")
                        {

                            Tilemap tileMap = Physics2D.OverlapCircle(ExploPos[i], .2f, otherTilemaps).GetComponent<Tilemap>();
                            spawnExplosion(ExploPos[i]);
                            DestroyTile(tileMap, ExploPos[i]);
                        }
                        //check if it interactional tile, object
                        else if (tagName == "Interactions")
                        {
                            Tilemap tileMap = Physics2D.OverlapCircle(ExploPos[i], .2f, otherTilemaps).GetComponent<Tilemap>();
                            string tileName = tileMap.GetTile(tileMap.WorldToCell(ExploPos[i])).name;
                            //check if it is damaged stone
                            if (tileName.Contains("Damaged"))
                            {
                                spawnExplosion(ExploPos[i]);
                                DestroyTile(tileMap, ExploPos[i]);
                            }
                            //check if it is a lava
                            if (tileName.Contains("Lava"))
                            {
                                spawnExplosion(ExploPos[i]);
                            }
                        }

                        //check if it is destroyable prefab
                        else if (tagName == "Player" || tagName == "Enemie"|| tagName == "Stone" || tagName == "Gems" || tagName == "Bomb" || tagName == "Key" || tagName == "Box")
                        {
                            if (Physics2D.OverlapCircle(ExploPos[i], .2f, otherTilemaps))
                            {
                                var tempCollider = Physics2D.OverlapCircle(ExploPos[i], .2f, otherTilemaps);
                                //check if it is a explosive stone - explode this stone (chain reaction)
                                if (tempCollider.name.Contains("Explo") && tempCollider.gameObject != gameObject)
                                {                            
                                    spawnExplosion(ExploPos[i]);
                                    tempCollider.GetComponent<BombExplosion>().Invoke("Explode",0.1f);
                                }
                                //check if it is Deployed Bomb - explode faster
                                else if (tempCollider.tag.Contains("Bomb") && tempCollider.gameObject != gameObject)
                                {
                                    spawnExplosion(ExploPos[i]);
                                    tempCollider.GetComponent<BombExplosion>().Invoke("Explode", 0.1f);
                                }                            
                                //if it is diffrent stone
                                else
                                {                                
                                    spawnExplosion(ExploPos[i]);
                                    Destroy(tempCollider.gameObject);
                                }
                            }
                            else if(Physics2D.OverlapCircle(ExploPos[i], .2f, Enemies))
                            {
                                    spawnExplosion(ExploPos[i]);
                                    Destroy(Physics2D.OverlapCircle(ExploPos[i], .2f, Enemies).gameObject);
                            }
                            //check if it is collectible (gems, bombs, keys)
                            else if (Physics2D.OverlapCircle(ExploPos[i], .2f, collectibles))
                            {                                                               
                                var obj = Physics2D.OverlapCircle(ExploPos[i], .2f, collectibles);
                                if (obj.tag.Contains("Gems"))
                                {
                                    GameObject.Find("GameManager").GetComponent<GameManager>().GemsLeft++;
                                    Debug.Log(GameObject.Find("GameManager").GetComponent<GameManager>().GemsLeft);
                                }
                                spawnExplosion(ExploPos[i]);
                                Destroy(obj.gameObject);
                            }
                        }

                    }
                    else
                    {
                        spawnExplosion(ExploPos[i]);
                    }
                } 
            }
        Destroy(gameObject);
        }
    }
    //start timer
    public void StartCountdown(float time)
    {
        StartCoroutine(ExplodeInTime(time));
    }
    //explode after time
    IEnumerator ExplodeInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }
}
