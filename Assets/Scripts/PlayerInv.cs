using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInv : MonoBehaviour
{
    public GameObject BombObj;
    public GameObject particles;
    public Transform PlayerMovePoint;
    GameObject LevelManager;
   
    public float BombTimer = 3f;
    float nextTry = 0f;

    public GUI_Manager Player_GUI;

    public int numOfPoints = 0;
    public int numOfBombs = 0;
    public int[] Keys = new int[6];
    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.Find("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        //check for fire input
        if (Input.GetAxisRaw("Fire1") == 1f && numOfBombs > 0 && nextTry < Time.time && Vector3.Distance(PlayerMovePoint.position, transform.position) <= .5f && Time.timeScale == 1)
        {           
            nextTry = Time.time + 0.2f; //small cooldown to prevent multiple bombs
            //check if there is bomb already
            if (Physics2D.OverlapCircle(PlayerMovePoint.position, .4f).tag != "Bomb")
            {
                numOfBombs--;
                Player_GUI.updateGUI();
                placeBomb();
            }
        }        
    }

    void placeBomb()
    {
        var tempObj = Instantiate(BombObj, PlayerMovePoint.position, Quaternion.identity);
        tempObj.GetComponent<BombExplosion>().StartCountdown(BombTimer);
    }

    private void OnCollisionEnter2D(Collision2D collision)    
    {
        //check if it is a gem
        if(collision.gameObject.tag == "Gems" && !collision.gameObject.GetComponent<LightStone>().isFalling)
        {
            //check for gem variant
            switch (collision.gameObject.name)
            {
                case "Diamond":
                    numOfPoints += 25;
                    break;
                case "Ruby":
                    numOfPoints += 10;
                    break;
                case "Emerald":
                    numOfPoints += 5;
                    break;
                case "Sapphire":
                    numOfPoints += 2;
                    break;                
                case "Amethyst":
                    numOfPoints++;
                    break;
                case "Diamond(Clone)":
                    numOfPoints += 25;
                    break;
                case "Ruby(Clone)":
                    numOfPoints += 10;
                    break;
                case "Emerald(Clone)":
                    numOfPoints += 5;
                    break;
                case "Sapphire(Clone)":
                    numOfPoints += 2;
                    break;
                case "Amethyst(Clone)":
                    numOfPoints++;
                    break;
            }
            Player_GUI.updateGUI();
            var tempObj = Instantiate(particles, PlayerMovePoint.position, Quaternion.identity);
            LevelManager.GetComponent<LevelSettings>().Score = numOfPoints * 100;
            Destroy(tempObj, 0.5f);
            Destroy(collision.gameObject);
        }
        //check if it is a bomb
        else if (collision.gameObject.tag == "Bomb"){
            numOfBombs++;
            Player_GUI.updateGUI();
            var tempObj = Instantiate(particles, PlayerMovePoint.position, Quaternion.identity);
            Destroy(tempObj, 0.5f);
            Destroy(collision.gameObject);
        }
        //check if it is a key
        else if(collision.gameObject.tag == "Key")
        {
            Keys[collision.gameObject.GetComponent<KeyController>().KeyID]++;
            Player_GUI.updateGUI();
            var tempObj = Instantiate(particles, PlayerMovePoint.position, Quaternion.identity);
            Destroy(tempObj, 0.5f);
            Destroy(collision.gameObject);
        }
    }
}
