using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySlotActionUI : MonoBehaviour {

    [HideInInspector]
    public AInventory inventoryAssociated;
    [HideInInspector]
    public int index;
    [HideInInspector]
    public InventoryBuilder builderReference;
    [HideInInspector]
    public InventorySlot slotAssociated;
    [HideInInspector]
    public IItemAction action;

    public TextMeshProUGUI actionText;

    public void Build()
    {
        actionText.text = action.GetI18nName();
    }

    public void OnActionClicked()
    {
        action.SlotAssociated = this;
        action.inventoryBuilder = builderReference;
        action.TriggerAction();
        builderReference.HideActionPanel();
        builderReference.BuildInventory();
    }
}
