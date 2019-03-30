using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDualBuilder : MonoBehaviour
{
    public GameObject prefabInventoryDualSlot;
    public GameObject containerInventorySlotPlayer;
    public GameObject containerInventorySlotOther;

    [HideInInspector]
    public bool isDragging;
    [HideInInspector]
    public InventoryDualSlotUI currentlyDragging;
    [HideInInspector]
    public InventoryDualSlotUI endDragging;

    public AInventory inventoryPlayer;
    [HideInInspector]
    public static AInventory inventoryOther;

    void OnEnable()
    {
        isDragging = false;
        currentlyDragging = null;
        endDragging = null;
        BuildInventories();
    }

    private void BuildInventories()
    {
        BuildPlayerInventory();
        BuildOtherInventory();
    }

    private void BuildPlayerInventory()
    {
        BuildInventory(containerInventorySlotPlayer, inventoryPlayer, 1);
    }
    private void BuildOtherInventory()
    {
        BuildInventory(containerInventorySlotOther, inventoryOther, 2);
    }
    private void BuildInventory(GameObject container, AInventory inventory, int containerAssociatedIndex)
    {
        int childs = container.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(container.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            GameObject slot = Instantiate(prefabInventoryDualSlot);
            slot.transform.SetParent(container.transform, false);
            slot.GetComponent<InventoryDualSlotUI>().index = i;
            slot.GetComponent<InventoryDualSlotUI>().containerAssociated = containerAssociatedIndex;
            slot.GetComponent<InventoryDualSlotUI>().slotAssociated = inventory.Slots[i];
            slot.GetComponent<InventoryDualSlotUI>().builderReference = this;
            slot.GetComponent<InventoryDualSlotUI>().Build();
        }
    }

    public void DoubleClicked(InventoryDualSlotUI slot)
    {
        InventorySlot slotLogic = slot.slotAssociated;

        if (slot.containerAssociated == 1)
        {
            int quantityRemaining = inventoryOther.AddElementToInventory(slotLogic.Item.GetID(), slotLogic.Quantity, slotLogic.Item.CurrentDurability);
            if(quantityRemaining > 0)
            {
                inventoryPlayer.Slots[slot.index].Quantity = quantityRemaining;
            }
            else
            {
                inventoryPlayer.RemoveSlotFromInventory(slot.index);
            }
        }
        else
        {
            int quantityRemaining = inventoryPlayer.AddElementToInventory(slotLogic.Item.GetID(), slotLogic.Quantity, slotLogic.Item.CurrentDurability);
            if(quantityRemaining > 0)
            {
                inventoryOther.Slots[slot.index].Quantity = quantityRemaining;
            }
            else
            {
                inventoryOther.RemoveSlotFromInventory(slot.index);
            }
        }

        BuildInventories();
    }
    public void EndDraggingOperation()
    {
        if(isDragging && currentlyDragging != null && endDragging != null)
        {
            if (currentlyDragging.containerAssociated == endDragging.containerAssociated)
            {
                if(currentlyDragging.containerAssociated == 1)
                {
                    inventoryPlayer.Swap(currentlyDragging.index, endDragging.index);
                }
                else
                {
                    inventoryOther.Swap(currentlyDragging.index, endDragging.index);
                }
            }
            else
            {
                if(currentlyDragging.containerAssociated == 1)
                {
                    inventoryPlayer.Swap(inventoryOther, currentlyDragging.index, endDragging.index);
                }
                else
                {
                    //inventoryPlayer.Swap(inventoryOther, endDragging.index, currentlyDragging.index);
                    inventoryOther.Swap(inventoryPlayer, currentlyDragging.index, endDragging.index);
                }
            }

            BuildInventories();
        }
    }
}
