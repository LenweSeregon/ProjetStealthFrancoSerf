using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represent a bundle of all items available in the game.
/// Because the game is founded on a id-representation of datas, we need to
/// have a way to retrieve a item that we don't know the type (can be a raw resource,
/// an armor, etc...) without trouble.
/// So, this static class is a way to know if he item with an id exist.
/// </summary>
public static class ItemGeneralizer
{
    /// <summary>
    /// Public method which is the only one expose to retrieve an item
    /// according to a unique id given in parameter.
    /// </summary>
    /// <param name="id">the item's id we are looking for</param>
    /// <returns></returns>
    public static Item GetItemFromID(string id)
    { 
        Item asRawResource = RetrieveAsRawResource(id);
        if (asRawResource != null)
        {
            return asRawResource;
        }

        return null;
    }

    /// <summary>
    /// Private method that handle the search concerning all raw resource
    /// about an id given in parameter
    /// </summary>
    /// <param name="id">the item's id we are looking for</param>
    /// <returns>The IStorable as a raw resource if it exist, null otherwise</returns>
    private static Item RetrieveAsRawResource(string id)
    {
        if (ItemCollection.Exists(id))
        {
            return new RawResource(id);
        }

        return null;
    }
}
