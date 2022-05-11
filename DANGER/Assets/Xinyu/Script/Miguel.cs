using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miguel : MonoBehaviour
{
    public Texture2D cursorTexture;
    QuizExtraInfo quizExtraInfo;



    // Start is called before the first frame update
    void Start()
    {
        quizExtraInfo = GetComponent<QuizExtraInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {


    }

    void OnMouseExit()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject quizUI = GameObject.FindGameObjectWithTag("QuizUI");

            quizController quiz = quizUI.GetComponent<quizController>();
            quiz.setCanvasActive(true);

            string quistion = "¿Cómo debo usar el extintor?";
            string optionA = "A.Quitar el seguro";
            string optionB = "B.Poner en horizontal";
            string optionC = "Apretar la palanca";
            int coorect = 1;

            //quiz.SetQuiz(id,quistion, optionA, optionB, optionC,coorect,quizExtraInfo);

        }
    }
}
