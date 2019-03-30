using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDoorInGame : Interactable
{
    public SaveSystemManager saveSystem;
    public SceneHandler sceneHandler;

    public override void Interact(Player player)
    {
        Save tmpSave = saveSystem.GetTemporarySave();
        InGameInformationHolder.dataSave = new SaveData(tmpSave);
        sceneHandler.SwitchToScene(3);
    }
}