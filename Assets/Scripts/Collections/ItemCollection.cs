using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the collection for items in the game.
/// It mean that if we need to access a general informations about an item
/// we can request GetDataFromID in this class to get a unique instance that give
/// all informations. It mean that classes don't need to have all informations as attribute
/// but just a ID redirecting to this resource.
/// </summary>
public static class ItemCollection
{
    /// <summary>
    /// Intern class use for reading in json file.
    /// This class is simply the marshaller for reading the json content of item
    /// It is then, use to fill the real data in the ItemData class
    /// </summary>
    [System.Serializable]
    public class ItemReader
    {
        public string id;
        public string nameI18n;
        public string icon;
        public string model3D;
        public float weight;
        public string descriptionI18n;
        public bool isStackable;
        public int durability;
        public string[] actionsID;
    }

    /// <summary>
    /// Intern class use to represent every item in the game.
    /// This class is use when we want to have general information about an item
    /// We simply use the id and we can access attributes of this item.
    /// </summary>
    public class ItemData
    {
        public string Id;
        public string NameI18n;
        public Sprite Icon;
        public GameObject Model3D;
        public float Weight;
        public string DescriptionI18n;
        public bool IsStackable;
        public int Durability;
        public string[] ActionsID;

        /// <summary>
        /// Constructor for ItemData, it simply take an ItemReader
        /// which is a format compatible with JSON and turn it into an object that can 
        /// be use in the game.
        /// </summary>
        /// <param name="reader">The compatible json class that we want to transform</param>
        public ItemData(ItemReader reader)
        {
            Id = reader.id;
            NameI18n = reader.nameI18n;
            Icon = Resources.Load<Sprite>("Icons/" + reader.icon);
            Model3D = (reader.model3D == "null") ? (null) : (Resources.Load(reader.model3D) as GameObject);            
            Weight = reader.weight;
            DescriptionI18n = reader.descriptionI18n;
            IsStackable = reader.isStackable;
            Durability = reader.durability;
            ActionsID = reader.actionsID;
        }
    }

    private static Dictionary<string, ItemData> collection = null;

    /// <summary>
    /// This public method allow to load the items and to encapsulate them in a collection which
    /// is here a dictionnary with a pair-value as ID/ItemData
    /// </summary>
    /// <param name="filenameRawResourcesData">the filename of the json file containing items</param>
    public static void Load(string filenameRawResourcesData)
    {
        if (collection == null)
        {
            collection = new Dictionary<string, ItemData>();
            string filepath = filenameRawResourcesData.Replace(".json", "");
            TextAsset textAsset = Resources.Load<TextAsset>(filepath);
            if (textAsset != null)
            {
                string jsonContent = textAsset.text;
                ItemReader[] resources = JsonHelper.FromJson<ItemReader>(jsonContent);
                foreach (ItemReader r in resources)
                {
                    ItemData data = new ItemData(r);
                    collection.Add(r.id, data);
                }
            }
            else
            {
                Debug.LogError("The file : " + filepath + " does not exist");
            }
        }
    }

    /// <summary>
    /// This public method simply tell if item with the id given in
    /// parameter exist in the collection
    /// </summary>
    /// <param name="id">the item's id that we are looking for</param>
    /// <returns>true if the item exist, false otherwise</returns>
    public static bool Exists(string id)
    {
        return collection.ContainsKey(id);
    }

    /// <summary>
    /// This public method can be call to retrieve an information about an item
    /// according to his id. This method is the only access to those information and should
    /// be call everytime information is needed.
    /// </summary>
    /// <param name="id">the item's id that we are looking for</param>
    /// <returns>The ItemData if it exist, null otherwise</returns>
    public static ItemData GetDataFromID(string id)
    {
        if (Exists(id))
        {
            return collection[id];
        }
        else
        {
            Debug.LogError("No raw resource present in collection with id : " + id);
            return null;
        }
    }
}
