using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InfoController : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public int TotalTime = 60;


    void Start()
    {
        StartCoroutine(Time()); //Corutina, para controlar tiempo
    }

    IEnumerator Time()
    {
        while (TotalTime >= 0)
        {
            TimeSpan ts = TimeSpan.FromSeconds(TotalTime);
            //text.text = ts.ToString("@\:mm\:ss");
            yield return new WaitForSeconds(1);
            TotalTime--;
        }
    }

    void update()
    {
        if (TotalTime == 0)
        {
            Debug.Log("game over");
        }
    }

}
