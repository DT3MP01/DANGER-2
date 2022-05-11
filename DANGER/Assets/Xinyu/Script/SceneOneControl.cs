using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOneControl : MonoBehaviour
{
    //Flags

    //Objectos coleccionable
    public bool fireAlarmActived = false;
    public bool wetTowelGot = false;

    //Comportamiento
    public bool followSign = true;
    public bool OpenDoorCarefully = true;
    public bool avoidFire = true;

    //Quiz correcto sin fallar
    public bool quizOneCorrect;
    public bool quizTwoCorrect;
    public bool quizThreeCorrect;
    public bool quizFourCorrect;
    public bool quizFiveCorrect;

    //Flags para bloquear pasos
    public bool quizOneAnswered = false;
    public bool quizTwoAnsewed = false;
    public bool quizThreeAnswered = false;
    public bool quizFourAnswered = false;
    public bool quzFiveAnswered = false;

    public string usedTime;

    public bool superated;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        fireAlarmActived = false;
        wetTowelGot = false;
        followSign = true;
        OpenDoorCarefully = true;
        avoidFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (quzFiveAnswered && !superated) {
            TimeController tc = GameObject.FindGameObjectWithTag("TeamUI").GetComponent<TimeController>();
            usedTime = tc.getUsedTime();
            SceneManager.LoadScene("Level1Supered");
            superated = true;
        }
    }
}
