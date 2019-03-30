using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableConditions : MonoBehaviour {
    
    public List<ICondition> conditions;

    public bool Satisfy(Player player)
    {
        bool isSatisfying = true;
        foreach(ICondition condition in conditions)
        {
            isSatisfying &= condition.SatisfyCondition(player);
        }

        return isSatisfying;
    }
}
