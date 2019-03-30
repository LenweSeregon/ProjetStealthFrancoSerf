using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionErase : IItemAction
{
    private string identifier;

    public ItemActionErase(string _identifier)
    {
        identifier = _identifier;
    }

    public override void TriggerAction()
    {
        SlotAssociated.inventoryAssociated.RemoveSlotFromInventory(SlotAssociated.index);
    }

    public override string GetI18nName()
    {
        return I18nManager.Fields[identifier];
    }
}
