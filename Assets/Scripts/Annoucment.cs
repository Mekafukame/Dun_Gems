using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annoucment : MonoBehaviour
{

    public TMPro.TextMeshProUGUI Text;
    bool isScaling = true;
    bool ScaleDown = false;

    // Start is called before the first frame update
    void Start()
    {
        Text.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(Timer());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isScaling)
        {
            Text.transform.localScale = Vector3.Lerp(Text.transform.localScale, new Vector3(1, 1, 1),20 * Time.deltaTime);
            if(Vector3.Distance(Text.transform.localScale, new Vector3(1, 1, 1)) < 0.05f)
            {
                Text.transform.localScale = new Vector3(1, 1, 1);
                isScaling = false;
            }
        }
        else if(ScaleDown)
        {
            Text.transform.localScale = Vector3.Lerp(Text.transform.localScale, new Vector3(0, 0, 0), 20 * Time.deltaTime);
            if (Vector3.Distance(Text.transform.localScale, new Vector3(0, 0, 0)) < 0.05f)
            {
                Text.transform.localScale = new Vector3(0, 0, 0);
                isScaling = false;
            }
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(Text.text.Length/10);
        ScaleDown = true;
        Destroy(gameObject, 1f);
    }
}
