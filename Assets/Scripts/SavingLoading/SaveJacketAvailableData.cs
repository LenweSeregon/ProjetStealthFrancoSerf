using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveJacketAvailableData
{
    public SaveJacketData[] savesAvailables;

    public SaveJacketAvailableData(List<SaveJacket> saves)
    {
        savesAvailables = new SaveJacketData[saves.Count];
        for(int i = 0; i < saves.Count; i++)
        {
            savesAvailables[i] = new SaveJacketData(saves[i]);
        }
    }
}
