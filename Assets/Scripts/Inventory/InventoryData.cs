using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to save an inventory to a binary formatter format (using only
/// type that can be represented). This class simply use a collection of InventorySlotData
/// which is also a compatible type.
/// </summary>
[System.Serializable]
public class InventoryData
{
    /// <summary>
    /// This intern class represent a slot in a save format.
    /// Basically, the item is representing with it unique ID
    /// that we can found back in collections and a quantity.
    /// </summary>
    [System.Serializable]
    public class InventorySlotData
    {
        public int quantity;
        public string itemID;
        public int durability;

        /// <summary>
        /// This constructor simply take a slot that can may be not in a compatible and turn 
        /// in into a compatible format (ie : a string representing item id and a quantity)
        /// </summary>
        /// <param name="slot">the slot that we want to save in compatible format</param>
        public InventorySlotData(InventorySlot slot)
        {
            quantity = slot.Quantity;
            itemID = slot.Item.GetID();
            durability = slot.Item.CurrentDurability;
        }
    }

    public InventorySlotData[] slots;

    /// <summary>
    /// This constructor simply take an inventory (limited or unlimited) and turn it into
    /// a compatible format. To do that, it simply has an array of InventorySlotData that are
    /// also compatible.
    /// </summary>
    /// <param name="inventory"></param>
    public InventoryData(AInventory inventory)
    {
        slots = new InventorySlotData[inventory.Slots.Count];

        for(int i = 0; i < inventory.Slots.Count; i++)
        {
            if (inventory.Slots[i] == null)
            {
                slots[i] = null;
            }
            else
            {
                slots[i] = new InventorySlotData(inventory.Slots[i]);
            }
        }
    }
}
