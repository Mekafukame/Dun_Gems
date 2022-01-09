using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{    
    public int Level;
    public string Difficulty;
    public string ProfileName;

    public PlayerProfile(int Lvl, string Diff,string Name)
    {
        Level = Lvl;
        Difficulty = Diff;
        ProfileName = Name;
    }
}
