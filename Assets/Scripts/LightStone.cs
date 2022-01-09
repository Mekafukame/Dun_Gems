using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightStone : MonoBehaviour
{
    Tilemap DirtTileMap;
    public ParticleSystem dirtParticles;
    public ParticleSystem destroyParticles;    
       
    public Color particleColor;

    public Transform StoneMovePoint;
    public float movementSpeed = 5f;

    public bool isFalling = false;
    bool isMoving = false;
    bool isPushed = false;
    public bool isScaling = false;
    public bool isRotational = true;
    public bool isSecret = false;

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
            if(isRotational)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, StoneMovePoint.rotation, movementSpeed * 90 * Time.deltaTime);

        }
        else
            transform.position = Vector3.MoveTowards(transform.position, StoneMovePoint.position, movementSpeed * Time.deltaTime);
        if (isScaling)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), movementSpeed  * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, new Vector3(1, 1, 1)) < 0.05f)
            {
                if (Physics2D.OverlapCircle(transform.position, .4f, otherTilemaps))
                {
                    if(Physics2D.OverlapCircle(transform.position, .4f, otherTilemaps).tag == "Player")
                    {                        
                        Destroy(Physics2D.OverlapCircle(transform.position, .4f, otherTilemaps).gameObject);
                    }
                }
                else if (Physics2D.OverlapCircle(transform.position, .4f, enemies))
                {
                    if (Physics2D.OverlapCircle(transform.position, .4f, enemies))
                    {                        
                        Destroy(Physics2D.OverlapCircle(transform.position, .4f, enemies).gameObject);
                    }
                }
                    transform.localScale = new Vector3(1, 1, 1);
                GetComponent<CircleCollider2D>().enabled = true;
                isScaling = false;
            }
        }
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
        if (!isScaling)
        {            
            if (Vector3.Distance(transform.position, StoneMovePoint.position) == 0f && isMoving == false)
            {
                isMoving = true;
                SearchForRoad();
            }
        }

    }
    //chekc if there is possible move
    void SearchForRoad()
    {
        Tilemap CheckTilemap;
        string tileName = string.Empty;
        if (isSecret)
        {
            isMoving = false;
              
        }
        if(isMoving == true)
        {
            for (int i = 0, end = 0; i < CheckPoints.Length && end == 0; i++)
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
                    //check if there is any movepoint
                    else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, movePoints))
                    {
                        tagName = Physics2D.OverlapCircle(CheckPoints[i], .4f, movePoints).tag;
                    }
                    //check if there is any collectible
                    else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles))
                    {
                        tagName = Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles).tag;
                    }
                    else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, enemies))
                    {
                        tagName = Physics2D.OverlapCircle(CheckPoints[i], .4f, enemies).tag;
                    }
                    if (tagName != string.Empty)
                    {
                        //check if there is a dirt or secret
                        if (tagName.Contains("Dirt") || tagName.Contains("Secret"))
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
                        //check if it is a bomb
                        else if (tagName.Contains("Bomb"))
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
                                    if (isFalling)
                                        GetComponent<AudioSource>().Play();
                                    Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).GetComponent<BombExplosion>().Explode();
                                    end = 1;
                                }
                            }
                        }
                        //check if there is gem
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
                            else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles))
                            {
                                if (Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles).tag.Contains("Gems"))
                                {
                                    if (Physics2D.OverlapCircle(CheckPoints[i], .4f, collectibles).GetComponent<LightStone>().isFalling == true)
                                    {
                                        end = 1;
                                    }
                                }
                            }
                            continue;
                        }
                        //check if there is stone
                        else if (tagName.Contains("Stone"))
                        {
                            if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps))
                            {
                                if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).name.Contains("Explo") && isFalling)
                                {
                                    Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).GetComponent<BombExplosion>().Explode();
                                }
                                else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).name.Contains("Light"))
                                {

                                    if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).GetComponent<LightStone>().isFalling == true)
                                    {
                                        end = 1;
                                    }

                                }
                                else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).name.Contains("Heavy"))
                                {
                                    if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).GetComponent<HeavyStone>().isFalling)
                                    {
                                        end = 1;
                                    }
                                }
                                else if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).name.Contains("Solid"))
                                {
                                    if (Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).GetComponent<SolidStone>().isFalling)
                                    {
                                        end = 1;
                                    }
                                }
                            }
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
                        //check for Enemie
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
                        //check for Box
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
                                    if (isFalling)
                                        GetComponent<AudioSource>().Play();
                                    isFalling = false;
                                    Destroy(Physics2D.OverlapCircle(CheckPoints[i], .4f, otherTilemaps).gameObject);
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
                            catch {}
                        }
                        //check if there is explosion particles
                        else if (tagName == "Explosion")
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
                    else
                    {
                        if (i == 0)
                        {
                            isFalling = true;
                            MovePoint(i);
                            end = 1;
                        }
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
                else if (i == 0)
                {
                    if(isFalling)
                        GetComponent<AudioSource>().Play();
                    isFalling = false;
                }
            }
            isMoving = false;
        }        
    }

    //change position of move point
    void MovePoint(int index)
    {
        if (gameObject.name != "Diamond")
        {
            if (index == 2)
                StoneMovePoint.Rotate(new Vector3(0, 0, 90));
            else if (index == 4)
                StoneMovePoint.Rotate(new Vector3(0, 0, -90));
        }
        
        StoneMovePoint.position = CheckPoints[index];
        UpdatePoints();
        
    }
    void MovePoint(Vector3 position)
    {
        
        StoneMovePoint.position = position;        
        UpdatePoints();

    }
    //try to move left
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
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, movePoints))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, movePoints).tag;
                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, collectibles))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, collectibles).tag;

                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, enemies))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, enemies).tag;
                }
                if (tagName != string.Empty)
                {
                    //check if there is something interactional
                    if (tagName.Contains("Interactions"))
                    {
                        CheckTilemap = GameObject.Find("Tilemap Interaction Blocks").GetComponent<Tilemap>();
                        tileName = CheckTilemap.GetTile(CheckTilemap.WorldToCell(Vector3Int.FloorToInt(StoneMovePoint.position + new Vector3(-1f, 0f, 0f)))).name;
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
    //try to move right
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
                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, movePoints))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, movePoints).tag;                    
                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, collectibles))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, collectibles).tag;
                }
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, enemies))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, enemies).tag;
                }
                if (tagName != string.Empty)
                {
                    //check if there is something interactional
                    if (tagName.Contains("Interactions"))
                    {
                        CheckTilemap = GameObject.Find("Tilemap Interaction Blocks").GetComponent<Tilemap>();
                        tileName = CheckTilemap.GetTile(CheckTilemap.WorldToCell(Vector3Int.FloorToInt(StoneMovePoint.position + new Vector3(1f, 0f, 0f)))).name;
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
        //check if there is something interactional
        if (collision.tag == "Interactions")
        {
            string Tilename = collision.gameObject.GetComponent<Tilemap>().GetTile(collision.gameObject.GetComponent<Tilemap>().WorldToCell(Vector3Int.FloorToInt(StoneMovePoint.position))).name;                       
            if (Tilename.Contains("Lava"))
            {                
                var tempObj = Instantiate(destroyParticles, StoneMovePoint.position + new Vector3(0f,0.5f,0f), Quaternion.identity);
                var main = tempObj.main;
                main.startColor = particleColor;                
                Destroy(tempObj.gameObject, 0.5f);
                if (gameObject.tag.Contains("Gems"))
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().GemsLeft++;                    
                }
                Destroy(gameObject);
            }
            else if (Tilename.Contains("Damaged Dirt"))
            {
                collision.GetComponent<Tilemap>().SetTile(DirtTileMap.WorldToCell(StoneMovePoint.position), null);
                var tmpObj = Instantiate(dirtParticles, StoneMovePoint.position, Quaternion.identity);
                tmpObj.Play();
                Destroy(tmpObj.gameObject, 1f);
            }
        }        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if it is a player
        if (collision.gameObject.tag == "Player" && isFalling == true)
        {            
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemie" && isFalling == true)
        {                      
            Destroy(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        try { 
            if (StoneMovePoint)
                Destroy(StoneMovePoint.gameObject);
        }
        catch { }
    }
}
