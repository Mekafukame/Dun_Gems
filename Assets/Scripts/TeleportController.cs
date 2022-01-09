using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform SecondTeleport;
    public GameObject Annoucement;
    public ParticleSystem particles;
       
    IEnumerator StartTeleport()
    {
        yield return new WaitForSeconds(2f);
       //check if there is a player on teleport
        if (Physics2D.OverlapCircle(transform.position, .2f, LayerMask.GetMask("OtherTilemaps")))
        {           
            GameObject tempObj = Physics2D.OverlapCircle(transform.position, .2f, LayerMask.GetMask("OtherTilemaps")).gameObject;
            if (tempObj.tag == "Player")
            {
                if (Physics2D.OverlapCircle(SecondTeleport.position, .2f, LayerMask.GetMask("OtherTilemaps")))
                {
                    var Obj = Physics2D.OverlapCircle(SecondTeleport.position, .2f, LayerMask.GetMask("OtherTilemaps"));
                    if(Obj.tag != "Stone" && Obj.tag != "Box" && Obj.tag != "Gems")
                    {
                        tempObj.GetComponent<PlayerMovement>().PlayerMovePoint.position = SecondTeleport.transform.position;
                        tempObj.transform.position = SecondTeleport.transform.position;
                    }
                    else
                    {
                        tempObj = Instantiate(Annoucement);
                        tempObj.GetComponent<Annoucment>().Text.text = "There is something on the other side!";
                    }
                }
                else
                {
                    tempObj.GetComponent<PlayerMovement>().PlayerMovePoint.position = SecondTeleport.transform.position;
                    tempObj.transform.position = SecondTeleport.transform.position;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)   
    {       
        if (collision.tag == "Player")
        {
            particles.Play();
            particles.GetComponent<AudioSource>().Play();
            StartCoroutine(StartTeleport());
        }        
    }                     
    
}
