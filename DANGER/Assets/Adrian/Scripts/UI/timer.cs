using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class timer : MonoBehaviour
{
    public bool isActive;
    public int startMinutes;
    public string timerString;
    float currentTime;
    [SerializeField] GameObject currentTimeText;

    void Start()
    {
        isActive = true;
        currentTime = startMinutes * 60;
    }

    void Update()
    {
        if (isActive)
        {
            currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        if(time.Minutes < 10)
        {
            if(time.Seconds < 10) timerString = "0" + time.Minutes.ToString() + ":0" + time.Seconds.ToString();
            else timerString = "0" + time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }
        else
        {
            if (time.Seconds < 10) timerString = time.Minutes.ToString() + ":0" + time.Seconds.ToString();
            else timerString = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }

        currentTimeText.GetComponent<TMPro.TextMeshProUGUI>().text = timerString;
    }
}
