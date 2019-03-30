using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceBuilder : MonoBehaviour {

    [HideInInspector]
    public bool isDragging;
    [HideInInspector]
    public InventoryFurnaceSlotUI currentlyDragging;
    [HideInInspector]
    public InventoryFurnaceSlotUI endDragging;

    public int minTemperature = 0;
    public int maxTemperature = 2000;

    public AInventory inventoryPlayer;
    public GameObject prefabSlotFurnace;
    public GameObject containerInventoryPlayer;
    public Slider temperatureSlider;
    public RawImage onOff;

    public InventoryFurnaceSlotUI combustible;
    public InventoryFurnaceSlotUI toHeat;
    public InventoryFurnaceSlotUI heated;

    [HideInInspector]
    public static Furnace currentFurnace;

    void Awake()
    {
        BuildFurnacePart();
    }

    void OnEnable()
    {
        BuildPlayerInventory();
        UpdateTemperature();
        UpdateFurnace();
    }

    public void BlowButtonClicked()
    {
        currentFurnace.IncreaseTemperature();
    }

    public void UpdateTemperature()
    {
        temperatureSlider.value = ((float)currentFurnace.Temperature / (float)maxTemperature);
    }

    public void FurnaceOn()
    {
        onOff.color = Color.green;
    }

    public void FurnaceOff()
    {
        onOff.color = Color.red;
    }

    public void UpdateFurnace()
    {
        combustible.slotAssociated = currentFurnace.combustible;
        toHeat.slotAssociated = currentFurnace.toHeat;
        heated.slotAssociated = currentFurnace.heated;

        combustible.Build();
        toHeat.Build();
        heated.Build();
    }

    public void EndDraggingOperation()
    {
        if (isDragging && currentlyDragging != null && endDragging != null)
        {
            if(currentlyDragging.IsInventory && endDragging.IsInventory)
            {
                inventoryPlayer.Swap(currentlyDragging.index, endDragging.index);
                BuildPlayerInventory();
            }
            else if(currentlyDragging.IsInventory && endDragging.IsFurnaceCombustible)
            {
                ManageFromInventoryToCombustible();
            }
            else if(currentlyDragging.IsInventory && endDragging.IsFurnaceToHeat)
            {
                ManageFromInventoryToToHeat();
            }
            else if(currentlyDragging.IsFurnaceCombustible && endDragging.IsInventory)
            {
                ManageFromCombustibleToInventory();
            }
            else if(currentlyDragging.IsFurnaceToHeat && endDragging.IsInventory)
            {
                ManageFromToHeatToInventory();
            }
            else if(currentlyDragging.IsFurnaceHeated && endDragging.IsInventory)
            {
                ManageFromHeatedToInventory();
            }
        }
    }
    public void DoubleClicked(InventoryFurnaceSlotUI slot)
    {
        if(slot.IsInventory)
        {
            if(currentFurnace.combustibleAvailableList.Contains(slot.slotAssociated.Item.GetID()))
            {
                if(currentFurnace.combustible == null)
                {
                    inventoryPlayer.RemoveSlotFromInventory(slot.index);
                    currentFurnace.combustible = new InventorySlot();
                    currentFurnace.combustible.Item = ItemGeneralizer.GetItemFromID(slot.slotAssociated.Item.GetID());
                    currentFurnace.combustible.Quantity = slot.slotAssociated.Quantity;
                }
                else if (currentFurnace.combustible.Item.GetID() == slot.slotAssociated.Item.GetID())
                {
                    inventoryPlayer.RemoveSlotFromInventory(slot.index);
                    currentFurnace.combustible.Quantity += slot.slotAssociated.Quantity;
                }

                UpdateFurnace();
                BuildPlayerInventory();
            }
            else if(currentFurnace.toHeatAvailableList.Contains(slot.slotAssociated.Item.GetID()))
            {
                if(currentFurnace.toHeat == null)
                {
                    inventoryPlayer.RemoveSlotFromInventory(slot.index);
                    currentFurnace.toHeat = new InventorySlot();
                    currentFurnace.toHeat.Item = ItemGeneralizer.GetItemFromID(slot.slotAssociated.Item.GetID());
                    currentFurnace.toHeat.Quantity = slot.slotAssociated.Quantity;
                }
                else if(currentFurnace.toHeat.Item.GetID() == slot.slotAssociated.Item.GetID())
                {
                    inventoryPlayer.RemoveSlotFromInventory(slot.index);
                    currentFurnace.toHeat.Quantity += slot.slotAssociated.Quantity;
                }

                UpdateFurnace();
                BuildPlayerInventory();
            }
        }
        else if(slot.IsFurnaceCombustible)
        {
            int quantityRemaining = inventoryPlayer.AddElementToInventory(slot.slotAssociated.Item.GetID(), slot.slotAssociated.Quantity);
            if (quantityRemaining > 0)
            {
                currentFurnace.combustible.Quantity = quantityRemaining;
            }
            else
            {
                currentFurnace.combustible = null;
            }
            
            UpdateFurnace();
            BuildPlayerInventory();
        }
        else if(slot.IsFurnaceToHeat)
        {
            int quantityRemaining = inventoryPlayer.AddElementToInventory(slot.slotAssociated.Item.GetID(), slot.slotAssociated.Quantity);
            if (quantityRemaining > 0)
            {
                currentFurnace.toHeat.Quantity = quantityRemaining;
            }
            else
            {
                currentFurnace.toHeat = null;
            }
            
            UpdateFurnace();
            BuildPlayerInventory();
        }
        else if(slot.IsFurnaceHeated)
        {
            int quantityRemaining = inventoryPlayer.AddElementToInventory(slot.slotAssociated.Item.GetID(), slot.slotAssociated.Quantity);
            if(quantityRemaining > 0)
            {
                currentFurnace.heated.Quantity = quantityRemaining;
            }
            else
            {
                currentFurnace.heated = null;
            }
            
            UpdateFurnace();
            BuildPlayerInventory();
        }
    }

    private void ManageFromCombustibleToInventory()
    {
        ManageFromXToInventory(ref currentFurnace.combustible);
        UpdateFurnace();
        BuildPlayerInventory();
    }
    private void ManageFromToHeatToInventory()
    {
        ManageFromXToInventory(ref currentFurnace.toHeat);
        UpdateFurnace();
        BuildPlayerInventory();
    }
    private void ManageFromHeatedToInventory()
    {
        ManageFromXToInventory(ref currentFurnace.heated);
        UpdateFurnace();
        BuildPlayerInventory();
    }
    private void ManageFromXToInventory(ref InventorySlot slot)
    {
        InventorySlot slotInventory = inventoryPlayer.Slots[endDragging.index];
        if (slotInventory == null)
        {
            if (slot.Quantity > inventoryPlayer.maxQuantityPerSlot)
            {
                inventoryPlayer.AddAtIndex(slot.Item.GetID(), inventoryPlayer.maxQuantityPerSlot, endDragging.index);
                slot.Quantity -= 64;
            }
            else
            {
                inventoryPlayer.AddAtIndex(slot.Item.GetID(), slot.Quantity, endDragging.index);
                slot = null;
            }
        }
        else if (slot.Item.GetID() == slotInventory.Item.GetID())
        {
            int quantityToAdd = slot.Quantity;
            int quantityAlreadyIn = slotInventory.Quantity;
            if (quantityAlreadyIn + quantityToAdd > inventoryPlayer.maxQuantityPerSlot)
            {
                slot.Quantity = ((quantityToAdd + quantityAlreadyIn) - inventoryPlayer.maxQuantityPerSlot);
                inventoryPlayer.Slots[endDragging.index].Quantity = 64;
            }
            else
            {
                slot = null;
                inventoryPlayer.Slots[endDragging.index].Quantity = quantityAlreadyIn + quantityToAdd;
            }
        }
    }

    private void ManageFromInventoryToCombustible()
    {
        ManageFromInventoryToX(ref currentFurnace.combustible, ref combustible, currentFurnace.combustibleAvailableList);
        UpdateFurnace();
        BuildPlayerInventory();
    }
    private void ManageFromInventoryToToHeat()
    {
        ManageFromInventoryToX(ref currentFurnace.toHeat, ref toHeat, currentFurnace.toHeatAvailableList);
        UpdateFurnace();
        BuildPlayerInventory();
    }
    private void ManageFromInventoryToX(ref InventorySlot slot, ref InventoryFurnaceSlotUI slotUI, List<string> available)
    {
        InventorySlot slotInventory = inventoryPlayer.Slots[currentlyDragging.index];
        if(available.Contains(slotInventory.Item.GetID()))
        {
            if(slot == null)
            {
                inventoryPlayer.RemoveSlotFromInventory(currentlyDragging.index);
                slot = new InventorySlot();
                slot.Item = ItemGeneralizer.GetItemFromID(slotInventory.Item.GetID());
                slot.Quantity = slotInventory.Quantity;
            }
            else if(slot.Item.GetID() == slotInventory.Item.GetID())
            {
                slot.Quantity += slotInventory.Quantity;
                inventoryPlayer.Slots[currentlyDragging.index] = null;
            }
        }
    }

    private void BuildPlayerInventory()
    {
        int childs = containerInventoryPlayer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(containerInventoryPlayer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < inventoryPlayer.Slots.Count; i++)
        {
            GameObject slot = Instantiate(prefabSlotFurnace);
            slot.transform.SetParent(containerInventoryPlayer.transform, false);
            slot.GetComponent<InventoryFurnaceSlotUI>().index = i;
            slot.GetComponent<InventoryFurnaceSlotUI>().slotAssociated = inventoryPlayer.Slots[i];
            slot.GetComponent<InventoryFurnaceSlotUI>().builderReference = this;
            slot.GetComponent<InventoryFurnaceSlotUI>().IsFurnaceToHeat = false;
            slot.GetComponent<InventoryFurnaceSlotUI>().IsFurnaceHeated = false;
            slot.GetComponent<InventoryFurnaceSlotUI>().IsFurnaceCombustible = false;
            slot.GetComponent<InventoryFurnaceSlotUI>().IsInventory = true;
            slot.GetComponent<InventoryFurnaceSlotUI>().Build();
        }
    }

    private void BuildFurnacePart()
    {
        combustible.slotAssociated = null;
        combustible.IsFurnaceToHeat = false;
        combustible.IsFurnaceHeated = false;
        combustible.IsFurnaceCombustible = true;
        combustible.IsInventory = false;
        combustible.builderReference = this;
        combustible.Build();

        toHeat.slotAssociated = null;
        toHeat.IsFurnaceToHeat = true;
        toHeat.IsFurnaceHeated = false;
        toHeat.IsFurnaceCombustible = false;
        toHeat.IsInventory = false;
        toHeat.builderReference = this;
        toHeat.Build();

        heated.slotAssociated = null;
        heated.IsFurnaceToHeat = false;
        heated.IsFurnaceHeated = true;
        heated.IsFurnaceCombustible = false;
        heated.IsInventory = false;
        heated.builderReference = this;
        heated.Build();
    }
}
