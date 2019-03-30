using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FurnaceCollection
{
    [System.Serializable]
    public class RecipeFurnaceReader
    {
        public string idRecipe;
        public int heatTime;
        public int temperatureNeeded;
        public string itemToHeatID;
        public string itemHeatedID;
    }

    public class RecipeFurnaceData
    {
        public string IdRecipe;
        public int HeatTime;
        public int TemperatureNeeded;
        public string ItemToHeatID;
        public string ItemHeatedID;

        public RecipeFurnaceData(RecipeFurnaceReader reader)
        {
            IdRecipe = reader.idRecipe;
            HeatTime = reader.heatTime;
            TemperatureNeeded = reader.temperatureNeeded;
            ItemToHeatID = reader.itemToHeatID;
            ItemHeatedID = reader.itemHeatedID;
        }
    }

    private static Dictionary<string, RecipeFurnaceData> collection = null;

    public static void Load(string filenameDropProbability)
    {
        if (collection == null)
        {
            collection = new Dictionary<string, RecipeFurnaceData>();
            string filepath = filenameDropProbability.Replace(".json", "");
            TextAsset textAsset = Resources.Load<TextAsset>(filenameDropProbability);
            if (textAsset != null)
            {
                string jsonContent = textAsset.text;
                RecipeFurnaceReader[] resources = JsonHelper.FromJson<RecipeFurnaceReader>(jsonContent);

                foreach (RecipeFurnaceReader r in resources)
                {
                    RecipeFurnaceData data = new RecipeFurnaceData(r);
                    collection.Add(r.idRecipe, data);
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

    public static RecipeFurnaceData GetDataFromID(string id)
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
}
