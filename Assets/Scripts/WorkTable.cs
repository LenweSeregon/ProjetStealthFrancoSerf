using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkTable : Interactable {

    private GUIManager guiManager;

    private void Awake()
    {
        guiManager = FindObjectOfType<GUIManager>();
    }

    public override void Interact(Player player)
    {
        FactoryBuilder.IsCraftTableOpen = true;
        guiManager.SwitchToWindow("CraftMenu");
    }
}
