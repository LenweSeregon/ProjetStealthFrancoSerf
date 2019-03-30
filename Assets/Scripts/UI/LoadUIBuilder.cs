using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadUIBuilder : MonoBehaviour {

    public SaveSystemManager saveSystemManager;

    public Image screenshot;
    public TextMeshProUGUI saveName;
    public TextMeshProUGUI realtimeDate;
    public TextMeshProUGUI ingameTime;

    public GameObject loadContainer;
    public GameObject prefabLoadSave;
    public GameObject loadInformationPanel;

    private SaveLoadButton currentClicked;

    private void OnEnable()
    {
        currentClicked = null;
        loadInformationPanel.SetActive(false);
        BuildLoads();
    }

    private void BuildLoads()
    {
        int childs = loadContainer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(loadContainer.transform.GetChild(i).gameObject);
        }

        List<SaveJacket> jacketOrdered = SaveSystem.saveJacketAvailable;
        jacketOrdered.Sort((x, y) => y.realDateTime.CompareTo(x.realDateTime));

        foreach (SaveJacket jacket in jacketOrdered)
        {
            GameObject save = Instantiate(prefabLoadSave);
            save.GetComponentInChildren<TextMeshProUGUI>().text = jacket.saveName;
            save.GetComponentInChildren<SaveLoadButton>().loadBuilder = this;
            save.GetComponentInChildren<SaveLoadButton>().saveAssociated = jacket;
            save.transform.SetParent(loadContainer.transform, false);
        }
    }

    public void LoadButtonClicked()
    {
        if(currentClicked != null)
        {
            saveSystemManager.LoadGame(currentClicked.saveAssociated.saveName);
        }
    }

    public void BuildInformationLoad(SaveLoadButton buttonClicked)
    {
        saveName.text = buttonClicked.saveAssociated.saveName;
        realtimeDate.text = buttonClicked.saveAssociated.realDateTime.ToString("MM/dd/yyyy hh:mm:ss");
        ingameTime.text = FromIntSecondToStringHours(buttonClicked.saveAssociated.ingameSeconds);
        currentClicked = buttonClicked;

        string path = Application.persistentDataPath + "/" + buttonClicked.saveAssociated.saveName + ".png";
        Sprite imageAssociated = PNGToSpriteTool.Instance.LoadNewSprite(path);
        screenshot.sprite = imageAssociated;

        loadInformationPanel.SetActive(true);
    }
    public void UnbuildInformationSave()
    {
        loadInformationPanel.SetActive(false);
    }

    private string FromIntSecondToStringHours(int _seconds)
    {
        int seconds = _seconds % 60;
        int minutes = _seconds / 60;
        int hours = minutes / 60;
        return "" + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
