using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class, which is a manager present as a gameobject is our language updater
/// It function is pretty simple, with a coroutine, it manage to update all texts present
/// in the game that are target of internationalization.
/// This class should be use with a UI to know how the update is progressing. With some public 
/// attributes, it is possible to know how many text still need to be updated
/// </summary>
public class LanguageUpdater : MonoBehaviour
{
    private bool isUpdating;
    public bool IsUpdating
    {
        get
        {
            return isUpdating;
        }
        private set { }
    }

    private int totalToRegenerate;
    public int TotalToRegenerate
    {
        get
        {
            return totalToRegenerate;
        }
        private set { }
    }

    private int currentlyRegenerated;
    public int CurrentlyRegenerated
    {
        get
        {
            return currentlyRegenerated;
        }
        private set { }
    }

    /// <summary>
    /// This public method just allow to start the coroutine that will update all texts
    /// </summary>
    public void StartUpdatingTexts()
    {
        StartCoroutine(UpdateAllTexts());
    }

    /// <summary>
    /// This private Coroutine handle as an asynchronous function, the update of 
    /// all texts present in the game.
    /// To do so, it simply find all object that are of type TextUI and update their
    /// content by calling a method that will regenerate their text content according to
    /// I18nManager
    /// </summary>
    /// <returns>An IEnumerator that yield at the end</returns>
    private IEnumerator UpdateAllTexts()
    {
        isUpdating = true;
        TextUI[] texts = (TextUI[])Resources.FindObjectsOfTypeAll(typeof(TextUI));
        totalToRegenerate = texts.Length;
        currentlyRegenerated = 0;
        foreach (TextUI text in texts)
        {
            currentlyRegenerated++;
            text.OnLanguageUpdate();
            yield return null;
        }
        isUpdating = false;
        yield return null;
    }
}
