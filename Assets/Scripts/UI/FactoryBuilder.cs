using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactoryBuilder : MonoBehaviour {

    public static bool IsCraftTableOpen;

    private int quantityMaxCraftPossible;
    private bool hasRequestedStopCrafting;
    private bool isCrafting;
    private CraftRecipeCollection.CraftRecipeData recipeClickedReference;
    private string savedCraftAllTextI18n;
    private bool hasStartedSearch;
    private List<bool> headerWasOpened;
    private List<GameObject> headers;

    public Player player;

    public AudioSource hammerHitAnvilSound;
    public Image imageCraftedItem;
    public TextMeshProUGUI nameCraftedItem;
    public TextMeshProUGUI craftAllText;
    public TextMeshProUGUI quantityToCraft;
    public GameObject craftAnimationPanel;
    public Image craftAnimationHammer;

    public GameObject recipeListContainer;
    public GameObject ingredientsListContainer;

    public GameObject prefabRecipeListHeader;
    public GameObject prefabRecipeListItem;
    public GameObject prefabIngredientItem;

    public Button craftButton;
    public Button craftAllButton;
    public Button cancelButton;
    public Slider progressBar;


    private void Awake()
    {
        savedCraftAllTextI18n = craftAllText.GetComponent<TextUI>().textID;
    }

    private void OnEnable()
    {
        quantityMaxCraftPossible = 0;
        recipeClickedReference = null;
        hasStartedSearch = false;
        headers = new List<GameObject>();
        imageCraftedItem.sprite = null;
        nameCraftedItem.text = "";
        craftAllText.text = I18nManager.Fields[savedCraftAllTextI18n];
        IsNotCrafting();
        BuildRecipeList();
        EmptyIngredientList();

    }

    private void IsCrafting()
    {
        player.isCrafting = true;
        player.GetComponent<PlayerMotor>().Stop();
        isCrafting = true;
        progressBar.gameObject.SetActive(true);
        craftAnimationPanel.SetActive(true);
        craftButton.interactable = false;
        craftAllButton.interactable = false;
        quantityToCraft.gameObject.SetActive(true);
    }
    private void IsNotCrafting()
    {
        isCrafting = false;
        craftAnimationHammer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        progressBar.value = 0;
        progressBar.gameObject.SetActive(false);
        craftAnimationPanel.SetActive(false);
        craftButton.interactable = true;
        craftAllButton.interactable = true;
        cancelButton.gameObject.SetActive(false);
        quantityToCraft.gameObject.SetActive(false);
        quantityToCraft.text = "0";
        if(recipeClickedReference != null)
        {
            BuildIngredientsList(recipeClickedReference);
        }
        player.isCrafting = false;
    }
    private void RemoveIngredientsAndAddNewItem(CraftRecipeCollection.CraftRecipeData toCraft)
    {
        string idItemCreated = toCraft.IdCraftedItem;
        foreach (var ingredient in toCraft.Ingredients)
        {
            string ingredientID = ingredient.ItemID;
            int quantity = ingredient.Quantity;

            player.Inventory.RemoveQuantityFromInventory(ingredientID, quantity);
        }
        player.Inventory.AddElementToInventory(idItemCreated, 1);
        BuildIngredientsList(recipeClickedReference);
    }
    private IEnumerator AnimationCraftingHammer()
    {
        float rotation = -0.5f;
        float rotationCounter = 0;
        craftAnimationHammer.transform.rotation = Quaternion.identity;

        while (true)
        {
            craftAnimationHammer.transform.Rotate(0, 0, rotation);
            rotationCounter += rotation;
            if (rotationCounter <= -30)
            {
                hammerHitAnvilSound.Play();
                rotation = -rotation;
            }
            else if (rotationCounter >= 0)
            {
                rotation = -rotation;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator CraftRecipe(CraftRecipeCollection.CraftRecipeData toCraft, int nbItem)
    {
        IsCrafting();
        quantityToCraft.text = nbItem.ToString();
        float craftTimePerItemAsSecond = toCraft.MakingTimeAsSecond;

        for(int i = 0; i < nbItem; i++)
        {
            float currentSecond = 0;
            IEnumerator animationHammer = AnimationCraftingHammer();
            StartCoroutine(animationHammer);
            while (currentSecond < craftTimePerItemAsSecond)
            {
                currentSecond += Time.deltaTime;
                progressBar.value = currentSecond / craftTimePerItemAsSecond;
                yield return new WaitForEndOfFrame();
            }
            RemoveIngredientsAndAddNewItem(toCraft);
            StopCoroutine(animationHammer);
            quantityToCraft.text = (int.Parse(quantityToCraft.text) - 1).ToString();

            if (hasRequestedStopCrafting)
            {
                hasRequestedStopCrafting = false;
                break;
            }
        }

        IsNotCrafting();
        yield return null;
    }

    public void CraftButtonClicked()
    {
        if(recipeClickedReference != null && !player.isBuilding && !player.GetComponent<PlayerController>().isHarvesting)
        {
            cancelButton.gameObject.SetActive(false);
            cancelButton.interactable = false;

            StartCoroutine(CraftRecipe(recipeClickedReference, 1));
        }
    }
    public void CraftAllButtonClicked()
    {
        if(recipeClickedReference != null && !player.isBuilding && !player.GetComponent< PlayerController>().isHarvesting)
        {
            if(quantityMaxCraftPossible >= 1)
            {
                cancelButton.gameObject.SetActive(true);
                cancelButton.interactable = true;
            }
            else
            {
                cancelButton.gameObject.SetActive(false);
                cancelButton.interactable = false;
            }
            
            StartCoroutine(CraftRecipe(recipeClickedReference, quantityMaxCraftPossible));
        }
    }
    public void CancelCraftingList()
    {
        if(!hasRequestedStopCrafting)
        {
            hasRequestedStopCrafting = true;
            quantityToCraft.text = "1";
        }
    }

    public void RecipeItemClicked(string idRecipe)
    {
        
        var recipeClicked = CraftRecipeCollection.GetDataFromID(idRecipe);
        var recipeItemCrafted = ItemCollection.GetDataFromID(recipeClicked.IdCraftedItem);
        recipeClickedReference = recipeClicked;

        imageCraftedItem.sprite = recipeItemCrafted.Icon;
        nameCraftedItem.text = I18nManager.Fields[recipeItemCrafted.NameI18n];

        BuildIngredientsList(recipeClicked);
    }

    public void SaveHeaderState()
    {
        headerWasOpened = new List<bool>();
        foreach(GameObject goHeader in headers)
        {
            headerWasOpened.Add(goHeader.GetComponent<RecipeHeaderUI>().isOpen);
        }
    }
    public void RestaureHeaderState()
    {
        for(int i = 0; i < headerWasOpened.Count; i++)
        {
            headers[i].SetActive(true);
            if(headerWasOpened[i])
            {
                headers[i].GetComponent<RecipeHeaderUI>().TurnOnOpenIcon();
                headers[i].GetComponent<RecipeHeaderUI>().ShowAllChildren();
            }
            else
            {
                headers[i].GetComponent<RecipeHeaderUI>().TurnOffOpenIcon();
                headers[i].GetComponent<RecipeHeaderUI>().HideAllChildren();
            }
        }
    }

    // REFACTORISER LE CCODE
    public void SearchToRecipeRegex(string regex)
    {
        // Prevent empty (or only whitespaces)
        if(regex.Trim().Length == 0 && !hasStartedSearch)
        {
            return;
        }

        if (regex == "")
        {
            RestaureHeaderState();
            hasStartedSearch = false;
        }
        else
        {
            if(!hasStartedSearch)
            {
                SaveHeaderState();
                hasStartedSearch = true;
            }
            
            foreach (GameObject goHeader in headers)
            {
                RecipeHeaderUI header = goHeader.GetComponent<RecipeHeaderUI>();
                header.HideAllChildren();
                List<int> matchIndexes = header.ExistInChild(regex);
                if (matchIndexes.Count > 0)
                {
                    header.TurnOnOpenIcon();
                    header.gameObject.SetActive(true);
                    for (int i = 0; i < matchIndexes.Count; i++)
                    {
                        header.ShowChild(matchIndexes[i]);
                    }
                }
                else
                {
                    header.gameObject.SetActive(false);
                }
            }
        }
    }

    private void BuildRecipeList()
    {
        int childs = recipeListContainer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(recipeListContainer.transform.GetChild(i).gameObject);
        }

        foreach (string categoryI18n in CraftRecipeCollection.GetAllI18nRecipeCategories())
        {
            GameObject recipeListHeader = Instantiate(prefabRecipeListHeader);
            recipeListHeader.transform.SetParent(recipeListContainer.transform, false);
            recipeListHeader.GetComponent<RecipeHeaderUI>().CategoryI18nName = categoryI18n;

            foreach (string id in CraftRecipeCollection.GetAllRecipeIDByCategory(categoryI18n))
            {
                GameObject recipeListItem = Instantiate(prefabRecipeListItem);
                recipeListItem.transform.SetParent(recipeListContainer.transform, false);
                recipeListItem.GetComponent<RecipeItemUI>().RecipeID = id;
                recipeListItem.GetComponent<RecipeItemUI>().factoryBuilderReference = this;
                recipeListItem.GetComponent<RecipeItemUI>().SetIsAccessibleWithoutCraftingTable(IsCraftTableOpen);
                recipeListItem.SetActive(false);

                recipeListHeader.GetComponent<RecipeHeaderUI>().childItems.Add(recipeListItem);
            }
            headers.Add(recipeListHeader);
        }
    }
    private void BuildIngredientsList(CraftRecipeCollection.CraftRecipeData recipe)
    {
        EmptyIngredientList();

        int craftQuantityMax = int.MaxValue;
        bool enoughAllIngredients = true;
        foreach(var ingredient in recipe.Ingredients)
        {
            var itemIngredientAssocited = ItemCollection.GetDataFromID(ingredient.ItemID);
            int quantityOwned = player.Inventory.GetQuantityOfItem(itemIngredientAssocited.Id);
            GameObject goIngredient = Instantiate(prefabIngredientItem);
            goIngredient.transform.SetParent(ingredientsListContainer.transform, false);
            goIngredient.GetComponent<IngredientItemUI>().storableAssociated = itemIngredientAssocited;
            goIngredient.GetComponent<IngredientItemUI>().quantityRequired = ingredient.Quantity;
            goIngredient.GetComponent<IngredientItemUI>().quantityOwned = quantityOwned;
            goIngredient.GetComponent<IngredientItemUI>().Build();
            if(quantityOwned < ingredient.Quantity)
            {
                enoughAllIngredients = false;
            }
            craftQuantityMax = Mathf.Min(craftQuantityMax, quantityOwned / ingredient.Quantity);
        }

        if(enoughAllIngredients)
        {
            craftAllText.text = I18nManager.Fields[savedCraftAllTextI18n] + " ("+craftQuantityMax+")";
        }
        else
        {
            craftAllText.text = I18nManager.Fields[savedCraftAllTextI18n];
        }

        quantityMaxCraftPossible = craftQuantityMax;

        if (!isCrafting)
        {
            if(!recipe.CraftingTableRequired)
            {
                craftAllButton.interactable = enoughAllIngredients;
                craftButton.interactable = enoughAllIngredients;
            }
            else
            {
                craftAllButton.interactable = enoughAllIngredients && IsCraftTableOpen;
                craftButton.interactable = enoughAllIngredients && IsCraftTableOpen;
            }

        }
        else
        {
            craftAllButton.interactable = false;
            craftButton.interactable = false;
        }

    }
    private void EmptyIngredientList()
    {
        int childs = ingredientsListContainer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(ingredientsListContainer.transform.GetChild(i).gameObject);
        }
    }
}
