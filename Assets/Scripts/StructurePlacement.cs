using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacement : MonoBehaviour {

    private int currentlyColliding;
    
	void Start ()
    {
        currentlyColliding = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("World"))
        {
            currentlyColliding++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("World"))
        {
            currentlyColliding--;
        }
    }

    public bool IsColliding()
    {
        return currentlyColliding > 0;
    }
}
