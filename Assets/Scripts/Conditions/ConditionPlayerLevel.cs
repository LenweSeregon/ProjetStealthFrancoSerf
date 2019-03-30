using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionPlayerLevel : ICondition
{
    public int levelRequired;

    public override bool SatisfyCondition(Player player)
    {
        return player.level >= levelRequired;
    }
}
