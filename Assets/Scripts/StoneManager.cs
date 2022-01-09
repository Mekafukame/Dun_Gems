using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    public List<GameObject> LightStones = new List<GameObject>();
    public List<GameObject> HeavyStones = new List<GameObject>();
    public List<GameObject> SolidStones = new List<GameObject>();
    public List<GameObject> ExplosiveStones = new List<GameObject>();
    public List<GameObject> Gems = new List<GameObject>();
    public List<GameObject> Box = new List<GameObject>();
    void Start()
    {
        UpdateLists();
    }

    // Update is called once per frame
    void Update()    
    {
        StartCoroutine(MoveLightStones());
        StartCoroutine(MoveHeavyStones());
        StartCoroutine(MoveSolidStones());
        StartCoroutine(MoveExplosivestones());
        StartCoroutine(MoveGems());
        StartCoroutine(MoveBox());

    }

    public void ClearLists()
    {
        LightStones.Clear();
        HeavyStones.Clear();
        SolidStones.Clear();
        ExplosiveStones.Clear();
        Gems.Clear();
        Box.Clear();
    }
    public void UpdateLists()
    {
        //load all gameObjects with stone tag
        var objList = GameObject.FindGameObjectsWithTag("Stone");
        //add to diffrent lists stones without move point
        foreach (GameObject obj in objList)
        {
            if (obj.name.Contains("Light"))
            {
                LightStones.Add(obj);
            }
            else if (obj.name.Contains("Heavy"))
            {
                HeavyStones.Add(obj);
            }
            else if (obj.name.Contains("Solid"))
            {
                SolidStones.Add(obj);
            }
            else if (obj.name.Contains("Explosive"))
            {
                ExplosiveStones.Add(obj);
            }
        }
        //load all gameObjects with gems tag
        objList = GameObject.FindGameObjectsWithTag("Gems");
        //add to list only gems without move point
        foreach (GameObject obj in objList)
        {
            if (obj.layer == LayerMask.NameToLayer("Collectible"))
            {
                Gems.Add(obj);
            }
        }
        //load all gameObjects with Box tag
        objList = GameObject.FindGameObjectsWithTag("Box");
        //add to list only Box without move point
        foreach (GameObject obj in objList)
        {
            if (obj.layer == LayerMask.NameToLayer("OtherTilemaps"))
            {
                Box.Add(obj);
            }
        }

    }
    //update lightstone one by one
    IEnumerator MoveLightStones()
    {
        for (int i = 0; i < LightStones.Count; i++)
        {
            if(LightStones[i] == null)
            {
                LightStones.Remove(null);
            }
            else {
                LightStones[i].GetComponent<LightStone>().CheckMove();
            yield return new WaitForSeconds(0.001f);
            }
        }
    }
    IEnumerator MoveHeavyStones()
    {
        for (int i = 0; i < HeavyStones.Count; i++)
        {
            if (HeavyStones[i] == null)
            {
                HeavyStones.Remove(null);
            }
            else
            {
                HeavyStones[i].GetComponent<HeavyStone>().CheckMove();
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
    IEnumerator MoveSolidStones()
    {
        for (int i = 0; i < SolidStones.Count; i++)
        {
            if (SolidStones[i] == null)
            {
                SolidStones.Remove(null);
            }
            else
            {
                SolidStones[i].GetComponent<SolidStone>().CheckMove();
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
    IEnumerator MoveBox()
    {
        for (int i = 0; i < Box.Count; i++)
        {
            if (Box[i] == null)
            {
                Box.Remove(null);
            }
            else
            {
                Box[i].GetComponent<HeavyStone>().CheckMove();
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
    //update explosive stone one by one
    IEnumerator MoveExplosivestones()
    {
        for (int i = 0; i < ExplosiveStones.Count; i++)
        {
            if(ExplosiveStones[i] == null)
            {
                ExplosiveStones.Remove(null);
            }
            else {
                ExplosiveStones[i].GetComponent<ExplosiveStone>().CheckMove();
            yield return new WaitForSeconds(0.001f);
            }
        }
    }
    //update gems one by one
    public IEnumerator MoveGems()
    {
        for(int i = 0 ; i < Gems.Count ; i++) 
        {
            if (Gems[i] == null)
            {
                Gems.Remove(null);
            }
            else
            {
                Gems[i].GetComponent<LightStone>().CheckMove();
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
}
