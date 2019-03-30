using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionPlace : IItemAction {

    private string identifier;
    private StructureHandler structureHandler;

    public ItemActionPlace(string _identifier)
    {
        identifier = _identifier;
        structureHandler = GameObject.FindObjectOfType<StructureHandler>();
    }

    public override void TriggerAction()
    {
        structureHandler.StructureToPlace = SlotAssociated.inventoryAssociated.Slots[SlotAssociated.index].Item.Get3DModel();
        structureHandler.callbackAtPlacementValidated = CallbackAtPlacement;
    }

    private void CallbackAtPlacement()
    {
        SlotAssociated.inventoryAssociated.RemoveSlotFromInventory(SlotAssociated.index);
    }

    public override string GetI18nName()
    {
        return I18nManager.Fields[identifier];
    }
}
