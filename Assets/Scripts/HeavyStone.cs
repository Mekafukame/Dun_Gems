using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeavyStone : MonoBehaviour
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
    public bool isBox = false;
    public bool isScaling = false;
    public bool isRotational = true;
    public bool isSecret = false;

    public LayerMask stopMovement;
    public LayerMask otherTilemaps;
    public LayerMask movePoints;
    public LayerMask collectibles;
    public LayerMask enemies;
    Vector3 CheckPoints;

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
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), movementSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, new Vector3(1, 1, 1)) < 0.05f)
            {
                if (Physics2D.OverlapCircle(transform.position, .4f, otherTilemaps))
                {
                    if (Physics2D.OverlapCircle(transform.position, .4f, otherTilemaps).tag == "Player")
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
        CheckPoints = new Vector3(StoneMovePoint.position.x, StoneMovePoint.position.y - 1, 0f);
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
        if (isMoving == true)
        {

            //check if there is solid block layer
            if (!Physics2D.OverlapCircle(CheckPoints, .4f, stopMovement))
            {
                string tagName = string.Empty;
                //check if there is any tile in other layers
                if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps))
                {
                    tagName = Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).tag;

                }
                //check if there is any movepoint
                else if (Physics2D.OverlapCircle(CheckPoints, .4f, movePoints))
                {
                    tagName = Physics2D.OverlapCircle(CheckPoints, .4f, movePoints).tag;
                }
                //check if there is any collectible
                else if (Physics2D.OverlapCircle(CheckPoints, .4f, collectibles))
                {
                    tagName = Physics2D.OverlapCircle(CheckPoints, .4f, collectibles).tag;
                }
                else if (Physics2D.OverlapCircle(CheckPoints, .4f, enemies))
                {
                    tagName = Physics2D.OverlapCircle(CheckPoints, .4f, enemies).tag;
                }
                if (tagName != string.Empty)
                {
                    //check if there is a dirt or secret
                    if (tagName.Contains("Dirt") || tagName.Contains("Secret"))
                    {
                        if (isFalling)
                            GetComponent<AudioSource>().Play();
                        isFalling = false;
                    }
                    //check if it is a bomb
                    else if (tagName.Contains("Bomb"))
                    {
                        if (isFalling)
                        {
                            if (isFalling)
                                GetComponent<AudioSource>().Play();
                            Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).GetComponent<BombExplosion>().Explode();
                        }
                    }
                    //check if there is gem
                    else if (tagName.Contains("Gems"))
                    {                        
                        if (isFalling)
                            GetComponent<AudioSource>().Play();
                        isFalling = false;
                    }
                    //check if there is stone
                    else if (tagName.Contains("Stone"))
                    {

                        if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps))
                        {
                            if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).name.Contains("Explo") && isFalling)
                            {
                                Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).GetComponent<BombExplosion>().Explode();
                            }
                            else if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).name.Contains("Light"))
                            {
                                if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).GetComponent<LightStone>().isFalling) { }
                            }
                            else if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).name.Contains("Heavy"))
                            {
                                if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).GetComponent<HeavyStone>().isFalling) { }
                            }
                            else if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).name.Contains("Solid"))
                            {
                                if (Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).GetComponent<SolidStone>().isFalling) { }
                            }
                        }
                        if (isFalling)
                            GetComponent<AudioSource>().Play();
                        isFalling = false;
                    }
                    //check if there is player
                    else if (tagName.Contains("Player"))
                    {
                        if (isFalling)
                        {
                            MovePoint(CheckPoints);
                        }

                    }
                    //check for Enemie
                    else if (tagName.Contains("Enemie"))
                    {
                        if (isFalling)
                        {
                            MovePoint(CheckPoints);
                        }
                    }
                    //check for Box
                    else if (tagName.Contains("Box"))
                    {
                        if (isFalling)
                        {
                            isFalling = false;
                            if (isFalling)
                                GetComponent<AudioSource>().Play();
                            Destroy(Physics2D.OverlapCircle(CheckPoints, .4f, otherTilemaps).gameObject);
                            if (isBox)
                            {
                                Destroy(gameObject);
                            }
                        }
                    }
                    //if there is no tile 
                    //check if there is something interactional
                    else if (tagName.Contains("Interactions"))
                    {
                        CheckTilemap = GameObject.Find("Tilemap Interaction Blocks").GetComponent<Tilemap>();
                        tileName = CheckTilemap.GetTile(CheckTilemap.WorldToCell(Vector3Int.FloorToInt(CheckPoints))).name;
                        if (tileName.Contains("Lava"))
                        {
                            isFalling = true;
                            MovePoint(CheckPoints);
                        }
                        //check if there is damaged dirt
                        if (tileName.Contains("Damaged Dirt"))
                        {
                            isFalling = true;
                            MovePoint(CheckPoints);
                        }
                    }
                    //check if there is explosion particles
                    else if (tagName == "Explosion")
                    {
                        isFalling = true;
                        MovePoint(CheckPoints);
                    }
                    else if (tagName == "StoneSpawner")
                    {
                        isFalling = true;
                        MovePoint(CheckPoints);
                    }
                    else if (tagName == "Teleport")
                    {
                        isFalling = true;
                        MovePoint(CheckPoints);
                    }
                }
                else
                {
                    isFalling = true;
                    MovePoint(CheckPoints);
                }
            }
            else
            {
                if (isFalling)
                    GetComponent<AudioSource>().Play();
                isFalling = false;
            }            
        }
        isMoving = false;
    }

    //change position of move point   
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
                //check if there is any tile in movepoints layers
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, movePoints))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(1f, 0f, 0f), .4f, movePoints).tag;
                    Debug.Log(tagName);
                }
                //check if there is any tile in collectibles layers
                else if (Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, collectibles))
                {
                    tagName = Physics2D.OverlapCircle(StoneMovePoint.position + new Vector3(-1f, 0f, 0f), .4f, collectibles).tag;

                }
                //check if there is any tile in enemies layers
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
                    Debug.Log(tagName);
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
                if (destroyParticles)
                {
                    var tempObj = Instantiate(destroyParticles, StoneMovePoint.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
                    var main = tempObj.main;
                    main.startColor = particleColor;
                    Destroy(tempObj.gameObject, 0.5f);
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
        try
        {
            if (StoneMovePoint)
                Destroy(StoneMovePoint.gameObject);
        }
        catch { }
    }
}
