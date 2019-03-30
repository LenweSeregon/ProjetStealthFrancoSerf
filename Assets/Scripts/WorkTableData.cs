using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkTableData
{
    public float[] position;
    public float[] rotation;

    public WorkTableData(WorkTable table)
    {
        position = new float[3];
        rotation = new float[3];

        position[0] = table.transform.position.x;
        position[1] = table.transform.position.y;
        position[2] = table.transform.position.z;

        rotation[0] = table.transform.eulerAngles.x;
        rotation[1] = table.transform.eulerAngles.y;
        rotation[2] = table.transform.eulerAngles.z;
    }
}
