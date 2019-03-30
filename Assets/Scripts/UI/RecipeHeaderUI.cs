using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeHeaderUI : MonoBehaviour
{
    private bool isSliding;

    [HideInInspector]
    public bool isOpen;
    [HideInInspector]
    public List<GameObject> childItems;

    private string categoryI18nName;
    public string CategoryI18nName
    {
        get { return categoryI18nName; }
        set
        {
            categoryI18nName = value;
            categoryName.text = I18nManager.Fields[categoryI18nName];
        }
    }

    public Image image;
    public TextMeshProUGUI categoryName;
    
	void Awake () {
        isSliding = false;
        childItems = new List<GameObject>();
        isOpen = false;
        image.transform.localRotation = Quaternion.Euler(image.transform.localRotation.x, image.transform.localRotation.y, 180);
    }

    public void OnClick()
    {
        isOpen = !isOpen;
        UpdateListCategoryItemAndIcon();
    }

    public List<int> ExistInChild(string regex)
    {
        List<int> match = new List<int>();
        int indexChild = 0;
        foreach(GameObject child in childItems)
        {
            RecipeItemUI item = child.GetComponent<RecipeItemUI>();
            string idItemAssociated = CraftRecipeCollection.GetDataFromID(item.RecipeID).IdCraftedItem;
            string nameI18nItemAssociated = ItemCollection.GetDataFromID(idItemAssociated).NameI18n;
            string name = I18nManager.Fields[nameI18nItemAssociated];

            if (name.ToLower().Contains(regex.ToLower()))
            {
                match.Add(indexChild);
            }
            indexChild++;
        }

        return match;
    }

    public void HideAllChildren()
    {
        foreach (GameObject item in childItems)
        {
            item.SetActive(false);
        }
    }

    public void ShowAllChildren()
    {
        foreach (GameObject item in childItems)
        {
            item.SetActive(true);
        }
    }

    public void ShowChild(int index)
    {
        childItems[index].SetActive(true);
    }

    public void TurnOnOpenIcon()
    {
        image.transform.localRotation = Quaternion.Euler(image.transform.localRotation.x, image.transform.localRotation.y, 90);
    }

    public void TurnOffOpenIcon()
    {
        image.transform.localRotation = Quaternion.Euler(image.transform.localRotation.x, image.transform.localRotation.y, 180);
    }

    public void UpdateListCategoryItemAndIcon()
    {
        if(!isSliding)
        {
            if (isOpen)
            {
                TurnOnOpenIcon();
                StartCoroutine(SlideItems(true));
            }
            else
            {
                TurnOffOpenIcon();
                StartCoroutine(SlideItems(false));
            }
        }
    }

    private IEnumerator SlideItems(bool slidingUp)
    {
        isSliding = true;
        foreach(GameObject item in childItems)
        {
            item.SetActive(slidingUp);
            yield return new WaitForEndOfFrame();
        }
        isSliding = false;
    }


}
