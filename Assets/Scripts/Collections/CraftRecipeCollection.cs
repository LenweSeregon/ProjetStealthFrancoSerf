using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the collection for all recipe in the game.
/// It mean that if we need to access informations about a recipe,
/// we can request GetDataFromID in this class to get a unique instance that give
/// all informations. It mean that classes don't need to have all informations as attribute
/// but just a ID redirecting to this recipe.
/// </summary>
public static class CraftRecipeCollection
{
    /// <summary>
    /// Intern class use for reading in json file.
    /// This class is simply the marshaller for reading the json content ingredient for 
    /// each recipe. It is then, use to fill the real data in the CraftRecipeIngredientData
    /// class
    /// </summary>
    [System.Serializable]
    public class CraftRecipeIngredientReader
    {
        public string idItemIngredient;
        public int quantity;
    }

    /// <summary>
    /// Intern class use for reading in json file.
    /// This class is simply the marshaller for reading the json content for each recipe. 
    /// It is then, use to fill the real data in the CraftRecipeData class
    /// </summary>
    [System.Serializable]
    public class CraftRecipeReader
    {
        public string id;
        public string idCraftedItem;
        public string category;
        public int makingTimeAsSecond;
        public int levelRequired;
        public bool craftingTableRequired;
        public CraftRecipeIngredientReader[] ingredients;
    }

    /// <summary>
    /// Intern class use to represent recipe's ingredients in the game.
    /// This class is simply encapsulated in recipe data to know the ingredients
    /// that are needed to create the recipe
    /// </summary>
    public class CraftRecipeIngredientData
    {
        public string ItemID;
        public int Quantity;

        public CraftRecipeIngredientData(CraftRecipeIngredientReader ingredient)
        {
            ItemID = ingredient.idItemIngredient;
            Quantity = ingredient.quantity;
        }
    }

    /// <summary>
    /// Intern class use to representrecipe in the game.
    /// This class is containing all informations about a specific recipe
    /// and can be accessed through a dictionnary.
    /// </summary>
    public class CraftRecipeData
    {
        public string Id;
        public string IdCraftedItem;
        public string Category;
        public int MakingTimeAsSecond;
        public int LevelRequired;
        public bool CraftingTableRequired;
        public CraftRecipeIngredientData[] Ingredients;

        public CraftRecipeData(CraftRecipeReader recipe)
        {
            Id = recipe.id;
            IdCraftedItem = recipe.idCraftedItem;
            Category = recipe.category;
            MakingTimeAsSecond = recipe.makingTimeAsSecond;
            LevelRequired = recipe.levelRequired;
            CraftingTableRequired = recipe.craftingTableRequired;

            Ingredients = new CraftRecipeIngredientData[recipe.ingredients.Length];
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                Ingredients[i] = new CraftRecipeIngredientData(recipe.ingredients[i]);
            }
        }
    }

    private static Dictionary<string, CraftRecipeData> collection = null;

    /// <summary>
    /// This public method allow to load the recipes and to encapsulate them in a collection which
    /// is here a dictionnary with a pair-value as ID/CraftRecipeData
    /// </summary>
    /// <param name="filenameDropProbability">the filename of the json file contains recipes</param>
    public static void Load(string filenameDropProbability)
    {
        if (collection == null)
        {
            collection = new Dictionary<string, CraftRecipeData>();
            string filepath = filenameDropProbability.Replace(".json", "");
            TextAsset textAsset = Resources.Load<TextAsset>(filenameDropProbability);
            if (textAsset != null)
            {
                string jsonContent = textAsset.text;
                CraftRecipeReader[] resources = JsonHelper.FromJson<CraftRecipeReader>(jsonContent);

                foreach (CraftRecipeReader r in resources)
                {
                    CraftRecipeData data = new CraftRecipeData(r);
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
    /// This public method simply tell if the recipe with the id given in
    /// parameter exist in the collection
    /// </summary>
    /// <param name="id">the recipe's id that we are looking for</param>
    /// <returns>true if the recipe exist, false otherwise</returns>
    public static bool Exists(string id)
    {
        return collection.ContainsKey(id);
    }

    /// <summary>
    /// This public method allow the user to get a list of all category in the recipe collection.
    /// To do so, it simply iterate on the dictionnary and add if the category identifier is not already
    /// in the list.
    /// </summary>
    /// <returns>a list of unique category by I18n representation</returns>
    public static List<string> GetAllI18nRecipeCategories()
    {
        List<string> categories = new List<string>();
        
        foreach (KeyValuePair<string, CraftRecipeData> entry in collection)
        {
            if(!categories.Contains(entry.Value.Category))
            {
                categories.Add(entry.Value.Category);
            }
        }

        categories.Sort();
        return categories;
    }

    /// <summary>
    /// This public method allow the user to get a list of recipe id in the collection.
    /// To do so, it simply iterate on the dictionnary and add the key of each entry
    /// in the list.
    /// </summary>
    /// <returns>a list of all recipe's id</returns>
    public static List<string> GetAllRecipeID()
    {
        List<string> recipesID = new List<string>();

        foreach (KeyValuePair<string, CraftRecipeData> entry in collection)
        {
            recipesID.Add(entry.Key);
        }

        return recipesID;
    }

    /// <summary>
    /// This public method allow the user to get a list of recipe id in the collection if they are.
    /// in the category given in parameter. To do so, it simply iterate on the dictionnary and add the 
    /// key of each entry in the list.
    /// </summary>
    /// <param name="category">the category we want our objects</param>
    /// <returns>a list of all recipe's id if they are in the category argument</returns>
    public static List<string> GetAllRecipeIDByCategory(string category)
    {
        List<string> recipesIDByCategory = new List<string>();

        foreach (KeyValuePair<string, CraftRecipeData> entry in collection)
        {
            if(entry.Value.Category == category)
            {
                recipesIDByCategory.Add(entry.Value.Id);
            }
        }

        return recipesIDByCategory;
    }

    /// <summary>
    /// This public method can be call to retrieve an information about a resource point
    /// according to his id. This method is the only access to those information and should
    /// be call everytime information is needed.
    /// </summary>
    /// <param name="id">the resource point's id that we are looking for</param>
    /// <returns>The ResourcePointDropProbabilityData if it exist, null otherwise</returns>
    public static CraftRecipeData GetDataFromID(string id)
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
