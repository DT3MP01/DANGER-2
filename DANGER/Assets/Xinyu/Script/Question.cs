using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    private SceneOneControl sceneControl;

    public bool answered = false;
    public int id;
    public string quistion;
    public string optionA;
    public string optionB;
    public string optionC;

    public QuizExtraInfo quizExtraInfo;

    public int correcto;

    // Start is called before the first frame update
    void Start()
    {
        sceneControl = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //Hay que generalizar que alguna forma...
        if (id == 1 && sceneControl.quizOneAnswered) {

            answered = true;
        }
        else if (id == 2 && sceneControl.quizTwoAnsewed)
        {
            answered = true;
        }
        else if (id == 3 && sceneControl.quizThreeAnswered)
        {
            answered = true;
        }
        else if (id == 4 && sceneControl.quizFourAnswered)
        {
            answered = true;
        }
        else if (id == 5 && sceneControl.quizFiveCorrect)
        {
            answered = true;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !answered)
        {
            GameObject quizUI = GameObject.FindGameObjectWithTag("QuizUI");

            quizController quiz = quizUI.GetComponent<quizController>();
            quiz.setCanvasActive(true);

            quiz.SetQuiz(id,quistion, optionA, optionB, optionC, correcto, quizExtraInfo);
            answered = true;

        }
    }
    
}
