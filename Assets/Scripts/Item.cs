using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IStorable
{
    protected string id;
    protected int currentDurability;
    public int CurrentDurability
    {
        get { return currentDurability; }
        set
        {
            currentDurability = value;
            if(currentDurability < 0)
            {
                currentDurability = 0;
            }
        }
    }

    public Item(string _id)
    {
        id = _id;
        currentDurability = ItemCollection.GetDataFromID(id).Durability;
    }

    public string[] GetActionsID()
    {
        return ItemCollection.GetDataFromID(id).ActionsID;
    }

    public GameObject Get3DModel()
    {
        return ItemCollection.GetDataFromID(id).Model3D;
    }

    public int GetMaxDurability()
    {
        return ItemCollection.GetDataFromID(id).Durability;
    }

    public int GetCurrentDurability()
    {
        return currentDurability;
    }

    public string GetI18nDescriptionIdentifier()
    {
        return ItemCollection.GetDataFromID(id).DescriptionI18n;
    }

    public string GetI18nNameIdentifier()
    {
        return ItemCollection.GetDataFromID(id).NameI18n;
    }

    public Sprite GetIcon()
    {
        return ItemCollection.GetDataFromID(id).Icon;
    }

    public string GetID()
    {
        return id;
    }

    public float GetWeight()
    {
        return ItemCollection.GetDataFromID(id).Weight;
    }

    public bool IsStackable()
    {
        return ItemCollection.GetDataFromID(id).IsStackable;
    }
}
