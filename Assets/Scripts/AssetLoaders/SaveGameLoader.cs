using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityRPG;
using UnityEngine;

public class SaveGameLoader
{
    public static void SaveGame(SaveGameData saveGameData, string name)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string fullpath = GetSavePath(name);

        using (FileStream stream = new FileStream(fullpath, FileMode.Create))
        {
            try
            {
                formatter.Serialize(stream, saveGameData);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return;
            }
        }

        Debug.Log("Saved to " + fullpath);
    }

    public static List<string> getSaveGameList()
    {
        var fileArray = Directory.GetFiles(Application.persistentDataPath).ToList();
        List<string> fileList = new List<string>();
        foreach (var file in fileList)
        {
            fileList.Add(Path.GetFileNameWithoutExtension(file));
        }
        return fileList;
    }

    public static SaveGameData LoadGame(string name)
    {
        string fullpath = GetSavePath(name);

        if (!DoesSaveGameExist(name))
        {
            Debug.Log("Unable to load" + fullpath);
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(fullpath, FileMode.Open))
        {
            try
            {
                SaveGameData saveGameData = formatter.Deserialize(stream) as SaveGameData;
                Debug.Log("Loaded: " + fullpath + ", timestamp: " + saveGameData.timestamp);
                return saveGameData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    private static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".sav");
    }

    public static bool DoesSaveGameExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }
}

