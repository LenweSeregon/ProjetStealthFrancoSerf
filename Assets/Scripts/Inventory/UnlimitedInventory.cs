using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is implementation the abstract AInventory.
/// In this implementation, we don't care about limited behaviour for 
/// inventory. Instead, we can add as much as item as we want
/// </summary>
public class UnlimitedInventory : AInventory
{
    public int startingNbSlot = 20;

	/// <summary>
    /// Awake Method, calling before everything else
    /// This method is used to simply allocate our structure
    /// and place in it a certain amount of empty slot
    /// </summary>
	void Awake ()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < startingNbSlot; i++)
        {
            slots.Add(null);
        }
	}

    /// <summary>
    /// Override of the abstract mother class. Check comments for explanation.
    /// In the case of an unlimited inventory. If the algorithm do not succeed to place
    /// the entire quantity in the inventory, it create new slot and add it to them.
    /// Basically then, this implementation always return true because it is always
    /// possible to add new slot
    /// </summary>
    /// <param name="item">the item that we want to add</param>
    /// <param name="quantity">the quantity of item that we want to add</param>
    /// <returns>always true for an unlimited inventory</returns>
    public override int AddElementToInventory(string itemID, int quantity, int durability = -1)
    {
        int remainingQuantity = AddByFillingExistingItem(itemID, quantity, durability);
        if(remainingQuantity > 0)
        {
            int remainingQuantity2 = AddByInsertInEmpty(itemID, remainingQuantity, durability);
            if(remainingQuantity2 > 0)
            {
                int iterator = Slots.Count;
                while (remainingQuantity2 > 0)
                {
                    int quantityToAdd = Mathf.Min(maxQuantityPerSlot, remainingQuantity2);
                    slots.Add(new InventorySlot());
                    slots[iterator].Item = ItemGeneralizer.GetItemFromID(itemID);
                    slots[iterator].Quantity = quantityToAdd;
                    remainingQuantity2 -= quantityToAdd;
                    iterator++;
                }
            }
        }

        return 0;
    }
}
