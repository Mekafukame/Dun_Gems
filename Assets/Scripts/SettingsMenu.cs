using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider MusicSlider, SoundSlider;
    float MusicVolume, SoundVolume;

    private void Start()
    {        
        mainMixer.GetFloat("MusicVolume", out MusicVolume);        
        MusicSlider.value = MusicVolume;
        mainMixer.GetFloat("SoundVolume", out SoundVolume);
        SoundSlider.value = SoundVolume;

    }
    public void setMusicVolume()
    {
        MusicVolume = MusicSlider.value;
        if (MusicVolume == -20)
        {
            mainMixer.SetFloat("MusicVolume", -80f) ;
        }
        else
            mainMixer.SetFloat("MusicVolume", MusicVolume);
        
    }
    public void setSoundVolume()
    {
        SoundVolume = SoundSlider.value;
        if (SoundVolume == -20)
        {
            mainMixer.SetFloat("SoundVolume", -80f);
        }
        else
            mainMixer.SetFloat("SoundVolume", SoundVolume);
    }
}
