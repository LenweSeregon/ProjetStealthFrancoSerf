using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionItem : ICondition
{
    [System.Serializable]
    public class OrListConditionItem
    {
        public List<string> itemsID;
    }

    [System.Serializable]
    public class AndListConditionItem
    {
        public List<OrListConditionItem> andList;
    }

    public AndListConditionItem itemsConditions;

    public override bool SatisfyCondition(Player player)
    {
        bool satisfying = true;
        foreach(OrListConditionItem and in itemsConditions.andList)
        {
            bool exist = false;
            foreach(string itemID in and.itemsID)
            {
                if( player.Inventory.GetQuantityOfItem(itemID) > 0)
                {
                    exist = true;
                    break;
                }
            }

            exist &= (player.Inventory.GetFirstSlotIndexContainingOneOfThem(and.itemsID) >= 0);

            satisfying &= exist;
        }
        return satisfying;
    }

}
