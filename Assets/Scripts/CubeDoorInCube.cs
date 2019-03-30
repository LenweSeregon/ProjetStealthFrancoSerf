using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDoorInCube : Interactable
{
    public SceneHandler sceneHandler;
    public AInventory playerInventory;

    public override void Interact(Player player)
    {
        InGameInformationHolder.gettingOutFromCube = true;
        InGameInformationHolder.dataSave.playerData.inventory = new InventoryData(playerInventory);
        sceneHandler.SwitchToScene(2);
    }
}
