using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcePointData
{
    public float[] position;
    public bool hasBeenHarvested;
    public float timeSinceHarvested;

    public ResourcePointData(ResourcePoint rp)
    {
        position = new float[3];
        position[0] = rp.transform.position.x;
        position[1] = rp.transform.position.y;
        position[2] = rp.transform.position.z;

        hasBeenHarvested = rp.HasBeenHarvested;
        timeSinceHarvested = rp.TimeSinceHarvest;
    }
}
