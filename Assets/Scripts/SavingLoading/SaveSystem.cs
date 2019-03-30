using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string savesAvailableFilepath = "/savesAvailable.bin";
    public static List<SaveJacket> saveJacketAvailable;

    static SaveSystem()
    {
        saveJacketAvailable = new List<SaveJacket>();

        SaveJacketAvailableData availables = LoadAvailableSaves();
        if(availables != null)
        {
            for (int i = 0; i < availables.savesAvailables.Length; i++)
            {
                saveJacketAvailable.Add(new SaveJacket(availables.savesAvailables[i]));
            }
        }
    }

    public static List<string> AllSavesNameAvailable()
    {
        List<string> availables = new List<string>();
        foreach(SaveJacket jacket in saveJacketAvailable)
        {
            availables.Add(jacket.saveName);
        }
        return availables;
    }

    public static void DisplayAvailables()
    {
        Debug.Log("Saves availables : ");
        foreach(SaveJacket jacket in saveJacketAvailable)
        {
            Debug.Log(jacket.saveName);
            Debug.Log(jacket.realDateTime.ToString());
            Debug.Log(jacket.ingameSeconds);
        }
    }

    private static bool AddSaveJacketAvailable(SaveJacket saveJacket)
    {
        foreach(SaveJacket jacket in saveJacketAvailable)
        {
            if(jacket.saveName == saveJacket.saveName)
            {
                return false;
            }
        }

        saveJacketAvailable.Add(saveJacket);
        SaveAvailableJacket();
        return true;
    }

    private static void ReplaceSaveJacketAvailable(SaveJacket saveJacket)
    {
        for(int i = 0; i < saveJacketAvailable.Count; i++)
        {
            if(saveJacketAvailable[i].saveName == saveJacket.saveName)
            {
                saveJacketAvailable[i] = saveJacket;
            }
        }
        SaveAvailableJacket();
    }

    public static void SaveAvailableJacket()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savesAvailableFilepath;

        FileStream stream = new FileStream(path, FileMode.Create);
        SaveJacketAvailableData data = new SaveJacketAvailableData(saveJacketAvailable);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveJacketAvailableData LoadAvailableSaves()
    {
        string path = Application.persistentDataPath + savesAvailableFilepath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveJacketAvailableData data = formatter.Deserialize(stream) as SaveJacketAvailableData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in : " + path);
            return null;
        }
    }

    public static void CreateSave(Save save)
    {
        SaveJacket saveJacket = new SaveJacket(save.saveName, DateTime.Now, save.ingameSeconds);

        if(AddSaveJacketAvailable(saveJacket))
        {
            Save(save);
        }
    }

    public static void CreateSaveReplace(Save save)
    {
        SaveJacket saveJacket = new SaveJacket(save.saveName, DateTime.Now, save.ingameSeconds);
        ReplaceSaveJacketAvailable(saveJacket);
        Save(save);
    }

    public static void Save(Save save)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + save.saveName + ".bin";

        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(save);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load(string saveName)
    {
        string path = Application.persistentDataPath + "/"+saveName+".bin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in : " + path);
            return null;
        }
    }
}
