using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleGuardEnergySupplier : Interactable
{
    private bool hasBeenActivated;
    public AInventory playerInventory;
    public ChiefCubeGuard chiefGuard;
    public CubeGuard[] guards;
    public EgnimaDoorManager doorEnigma;
    public HQManager hqManager;

    public override bool IsInteractable()
    {
        return hasBeenActivated == false && playerInventory.GetQuantityOfItem("breaker") > 0;
    }

    public override string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.basicInteraction";
    }

    public override void Interact(Player player)
    {
        if(playerInventory.GetQuantityOfItem("breaker") > 0)
        {
            playerInventory.RemoveQuantityFromInventory("breaker", 1);
            doorEnigma.OpenDoorsForEver();
            hasBeenActivated = true;
            hqManager.Shutdown();
            // Power off all guards
            if (chiefGuard != null && chiefGuard.enabled)
            {
                StartCoroutine(chiefGuard.Fall());
            }

            foreach (CubeGuard guard in guards)
            {
                if (guard != null && guard.enabled)
                {
                    guard.down = true;
                    StartCoroutine(guard.Fall());
                }
            }
        }

    }
}
