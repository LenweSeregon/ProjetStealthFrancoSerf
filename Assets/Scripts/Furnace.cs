using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : Interactable
{
    public AudioSource fireSound;
    private FurnaceBuilder furnaceBuilder;
    private GUIManager guiManager;
    public bool IsOn
    {
        get;set;
    }
    public int Temperature
    {
        get;
        set;
    }

    public bool IsTransforming
    {
        get;set;
    }
    private FurnaceCollection.RecipeFurnaceData currentRecipe;

    [HideInInspector]
    public float startTransformingTime;
    public float secondBeforeConsumingCombustible;
    [HideInInspector]
    public float counterSecondBeforeConsuming;
    public float secondBeforeLoweringTemperature;
    [HideInInspector]
    public float counterSecondBeforeLowering;

    public List<string> combustibleAvailableList;
    public List<string> toHeatAvailableList;

    [HideInInspector]
    public InventorySlot combustible;
    [HideInInspector]
    public InventorySlot toHeat;
    [HideInInspector]
    public InventorySlot heated;

    private void Awake()
    {
        currentRecipe = null;
        IsOn = false;
        IsTransforming = false;
        combustible = null;
        toHeat = null;
        heated = null;
        Temperature = 0;
    }

    private void Start()
    {
        guiManager = FindObjectOfType<GUIManager>();
        furnaceBuilder = Resources.FindObjectsOfTypeAll<FurnaceBuilder>()[0];
    }

    public void LoadFurnace(FurnaceData furnaceData)
    {
        IsOn = furnaceData.isOn;
        Temperature = furnaceData.temperature;

        if(furnaceData.combustibleID != null)
        {
            combustible = new InventorySlot();
            combustible.Item = ItemGeneralizer.GetItemFromID(furnaceData.combustibleID);
            combustible.Quantity = furnaceData.combustibleQuantity;
        }
        if(furnaceData.toHeatID != null)
        {
            toHeat = new InventorySlot();
            toHeat.Item = ItemGeneralizer.GetItemFromID(furnaceData.toHeatID);
            toHeat.Quantity = furnaceData.toHeatQuantity;
        }
        if(furnaceData.heatedID != null)
        {
            heated = new InventorySlot();
            heated.Item = ItemGeneralizer.GetItemFromID(furnaceData.heatedID);
            heated.Quantity = furnaceData.heatedQuantity;
        }

        if(furnaceData.isTransforming)
        {
            currentRecipe = FurnaceCollection.GetDataFromID(toHeat.Item.GetID());
            IsTransforming = true;
            startTransformingTime = furnaceData.startTransformationTime;
        }
        else
        {
            currentRecipe = null;
            IsTransforming = false;
            startTransformingTime = 0;
        }

        counterSecondBeforeConsuming = furnaceData.counterSecondBeforeComsumingCombustible;
        counterSecondBeforeLowering = furnaceData.counterSecondBeforeLoweringTemperature;
    }

    public void Update()
    {
        if(IsOn)
        {
            if(toHeat != null && !IsTransforming)
            {
                currentRecipe = FurnaceCollection.GetDataFromID(toHeat.Item.GetID());
                if(Temperature >= currentRecipe.TemperatureNeeded && (heated == null || heated.Item.GetID() == currentRecipe.ItemHeatedID))
                {
                    IsTransforming = true;
                    startTransformingTime = 0.0f;
                }
            }

            if(IsTransforming)
            {
                if(startTransformingTime >= currentRecipe.HeatTime)
                {
                    toHeat.Quantity--;
                    if(toHeat.Quantity <= 0)
                    {
                        toHeat = null;
                    }

                    if(heated == null)
                    {
                        heated = new InventorySlot();
                        heated.Quantity = 1;
                        heated.Item = ItemGeneralizer.GetItemFromID(currentRecipe.ItemHeatedID);
                    }
                    else
                    {
                        heated.Quantity++;
                    }
                    furnaceBuilder.UpdateFurnace();
                    IsTransforming = false;
                    startTransformingTime = 0.0f;
                }
                else
                {
                    startTransformingTime += Time.deltaTime;
                }
            }

            if (counterSecondBeforeLowering > secondBeforeLoweringTemperature)
            {
                counterSecondBeforeLowering = 0.0f;
                Temperature -= 250;
                furnaceBuilder.UpdateTemperature();

                if(Temperature == 0)
                {
                    currentRecipe = null;
                    IsTransforming = false;
                    counterSecondBeforeLowering = 0.0f;
                    counterSecondBeforeConsuming = 0.0f;
                    startTransformingTime = 0.0f;
                    IsOn = false;
                    fireSound.Stop();
                    furnaceBuilder.FurnaceOff();
                    return;
                }
            }

            if (counterSecondBeforeConsuming > secondBeforeConsumingCombustible)
            {
                if (combustible != null)
                {
                    combustible.Quantity--;
                    if (combustible.Quantity <= 0)
                    {
                        combustible = null;
                    }
                }

                counterSecondBeforeConsuming = 0.0f;
                furnaceBuilder.UpdateFurnace();
            }

            counterSecondBeforeLowering += Time.deltaTime;
            counterSecondBeforeConsuming += Time.deltaTime;
        }
    }

    public void IncreaseTemperature()
    {
        if(IsOn)
        {
            if (combustible != null && combustible.Quantity > 0)
            {
                counterSecondBeforeConsuming += secondBeforeConsumingCombustible / 2;
                counterSecondBeforeLowering = 0.0f;
                Temperature += 250;
                furnaceBuilder.UpdateTemperature();
                furnaceBuilder.UpdateFurnace();
            }
        }
        else if(combustible != null && combustible.Quantity > 0)
        {
            fireSound.Play();
            IsOn = true;
            furnaceBuilder.FurnaceOn();
            if (combustible != null && combustible.Quantity > 0)
            {
                combustible.Quantity--;
                if(combustible.Quantity <= 0)
                {
                    combustible = null;
                }
                Temperature += 250;
                furnaceBuilder.UpdateFurnace();
                furnaceBuilder.UpdateTemperature();
            }
        }
    }

    public override void Interact(Player player)
    {
        FurnaceBuilder.currentFurnace = this;
        guiManager.SwitchToWindow("FurnaceMenu");
        furnaceBuilder.UpdateFurnace();
        furnaceBuilder.UpdateTemperature();
        if(IsOn)
        {
            furnaceBuilder.FurnaceOn();
        }
        else
        {
            furnaceBuilder.FurnaceOff();
        }
    }
}
