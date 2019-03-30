using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractableConditions))]
public class ResourcePoint : Interactable {

    public static int identifierAssigner = 0;

    private InteractableConditions interactionCondition;
    public BoxCollider nonHarvestedCollider;
    public BoxCollider harvestedCollider;
    public GameObject nonHarvestedGFX;
    public GameObject harvestedGFX;

    public string resourcePointID;

    private float timeSinceHarvest = 0.0f;
    public float TimeSinceHarvest
    {
        get { return timeSinceHarvest; }
        private set { }
    }
    private bool hasBeenHarvested = false;
    public bool HasBeenHarvested
    {
        get { return hasBeenHarvested; }
        private set { }
    }

    private void Awake()
    {
        interactionCondition = GetComponent<InteractableConditions>();
        UpdateResourcePointGFX();
    }

    public override void Interact(Player player)
    {
        if (!hasBeenHarvested && interactionCondition.Satisfy(player))
        {
            Item itemRelated = GetFirstItemInventoryForConditions(player);
            if(itemRelated != null)
            {
                player.GetComponent<PlayerAnimator>().toolToInstantiate = itemRelated.Get3DModel();
            }

            player.GetComponent<PlayerController>().isHarvesting = true;
            ActivateAnimation(player);
            StartCoroutine(HarvestingCoroutineWaitTime(player));
        }
    }

    private IEnumerator HarvestingCoroutineWaitTime(Player player)
    {
        int harvestTime = ResourcePointDropProbabilityCollection.GetDataFromID(resourcePointID).HarvestTime;
        float time = 0.0f;
        while(time < harvestTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Harvest(player);
        UpdateResourcePointGFX();
        player.GetComponent<PlayerController>().isHarvesting = false;
        UnactivateAnimation(player);
    }
    private void ActivateAnimation(Player player)
    {
        string eventName = ResourcePointDropProbabilityCollection.GetDataFromID(resourcePointID).EventName;
        if (eventName == "mining")
        {
            player.GetComponent<PlayerAnimator>().SetIsMining(true);
        }
        else
        {
            player.GetComponent<PlayerAnimator>().SetIsChopping(true);
        }
    }
    private void UnactivateAnimation(Player player)
    {
        string eventName = ResourcePointDropProbabilityCollection.GetDataFromID(resourcePointID).EventName;
        if (eventName == "mining")
        {
            player.GetComponent<PlayerAnimator>().SetIsMining(false);
        }
        else
        {
            player.GetComponent<PlayerAnimator>().SetIsChopping(false);
        }
    }

    public void LoadResourcePoint(ResourcePointData rpData)
    {
        hasBeenHarvested = rpData.hasBeenHarvested;
        timeSinceHarvest = rpData.timeSinceHarvested;
        UpdateResourcePointGFX();
    }

    private void Update()
    {
        if(hasBeenHarvested)
        {
            int respawnTime = ResourcePointDropProbabilityCollection.GetDataFromID(resourcePointID).RespawnTime;
            timeSinceHarvest += Time.deltaTime;
            if (timeSinceHarvest >= respawnTime)
            {
                hasBeenHarvested = false;
                timeSinceHarvest = 0.0f;
                gameObject.SetActive(true);
                UpdateResourcePointGFX();
            }
        }

    }

    private void UpdateResourcePointGFX()
    {
        if (hasBeenHarvested)
        {
            nonHarvestedCollider.enabled = false;
            harvestedCollider.enabled = true;
            nonHarvestedGFX.SetActive(false);
            harvestedGFX.SetActive(true);
        }
        else
        {
            nonHarvestedCollider.enabled = true;
            harvestedCollider.enabled = false;
            nonHarvestedGFX.SetActive(true);
            harvestedGFX.SetActive(false);
        }
    }

    private void Harvest(Player player)
    {
        var probabilities = ResourcePointDropProbabilityCollection.GetDataFromID(resourcePointID);
        foreach(var probabilityItem in probabilities.Probabilities)
        {
            float randomProbabilityOnItem = Random.Range(0.0f, 1.0f);
            if(randomProbabilityOnItem <= probabilityItem.Probability)
            {
                int quantity = Random.Range(probabilityItem.QuantityMin, probabilityItem.QuantityMax);
                player.Inventory.AddElementToInventory(probabilityItem.IdRawResource, quantity);
            }
        }

        RemoveDurabilityOnPlayerObject(player);
        hasBeenHarvested = true;
    }

    private int ItemConditionIndex()
    {
        int i = 0;
        foreach(ICondition condition in interactionCondition.conditions)
        {
            if(condition.GetType() == typeof(ConditionItem))
            {
                return i;
            }
            i++;
        }

        return -1;
    }

    private Item GetFirstItemInventoryForConditions(Player player)
    {
        List<Item> itemsCondition = new List<Item>();
        int index = ItemConditionIndex();
        if (index >= 0)
        {
            var andList = ((ConditionItem)interactionCondition.conditions[index]).itemsConditions.andList;
            foreach (var or in andList)
            {
                int indexSlot = player.Inventory.GetFirstSlotIndexContainingOneOfThem(or.itemsID);
                if (indexSlot >= 0)
                {
                    itemsCondition.Add(player.Inventory.Slots[indexSlot].Item);
                }
            }
        }

        return (itemsCondition.Count >= 1) ? (itemsCondition[0]) : (null);
    }

    private void RemoveDurabilityOnPlayerObject(Player player)
    {
        int index = ItemConditionIndex();
        if (index >= 0)
        {
            var andList = ((ConditionItem) interactionCondition.conditions[index]).itemsConditions.andList;
            foreach(var or in andList)
            {
                int indexSlot = player.Inventory.GetFirstSlotIndexContainingOneOfThem(or.itemsID);
                if(indexSlot >= 0)
                {
                    int durabilityMinus = ResourcePointDropProbabilityCollection.GetDataFromID(resourcePointID).DurabilityConsumption;
                    player.Inventory.Slots[indexSlot].Item.CurrentDurability -= durabilityMinus;
                }
                else
                {
                    Debug.Log("ERROR : Should not have been able to use resource point");
                }
            }
        }
    }
    
}
