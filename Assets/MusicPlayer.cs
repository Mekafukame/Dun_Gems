using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> Clips = new List<AudioClip>();
    AudioSource Audio;
    int lastClip, numOfClips;    
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        numOfClips = Clips.Count;
        if (Clips.Count > 0)
        {
            lastClip = Mathf.RoundToInt(Random.Range(0, numOfClips));
            StartCoroutine(PlaySong());
        }
    }
    IEnumerator PlaySong()
    {
        lastClip++;
        if (lastClip == numOfClips)
            lastClip = 0;
        float waitTime = Clips[lastClip].length + 2f;
        Audio.clip = Clips[lastClip];
        Audio.Play();        
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(PlaySong());
    }
  
}
