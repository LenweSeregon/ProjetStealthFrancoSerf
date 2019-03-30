using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour {

    public AudioManager audioManager;
    public GameObject[] alarms;
    public float alarmTick = 0.2f;

    private bool alarmSystemTriggered;
    private IEnumerator alarmSystem;

    private void Awake()
    {
        alarmSystemTriggered = false;
        alarmSystem = System();
    }

    private void Start()
    {
        ShutDownAlarmSystem();
        StartCoroutine(alarmSystem);
    }

    private void OnDestroy()
    {
        StopCoroutine(alarmSystem);   
    }

    public void TriggerAlarmSystem()
    {
        foreach (GameObject alarm in alarms)
        {
            alarm.GetComponentInChildren<Light>().enabled = true;
        }
        audioManager.PlayAlarm();
        alarmSystemTriggered = true;
    }

    public void ShutDownAlarmSystem()
    {
        foreach (GameObject alarm in alarms)
        {
            alarm.GetComponentInChildren<Light>().enabled = false;
        }
        audioManager.StopAlarm();
        alarmSystemTriggered = false;
    }

    private IEnumerator System()
    {
        bool alter = false;
        float time = 0.0f;
        while(true)
        {
            time += Time.deltaTime;
            if(alarmSystemTriggered)
            {
                if (time > alarmTick)
                {
                    time = 0.0f;
                    alter = !alter;
                    if (alter)
                    {
                        foreach (GameObject alarm in alarms)
                        {
                            if (alarm.GetComponentInChildren<Light>() != null)
                            {
                                alarm.GetComponentInChildren<Light>().range = 20;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject alarm in alarms)
                        {
                            if (alarm.GetComponentInChildren<Light>() != null)
                            {
                                alarm.GetComponentInChildren<Light>().range = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (GameObject alarm in alarms)
                {
                    if (alarm.GetComponent<Light>() != null)
                    {
                        alarm.GetComponent<Light>().range = 0;
                    }
                }
            }
            yield return null;
        }
    }
}
