using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AInventory is an abstract class to represent the 2 big
/// category of inventory in the game, which are limited inventory
/// and unlimited inventory.
/// In this abstract class, we implement every general method which 
/// are useful for the inventory : 
///     The actual structure to store the inventory slots
///     The max quantity per slot in the inventory
///     Methods such as removing and adding element in the inventory
///     Utils methods for example to know the quantity of an element
///     
/// There is also a method to load the inventory with a save data class
/// </summary>
public abstract class AInventory : MonoBehaviour
{
    protected List<InventorySlot> slots;
    public List<InventorySlot> Slots
    {
        get { return slots; }
        private set { }
    }

    public int maxQuantityPerSlot;

    /// <summary>
    /// This method can be used to load an inventory which as been saved.
    /// This method simply ensure to retrieve a consistent state according to
    /// the save.
    /// </summary>
    /// <param name="inventory">the inventory data containing saved values</param>
    public void LoadInventory(InventoryData inventory)
    {
        for(int i = 0; i < inventory.slots.Length; i++)
        {
            slots[i] = new InventorySlot();
            if (inventory.slots[i] != null)
            {
                slots[i].Item = ItemGeneralizer.GetItemFromID(inventory.slots[i].itemID);
                slots[i].Quantity = inventory.slots[i].quantity;
                slots[i].Item.CurrentDurability = inventory.slots[i].durability;
            }
            else
            {
                slots[i] = null;
            }
        }
    }

    /// <summary>
    /// This method allow the user to know the quantity of a specific item 
    /// in the inventory.
    /// </summary>
    /// <param name="item">the item that we want to know the quantity</param>
    /// <returns>the quantity of item requested in parameter</returns>
    public int GetQuantityOfItem(string itemID)
    {
        int quantity = 0;
        foreach(InventorySlot slot in slots)
        {
            if(slot != null && slot.Item.GetID() == itemID)
            {
                quantity += slot.Quantity;
            }
        }
        return quantity;
    }

    public int GetFirstSlotIndexContainingOneOfThem(List<string> list)
    {
        int index = 0;
        foreach (InventorySlot slot in slots)
        {
            foreach (string itemID in list)
            {
                if (slot != null && slot.Item.GetID() == itemID && slot.Item.GetCurrentDurability() > 0)
                {
                    return index;
                }
            }
            index++;

        }
        
        return -1;
    }

    /// <summary>
    /// This method allow the user to swap 2 inventory slot each other by
    /// swapping the content
    /// </summary>
    /// <param name="indexSlot1">index of the first slot to swap</param>
    /// <param name="indexSlot2">index of the second slot to swap</param>
    public void Swap(int indexSlot1, int indexSlot2)
    {
        if (slots[indexSlot1] != null && slots[indexSlot2] != null && slots[indexSlot1].Item.GetID() == slots[indexSlot2].Item.GetID())
        {
            int quantityAlreadyPresentInDestination = slots[indexSlot2].Quantity;
            int wantToDragQuantity = slots[indexSlot1].Quantity;
            if (wantToDragQuantity + quantityAlreadyPresentInDestination > maxQuantityPerSlot)
            {
                slots[indexSlot1].Quantity = ((wantToDragQuantity + quantityAlreadyPresentInDestination) -maxQuantityPerSlot);
                slots[indexSlot2].Quantity = 64;
            }
            else
            {
                slots[indexSlot1] = null;
                slots[indexSlot2].Quantity = wantToDragQuantity + quantityAlreadyPresentInDestination;
            }
        }
        else
        {
            InventorySlot tmp = slots[indexSlot1];
            slots[indexSlot1] = slots[indexSlot2];
            slots[indexSlot2] = tmp;
        }
    }

    /// <summary>
    /// This method allow to swap 2 inventory slot each other which are
    /// in a different inventory by swapping their content
    /// </summary>
    /// <param name="other">The inventory which we want to do the swap</param>
    /// <param name="indexSlot">index of the slot to swap in the inventory</param>
    /// <param name="indexSlotOther">index of the slot to swap in the other inventory</param>
    public void Swap(AInventory other, int fromIndexSlot, int toIndexSlotOther)
    {
        if (slots[fromIndexSlot] != null && other.Slots[toIndexSlotOther] != null && slots[fromIndexSlot].Item.GetID() == other.Slots[toIndexSlotOther].Item.GetID())
        {
            int quantityAlreadyPresentInDestination = other.Slots[toIndexSlotOther].Quantity;
            int wantToDragQuantity = slots[fromIndexSlot].Quantity;
            if(wantToDragQuantity + quantityAlreadyPresentInDestination > maxQuantityPerSlot)
            {
                slots[fromIndexSlot].Quantity = ((wantToDragQuantity + quantityAlreadyPresentInDestination) - maxQuantityPerSlot);
                other.Slots[toIndexSlotOther].Quantity = 64;
            }
            else
            {
                slots[fromIndexSlot] = null;
                other.Slots[toIndexSlotOther].Quantity = wantToDragQuantity + quantityAlreadyPresentInDestination;
            }
        }
        else
        {
            InventorySlot tmp = slots[fromIndexSlot];
            slots[fromIndexSlot] = other.Slots[toIndexSlotOther];
            other.Slots[toIndexSlotOther] = tmp;
        }
    }

    public void AddAtIndex(string itemID, int quantity, int index)
    {
        slots[index] = new InventorySlot();
        slots[index].Item = ItemGeneralizer.GetItemFromID(itemID);
        slots[index].Quantity = quantity;
    }

