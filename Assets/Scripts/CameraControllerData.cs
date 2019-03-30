using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraControllerData
{
    public float currentZoom;
    public float currentYall;

    public CameraControllerData(CameraController camera)
    {
        currentZoom = camera.CurrentZoom;
        currentYall = camera.CurrentYaw;
    }
}
