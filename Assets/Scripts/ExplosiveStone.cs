using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplosiveStone : MonoBehaviour
{
    Tilemap DirtTileMap;


    public Transform StoneMovePoint;
    public float movementSpeed = 1f;
    public BombExplosion scrExplosion;

    bool isMoving = false;
    bool isPushed = false;
    public bool isFalling = false;


    public LayerMask stopMovement;
    public LayerMask otherTilemaps;
    public LayerMask movePoints;
    public LayerMask collectibles;
    public LayerMask enemies;
    Vector3[] CheckPoints = new Vector3[5];

    // Start is called before the first frame update
    void Start()
    {
        DirtTileMap = GameObject.Find("Tilemap Dirt").GetComponent<Tilemap>();
        StoneMovePoint.parent = null;
        UpdatePoints();

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0f, 0f), new Vector3(StoneMovePoint.position.x, 0f, 0f)) != 0f)
        {
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, 0f), new Vector3(StoneMovePoint.position.x, transform.position.y, 0f), movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, StoneMovePoint.rotation, movementSpeed * 90 * Time.deltaTime);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, StoneMovePoint.position, movementSpeed * Time.deltaTime);
    }
    //update possible move points
    void UpdatePoints()
    {
        CheckPoints[0] = new Vector3(StoneMovePoint.position.x, StoneMovePoint.position.y - 1, 0f);
        CheckPoints[1] = new Vector3(StoneMovePoint.position.x - 1, StoneMovePoint.position.y, 0f);
        CheckPoints[2] = new Vector3(StoneMovePoint.position.x - 1, StoneMovePoint.position.y - 1, 0f);
        CheckPoints[3] = new Vector3(StoneMovePoint.position.x + 1, StoneMovePoint.position.y, 0f);
        CheckPoints[4] = new Vector3(StoneMovePoint.position.x + 1, StoneMovePoint.position.y - 1, 0f);

    }

    public void CheckMove()
    {
        
        if (Vector3.Distance(transform.position, StoneMovePoint.position) == 0f && isMoving == false && isPushed == false)
        {           
            isMoving = true;
            SearchForRoad();
        }
    }
    //chech for possible move
    void SearchForRoad()
    {
        Tilemap CheckTilemap;
        string tileName = string.Empty;
        

        for (int i = 0, end = 0; i < CheckPoints.Length && end == 0; i++)
        {
            
            if (!Physics2D.OverlapCircle(CheckPoints[i], .4f, LayerMask.GetMask("Particles")))
            {
               
                //check if there is solid block layer
                if (!Physics2D.OverlapCircle(CheckPoints[i], .4f, stopMovement))
                {
                    
                    string tagName = string.Empty;
                    //check if there is any tile in other layers
                    if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps))
                    {
                        tagName = Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).tag;
                        
                    }
                    //if there is any move point
                    else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, movePoints))
                    {
                        tagName = Physics2D.OverlapCircle(CheckPoints[i], .4f, movePoints).tag;
                       
                    }
                    //if there is collectible
                    else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles))
                    {
                        tagName = Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles).tag;
                        
                    }
                    if (tagName != string.Empty)
                    {
                        //check if there is a dirt or secret
                        if (tagName.Contains("Dirt") || tagName.Contains("Secret"))
                        {
                            if (isFalling)
                            {
                                scrExplosion.Explode();

                                end = 1;
                            }
                            if (i == 1 || i == 3)
                            {
                                i++;
                            }
                            continue;
                        }
                        else if (tagName.Contains("Gems") || tagName.Contains("Bomb"))
                        {
                            if (isFalling)
                            {
                                scrExplosion.Explode();

                                end = 1;
                            }
                            if (i == 1 || i == 3)
                            {
                                i++;
                            }
                            continue;
                        }
                        //check if there is other stone
                        else if (tagName.Contains("Stone"))
                        {

                            if (isFalling)
                            {
                                scrExplosion.Explode();

                                end = 1;
                            }
                            if (i == 1 || i == 3)
                            {
                                i++;
                            }
                            continue;
                        }
                        //check if there is player
                        else if (tagName.Contains("Player"))
                        {
                            if (!isFalling)
                            {
                                if (i == 1 || i == 3)
                                {
                                    i++;
                                }
                                else if (i == 0)
                                {
                                    end = 1;
                                }
                                continue;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    MovePoint(i);
                                    end = 1;
                                }
                            }
                        }
                        else if (tagName.Contains("Enemie"))
                        {
                            if (!isFalling)
                            {
                                if (i == 1 || i == 3)
                                {
                                    i++;
                                }
                                else if (i == 0)
                                {
                                    end = 1;
                                }
                                continue;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    MovePoint(i);
                                    end = 1;
                                }
                            }
                        }
                        else if (tagName.Contains("Box"))
                        {
                            if (!isFalling)
                            {
                                if (i == 1 || i == 3)
                                {
                                    i++;
                                }
                                else if (i == 0)
                                {
                                    end = 1;
                                }
                                continue;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    isFalling = false;
                                    scrExplosion.Explode();
                                }
                            }
                        }
                        //if there is no tile 
                        //check if there is something interactional
                        else if (tagName.Contains("Interactions"))
                        {
                            CheckTilemap = GameObject.Find("Tilemap Interaction Blocks").GetComponent<Tilemap>();
                            try
                            {
                                tileName = CheckTilemap.GetTile(CheckTilemap.WorldToCell(Vector3Int.FloorToInt(CheckPoints[i]))).name;
                                if (tileName.Contains("Lava"))
                                {
                                    if (i == 0)
                                    {
                                        isFalling = true;
                                        MovePoint(i);
                                        end = 1;
                                    }
                                    else if (i == 2 || i == 4)
                                    {
                                        isFalling = true;
                                        MovePoint(i);
                                        end = 1;
                                    }
                                }
                                //check if there is damaged dirt
                                if (tileName.Contains("Damaged Dirt"))
                                {
                                    if (i == 0)
                                    {
                                        isFalling = true;
                                        MovePoint(i);
                                        end = 1;
                                    }
                                    else if (i == 1 || i == 3)
                                    {
                                        i++;
                                    }
                                }
                            }
                            catch { }
                        }
                        else if (tagName == "StoneSpawner")
                        {
                            if (i == 0)
                            {
                                isFalling = true;
                                MovePoint(i);
                                end = 1;
                            }
                            else if (i == 2 || i == 4)
                            {
                                isFalling = true;
                                MovePoint(i);
                                end = 1;
                            }
                        }
                        else if (tagName == "Teleport")
                        {
                            if (i == 0)
                            {
                                isFalling = true;
                                MovePoint(i);
                                end = 1;
                            }
                            else if (i == 2 || i == 4)
                            {
                                isFalling = true;
                                MovePoint(i);
                                end = 1;
                            }
                        }

                    }
                    //if there is nothing
                    else
                    {
                        if (i == 0)
                        {
                            
                            isFalling = true;
                            MovePoint(i);
                            end = 1;
                        }                        
                        //check if there is gem on right side of hole
                        else if (i == 2)
                        {
                            if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 1f, 0f), .4f, otherTilemaps))
                            {
                                tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 1f, 0f), .4f, otherTilemaps).tag;
                            }
                            //check if there is gem on the other side of hole
                            else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 1f, 0f), .4f, collectibles))
                            {
                                tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 1f, 0f), .4f, collectibles).tag;
                            }
                            if (tagName != string.Empty)
                            {
                                if (tagName.Contains("Gems") || tagName.Contains("Stone") || tagName.Contains("Box"))
                                {
                                    if (i == 1 || i == 3)
                                    {
                                        i++;
                                    }
                                    else if (i == 0)
                                    {
                                        if (isFalling)
                                            GetComponent<AudioSource>().Play();
                                        isFalling = false;
                                    }
                                    continue;
                                }
                                else
                                {
                                    isFalling = true;
                                    MovePoint(i);
                                    end = 1;
                                }
                            }
                            else
                            {
                                isFalling = true;
                                MovePoint(i);
                                end = 1;
                            }
                        }
                        else if (i == 4)
                        {
                            //check if there is stone above hole
                            if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 1f, 0f), .4f, otherTilemaps))
                            {
                                tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 1f, 0f), .4f, otherTilemaps).tag;
                            }
                            //check if there is gem on the other side of hole
                            else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 1f, 0f), .4f, collectibles))
                            {
                                tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 1f, 0f), .4f, collectibles).tag;
                            }
                            if (tagName != string.Empty)
                            {
                                if (tagName.Contains("Gems") || tagName.Contains("Stone") || tagName.Contains("Box"))
                                {
                                    if (i == 1 || i == 3)
                                    {
                                        i++;
                                    }
                                    else if (i == 0)
                                    {
                                        if (isFalling)
                                            GetComponent<AudioSource>().Play();
                                        isFalling = false;
                                    }
                                    continue;
                                }
                                else
                                {
                                    if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, otherTilemaps))
                                    {
                                        tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, otherTilemaps).tag;
                                    }
                                    //check if there is gem on the other side of hole
                                    else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, collectibles))
                                    {
                                        tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, collectibles).tag;
                                    }
                                    if (tagName != string.Empty)
                                    {
                                        if (tagName.Contains("Stone") || tagName.Contains("Box"))
                                        {
                                            var tempObj = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, otherTilemaps);
                                            if (tempObj.name.Contains("Explosive") || tempObj.name.Contains("Light"))
                                            {
                                                if (i == 1 || i == 3)
                                                {
                                                    i++;
                                                }
                                                else if (i == 0)
                                                {
                                                    if (isFalling)
                                                        GetComponent<AudioSource>().Play();
                                                    isFalling = false;
                                                }
                                                continue;

                                            }
                                        }
                                        else if (tagName.Contains("Gems"))
                                        {
                                            if (i == 1 || i == 3)
                                            {
                                                i++;
                                            }
                                            else if (i == 0)
                                            {
                                                if (isFalling)
                                                    GetComponent<AudioSource>().Play();
                                                isFalling = false;
                                            }
                                            continue;
                                        }
                                        else
                                        {
                                            isFalling = true;
                                            MovePoint(i);
                                            end = 1;
                                        }
                                    }
                                    else
                                    {
                                        isFalling = true;
                                        MovePoint(i);
                                        end = 1;
                                    }
                                }
                            }
                            else
                            {


                                if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, otherTilemaps))
                                {
                                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, otherTilemaps).tag;
                                }
                                //check if there is gem on the other side of hole
                                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, collectibles))
                                {
                                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, collectibles).tag;
                                }
                                if (tagName != string.Empty)
                                {
                                    if (tagName.Contains("Stone") || tagName.Contains("Box"))
                                    {
                                        var tempObj = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(2f, 0f, 0f), .4f, otherTilemaps);
                                        if (tempObj.name.Contains("Explosive") || tempObj.name.Contains("Light"))
                                        {
                                            if (i == 1 || i == 3)
                                            {
                                                i++;
                                            }
                                            else if (i == 0)
                                            {
                                                if (isFalling)
                                                    GetComponent<AudioSource>().Play();
                                                isFalling = false;
                                            }
                                            continue;

                                        }
                                    }
                                    else if (tagName.Contains("Gems"))
                                    {
                                        if (i == 1 || i == 3)
                                        {
                                            i++;
                                        }
                                        else if (i == 0)
                                        {
                                            if (isFalling)
                                                GetComponent<AudioSource>().Play();
                                            isFalling = false;
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        isFalling = true;
                                        MovePoint(i);
                                        end = 1;
                                    }
                                }
                                else
                                {
                                    isFalling = true;
                                    MovePoint(i);
                                    end = 1;
                                }
                            }
                        }
                    }

                }
                else if (i == 1 || i == 3)
                {
                    i++;
                }
                else if (i == 0 && isFalling)
                {
                    scrExplosion.Explode();
                }
            }
           
            
        }
        isMoving = false;
    } 
    //move point to next position
    void MovePoint(int index)
    {
                
        if (index == 2)
            StoneMovePoint.Rotate(new Vector3(0, 0, 90));
        else if (index == 4)
            StoneMovePoint.Rotate(new Vector3(0, 0, -90));

        StoneMovePoint.position = CheckPoints[index];
        scrExplosion.updateExploPos();
        UpdatePoints();             
              
    }
    //move point to next position
    void MovePoint(Vector3 position)
    {                
        StoneMovePoint.position = position;
        scrExplosion.updateExploPos();
        UpdatePoints();

    }
    //Try to move stone left
    public void PushLeft()
    {
        if (Vector3.Distance(transform.position, StoneMovePoint.position) == 0f && isPushed == false)
        {

            isPushed = true;
            Tilemap CheckTilemap;
            string tileName = string.Empty;
            //check if there is solid block layer
            if (!Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, stopMovement))
            {
                string tagName = string.Empty;
                //check if there is any tile in other layers
                if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, otherTilemaps))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, otherTilemaps).tag;

                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, collectibles))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, collectibles).tag;

                }
                if (tagName != string.Empty)
                {
                    //check if there is something interactional
                    if (tagName.Contains("Interactions"))
                    {
                        CheckTilemap = GameObject.Find("Tilemap Interaction Blocks").GetComponent<Tilemap>();
                        tileName = CheckTilemap.GetTile(Vector3Int.FloorToInt(StoneMovePoint.position + new Vector3(-1f, 0f, 0f))).name;
                        if (tileName.Contains("Lava"))
                        {
                            StoneMovePoint.Rotate(new Vector3(0, 0, 90));
                            MovePoint(StoneMovePoint.position + new Vector3(-1f, 0f, 0f));
                        }
                    }
                }
                else
                {
                    StoneMovePoint.Rotate(new Vector3(0, 0, 90));
                    MovePoint(StoneMovePoint.position + new Vector3(-1f, 0f, 0f));


                }
            }
            isPushed = false;
        }
    }
    //try to move stone right
    public void PushRight()
    {
        if (Vector3.Distance(transform.position, StoneMovePoint.position) == 0f && isPushed == false)
        {
            isPushed = true;
            Tilemap CheckTilemap;
            string tileName = string.Empty;
            //check if there is solid block layer
            if (!Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, stopMovement))
            {
                string tagName = string.Empty;
                //check if there is any tile in other layers
                if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, otherTilemaps))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, otherTilemaps).tag;
                    Debug.Log(tagName);
                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, collectibles))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, collectibles).tag;

                }
                if (tagName != string.Empty)
                {
                    //check if there is something interactional
                    if (tagName.Contains("Interactions"))
                    {
                        CheckTilemap = GameObject.Find("Tilemap Interaction Blocks").GetComponent<Tilemap>();
                        tileName = CheckTilemap.GetTile(Vector3Int.FloorToInt(StoneMovePoint.position + new Vector3(1f, 0f, 0f))).name;
                        if (tileName.Contains("Lava"))
                        {
                            StoneMovePoint.Rotate(new Vector3(0, 0, -90));
                            MovePoint(StoneMovePoint.position + new Vector3(1f, 0f, 0f));
                        }
                    }
                }
                else
                {
                    StoneMovePoint.Rotate(new Vector3(0, 0, -90));
                    MovePoint(StoneMovePoint.position + new Vector3(1f, 0f, 0f));


                }
            }
            isPushed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        //check if it is a interactional 
        if (collision.tag == "Interactions")
        {
            string Tilename = collision.gameObject.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(StoneMovePoint.position)).name;           
            if (Tilename.Contains("Lava"))
            {
                scrExplosion.Explode();
            }
            else if (Tilename.Contains("Damaged Dirt"))
            {
                collision.GetComponent<Tilemap>().SetTile(DirtTileMap.WorldToCell(StoneMovePoint.position), null);                
                
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        //check if it is a player 
        if (collision.gameObject.tag == "Player" && isFalling == true)
        {            
            Destroy(collision.gameObject);
            scrExplosion.Explode();
        }
        else if (collision.gameObject.tag == "Enemie" && isFalling == true)
        {            
            Destroy(collision.gameObject);
            scrExplosion.Explode();
        }
    }

    private void OnDestroy()
    {        
        try
        {
            if (StoneMovePoint)
                Destroy(StoneMovePoint.gameObject);
        }
        catch { }
    }
}
