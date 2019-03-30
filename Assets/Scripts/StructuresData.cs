using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructuresData
{
    public WorkTableData[] workTables;
    public ChestData[] chests;
    public FurnaceData[] furnaces;

    public StructuresData(WorkTable[] _workTables, Chest[] _chests, Furnace[] _furnaces)
    {
        workTables = new WorkTableData[_workTables.Length];
        chests = new ChestData[_chests.Length];
        furnaces = new FurnaceData[_furnaces.Length];

        for(int i = 0; i < _workTables.Length; i++)
        {
            workTables[i] = new WorkTableData(_workTables[i]);
        }

        for(int i = 0; i < _chests.Length; i++)
        {
            chests[i] = new ChestData(_chests[i]);
        }

        for(int i = 0; i < _furnaces.Length; i++)
        {
            furnaces[i] = new FurnaceData(_furnaces[i]);
        }
    }
}
