using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgnimaDoorController : Interactable
{
    public EgnimaDoorManager doorManager;
    private bool hasBeenDestroyed;

    private void Awake()
    {
        hasBeenDestroyed = false;
    }

    public override bool IsInteractable()
    {
        return hasBeenDestroyed == false;
    }

    public override string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.break";
    }

    public override void Interact(Player player)
    {
        hasBeenDestroyed = true;
        doorManager.OpenDoorsForEver();
    }
}
