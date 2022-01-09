using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config
{
    public float SoundVolume, MusicVolume;   
    public PlayerProfile LastProfile;
    public int LastProfileID;

    public Config(float music, float sound, PlayerProfile profile, int profileID)
    {
        SoundVolume = sound;
        MusicVolume = music;
        LastProfile = profile;
        LastProfileID = profileID;

    }
    public Config(float music, float sound)
    {
        SoundVolume = sound;
        MusicVolume = music;
        LastProfile = null;
        LastProfileID = 0;
    }
}

