using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveUIBuilder : MonoBehaviour
{
    public SaveSystemManager saveSystemManager;

    public Image screenshot;
    public TextMeshProUGUI saveName;
    public TextMeshProUGUI realtimeDate;
    public TextMeshProUGUI ingameTime;

    public GameObject confirmationErasePanel;
    public TextMeshProUGUI confirmationEraseSaveName;
    public GameObject newSavePanel;
    public TMP_InputField inputFilename;
    public GameObject errorPanel;
    public TextMeshProUGUI errorMessage;

    public GameObject saveContainer;
    public GameObject prefabSave;
    public GameObject saveInformationPanel;

    private SaveLoadButton currentClicked;


    private void OnEnable()
    {
        currentClicked = null;
        saveInformationPanel.SetActive(false);
        BuildSaves();
    }

    private void BuildSaves()
    {
        int childs = saveContainer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(saveContainer.transform.GetChild(i).gameObject);
        }

        List<SaveJacket> jacketOrdered = SaveSystem.saveJacketAvailable;
        jacketOrdered.Sort((x, y) => y.realDateTime.CompareTo(x.realDateTime));

        foreach (SaveJacket jacket in jacketOrdered)
        {
            GameObject save = Instantiate(prefabSave);
            save.GetComponentInChildren<TextMeshProUGUI>().text = jacket.saveName;
            save.GetComponentInChildren<SaveLoadButton>().saveBuilder = this;
            save.GetComponentInChildren<SaveLoadButton>().saveAssociated = jacket;
            save.transform.SetParent(saveContainer.transform, false);
        }
    }

    public void ErrorOkClicked()
    {
        errorPanel.SetActive(false);
        newSavePanel.SetActive(true);
    }

    public void NewSaveButtonClicked()
    {
        inputFilename.text = "";
        newSavePanel.SetActive(true);
    }
    public void NewSaveButtonSave()
    {
        // TODO: Il faut creer un fichier de sauvegarde avec ce nom
        string filenameSave = inputFilename.text;
        if (saveSystemManager.SaveNameAcceptable(filenameSave))
        {
            if(!saveSystemManager.CreateSave(filenameSave, false))
            {
                newSavePanel.SetActive(false);
                errorMessage.text = I18nManager.Fields["menu.saveMenu.error.errorMessage.alreadyExist"];
                errorPanel.SetActive(true);
            }
            UnbuildInformationSave();
            currentClicked = null;
            newSavePanel.SetActive(false);
            BuildSaves();
        }
        else
        {
            newSavePanel.SetActive(false);
            errorMessage.text = I18nManager.Fields["menu.saveMenu.error.errorMessage.emptyName"];
            errorPanel.SetActive(true);
        }
    }
    public void NewSaveButtonCancel()
    {
        newSavePanel.SetActive(false);
    }

    public void SaveButtonClicked()
    {
        if(currentClicked != null)
        {
            confirmationEraseSaveName.text = "\"" + currentClicked.saveAssociated.saveName + "\"";
            confirmationErasePanel.SetActive(true);
        }
    }
    public void SaveButtonConfirmationYesClicked()
    {
        // TODO: Il faut ici recreer un fichier de sauvegarde avec ce nom
        saveSystemManager.CreateSave(currentClicked.saveAssociated.saveName, true);
        confirmationErasePanel.SetActive(false);
        UnbuildInformationSave();
        currentClicked = null;
        BuildSaves();
    }
    public void SaveButtonConfirmationNoClicked()
    {
        confirmationErasePanel.SetActive(false);
    }

    public void BuildInformationSave(SaveLoadButton buttonClicked)
    {
        saveName.text = buttonClicked.saveAssociated.saveName;
        realtimeDate.text = buttonClicked.saveAssociated.realDateTime.ToString("MM/dd/yyyy hh:mm:ss");
        ingameTime.text = FromIntSecondToStringHours(buttonClicked.saveAssociated.ingameSeconds);
        currentClicked = buttonClicked;

        string path = Application.persistentDataPath + "/" + buttonClicked.saveAssociated.saveName + ".png";
        Sprite imageAssociated = PNGToSpriteTool.Instance.LoadNewSprite(path);
        screenshot.sprite = imageAssociated;

        saveInformationPanel.SetActive(true);
    }
    public void UnbuildInformationSave()
    {
        saveInformationPanel.SetActive(false);
    }

    private string FromIntSecondToStringHours(int _seconds)
    {
        int seconds = _seconds % 60;
        int minutes = _seconds / 60;
        int hours = minutes / 60;
        return "" + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

}
