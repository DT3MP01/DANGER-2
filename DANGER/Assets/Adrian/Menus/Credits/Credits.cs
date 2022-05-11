using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    const float TimeOut = 20.0f;
    public enum SplashStates
    {
        Start,
        Finish
    }

    public SplashStates State;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        State = SplashStates.Start;
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case SplashStates.Start:
                
                break;
            case SplashStates.Finish:
                SceneManager.LoadScene("splash");
                break;
            default:
                break;
        }

        if (Time.time - startTime > TimeOut)
            State = SplashStates.Finish;

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            State = SplashStates.Finish;
    }
}
