using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptorOpenHQGate : Interactable
{
    public AInventory playerInventory;
    public AudioManager audioManager;
    public GameObject doorHQ;
    public float openTime = 1.0f;
    private bool doorHasBeenOpen;
    
	void Start ()
    {
        doorHasBeenOpen = false;
	}
	
    public override bool IsInteractable()
    {
        return doorHasBeenOpen == false && playerInventory.GetQuantityOfItem("breaker") > 0;
    }

    public override string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.basicInteraction";
    }

    public override void Interact(Player player)
    {
        if(playerInventory.GetQuantityOfItem("breaker") > 0)
        {
            playerInventory.RemoveQuantityFromInventory("breaker", 1);
            doorHasBeenOpen = true;
            StartCoroutine(OpenDoor(doorHQ));
        }
    }

    private IEnumerator OpenDoor(GameObject door)
    {
        Vector3 current = door.transform.localPosition;
        Vector3 target = door.transform.localPosition;
        target.y = 3.1f;
        float time = 0.0f;

        audioManager.PlayDoorOpen();
        while (time < 1)
        {
            time += Time.deltaTime / openTime;
            door.transform.localPosition = Vector3.Lerp(current, target, time);
            yield return null;
        }

        audioManager.StopDoorOpen();
    }
}