    /// <summary>
    /// This method allow the user to remove a quantity of item from the inventory.
    /// It basically check first if there is enough item to remove them. If not, the method
    /// stop by returning false.
    /// If there is enough, the algorithm iterate on the inventory slot passing through all, 
    /// until there is not quantity to remove and remove as much as possible in each slot.
    /// </summary>
    /// <param name="item">the item that we want to remove</param>
    /// <param name="quantity">the quantity that we want to remove</param>
    /// <returns>true if possible to remove quantity from inventory, false otherwise</returns>
    public bool RemoveQuantityFromInventory(string itemID, int quantity)
    {
        if (GetQuantityOfItem(itemID) < quantity)
        {
            return false;
        }

        int iterator = 0;
        while(quantity > 0 && iterator < slots.Count)
        {
            if(slots[iterator] != null && slots[iterator].Item.GetID() == itemID)
            {
                int quantityToRemove = Mathf.Min(slots[iterator].Quantity, quantity);
                slots[iterator].Quantity -= quantityToRemove;
                quantity -= quantityToRemove;

                if(slots[iterator].Quantity == 0)
                {
                    slots[iterator] = null;
                }
            }

            iterator++;
        }
        return true;
    }

    /// <summary>
    /// This method allow the user's class to simply remove a slot from the inventory
    /// It's mean that he set to null the current slot and it reflect a removing of a item
    /// basically.
    /// </summary>
    /// <param name="slotIndex">the index in the inventory that we want to remove</param>
    public void RemoveSlotFromInventory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count)
        {
            slots[slotIndex] = null;
        }
    }

    /// <summary>
    /// This method is a protected one and is use by class's implementation to
    /// add an item with a quantity specify in parameter.
    /// This method works as follow :
    ///     From the beginning of the inventory, and passing though every slot until
    ///     there is no more item to add, the algorithm check if the current slot is 
    ///     holding a similar item to the one that we want to add. If so, he add as much 
    ///     as possible item in the slot and continue.
    ///     The algorithm end when there is no item or every slot has been visited.
    ///     Finally, it return the quantity of item remaining to add
    /// </summary>
    /// <param name="item">the item that we want to add</param>
    /// <param name="quantity">the quantity of item that we want to add</param>
    /// <returns>quantity of item remaining to add</returns>
    protected int AddByFillingExistingItem(string itemID, int quantity, int durability)
    {
        int iterator = 0;
        while (quantity > 0 && iterator < slots.Count)
        {
            InventorySlot slot = slots[iterator];
            if (slot != null && slot.Item.GetID() == itemID && slot.Item.IsStackable())
            {
                int quantityToAdd = Mathf.Min(quantity, maxQuantityPerSlot - slot.Quantity);
                quantity -= quantityToAdd;
                slot.Quantity += quantityToAdd;
                if(durability == -1)
                {
                    slot.Item.CurrentDurability = ItemCollection.GetDataFromID(itemID).Durability;
                }
                else
                {
                    slot.Item.CurrentDurability = durability;
                }
            }
            iterator++;
        }

        return quantity;
    }

    /// <summary>
    /// This method is a protected one and is use by class's implementation to
    /// add an item with a quantity specify in parameter.
    /// This method works as follow :
    ///     From the beginning of the inventory, and passing though every slot until
    ///     there is no more item to add, the algorithm check if the current slot is 
    ///     not holding a item. If so, he add as much as possible item in the slot and continue.
    ///     The algorithm end when there is no item or every slot has been visited.
    ///     Finally, it return the quantity of item remaining to add
    /// </summary>
    /// <param name="item">the item that we want to add</param>
    /// <param name="quantity">the quantity of item that we want to add</param>
    /// <returns>quantity of item remaining to add</returns>
    protected int AddByInsertInEmpty(string itemID, int quantity, int durability)
    {
        int iterator = 0;
        while(quantity > 0 && iterator < slots.Count)
        {
            if(slots[iterator] == null)
            {
                int quantityToAdd = Mathf.Min(quantity, maxQuantityPerSlot);
                slots[iterator] = new InventorySlot();
                slots[iterator].Item = ItemGeneralizer.GetItemFromID(itemID);
                slots[iterator].Quantity = quantityToAdd;
                if (durability == -1)
                {
                    slots[iterator].Item.CurrentDurability = ItemCollection.GetDataFromID(itemID).Durability;
                }
                else
                {
                    slots[iterator].Item.CurrentDurability = durability;
                }
                quantity -= quantityToAdd;
            }
            iterator++;
        }

        return quantity;
    }

    /// <summary>
    /// This method is the one exposed to external classes to add item in our
    /// inventory.
    /// This method is abstract and should be override by implementation because treatment
    /// can differ if you want to add in an unlimited inventory for example by adding new
    /// inventory slot. 
    /// Generally speaking, implementation will proceed as follow :
    ///     Trying to fill inventory slot on existing item via @AddByFillingExistingItem
    ///     Trying to fill inventory slot with empty slot via @AddByInsertInEmpty
    ///     Specific operation concerning implementation
    /// </summary>
    /// <param name="item">the item that we want to add</param>
    /// <param name="quantity">the quantity of item that we want to add</param>
    /// <returns>true if there is no more item remaining, false otherwise</returns>
    public abstract int AddElementToInventory(string itemID, int quantity, int durability = -1);
}
