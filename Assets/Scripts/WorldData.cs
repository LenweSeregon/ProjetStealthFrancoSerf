using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public StructuresData structures;
    public ResourcePointData[] resourcePointsData;

    public WorldData(ResourcePoint[] resourcePoints, Chest[] chests, WorkTable[] tables, Furnace[] furnaces)
    {
        structures = new StructuresData(tables, chests, furnaces);
        resourcePointsData = new ResourcePointData[resourcePoints.Length];
        for(int i = 0; i < resourcePoints.Length; i++)
        {
            resourcePointsData[i] = new ResourcePointData(resourcePoints[i]);
        }
    }
    
}
