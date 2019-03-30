using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is managing all stuff in GUI area
/// It can delegate to other class but the interaction
/// on GUI are centralized here
/// </summary>
public class GUIManager : MonoBehaviour
{

    /// <summary>
    /// Intern class to represent GUI in inspector
    /// This class allow the GUI manager to handle several window
    /// and to swap between them easily
    /// </summary>
    [System.Serializable]
    public class GUIWindow
    {
        public GameObject window;
        public bool isPausingGame;
        public bool isFinalCandidate;
        public bool isPreviousCandidate;
        public string name;
    }

    public string startingMenuName;
    public GUIWindow[] windowsInspector;
    private Dictionary<string, GUIWindow> windows;

    private Stack<string> windowIDStack;
    private GUIWindow currentWindow;
    [HideInInspector]
    public string currentWindowName;

    /// <summary>
    /// Start method, initializing the dictionary (string,GUIWindow)
    /// with the array in inspector
    /// It finally start the game by showing the main menu
    /// </summary>
    private void Start()
    {
        windowIDStack = new Stack<string>();
        currentWindowName = "null";
        currentWindow = null;
        windows = new Dictionary<string, GUIWindow>();
        windowIDStack.Push("null");

        for(int i = 0; i < windowsInspector.Length; i++)
        {
            windows.Add(windowsInspector[i].name, windowsInspector[i]);
        }

        if(startingMenuName != "")
        {
            SwitchToWindow(startingMenuName);
        }
    }
    
    /// <summary>
    /// Method allowing to know if we're currently in an panel
    /// </summary>
    /// <returns>true if we are in panel, false otherwise </returns>
    public bool InMenu()
    {
        return currentWindowName != "null" && currentWindowName != "InGameGUI";
    }

    /// <summary>
    /// According to the stack builded during the game
    /// This method allow the user to switch back to the previous window
    /// Basically, this method respond to 3 logic statement :
    ///     1. If our current window is a final one, do nothing (useful for main menu)
    ///     2. If our previous window was null, switch back to null (no menu)
    ///     3. Else remove the stack'head and switch to next window in stack
    /// </summary>
    public void SwitchBackToPrevious()
    {
        if(currentWindow.isFinalCandidate)
        {
            return;
        }

        if(windowIDStack.Peek() == "null")
        {
            SwitchToWindow("null");
            return;
        }
        else
        {
            windowIDStack.Pop();
            SwitchToWindow(windowIDStack.Peek());
        }
    }

    /// <summary>
    /// Method allowing to switch between windows
    /// The method ensure that the requested window exist
    /// Then it's set unactive the previous window and active the new one
    /// </summary>
    /// <param name="name">The window's name that we want to display </param>
    public void SwitchToWindow(string name)
    {
        if (name == "null")
        {
            if(currentWindow != null)
            {
                currentWindow.window.SetActive(false);
            }
            windowIDStack.Clear();

            windowIDStack.Push("null");
            currentWindow = null;
            currentWindowName = "null";
            Time.timeScale = 1.0f;


            SwitchToWindow("InGameGUI");
            return;
        }

        if(!windows.ContainsKey(name))
        {
            Debug.LogError("The window " + name + " doesn't exist !");
            return;
        }
        else if (currentWindowName == name)
        {
            if(windowIDStack != null && windowIDStack.Count > 0)
            {
                windowIDStack.Pop();
            }
            
            SwitchBackToPrevious();
            return;
        }
        else
        {
            if (currentWindow != null)
            {
                currentWindow.window.SetActive(false);
            }

            currentWindow = windows[name];
            currentWindow.window.SetActive(true);
            currentWindowName = name;
            if (currentWindow.isPreviousCandidate && windowIDStack.Peek() != name)
            {
                windowIDStack.Push(name);
            }

            if (currentWindow.isPausingGame)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

    }

    /// <summary>
    /// Method that simply close all window (panel) open
    /// </summary>
    public void CloseAllWindow()
    {
        if(currentWindow != null)
        {
            currentWindow.window.SetActive(false);
        }

        currentWindow = null;
        windowIDStack.Clear();
        SwitchToWindow("null");
    }
}
