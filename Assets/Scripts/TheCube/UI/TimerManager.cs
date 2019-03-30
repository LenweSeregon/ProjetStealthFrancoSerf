using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private bool hasStart;
    private float timer;
    public float timerStart = 30.0f;
    public CubeManager cubeManager;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        hasStart = false;
        timerText.gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        hasStart = true;
        timer = timerStart;
        timerText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(hasStart)
        {
            timer -= Time.deltaTime;
            int asInt = (int)timer;
            timerText.text = "00:" + asInt.ToString("00") + "";

            if(timer < 0)
            {
                cubeManager.Lose(CubeManager.LoseReason.OVER_TIME);
            }
        }
    }
}
