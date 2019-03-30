using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This class is the collection for all resource point in the game.
/// It mean that if we need to access informations about a resource point,
/// we can request GetDataFromID in this class to get a unique instance that give
/// all informations. It mean that classes don't need to have all informations as attribute
/// but just a ID redirecting to this resource.
/// </summary>
public static class ResourcePointDropProbabilityCollection
{
    /// <summary>
    /// Intern class use for reading in json file.
    /// This class is simply the marshaller for reading the json content probability for 
    /// each resource point. It is then, use to fill the real data in the ResourcePointProbabilityData
    /// class
    /// </summary>
    [Serializable]
    public class ResourcePointProbabilityReader
    {
        public string idRawResource;
        public float probability;
        public int quantityMin;
        public int quantityMax;
    }

    /// <summary>
    /// Intern class use for reading in json file.
    /// This class is simply the marshaller for reading the json content for each resource point. 
    /// It is then, use to fill the real data in the ResourcePointDropProbabilityData class
    /// </summary>
    [Serializable]
    public class ResourcePointDropProbabilityReader
    {
        public string id;
        public string name;
        public string description;
        public int respawnTime;
        public int durabilityConsumption;
        public int harvestTime;
        public string eventName;
        public ResourcePointProbabilityReader[] probabilities;
    }

    /// <summary>
    /// Intern class use to represent resource point probability in the game.
    /// This class is simply encapsulated in resource point data to know the probability
    /// of drop for each raw resoure in the resource point.
    /// </summary>
    public class ResourcePointProbabilityData
    {
        public string IdRawResource;
        public float Probability;
        public int QuantityMin;
        public int QuantityMax;

        /// <summary>
        /// Constructor for ResourcePointProbabilityData which transform a json compatible format
        /// for drop probabilities in resource point into a usable class to retrieve information.
        /// </summary>
        /// <param name="rpProbabilityReader">The probability for resource point in json compatible format</param>
        public ResourcePointProbabilityData(ResourcePointProbabilityReader rpProbabilityReader)
        {
            IdRawResource = rpProbabilityReader.idRawResource;
            Probability = rpProbabilityReader.probability;
            QuantityMin = rpProbabilityReader.quantityMin;
            QuantityMax = rpProbabilityReader.quantityMax;
        }
    }

    /// <summary>
    /// Intern class use to represent resource point in the game.
    /// This class is containing all informations about a specific resource point
    /// and can be accessed through a dictionnary.
    /// </summary>
    public class ResourcePointDropProbabilityData
    {
        public string Id;
        public string Name;
        public string Description;
        public int RespawnTime;
        public int DurabilityConsumption;
        public int HarvestTime;
        public string EventName;
        public ResourcePointProbabilityData[] Probabilities;

        /// <summary>
        /// Constructor for ResourcePointDropProbabilityData, it simply take a 
        /// ResourcePointDropProbabilityReader which is a format compatible with JSON and
        /// turn it into an object that can be use in the game.
        /// </summary>
        /// <param name="rppd">The compatible json class that we want to transform</param>
        public ResourcePointDropProbabilityData(ResourcePointDropProbabilityReader rppd)
        {
            Id = rppd.id;
            Name = rppd.name;
            Description = rppd.description;
            RespawnTime = rppd.respawnTime;
            DurabilityConsumption = rppd.durabilityConsumption;
            HarvestTime = rppd.harvestTime;
            EventName = rppd.eventName;

            Probabilities = new ResourcePointProbabilityData[rppd.probabilities.Length];
            for(int i = 0; i < rppd.probabilities.Length; i++)
            {
                Probabilities[i] = new ResourcePointProbabilityData(rppd.probabilities[i]);
            }
        }
    }

    private static Dictionary<string, ResourcePointDropProbabilityData> collection = null;

    /// <summary>
    /// This public method allow to load the resource and to encapsulate them in a collection which
    /// is here a dictionnary with a pair-value as ID/ResourcePointDropProbabilityData
    /// </summary>
    /// <param name="filenameDropProbability">the filename of the json file contains resource point</param>
    public static void Load(string filenameDropProbability)
    {
        if(collection == null)
        {
            collection = new Dictionary<string, ResourcePointDropProbabilityData>();
            string filepath = filenameDropProbability.Replace(".json", "");
            TextAsset textAsset = Resources.Load<TextAsset>(filenameDropProbability);
            if (textAsset != null)
            {
                string jsonContent = textAsset.text;
                ResourcePointDropProbabilityReader[] resources = JsonHelper.FromJson<ResourcePointDropProbabilityReader>(jsonContent);

                foreach (ResourcePointDropProbabilityReader r in resources)
                {
                    ResourcePointDropProbabilityData data = new ResourcePointDropProbabilityData(r);
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
    /// This public method simply tell if the resource point with the id given in
    /// parameter exist in the collection
    /// </summary>
    /// <param name="id">the resource point's id that we are looking for</param>
    /// <returns>true if the resource point exist, false otherwise</returns>
    public static bool Exists(string id)
    {
        return collection.ContainsKey(id);
    }

    /// <summary>
    /// This public method can be call to retrieve an information about a resource point
    /// according to his id. This method is the only access to those information and should
    /// be call everytime information is needed.
    /// </summary>
    /// <param name="id">the resource point's id that we are looking for</param>
    /// <returns>The ResourcePointDropProbabilityData if it exist, null otherwise</returns>
    public static ResourcePointDropProbabilityData GetDataFromID(string id)
    {
        if (Exists(id))
        {
            return collection[id];
        }
        else
        {
            Debug.LogError("No resource point present in collection with id : " + id);
            return null;
        }
    }

}
