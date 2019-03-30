using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgnimaDoorManager : MonoBehaviour {

    public bool systemReady;

    public AudioManager audioManager;
    public float openCloseTime = 1.0f;
    public float timeBeforeClosing = 5.0f;
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;

    private IEnumerator openingDoor1IEn;
    private IEnumerator closingDoor1IEn;
    private IEnumerator openingDoor2IEn;
    private IEnumerator closingDoor2IEn;
    private IEnumerator openingDoor3IEn;
    private IEnumerator closingDoor3IEn;
    private IEnumerator openingDoor4IEn;
    private IEnumerator closingDoor4IEn;

    private bool forEverDoor;

    private float time;
    [HideInInspector]
    public bool hasStartTimer;
    [HideInInspector]
    public bool startTimer;
    
	void Start ()
    {
        forEverDoor = false;
        systemReady = true;
        time = 0.0f;
        startTimer = false;
        hasStartTimer = false;

        openingDoor1IEn = OpenDoor(door1);
        closingDoor1IEn = CloseDoor(door1);

        openingDoor2IEn = OpenDoor(door2);
        closingDoor2IEn = CloseDoor(door2);

        openingDoor3IEn = OpenDoor(door3);
        closingDoor3IEn = CloseDoor(door3);

        openingDoor4IEn = OpenDoor(door4);
        closingDoor4IEn = CloseDoor(door4);
    }

    public void StartTimer()
    {
        systemReady = false;
        time = 0.0f;
        hasStartTimer = false;
        startTimer = true;
    }
	
	void Update ()
    {
		if(startTimer && !hasStartTimer)
        {
            hasStartTimer = true;
            systemReady = false;
        }

        if(hasStartTimer && !forEverDoor)
        {
            time += Time.deltaTime;
            if (time >= timeBeforeClosing)
            {
                CloseDoors();
            }
        }
	}

    public void OpenDoorsForEver()
    {
        if(forEverDoor == false)
        {
            systemReady = false;
            forEverDoor = true;
            OpenDoor1();
            OpenDoor2();
            OpenDoor3();
            OpenDoor4();
        }
    }

    public void CloseDoors()
    {
        systemReady = false;
        CloseDoor1();
        CloseDoor2();
        CloseDoor3();
        CloseDoor4();
    }

    public void OpenDoor1()
    {
        openingDoor1IEn = OpenDoor(door1);
        StopCoroutine(closingDoor1IEn);
        StartCoroutine(openingDoor1IEn);
    }
    public void OpenDoor2()
    {
        openingDoor2IEn = OpenDoor(door2);
        StopCoroutine(closingDoor2IEn);
        StartCoroutine(openingDoor2IEn);
    }
    public void OpenDoor3()
    {
        openingDoor3IEn = OpenDoor(door3);
        StopCoroutine(closingDoor3IEn);
        StartCoroutine(openingDoor3IEn);
    }
    public void OpenDoor4()
    {
        openingDoor4IEn = OpenDoor(door4);
        StopCoroutine(closingDoor4IEn);
        StartCoroutine(openingDoor4IEn);
    }

    public void CloseDoor1()
    {
        closingDoor1IEn = CloseDoor(door1);
        StopCoroutine(openingDoor1IEn);
        StartCoroutine(closingDoor1IEn);
    }
    public void CloseDoor2()
    {
        closingDoor2IEn = CloseDoor(door2);
        StopCoroutine(openingDoor2IEn);
        StartCoroutine(closingDoor2IEn);
    }
    public void CloseDoor3()
    {
        closingDoor3IEn = CloseDoor(door3);
        StopCoroutine(openingDoor3IEn);
        StartCoroutine(closingDoor3IEn);
    }
    public void CloseDoor4()
    {
        closingDoor4IEn = CloseDoor(door4);
        StopCoroutine(openingDoor4IEn);
        StartCoroutine(closingDoor4IEn);
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
            time += Time.deltaTime / openCloseTime;
            door.transform.localPosition = Vector3.Lerp(current, target, time);
            yield return null;
        }

        audioManager.StopDoorOpen();
    }

    private IEnumerator CloseDoor(GameObject door)
    {
        audioManager.PlayDoorOpen();
        Vector3 current = door.transform.localPosition;
        Vector3 target = door.transform.localPosition;
        target.y = 1.14f;
        float time = 0.0f;

        while (time < 1)
        {
            time += Time.deltaTime / openCloseTime;
            door.transform.localPosition = Vector3.Lerp(current, target, time);
            yield return null;
        }
        audioManager.StopDoorOpen();
        startTimer = false;
        hasStartTimer = false;
        systemReady = true;
    }
}
