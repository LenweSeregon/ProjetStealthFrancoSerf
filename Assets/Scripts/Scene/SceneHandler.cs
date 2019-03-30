using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public UILoadingScreenUpdater loadingUpdater;
    public GUIManager guiManager;

    public void SwitchToSceneWithoutParameter(int sceneIndex)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex == 3 && sceneIndex == 2)
        {
            InGameInformationHolder.gettingOutFromCube = true;
        }
        else
        {
            InGameInformationHolder.gettingOutFromCube = false;
        }
        InGameInformationHolder.dataSave = null;
        StartCoroutine(Switch(sceneIndex));
    }

    public void SwitchToScene(int sceneIndex)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentIndex == 3 && sceneIndex == 2)
        {
            InGameInformationHolder.gettingOutFromCube = true;
        }
        else
        {
            InGameInformationHolder.gettingOutFromCube = false;
        }
        StartCoroutine(Switch(sceneIndex));
    }

    private IEnumerator Switch(int sceneIndex)
    {
        if(sceneIndex == 0)
        {
            loadingUpdater.UpperTextID = "menus.exitingToMainMenu.message";
            loadingUpdater.LowerTextID = "menus.exitingToMainMenu.wait";
        }
        else
        {
            loadingUpdater.UpperTextID = "menus.loadingScreen.message";
            loadingUpdater.LowerTextID = "menus.loadingScreen.wait";
        }

        guiManager.SwitchToWindow("LoadingMenu");
        loadingUpdater.StartUILoadingScreenUpdater();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            float percentage = asyncLoad.progress;
            loadingUpdater.PercentageComplete = asyncLoad.progress;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    
}
