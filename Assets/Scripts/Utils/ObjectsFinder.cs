using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectsFinder
{
    public static List<T> FindAllResourcePointEvenNotInScene<T>()
    {
        List<T> resourcesPointsList = new List<T>();
        T[] rps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
        foreach (T rp in rps)
        {
            resourcesPointsList.Add(rp);
        }
        return resourcesPointsList;
    }
}
