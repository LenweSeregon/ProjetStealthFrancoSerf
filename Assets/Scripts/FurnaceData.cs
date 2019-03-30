using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnaceData
{
    public float[] position;
    public float[] rotation;

    public bool isOn;
    public int temperature;

    public string combustibleID;
    public int combustibleQuantity;
    public string toHeatID;
    public int toHeatQuantity;
    public string heatedID;
    public int heatedQuantity;

    public bool isTransforming;
    public float startTransformationTime;

    public float counterSecondBeforeComsumingCombustible;
    public float counterSecondBeforeLoweringTemperature;

    public FurnaceData(Furnace furnace)
    {
        position = new float[3];
        rotation = new float[3];
        position[0] = furnace.transform.position.x;
        position[1] = furnace.transform.position.y;
        position[2] = furnace.transform.position.z;

        rotation[0] = furnace.transform.eulerAngles.x;
        rotation[1] = furnace.transform.eulerAngles.y;
        rotation[2] = furnace.transform.eulerAngles.z;


        isOn = furnace.IsOn;
        temperature = furnace.Temperature;

        combustibleID = null;
        combustibleQuantity = -1;
        toHeatID = null;
        toHeatQuantity = -1;
        heatedID = null;
        heatedQuantity = -1;
        if (furnace.combustible != null)
        {
            combustibleID = furnace.combustible.Item.GetID();
            combustibleQuantity = furnace.combustible.Quantity;
        }
        if (furnace.toHeat != null)
        {
            toHeatID = furnace.toHeat.Item.GetID();
            toHeatQuantity = furnace.toHeat.Quantity;
        }
        if (furnace.heated != null)
        {
            heatedID = furnace.heated.Item.GetID();
            heatedQuantity = furnace.heated.Quantity;
        }

        isTransforming = furnace.IsTransforming;
        startTransformationTime = furnace.startTransformingTime;

        counterSecondBeforeComsumingCombustible = furnace.counterSecondBeforeConsuming;
        counterSecondBeforeLoweringTemperature = furnace.counterSecondBeforeLowering;
    }
}
