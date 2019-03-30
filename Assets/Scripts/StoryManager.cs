using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [HideInInspector]
    public int currentStoryLevel;

    [HideInInspector]
    public List<StoryFragment> storyFragments;
    private StoryFragment currentStoryFragment;

    private void Awake()
    {
        storyFragments = new List<StoryFragment>();
    }

    void Start ()
    {
        if (InGameInformationHolder.dataSave != null)
        {
            currentStoryLevel = InGameInformationHolder.dataSave.storyManagerData.currentStoryLevel;
        }
        else
        {
            currentStoryLevel = 1;
        }

        foreach (var story in StoryFragmentCollection.GetAllStories())
        {
            storyFragments.Add(new StoryFragment(story.Level, story.I18nIDText, story.I18nIDTitle, story.Illustration));
        }
	}

    public StoryFragment GetStoryAtLevel(int level)
    {
        if(level >= 1 && level <= storyFragments.Count)
        {
            return storyFragments[level-1];
        }
        return null;
    }

}
