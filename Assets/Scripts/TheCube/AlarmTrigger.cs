using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : Interactable {

    [HideInInspector]
    public bool hasBeenTricked;
    public AInventory playerInventory;

    private void Awake()
    {
        hasBeenTricked = false;
    }

    public override bool IsInteractable()
    {
        return hasBeenTricked == false && playerInventory.GetQuantityOfItem("trap") > 0;
    }

    public override string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.trapInteraction";
    }

    public override void Interact(Player player)
    {
        // Test if player has tricker
        if(playerInventory.GetQuantityOfItem("trap") > 0)
        {
            playerInventory.RemoveQuantityFromInventory("trap", 1);
            hasBeenTricked = true;
        }
    }
}
