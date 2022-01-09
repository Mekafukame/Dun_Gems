using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{   
    
    public static void SaveConfig(float music ,float sound,PlayerProfile profile, int profileID)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.cfg";
        FileStream stream = new FileStream(path, FileMode.Create);
        Config conf = new Config(music, sound, profile, profileID);

        formatter.Serialize(stream, conf);
        stream.Close();
    }
    public static Config LoadConfig()
    {
        string path = Application.persistentDataPath + "/settings.cfg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Config conf = formatter.Deserialize(stream) as Config;
            stream.Close();
            return conf;

        }
        else
        {
            Debug.LogError("Config file not found in " + path);
            return null;
        }
    }
    public static void CreateProfile(int Lvl, string Diff,string profileName, string Profile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + Profile + ".prof";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerProfile profile = new PlayerProfile(Lvl, Diff, profileName);

        formatter.Serialize(stream, profile);
        stream.Close();
    }
    public static void DeleteFile(string path)
    {
        File.Delete(path);        
    }
    public static PlayerProfile LoadProfile(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerProfile profile = formatter.Deserialize(stream) as PlayerProfile;
            stream.Close();
            return profile;
            
        }
        else
        {
            Debug.LogError("Profile file not found in " + path);
            return null;
        }
    }
}
