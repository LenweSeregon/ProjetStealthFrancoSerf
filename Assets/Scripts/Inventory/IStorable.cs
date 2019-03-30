using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IStorable interface represent the contract that every item wanted
/// to be represented in an inventory has to follow. 
/// Basically, it force implementing components to give informations about themselves
/// such as ID, weight, name 18n, description i18n and icon.
/// </summary>
public interface IStorable
{
    /// <summary>
    /// This method return a list of string representating the id
    /// of action that are possible with our item. We then use the
    /// item action collection to know the possibilities
    /// </summary>
    /// <returns>list of action's id</returns>
    string[] GetActionsID();

    /// <summary>
    /// This method return a bool to know if our item is stackable or not
    /// This property can be useful when we want to only has 1 type of item
    /// in inventory slot
    /// </summary>
    /// <returns>true if stackable, false otherwise</returns>
    bool IsStackable();

    /// <summary>
    /// This method return the id, which is represent in collection charged in json.
    /// This id can be useful when we need to load back an inventory.
    /// </summary>
    /// <returns>the id representing an item in a collection</returns>
    string GetID();

    /// <summary>
    /// This method return the weight of an item. This weight can be use for 
    /// example when we want to limit the weight in an inventory.
    /// </summary>
    /// <returns>the weight of the item</returns>
    float GetWeight();

    /// <summary>
    /// This method return the identifier present in language file that represent
    /// the item's name. This identifier will then be use in a display context by calling
    /// i18nManager
    /// </summary>
    /// <returns>the i18n identifier of the item's name</returns>
    string GetI18nNameIdentifier();

    /// <summary>
    /// This method return the identifier present in language file that represent
    /// the item's description. This identifier will then be use in a display context by calling
    /// i18nManager
    /// </summary>
    /// <returns>the i18n identifier of the item's description</returns>
    string GetI18nDescriptionIdentifier();

    /// <summary>
    /// This method return the sprite representing for example a tiny icon in inventory system
    /// to recognize it.
    /// </summary>
    /// <returns>the icon representing our storable</returns>
    Sprite GetIcon();

    /// <summary>
    /// This method return the max durability that an object can have.
    /// Basically, a max durability of -1 if object is infinite
    /// </summary>
    /// <returns>the max durability of the item</returns>
    int GetMaxDurability();

    /// <summary>
    /// This method return the current durability that an object has.
    /// Basically, a current durability of -1 if object is infinite
    /// </summary>
    /// <returns>the current durability of the item</returns>
    int GetCurrentDurability();
}
