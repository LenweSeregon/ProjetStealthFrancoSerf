using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemActionCollection
{
    public static Dictionary<string, IItemAction> actions;
    public static Dictionary<string, IItemAction> Actions
    {
        get
        {
            if(actions == null)
            {
                BuildActionsCollection();
            }

            return actions;
        }
        private set { }
    }

    public static void BuildActionsCollection()
    {
        actions = new Dictionary<string, IItemAction>();
        actions.Add("items.actions.erase", new ItemActionErase("items.actions.erase"));
        actions.Add("items.actions.place", new ItemActionPlace("items.actions.place"));
    }

}
