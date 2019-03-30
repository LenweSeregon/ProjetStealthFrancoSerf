using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This class regroup and manage all options according to settings
/// in the game. This class need to be used to access or modify options
/// </summary>
public class OptionManager : MonoBehaviour
{
    private bool isInitializing;
    public bool IsInitializing
    {
        get
        {
            return isInitializing;
        }
        set
        {
            isInitializing = value;
        }
    }

    #region OptionVariables
    private int languageIndex;
    public int LanguageIndex
    {
        get
        {
            return languageIndex;
        }
        set
        {
            languageIndex = value;
            PlayerPrefs.SetInt("language", languageIndex);
            string languageAsString = FromLanguageIndexToString(languageIndex);
            if (languageAsString != I18nManager.Language)
            {
                I18nManager.Language = languageAsString;
                if (!IsInitializing)
                {
                    StartCoroutine(UpdateLanguage());
                }
            }
        }
    }

    private bool musicActivated;
    public bool MusicActivated
    {
        get
        {
            return musicActivated;
        }
        set
        {
            musicActivated = value;
            if(musicActivated)
            {
                audioMixer.SetFloat("MasterVolume", musicVolume);
            }
            else
            {
                audioMixer.SetFloat("MasterVolume", 0);
            }
            PlayerPrefs.SetInt("musicActivated", (musicActivated) ? (1) : (0));
        }
    }

    private float musicVolume;
    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            audioMixer.SetFloat("MasterVolume", musicVolume);
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        }
    }

    private int resolutionIndex;
    public int ResolutionIndex
    {
        get
        {
            return resolutionIndex;
        }
        set
        {
            resolutionIndex = value;
            PlayerPrefs.SetInt("resolution", resolutionIndex);
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }

    private bool fullscreen;
    public bool Fullscreen
    {
        get
        {
            return fullscreen;
        }
        set
        {
            fullscreen = value;
            Screen.fullScreen = fullscreen;
            PlayerPrefs.SetInt("fullscreen", (fullscreen) ? (1) : (0));
        }
    }

    private int qualityIndex;
    public int QualityIndex
    {
        get
        {
            return qualityIndex;
        }
        set
        {
            qualityIndex = value;
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt("quality", qualityIndex);
        }
    }

    private Resolution[] resolutions;
    public Resolution[] Resolutions
    {
        get
        {
            if(resolutions == null)
            {
                resolutions = Screen.resolutions;
            }
            return resolutions;
        }

    }
    #endregion
    #region LogicVariables
    public AudioMixer audioMixer;
    public UILoadingScreenUpdater loadingUpdater;
    public LanguageUpdater languageUpdater;
    public GUIManager guiManager;
    #endregion

    /// <summary>
    /// At start, the option manager simply load (via PlayerPrefs) the options 
    /// and assign them to current option variable
    /// </summary>
    private void Start()
    {
        LoadAndAssignOptions();
    }

    /// <summary>
    /// This method firsly load if possible the settings, and if not, just assign them
    /// to a default value.
    /// </summary>
    private void LoadAndAssignOptions()
    {
        Resolution[] res = Resolutions;
        IsInitializing = true;

        LanguageIndex = (PlayerPrefs.HasKey("language")) ? (PlayerPrefs.GetInt("language")) : (0);
        MusicActivated = PlayerPrefs.HasKey("musicActivated") ? (PlayerPrefs.GetInt("musicActivated") == 1) : (true);
        MusicVolume = PlayerPrefs.HasKey("musicVolume") ? (PlayerPrefs.GetFloat("musicVolume")) : (0);
        ResolutionIndex = PlayerPrefs.HasKey("resolution") ? (PlayerPrefs.GetInt("resolution")) : (res.Length - 1);
        Fullscreen = (PlayerPrefs.HasKey("fullscreen")) ? (PlayerPrefs.GetInt("fullscreen") == 1) : (true);
        QualityIndex = (PlayerPrefs.HasKey("quality")) ? (PlayerPrefs.GetInt("quality")) : (2);

        IsInitializing = false;
    }

    /// <summary>
    /// Coroutine allowing to update the language at runtime
    /// This function simply check if language change is really neccessary
    /// then it start both logic and graphic coroutine for update
    /// and update the animation. When it finish, we get back to the option menu
    /// </summary>
    /// <returns>IEnumerator return value ie null</returns>
    private IEnumerator UpdateLanguage()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        guiManager.SwitchToWindow("LoadingMenu");
        loadingUpdater.UpperTextID = "menus.updateLanguage.update";
        loadingUpdater.LowerTextID = "menus.updateLanguage.wait";
        languageUpdater.StartUpdatingTexts();
        loadingUpdater.StartUILoadingScreenUpdater();

        while(languageUpdater.IsUpdating)
        {
            int currentlyGenerated = languageUpdater.CurrentlyRegenerated;
            int totalToRegenerate = languageUpdater.TotalToRegenerate;
            loadingUpdater.PercentageComplete = (float)currentlyGenerated / (float)totalToRegenerate;
            yield return null;
        }

        loadingUpdater.IsUpdating = false;
        guiManager.SwitchToWindow("OptionMenu");
        yield return null;
    }

    /// <summary>
    /// This methode transform a language index to an str representing this language
    /// </summary>
    /// <param name="languageIndex">index of the language</param>
    /// <returns>string representing this language</returns>
    private string FromLanguageIndexToString(int languageIndex)
    {
        if (languageIndex == 0)
        {
            return "EN";
        }
        else
        {
            return "FR";
        }
    }



}
