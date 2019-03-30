using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveName;
    public int ingameSeconds;
    public PlayerData playerData;
    public CameraControllerData cameraData;
    public WorldData worldData;
    public StoryManagerData storyManagerData;

    public SaveData(Save save)
    {
        saveName = save.saveName;
        ingameSeconds = save.ingameSeconds;
        playerData = new PlayerData(save.player);
        worldData = new WorldData(save.rps, save.chests, save.workTables, save.furnaces);
        cameraData = new CameraControllerData(save.camera);
        storyManagerData = new StoryManagerData(save.storyManager);
    }

}
