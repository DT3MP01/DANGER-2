using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject quizUI = GameObject.FindGameObjectWithTag("QuizUI");

            quizController quiz = quizUI.GetComponent<quizController>();
            quiz.setCanvasActive(true);

            string quistion = "¿Cómo debo usar el casa?";
            string optionA = "A.Quitar el abanico";
            string optionB = "B.Poner en wua";
            string optionC = "C.Iuuu la palanca";
            int coorect = 1;

            //quiz.SetQuiz(quistion, optionA, optionB, optionC, coorect, null);

        }
    }
}
