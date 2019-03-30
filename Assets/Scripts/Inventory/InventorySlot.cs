using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represent a slot in the inventory
/// Basically, it only hold a quantity and a storable item
/// which will be represented in most case by an ID
/// </summary>
public class InventorySlot
{
    private int quantity;
    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    private Item item;
    public Item Item
    {
        get { return item; }
        set { item = value; }
    }

    /// <summary>
    /// Inventory Slot constructor
    /// The default constructor basically create an inventory slot empty
    /// which mean that the quantity is 0 and the item is null
    /// </summary>
    public InventorySlot()
    {
        quantity = 0;
        item = null;
    }
}
