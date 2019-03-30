using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManagerTheCube : MonoBehaviour
{
    public GUIManager guiManager;
    public Player player;

    private void EscapeKeyPress()
    {
        if (guiManager.currentWindowName == "EgnimaMenu")
        {
            player.GetComponent<PlayerControllerCube>().isEnigma = false;
        }
        if (guiManager.currentWindowName == "InventoryMenu")
        {
            player.GetComponent<PlayerControllerCube>().isInventory = false;
        }

        if (!guiManager.InMenu())
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
        if (!guiManager.InMenu() || guiManager.currentWindowName == "InventoryMenu")
        {
            player.GetComponent<PlayerControllerCube>().isInventory = true;
            guiManager.SwitchToWindow("InventoryMenu");
        }
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeKeyPress();
        else if (Input.GetKeyDown(KeyCode.I))
            InventoryKeyPress();
    }
}
