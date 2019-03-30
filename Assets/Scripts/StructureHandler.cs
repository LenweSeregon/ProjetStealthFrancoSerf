using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StructureHandler : MonoBehaviour {

    public NavMeshSurface surface;

    public Player player;
    public GUIManager guiManager;
    public Camera mainCamera;
    public LayerMask worldLayer;
    public GameObject structuresParent;

    private float mouseWheelRotation;

    private bool angleOK;
    private bool canBuild;
    private Color savedColor;
    private GameObject currentStructure;
    private StructurePlacement currentStructurePlacement;
    public Action callbackAtPlacementValidated;
    private GameObject structureToPlace;
    public GameObject StructureToPlace
    {
        get { return structureToPlace; }
        set
        {
            if(value == null)
            {
                structureToPlace = null;
                player.isBuilding = false;
            }
            else
            {
                angleOK = false;
                canBuild = false;
                structureToPlace = value;
                player.isBuilding = true;
                guiManager.SwitchToWindow("null");
                currentStructure = Instantiate(structureToPlace);
                currentStructure.layer = LayerMask.NameToLayer("InPlacement");
                savedColor = currentStructure.GetComponentInChildren<MeshRenderer>().material.color;

                Color currentColor = currentStructure.GetComponentInChildren<MeshRenderer>().material.color;
                currentColor.a = 0.5f;
                currentStructure.GetComponentInChildren<MeshRenderer>().material.color = currentColor;
                currentStructure.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                currentStructure.GetComponentInChildren<Collider>().isTrigger = true;
                currentStructure.GetComponentInChildren<NavMeshObstacle>().enabled = false;
                currentStructurePlacement = currentStructure.AddComponent<StructurePlacement>();

                if (currentStructure.GetComponent<Interactable>() != null)
                {
                    currentStructure.GetComponent<Interactable>().enabled = false;

                }
            }
        }
    }
    
    void Start () {
        currentStructure = null;	
	}
	
	void Update () {
		
        if(currentStructure != null)
        {
            MoveStructureToMouse();
            RotateFromMouseWheel();
            UpdateCanBuild();
            PlaceStructure();
        }
	}

    private void UpdateCanBuild()
    {
        Color colorInformation;
        if(!currentStructurePlacement.IsColliding() && angleOK)
        {
            canBuild = true;
            colorInformation = Color.green;
        }
        else
        {
            canBuild = false;
            colorInformation = Color.red;
        }
        colorInformation.a = 0.5f;
        currentStructure.GetComponentInChildren<MeshRenderer>().material.color = colorInformation;
    }

    public void RemoveCurrentStructurePlacing()
    {
        if(currentStructure != null)
        {
            Destroy(currentStructure);
        }
    }

    private void PlaceStructure()
    {
        if(Input.GetMouseButtonDown(1) && canBuild)
        {
            Color currentColor = savedColor;
            currentColor.a = 1;
            currentStructure.transform.SetParent(structuresParent.transform, true);
            currentStructure.GetComponentInChildren<MeshRenderer>().material.color = currentColor;
            currentStructure.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            currentStructure.GetComponentInChildren<Collider>().isTrigger = false;
            currentStructure.GetComponentInChildren<NavMeshObstacle>().enabled = true;
            Destroy(currentStructure.GetComponent<StructurePlacement>());

            if (currentStructure.GetComponent<Interactable>() != null)
            {
                currentStructure.GetComponent<Interactable>().enabled = true;
                currentStructure.layer = LayerMask.NameToLayer("Default");
            }
            currentStructure = null;
            player.isBuilding = false;
            callbackAtPlacementValidated.Invoke();
        }
    }

    private void MoveStructureToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, int.MaxValue, worldLayer))
        {
            currentStructure.transform.position = hitInfo.point + new Vector3(0,(currentStructure.transform.localScale.y / 2), 0);
            currentStructure.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            float angle = Vector3.Angle(hitInfo.transform.right, hitInfo.normal);
            angleOK = (angle >= 75 && angle <= 105);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentStructure.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }
}
