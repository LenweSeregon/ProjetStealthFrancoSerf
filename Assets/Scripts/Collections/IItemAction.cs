using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItemAction
{
    public InventorySlotActionUI SlotAssociated
    {
        get;
        set;
    }
    public InventoryBuilder inventoryBuilder;

    public abstract void TriggerAction();
    public abstract string GetI18nName();
}
