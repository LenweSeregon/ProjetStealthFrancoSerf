using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryFragment
{
    private int storyLevel;
    public int StoryLevel
    {
        get { return storyLevel; }
        private set { }
    }
    private string storyI18nIDtext;
    public string StoryI18nIDtext
    {
        get { return storyI18nIDtext; }
        private set { }
    }
    private string storyI18nIDtitle;
    public string StoryI18nIDtitle
    {
        get { return storyI18nIDtitle; }
        private set { }
    }
    private Sprite illustration;
    public Sprite Illustration
    {
        get { return illustration; }
        private set { }
    }

    public StoryFragment(int _storyLevel, string _i18nIDtext, string _i18nIDtitle, Sprite _illustration)
    {
        storyLevel = _storyLevel;
        storyI18nIDtext = _i18nIDtext;
        storyI18nIDtitle = _i18nIDtitle;
        illustration = _illustration;
    }
}
