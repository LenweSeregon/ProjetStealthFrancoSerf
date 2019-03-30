using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public float radius = 1.0f;

    public virtual void Interact(Player player)
    {
        Debug.Log("Interact with :" + transform.name);
    }

    public virtual bool IsInteractable()
    {
        return true;
    }

    public virtual string GetInteractableTextI18nID()
    {
        return "thecube.interaction.text.basicInteraction";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
