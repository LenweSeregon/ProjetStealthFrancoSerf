using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManager : MonoBehaviour {

    public StructureHandler structureHandler;
    public GUIManager guiManager;
    public Player player;

    private void EscapePressed()
    {
        if(player.isBuilding)
        {
            structureHandler.StructureToPlace = null;
            structureHandler.RemoveCurrentStructurePlacing();
            return;
        }

        if(!guiManager.InMenu())
        {
            guiManager.SwitchToWindow("PauseMenu");
        }
        else
        {
            guiManager.SwitchBackToPrevious();
        }
    }

    private void InventoryKeyPress()
    {
        if (!player.isBuilding && !player.isCrafting && !guiManager.InMenu() || guiManager.currentWindowName == "InventoryMenu")
        {
            guiManager.SwitchToWindow("InventoryMenu");
        }
    }

    private void CraftKeyPress()
    {
        if (!player.isBuilding && !player.isCrafting && !guiManager.InMenu() || guiManager.currentWindowName == "CraftMenu")
        {
            FactoryBuilder.IsCraftTableOpen = false;
            guiManager.SwitchToWindow("CraftMenu");
        }
    }

    private void StoryKeyPress()
    {
        if (!player.isBuilding && !player.isCrafting && !guiManager.InMenu() || guiManager.currentWindowName == "StoryMenu")
        {
            FactoryBuilder.IsCraftTableOpen = false;
            guiManager.SwitchToWindow("StoryMenu");
        }
    }
	
	void Update ()
    {
        if (!player.isCrafting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                EscapePressed();
            else if (Input.GetKeyDown(KeyCode.I))
                InventoryKeyPress();
            else if (Input.GetKeyDown(KeyCode.C))
                CraftKeyPress();
            else if (Input.GetKeyDown(KeyCode.P))
                StoryKeyPress();
        }

	}
}
