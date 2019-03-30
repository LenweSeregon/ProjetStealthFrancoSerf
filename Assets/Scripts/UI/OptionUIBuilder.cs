using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionUIBuilder : MonoBehaviour {

    public OptionManager options;

    public TMP_Dropdown languagePicker;
    public Toggle musicActivatedToggle;
    public Slider volumePicker;
    public TMP_Dropdown resolutionPicker;
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityPicker;

    private void OnEnable()
    {
        BuildResolutionListDropdown();
        BuildLanguageListDropdown();
        BuildQualityListDropdown();
        musicActivatedToggle.isOn = options.MusicActivated;
        volumePicker.value = options.MusicVolume;
        fullscreenToggle.isOn = options.Fullscreen;
    }

    private void BuildResolutionListDropdown()
    {
        resolutionPicker.ClearOptions();

        List<string> resolutionsAsString = new List<string>();

        foreach (Resolution resolution in options.Resolutions)
        {
            string resolutionStr = resolution.width + " x " + resolution.height;
            resolutionsAsString.Add(resolutionStr);
        }

        resolutionPicker.AddOptions(resolutionsAsString);
        resolutionPicker.value = options.ResolutionIndex;
        resolutionPicker.RefreshShownValue();
    }
    private void BuildLanguageListDropdown()
    {
        languagePicker.ClearOptions();
        List<string> languages = new List<string>();
        languages.Add(I18nManager.Fields["menus.settingsMenu.languages.english"]);
        languages.Add(I18nManager.Fields["menus.settingsMenu.languages.french"]);
        languagePicker.AddOptions(languages);
        languagePicker.value = options.LanguageIndex;
        languagePicker.RefreshShownValue();
    }
    private void BuildQualityListDropdown()
    {
        qualityPicker.ClearOptions();
        List<string> qualities = new List<string>();
        qualities.Add(I18nManager.Fields["menus.settingsMenu.graphics.quality.low"]);
        qualities.Add(I18nManager.Fields["menus.settingsMenu.graphics.quality.medium"]);
        qualities.Add(I18nManager.Fields["menus.settingsMenu.graphics.quality.high"]);
        qualityPicker.AddOptions(qualities);
        qualityPicker.value = options.QualityIndex;
        qualityPicker.RefreshShownValue();
    }

}
