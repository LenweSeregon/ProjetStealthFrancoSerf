using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryBuilder : MonoBehaviour {

    public AInventory inventory;

    public GameObject actionPanel;
    public GameObject actionItemPrefab;
    public GameObject informationPanel;
    public GameObject containerInventorySlot;
    public GameObject prefabInventorySlot;

    public TextMeshProUGUI nameInfo;
    public TextMeshProUGUI weightInfo;
    public TextMeshProUGUI otherInfo;
    public TextMeshProUGUI descriptionInfo;

    [HideInInspector]
    public bool isDragging;
    [HideInInspector]
    public InventorySlotUI currentlyDragging;
    [HideInInspector]
    public InventorySlotUI endDragging;

    void OnEnable()
    {
        actionPanel.SetActive(false);
        isDragging = false;
        currentlyDragging = null;
        endDragging = null;
        if(inventory != null)
        {
            informationPanel.SetActive(false);
            BuildInventory();
        }
    }

    /// <summary>
    ///  ATTENTION ICI childs -2 => CAR ON NE VEUT PAS EFFACER LE ACTION PANEL qui se trouve toujours
    ///  en dernière position dans les enfants
    /// </summary>
    public void BuildInventory()
    {
        int childs = containerInventorySlot.transform.childCount;
        for (int i = childs - 2; i >= 0; i--)
        {
            GameObject.Destroy(containerInventorySlot.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            GameObject slot = Instantiate(prefabInventorySlot);
            slot.transform.SetParent(containerInventorySlot.transform, false);
            slot.GetComponent<InventorySlotUI>().index = i;
            slot.GetComponent<InventorySlotUI>().slotAssociated = inventory.Slots[i];
            slot.GetComponent<InventorySlotUI>().builderReference = this;
            slot.GetComponent<InventorySlotUI>().Build();
        }

        actionPanel.transform.SetAsLastSibling();
    }

    public void EndDraggingOperation()
    {
        if (isDragging && currentlyDragging != null && endDragging != null)
        {
            inventory.Swap(currentlyDragging.index, endDragging.index);
            BuildInventory();
        }
    }

    public void InventorySlotClicked(InventorySlotUI slot)
    {
        informationPanel.SetActive(true);
        nameInfo.text = I18nManager.Fields[slot.slotAssociated.Item.GetI18nNameIdentifier()];
        weightInfo.text = slot.slotAssociated.Item.GetWeight().ToString();
        descriptionInfo.text = I18nManager.Fields[slot.slotAssociated.Item.GetI18nDescriptionIdentifier()];

    }

    public void InventorySlotActionClicked(InventorySlotUI slot)
    {
        int childs = actionPanel.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(actionPanel.transform.GetChild(i).gameObject);
        }

        foreach (string action in slot.slotAssociated.Item.GetActionsID())
        {
            GameObject actionItem = Instantiate(actionItemPrefab);
            actionItem.transform.SetParent(actionPanel.transform, false);
            actionItem.GetComponent<InventorySlotActionUI>().slotAssociated = slot.slotAssociated;
            actionItem.GetComponent<InventorySlotActionUI>().index = slot.index;
            actionItem.GetComponent<InventorySlotActionUI>().action = ItemActionCollection.Actions[action];
            actionItem.GetComponent<InventorySlotActionUI>().inventoryAssociated = inventory;
            actionItem.GetComponent<InventorySlotActionUI>().builderReference = this;
            actionItem.GetComponent<InventorySlotActionUI>().Build();
        }
        HideActionPanel();
        actionPanel.SetActive(true);
        actionPanel.transform.position = slot.transform.position;
    }

    public void HideActionPanel()
    {
        actionPanel.SetActive(false);
    }
}
