using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public Transform PlayerMovePoint;    
    public float movementSpeed = 5f;

    public GameObject Annoucement;
    
    public Animator animator;
    
    public Camera playerCam;
    public LayerMask StopMovement;
    public LayerMask otherTilemaps;
    public LayerMask movePoints;    


    void Start()
    {        
        PlayerMovePoint.parent = null;
        playerCam.transform.parent = null;                    
    }

    // Update is called once per frame
    void Update()
    {
        playerCam.transform.position = Vector3.MoveTowards(transform.position + new Vector3(-1.5f, 0, 0), PlayerMovePoint.position + new Vector3(-1.5f, 0, 0), movementSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, PlayerMovePoint.position, movementSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, PlayerMovePoint.position) == 0f && Time.timeScale == 1)
        {
            //Check for horizontal input
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                
                //Check if there is Obstacle
                if (!Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, StopMovement))
                {                    
                    //Check if there is any move point (prevent movement collision)
                    if (!Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, movePoints))
                    {                      
                        //check if there is other tilemap obstacle
                        if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps))
                        {
                            var tempObj = Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps);                            
                            //check if its a stone
                            if (tempObj.tag == "Dirt")
                            {
                                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                animator.SetFloat("Vertical", 0f);
                                PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                            else if (tempObj.tag == "Stone" || tempObj.tag == "Box")
                            {
                                
                                if (Input.GetAxisRaw("Horizontal") > 0.01f)
                                    pushRight();
                                else if (Input.GetAxisRaw("Horizontal") < 0.01f)
                                    pushLeft();

                            }
                            //check if its a door
                            else if(tempObj.tag == "Door")
                            {
                                var Door = tempObj.GetComponent<DoorController>();
                                if (!Door.isOpen )
                                {
                                    var inventory = GetComponent<PlayerInv>();
                                    if(inventory.Keys[Door.DoorID] > 0)
                                    {
                                        Door.OpenDoor();
                                        inventory.Keys[Door.DoorID]--;
                                        inventory.Player_GUI.updateGUI();
                                    }
                                    else
                                    {
                                        if (!GameObject.Find("Annoucement(Clone)"))
                                        {
                                            var annouce = Instantiate(Annoucement, new Vector3(0, 0, 0), Quaternion.identity);
                                            annouce.GetComponent<Annoucment>().Text.text = "You don't have the key!";
                                        }
                                    }
                                }                                                            
                                else
                                {
                                    
                                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                    animator.SetFloat("Vertical", 0f);
                                    PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                                }
                            }
                            //check if its Exit
                            else if (tempObj.tag == "Exit")
                            {
                                var Door = tempObj.GetComponent<ExitDoorController>();
                                if (Door.isOpen)
                                {
                                   
                                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                    animator.SetFloat("Vertical", 0f);
                                    PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                                }
                                else
                                {
                                    if (!GameObject.Find("Annoucement(Clone)"))
                                    {
                                        var annouce = Instantiate(Annoucement, new Vector3(0, 0, 0), Quaternion.identity);
                                        annouce.GetComponent<Annoucment>().Text.text = "You need more gems!";
                                    }
                                }
                            }
                            //check if its teleport
                            else if (tempObj.tag == "Teleport") 
                            {
                                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                animator.SetFloat("Vertical", 0f);
                                PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }                            
                            else if(tempObj.tag == "Interactions")
                            {
                                string tileName = tempObj.GetComponent<Tilemap>().GetTile(tempObj.GetComponent<Tilemap>().WorldToCell(Vector3Int.FloorToInt(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f)))).name;
                                if (tileName.Contains("Dirt")) { 
                                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                    animator.SetFloat("Vertical", 0f);
                                    PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                                }
                                else if (tileName.Contains("Lava"))
                                {
                                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                    animator.SetFloat("Vertical", 0f);
                                    PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                                }
                            }
                            else if(tempObj.tag == "Secret"){
                                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                                animator.SetFloat("Vertical", 0f);
                                PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            
                            animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                            animator.SetFloat("Vertical", 0f);
                            PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        }
                    }
                    //check if its a gem
                    else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, movePoints).tag == "Gems")
                    {
                        animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                        animator.SetFloat("Vertical", 0f);
                        PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
                    else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, movePoints).tag == "Enemie")
                    {
                        animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                        animator.SetFloat("Vertical", 0f);
                        PlayerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
                    //try to push stone
                    else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, movePoints).tag == "Stone" || Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, movePoints).tag == "Box")
                    {
                        try
                        {
                            if (Input.GetAxisRaw("Horizontal") > 0.01f)
                                pushRight();
                            else if (Input.GetAxisRaw("Horizontal") < 0.01f)
                                pushLeft();
                        }
                        catch { }
                    }
                }
            }
            //Check for vertical input
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                //check for obstacle
                if (!Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, StopMovement))
                {
                    //check for move points
                    if (!Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, movePoints))
                    {
                        //check for other obstacles
                        if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, otherTilemaps))
                        {
                            var tempObj = Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, otherTilemaps);
                            //check if it is stone
                            if (tempObj.tag == "Dirt") 
                            {                            
                                animator.SetFloat("Horizontal", 0f);
                                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);                                
                            }
                            //check if it is door
                            else if (tempObj.tag == "Door")
                            {
                                var Door = tempObj.GetComponent<DoorController>();
                                if (!Door.isOpen )
                                {
                                    var inventory = GetComponent<PlayerInv>();
                                    if (inventory.Keys[Door.DoorID] > 0)
                                    {
                                        Door.OpenDoor();
                                        inventory.Keys[Door.DoorID]--;
                                        inventory.Player_GUI.updateGUI();
                                    }
                                    else
                                    {
                                        if (!GameObject.Find("Annoucement(Clone)"))
                                        {
                                            var annouce = Instantiate(Annoucement, new Vector3(0, 0, 0), Quaternion.identity);
                                            annouce.GetComponent<Annoucment>().Text.text = "You don't have the key!";
                                        }
                                    }
                                }                               
                                else
                                {
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                    PlayerMovePoint.position += new Vector3( 0f, Input.GetAxisRaw("Vertical"), 0f);
                                }
                            }
                            //check if it is Exit
                            else if (tempObj.tag == "Exit")
                            {
                               
                                var Door = tempObj.GetComponent<ExitDoorController>();
                                if (Door.isOpen)
                                {
                                    
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                    PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                                }
                                else
                                {
                                    if (!GameObject.Find("Annoucement(Clone)"))
                                    {
                                        var annouce = Instantiate(Annoucement, new Vector3(0, 0, 0), Quaternion.identity);
                                        annouce.GetComponent<Annoucment>().Text.text = "You need more gems!";
                                    }
                                }

                            }
                            //check its tilemap
                            else if (tempObj.tag == "Teleport")
                            {
                                animator.SetFloat("Horizontal", 0f);
                                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                            else if (tempObj.tag == "Interactions")
                            {
                                string tileName = tempObj.GetComponent<Tilemap>().GetTile(tempObj.GetComponent<Tilemap>().WorldToCell(Vector3Int.FloorToInt(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f)))).name;
                                if (tileName.Contains("Dirt"))
                                {
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                    PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                                }
                                else if (tileName.Contains("Lava") && Input.GetAxisRaw("Vertical") < 0)
                                {
                                    animator.SetFloat("Horizontal", 0f);
                                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                    PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                                }
                            }
                            else if (tempObj.tag == "Secret")
                            {
                                animator.SetFloat("Horizontal", 0f);
                                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                                PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {

                            animator.SetFloat("Horizontal", 0f);
                            animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                            PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        }
                    }
                    else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, movePoints).tag == "Gems")
                    {
                        animator.SetFloat("Horizontal", 0f);
                        animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                        PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    }
                    else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, movePoints).tag == "Enemie")
                    {
                        animator.SetFloat("Horizontal", 0f);
                        animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                        PlayerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    }
                }
            }            
        }
    }
    //push stone
    void pushLeft()
    {
        if(Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Light"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<LightStone>().PushLeft();
        }
        else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Explosive"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<ExplosiveStone>().PushLeft();
        }
        else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Heavy"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<HeavyStone>().PushLeft();
        }
        else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Box"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<HeavyStone>().PushLeft();
        }
    }
    //push stone
    void pushRight()
    {
        if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Light"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<LightStone>().PushRight();
        }
        else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Explosive"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<ExplosiveStone>().PushRight();
        }
        else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Heavy"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<HeavyStone>().PushRight();
        }
        else if (Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).name.Contains("Box"))
        {
            Physics2D.OverlapCircle(PlayerMovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, otherTilemaps).GetComponent<HeavyStone>().PushRight();
        }
    }
    

    
}
