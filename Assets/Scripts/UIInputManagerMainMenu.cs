using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManagerMainMenu : MonoBehaviour {

    public GUIManager guiManager;

    private void EscapePressed()
    {
        guiManager.SwitchBackToPrevious();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            EscapePressed();
    }
}
