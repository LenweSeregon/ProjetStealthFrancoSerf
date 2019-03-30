using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {

    private float timeInSecond;
    public float TimeInSecond
    {
        get { return timeInSecond; }
        private set { }
    }

    public Player player;
    public new CameraController camera;

	// Use this for initialization
	void Start ()
    {
        
        Time.timeScale = 1.0f;
        if (InGameInformationHolder.dataSave != null)
        {
            timeInSecond = InGameInformationHolder.dataSave.ingameSeconds;
        }
        else
        {
            timeInSecond = 0.0f;
        }

        ItemActionCollection.BuildActionsCollection();
        ItemCollection.Load("Datas/Items");
        ResourcePointDropProbabilityCollection.Load("Datas/ResourcePointDropProbability");
        CraftRecipeCollection.Load("Datas/CraftRecipes");
        FurnaceCollection.Load("Datas/Furnace");
        StoryFragmentCollection.Load("Datas/Story");

    }
	
	// Update is called once per frame
	void Update ()
    {
        timeInSecond += Time.deltaTime;
	}
}
