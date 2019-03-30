using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StoryFragmentCollection
{
    [System.Serializable]
    public class StoryFragmentReader
    {
        public string id;
        public int level;
        public string i18nIDTitle;
        public string i18nIDText;
        public string illustrationPath;
    }

    public class StoryFragmentData
    {
        public string Id;
        public int Level;
        public string I18nIDTitle;
        public string I18nIDText;
        public Sprite Illustration;

        public StoryFragmentData(StoryFragmentReader reader)
        {
            Id = reader.id;
            Level = reader.level;
            I18nIDTitle = reader.i18nIDTitle;
            I18nIDText = reader.i18nIDText;
            Illustration = Resources.Load<Sprite>("IllustrationsStory/" + reader.illustrationPath);
        }
    }

    private static Dictionary<string, StoryFragmentData> collection = null;

    public static void Load(string filenameDropProbability)
    {
        if (collection == null)
        {
            collection = new Dictionary<string, StoryFragmentData>();
            string filepath = filenameDropProbability.Replace(".json", "");
            TextAsset textAsset = Resources.Load<TextAsset>(filenameDropProbability);
            if (textAsset != null)
            {
                string jsonContent = textAsset.text;
                StoryFragmentReader[] resources = JsonHelper.FromJson<StoryFragmentReader>(jsonContent);

                foreach (StoryFragmentReader r in resources)
                {
                    StoryFragmentData data = new StoryFragmentData(r);
                    collection.Add(r.id, data);
                }
            }
            else
            {
                Debug.LogError("The file : " + filepath + " does not exist");
            }
        }
    }

    public static bool Exists(string id)
    {
        return collection.ContainsKey(id);
    }

    public static StoryFragmentData GetDataFromID(string id)
    {
        if (Exists(id))
        {
            return collection[id];
        }
        else
        {
            return null;
        }
    }

    public static StoryFragmentData[] GetAllStories()
    {
        return collection.Values.ToArray();
    }
}
