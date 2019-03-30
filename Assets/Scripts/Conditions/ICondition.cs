using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICondition : MonoBehaviour
{
    public abstract bool SatisfyCondition(Player player);
}
