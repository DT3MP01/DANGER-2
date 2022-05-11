using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    // Start is called before the first frame update
    public Text countDown;
    public int TotalTime = 60;
    public int CurrentTime = 60;

    private string CountDownString = "";

    void Start()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        while (TotalTime >= 0)
        {
            TimeSpan ts = TimeSpan.FromSeconds(CurrentTime);
            CountDownString = ts.ToString(@"mm\:ss");
            countDown.text = CountDownString;
            yield return new WaitForSeconds(1);
            CurrentTime--;

            if (CurrentTime == 0)
                SceneManager.LoadScene("GameOver");
        }
    }

    void update()
    {

    }

    public void setCurrentTime(int time)
    {
        CurrentTime += time;
    }

    public void LossTime(int time)
    {
        TotalTime -= time;
    }

    public string getCountDownString()
    {
        return CountDownString;
    }

    public string getUsedTime()
    {
        int usedTime = TotalTime - CurrentTime;
        return TimeSpan.FromSeconds(usedTime).ToString(@"mm\:ss");
    }
}
