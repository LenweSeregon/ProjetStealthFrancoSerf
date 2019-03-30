using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientItemUI : MonoBehaviour {

    [HideInInspector]
    public int quantityRequired;
    [HideInInspector]
    public int quantityOwned;
    [HideInInspector]
    public ItemCollection.ItemData storableAssociated;

    public Image icon;
    public TextMeshProUGUI quantity;

    void Awake()
    {

    }

    public void Build()
    {
        icon.sprite = storableAssociated.Icon;
        quantity.text = quantityOwned + " / " + quantityRequired;
        if(quantityOwned < quantityRequired)
        {
            quantity.color = Color.red;
        }
    }
}
