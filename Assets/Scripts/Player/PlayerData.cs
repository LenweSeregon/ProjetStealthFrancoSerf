using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] position;
    public float[] rotation;

    public InventoryData inventory;

    public PlayerData(Player player)
    {
        level = player.level;
        health = player.health;

        Debug.Log("At Saving : ");
        Debug.Log("X = " + player.transform.position.x);
        Debug.Log("Y = " + player.transform.position.y);
        Debug.Log("Z = " + player.transform.position.z);

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        rotation = new float[3];
        rotation[0] = player.transform.eulerAngles.x;
        rotation[1] = player.transform.eulerAngles.y;
        rotation[2] = player.transform.eulerAngles.z;

        inventory = new InventoryData(player.Inventory);
    }

}
