using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This static class is used to represent our internationalization in the game
/// Through this class, your can load a language by setting the language attribut
/// and then access at any time, a field present in language file via the Fields attributes
/// </summary>
public static class I18nManager
{
    private static string language;
    public static string Language
    {
        get
        {
            return language;
        }
        set
        {
            language = value;
            LoadLanguage();
        }
    }

    public static Dictionary<string, string> Fields
    {
        get;
        private set;
    }

    /// <summary>
    /// This private method is call when we set the language attribut to a value.
    /// It simply load a language file according to the filename and fill the Fields
    /// attributes with a pair-value system.
    /// </summary>
    /// <returns>true if language has been loaded (file exist), false otherwise</returns>
    private static bool LoadLanguage()
    {
        if(Fields == null)
        {
            Fields = new Dictionary<string, string>();
        }

        Fields.Clear();
        
        TextAsset textFile = Resources.Load("I18n/"+Language) as TextAsset;
        string allTexts = "";

        if (textFile == null)
        {
            Debug.LogError("File not found for I18n : Assets/Resources/I18n/" + language + " trying to load english one");
            textFile = Resources.Load(@"I18n/EN") as TextAsset;
        }

        if(textFile == null)
        {
            Debug.LogError("File not found for I18n: Assets/Resources/I18n/" + language + ".txt");
            return false;
        }

        allTexts = textFile.text;

        string[] lines = allTexts.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        
        string key, value;
        foreach(string line in lines)
        {
            if (line.IndexOf("=") >= 0 && !line.StartsWith("#"))
            {
                key = line.Substring(0, line.IndexOf("="));
                value = line.Substring(line.IndexOf("=") + 1, line.Length - line.IndexOf("=") - 1);
                value = value.Replace("\\n", System.Environment.NewLine);
                Fields.Add(key.Trim(), value.Trim());
            }
        }

        return true;
    }
}
