using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgnimaInterruptor : Interactable
{
    private GUIManager guiManager;
    public EgnimaDoorManager doorManager;

    private void Awake()
    {
        guiManager = FindObjectOfType<GUIManager>();
    }

    public override bool IsInteractable()
    {
        return doorManager.systemReady;
    }

    public override string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.basicInteraction";
    }

    public override void Interact(Player player)
    {
        player.GetComponent<PlayerControllerCube>().isEnigma = true;
        guiManager.SwitchToWindow("EgnimaMenu");
    }
}
