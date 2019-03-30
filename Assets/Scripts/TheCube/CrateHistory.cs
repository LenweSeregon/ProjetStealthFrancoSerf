using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateHistory : Interactable
{
    public AInventory playerInventory;
    public CubeManager cubeManager;
    private bool hasGetFragment;

    private void Start()
    {
        hasGetFragment = false;
    }


    public override bool IsInteractable()
    {
        return hasGetFragment == false && playerInventory.GetQuantityOfItem("key") > 0;
    }

    public override string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.fragmentInteraction";
    }

    public override void Interact(Player player)
    {
        if(playerInventory.GetQuantityOfItem("key") > 0)
        {
            playerInventory.RemoveQuantityFromInventory("key", 1);
            hasGetFragment = true;
            cubeManager.HistoryFragmentGathered();
        }
    }
}
