using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveJacketData
{
    public string saveName;
    public string realDateTimeAsString;
    public int ingameSeconds;

    public SaveJacketData(SaveJacket saveJacket)
    {
        saveName = saveJacket.saveName;
        realDateTimeAsString = saveJacket.realDateTime.ToString("MM/dd/yyyy hh:mm:ss");
        ingameSeconds = saveJacket.ingameSeconds;
    }
}
