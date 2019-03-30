using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LimitedInventory))]
public class Chest : Interactable
{

    private GUIManager guiManager;

    private void Awake()
    {
        guiManager = FindObjectOfType<GUIManager>();
    }

    public override void Interact(Player player)
    {
        InventoryDualBuilder.inventoryOther = GetComponent<AInventory>();
        guiManager.SwitchToWindow("InventoryDualMenu");
    }
}
