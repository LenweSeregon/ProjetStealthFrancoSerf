using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData
{
    public float[] position;
    public float[] rotation;
    public InventoryData inventory;

    public ChestData(Chest chest)
    {
        position = new float[3];
        rotation = new float[3];

        position[0] = chest.transform.position.x;
        position[1] = chest.transform.position.y;
        position[2] = chest.transform.position.z;

        rotation[0] = chest.transform.eulerAngles.x;
        rotation[1] = chest.transform.eulerAngles.y;
        rotation[2] = chest.transform.eulerAngles.z;

        inventory = new InventoryData(chest.GetComponent<AInventory>());
    }
}
