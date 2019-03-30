using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GUIManager guiManager;
    public Player player;
    public Vector3 offset;
    public float pitch = 2.0f;
    public float zoomSpeed = 4.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 15.0f;
    public float yawSpeed = 200.0f;

    private float currentZoom = 10f;
    public float CurrentZoom
    {
        get { return currentZoom; }
        private set { }
    }
    private float currentYaw = 0.0f;
    public float CurrentYaw
    {
        get { return currentYaw; }
        private set { }
    }

    private void Start()
    {
        if(InGameInformationHolder.dataSave != null)
        {
            currentZoom = InGameInformationHolder.dataSave.cameraData.currentZoom;
            currentYaw = InGameInformationHolder.dataSave.cameraData.currentYall;
        }
    }

    void Update()
    {
        if(!guiManager.InMenu())
        {
            if(!player.isBuilding)
            {
                currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            }
            
            if (Input.GetMouseButton(2))
            {
                float x = Input.GetAxis("Mouse X");
                currentYaw += x * yawSpeed * Time.deltaTime;
            }
        }
    }

    void LateUpdate ()
    {
        transform.position = player.transform.position - offset * currentZoom;
        transform.LookAt(player.transform.position + Vector3.up * pitch);
        
        transform.RotateAround(player.transform.position, Vector3.up, currentYaw);
    }
}
