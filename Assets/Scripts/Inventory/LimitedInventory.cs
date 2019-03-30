using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is implementation the abstract AInventory.
/// In this implementation, we do care about limited behaviour for 
/// inventory. So if there is not more place, we can't add more items
/// </summary>
public class LimitedInventory : AInventory
{
    public float maxWeight = 1000;
    public int nbSlot = 20;

    /// <summary>
    /// Awake Method, calling before everything else
    /// This method is used to simply allocate our structure
    /// and place in it a certain amount of empty slot
    /// </summary>
    void Awake()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < nbSlot; i++)
        {
            slots.Add(null);
        }
	}

    /// <summary>
    /// Override of the abstract mother class. Check comments for explanation.
    /// In the case of a limited inventory. If the algorithm do not succeed to place
    /// the entire quantity in the inventory, it simply stop and return false
    /// </summary>
    /// <param name="item">the item that we want to add</param>
    /// <param name="quantity">the quantity of item that we want to add</param>
    /// <returns>true if every item has been added, false otherwise</returns>
    public override int AddElementToInventory(string itemID, int quantity, int durability = -1)
    {
        int remainingQuantity = AddByFillingExistingItem(itemID, quantity, durability);
        if(remainingQuantity > 0)
        {
            int remainingAfterInEmpty = AddByInsertInEmpty(itemID, remainingQuantity, durability);
            return remainingAfterInEmpty;
        }

        return 0;
    }
}
