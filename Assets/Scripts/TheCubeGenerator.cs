using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCubeGenerator : MonoBehaviour {

    public int nbRoom;
    public Transform attachsContainer;
    public Transform wallsInsideParent;
    public Material wallMaterial;
    public float wallHeight = 10.0f;

    private Transform[] attachs;
    
	void Start () {
        int count = attachsContainer.childCount;
        attachs = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            attachs[i] = attachsContainer.GetChild(i);
        }

        CreateWallBetweenAttach(attachs[0], attachs[3]);
	}
	
	void Update () {
		//Cre
	}

    void CreateWallBetweenAttach(Transform attach1, Transform attach2)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 size = (attach2.localPosition - attach1.localPosition);
        wall.transform.SetParent(wallsInsideParent, false);
        wall.transform.localScale = new Vector3(size.x, wallHeight, size.z);
        wall.transform.localPosition = attach1.localPosition + (wall.transform.localScale / 2);
        wall.transform.localPosition = new Vector3(wall.transform.localPosition.x, 4.71f, wall.transform.localPosition.z);
    }


}
