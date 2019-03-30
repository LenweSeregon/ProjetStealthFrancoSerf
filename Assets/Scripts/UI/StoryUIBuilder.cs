using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryUIBuilder : MonoBehaviour {

    public StoryManager storyManager;

    public Button previousButton;
    public Button nextButton;
    public TextMeshProUGUI title;
    public TextMeshProUGUI text;
    public Image illustration;

    private int currentStoryVisualization = -1;

    private void OnEnable()
    {
        if(currentStoryVisualization == -1)
        {
            currentStoryVisualization = storyManager.currentStoryLevel;
        }

        UpdateToMatchCurrentStoryVisualization();
    }

    private void UpdateToMatchCurrentStoryVisualization()
    {
        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        if (currentStoryVisualization > 1)
        {
            previousButton.gameObject.SetActive(true);
        }
        if(currentStoryVisualization < storyManager.currentStoryLevel)
        {
            nextButton.gameObject.SetActive(true);
        }
        
        title.text = I18nManager.Fields[storyManager.GetStoryAtLevel(currentStoryVisualization).StoryI18nIDtitle];
        text.text = I18nManager.Fields[storyManager.GetStoryAtLevel(currentStoryVisualization).StoryI18nIDtext];
        illustration.sprite = storyManager.GetStoryAtLevel(currentStoryVisualization).Illustration;
    }

    public void PreviousButtonClicked()
    {
        if(currentStoryVisualization > 1)
        {
            currentStoryVisualization--;
            UpdateToMatchCurrentStoryVisualization();
        }
    }

    public void NextButtonClicked()
    {
        if(currentStoryVisualization + 1 <= storyManager.currentStoryLevel)
        {
            currentStoryVisualization++;
            UpdateToMatchCurrentStoryVisualization();
        }
    }
}
