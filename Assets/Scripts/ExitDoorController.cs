using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorController : DoorController
{
    PlayerInv inv;
    Animator DoorsOpen;
    public GameObject Annoucement;
    public TMPro.TMP_ColorGradient textGrad;
    int numOfGemsRequired = 1;
    private void Start()
    {
        inv = GameObject.Find("Player").GetComponent<PlayerInv>();                
    }
    private void Awake()
    {
        DoorsOpen = GetComponent<Animator>();
    }
    public void updatePoints()
    {
        numOfGemsRequired = GameObject.Find("LevelManager").GetComponent<LevelSettings>().GemsReq;
        isOpen = false;
        DoorsOpen.SetBool("IsOpen", false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isOpen && inv)
        {
            if(numOfGemsRequired <= inv.numOfPoints)
            {
                isOpen = true;
                DoorsOpen.SetBool("IsOpen",true);
                var tempObj = Instantiate(Annoucement);
                tempObj.GetComponent<Annoucment>().Text.text = "The exit gate is now open!";
                tempObj.GetComponent<Annoucment>().Text.rectTransform.position -= new Vector3(0, 700, 0);
                tempObj.GetComponent<Annoucment>().Text.colorGradientPreset = textGrad;
            }
        }
    }
    
}
