using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour {

    public SceneHandler sceneHandler;
    public InGameManager inGameManager;
    public StoryManager storyManager;
    public Canvas canvas;

    public Save GetTemporarySave()
    {
        ResourcePoint[] rpsList = FindObjectsOfType<ResourcePoint>();
        Chest[] chests = FindObjectsOfType<Chest>();
        WorkTable[] workTables = FindObjectsOfType<WorkTable>();
        Furnace[] furnaces = FindObjectsOfType<Furnace>();

        return new Save("", (int)inGameManager.TimeInSecond, inGameManager.player, storyManager, inGameManager.camera, rpsList, chests, workTables, furnaces);
    }

    public bool CreateSave(string saveName, bool replace)
    {
        ResourcePoint[] rpsList = FindObjectsOfType<ResourcePoint>();
        Chest[] chests = FindObjectsOfType<Chest>();
        WorkTable[] workTables = FindObjectsOfType<WorkTable>();
        Furnace[] furnaces = FindObjectsOfType<Furnace>();
        if (replace)
        {
            Save save = new Save(saveName, (int)inGameManager.TimeInSecond, inGameManager.player, storyManager, inGameManager.camera, rpsList, chests, workTables, furnaces);
            StartCoroutine(TakeScreenshot(saveName));
            SaveSystem.CreateSaveReplace(save);
            return true;
        }
        else
        {
            List<string> availables = SaveSystem.AllSavesNameAvailable();
            if(availables.Contains(saveName))
            {
                return false;
            }

            Save save = new Save(saveName, (int)inGameManager.TimeInSecond, inGameManager.player, storyManager, inGameManager.camera, rpsList, chests, workTables, furnaces);
            StartCoroutine(TakeScreenshot(saveName));
            SaveSystem.CreateSave(save);
            return true;
        }
    }

    private IEnumerator TakeScreenshot(string saveName)
    {
        yield return null;

        canvas.enabled = false;
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/" + saveName + ".png");

        canvas.enabled = true;
    }
    public bool SaveNameAcceptable(string saveName)
    {
        if (saveName.Length == 0)
            return false;

        return true;
    }

    public void LoadGame(string saveName)
    {
        SaveData data = SaveSystem.Load(saveName);
        InGameInformationHolder.dataSave = data;
        sceneHandler.SwitchToScene(2);
    }
}
