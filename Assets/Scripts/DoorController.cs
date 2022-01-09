using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DoorController : MonoBehaviour
{
    public Sprite OpenedDoors;

    public bool isOpen = false;   

    public int DoorID;
    
    public void OpenDoor()
    {
        isOpen = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = OpenedDoors;
        GetComponent<AudioSource>().Play();
    }
    
}
