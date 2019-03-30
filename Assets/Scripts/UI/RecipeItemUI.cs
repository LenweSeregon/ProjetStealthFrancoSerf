using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeItemUI : MonoBehaviour {

    [HideInInspector]
    public FactoryBuilder factoryBuilderReference;
    public TextMeshProUGUI recipeName;
    public Button button;
    public Image craftTableNeeded;

    private string recipeID;
    public string RecipeID
    {
        get { return recipeID; }
        set
        {
            recipeID = value;
            var idRecipeItem = CraftRecipeCollection.GetDataFromID(recipeID).IdCraftedItem;
            var recipeItemI18nName = ItemCollection.GetDataFromID(idRecipeItem).NameI18n;
            string idRecipeItemName = I18nManager.Fields[recipeItemI18nName];

            recipeName.text = idRecipeItemName;
        }
    }

    public void SetIsAccessibleWithoutCraftingTable(bool craftTableOpen)
    {
        craftTableNeeded.gameObject.SetActive(false);
        if(!CraftRecipeCollection.GetDataFromID(recipeID).CraftingTableRequired)
        {
            button.interactable = true;
        }
        else
        {
            if(!craftTableOpen)
            {
                craftTableNeeded.gameObject.SetActive(true);
            }
            button.interactable = craftTableOpen;
        }
    }

    public void OnClick()
    {
        factoryBuilderReference.RecipeItemClicked(recipeID);
    }
}
