using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQManager : MonoBehaviour {

    public Transform wallsGeneratedParent;
    public GameObject pillarParent;
    private List<HQPillar> pillars;
    private bool hasGenerate;
	// Use this for initialization
	void Start ()
    {
        hasGenerate = false;
        pillars = new List<HQPillar>();
	    foreach(Transform pillar in pillarParent.transform)
        {
            pillar.GetComponent<HQPillar>().wallsParent = wallsGeneratedParent;
            pillars.Add(pillar.GetComponent<HQPillar>());
        }
	}

    public void Shutdown()
    {
        foreach (Transform pillar in pillarParent.transform)
        {
            pillar.GetComponent<HQPillar>().Deactivate();
            Destroy(pillar.GetComponent<HQPillar>());
        }
    }
}
