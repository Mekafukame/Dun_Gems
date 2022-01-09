using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineEffect : MonoBehaviour
{

    public GameObject shineStar;
    public float minX, maxX , minY, maxY;
    void Start()
    {
        StartCoroutine(Shine());
    }

    private void Update()
    {
        
    }
    IEnumerator Shine()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
        Vector3 newPos = new Vector3(transform.position.x + Random.Range(minX, maxX), transform.position.y + Random.Range(minY, maxY), 0f);              
        shineStar.transform.position = newPos;
        shineStar.GetComponent<Animator>().Play(0);
        StartCoroutine(Shine());
    }
}
