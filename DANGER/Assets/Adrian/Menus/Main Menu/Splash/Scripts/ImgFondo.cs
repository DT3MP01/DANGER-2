using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImgFondo : MonoBehaviour
{
    const float ImageWidth = 2000.0f,
                  TimeOut = 10.0f;
    public enum SplashStates
    {
        Moving,
        Finish
    }

    public SplashStates State;
    public Vector2 Speed;

    float startTime;

    Image image;
    // Start is called before the first frame update
    void Start()
    {
        State = SplashStates.Moving;
        startTime = Time.time;
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case SplashStates.Moving:
                transform.Translate(Speed.x * Time.deltaTime, Speed.y * Time.deltaTime, 0.0f);
                break;
            case SplashStates.Finish:
                SceneManager.LoadScene("Menu Principal");
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
