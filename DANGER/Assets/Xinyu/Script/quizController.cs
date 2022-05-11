using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;



public class quizController : MonoBehaviour
{
    //HUD de sí mismo y HUD principal
    private Canvas canvas;
    private Canvas statUI;
    private Canvas dialogUI;

    private SceneOneControl sceneControl;


    //Pregunta y opción
    public int id;
    public Text quiz;
    public Button OptionA;
    public Button OptionB;
    public Button OptionC;


    //Info personaje
    public GameObject player;
    public CharacterStat playerStat;

    //Control tiempo
    public Text countDownText;
    private string countDownString;
    private TimeController timeController;

    //Control información extra
    public GameObject otherPanel;
    public Text[] extraCountDown;
    public Image[] extraPortrait;
    public Items.ItemType[] extraResquestItem;
    private string infoToShow;
    private Sprite pictureToShow;
    private float hpToLoss;
    private float stressToLoss;
    private int TimeToLoss;

    private int resquestItemNum = 0;
    private int remainPeoples = 0;
    private int attempts;




    public enum QuizType
    {
        QUIZ,
        ASK
    };


    //
    //Usar SetQuiz(int id,string quiz,string optionA,string optionB,string optionC,int correctOption,QuizExtraInfo extr) para ->
    //-----------------------------------------
    //|             [quiz]                    |
    //|optionA - correcto si correctOption = 1|
    //|optionB - correcto si correctOption = 2|
    //|optionC - correcto si correctOption = 3|
    //-----------------------------------------
    //|  Si hay extra info, mostrarlo aquí    |
    //----------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        statUI = GameObject.FindGameObjectWithTag("TeamUI").GetComponent<Canvas>();
        timeController = GameObject.FindGameObjectWithTag("TeamUI").GetComponent<TimeController>();
        sceneControl = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();

        dialogUI = GameObject.FindGameObjectWithTag("DialogUI").GetComponent<Canvas>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerStat = player.GetComponent<CharacterStat>();

        extraResquestItem = new Items.ItemType[8];


    }
    
    // Update is called once per frame
    void Update()
    {

        countDownString = timeController.getCountDownString();
        countDownText.text = countDownString;
        //statUI.enabled = !GetComponent<Canvas>().isActiveAndEnabled;

    }

    public void SetQuiz(int questionID,string quiz,string optionA,string optionB,string optionC,int correctOption,QuizExtraInfo extra,QuizType qt = QuizType.QUIZ)
    {
        id = questionID;
        statUI.enabled = false;
        this.quiz.text = quiz;

        SetOptionA(optionA);
        SetOptionB(optionB);
        SetOptionC(optionC);

        if (qt == QuizType.QUIZ)
            SetCorrectOption(correctOption);
        else if (qt == QuizType.ASK)
            SetChoice(correctOption);

        if (extra != null) {
            hpToLoss = extra.hearthToLoss;
            stressToLoss = extra.stressToLoss;
            TimeToLoss = extra.timeToLoss;
            otherPanel.SetActive(true);
            double[] hps = extra.getHps();
            Sprite[] sps = extra.getPortraits();
            Items.ItemType[] its = extra.getItemsToCheck();
            infoToShow = extra.getInfo();
            pictureToShow = extra.getPicture();


            remainPeoples = hps.Length;
            int i;
            for (i = 0; i < hps.Length; i++)
            {
                //CountDown de otros gentes
                StartCoroutine(Time(hps[i], extraCountDown[i]));
                extraPortrait[i].sprite = sps[i];

            }
            //Poner vacio a los demás
            while (i < extraCountDown.Length) {
                extraCountDown[i].enabled = false;
                extraPortrait[i].enabled = false;
                i++;
            }

            for (int j = 0; j < its.Length; j++)
            {
                resquestItemNum++;
                extraResquestItem[j] = its[j];
            }
        }
    }


    public void SetOptionA(string text)
    {
        OptionA.GetComponentInChildren<Text>().text = text;
    }

    public void SetOptionB(string text)
    {
        OptionB.GetComponentInChildren<Text>().text = text;
    }

    public void SetOptionC(string text)
    {
        OptionC.GetComponentInChildren<Text>().text = text;
    }

    public void SetCorrectOption(int option)
    {
        switch (option)
        {
            case 1: 
                    OptionA.onClick.AddListener(Correct);
                    OptionB.onClick.AddListener(Incorrect);
                    OptionC.onClick.AddListener(Incorrect);
                    break;
            
            case 2: 
                    OptionA.onClick.AddListener(Incorrect);
                    OptionB.onClick.AddListener(Correct);
                    OptionC.onClick.AddListener(Incorrect);
                    break;
            
            case 3:
                    OptionA.onClick.AddListener(Incorrect);
                    OptionB.onClick.AddListener(Incorrect);
                    OptionC.onClick.AddListener(Correct);
                    break;
            default: break;
        }
    }


    public void SetChoice(int option)
    {
        switch (option)
        {
            case 1:
                OptionA.onClick.AddListener(Correct);
                OptionB.onClick.AddListener(Incorrect);
                OptionC.onClick.AddListener(Incorrect);
                break;

            case 2:
                OptionA.onClick.AddListener(Incorrect);
                OptionB.onClick.AddListener(Correct);
                OptionC.onClick.AddListener(Incorrect);
                break;

            case 3:
                OptionA.onClick.AddListener(Incorrect);
                OptionB.onClick.AddListener(Incorrect);
                OptionC.onClick.AddListener(Correct);
                break;
            default: break;
        }
    }

    void Correct()
    {
        //Comprobar si hay cosa que requiere
        if (resquestItemNum > 0) 
        {
            List<Items.ItemType> actualItem = playerStat.GetItemList();
            bool found = true;
            Debug.Log(resquestItemNum);
            for (int i = 0; i < resquestItemNum; i++){
                if (!actualItem.Contains(extraResquestItem[i]))
                    found = false;
            }

            if (found == false)
            {
                setCanvasActive(false);
                dialogUI.GetComponent<DialogController>().text.text = infoToShow;
                dialogUI.GetComponent<DialogController>().image.sprite = pictureToShow;
                dialogUI.GetComponent<DialogController>().enableDialog();

                playerStat.LossHp(hpToLoss);
                playerStat.LossStress(stressToLoss);
                timeController.LossTime(TimeToLoss);
                remainPeoples = 0;
            }
        }

        if (id == 1)
        {
            sceneControl.quizOneAnswered = true;
            if (attempts == 0) sceneControl.quizOneCorrect = true; 
        }
        else if (id == 2) 
        {
            sceneControl.quizTwoAnsewed = true;
            if (attempts == 0) sceneControl.quizTwoCorrect = true;
        }
        else if (id == 3)
        {
            sceneControl.quizThreeAnswered = true;
            if (attempts == 0) sceneControl.quizThreeCorrect = true;
        }
        else if (id == 4) 
        {
            sceneControl.quizFourAnswered = true;
            if (attempts == 0) sceneControl.quizFourCorrect = true;
        }
        else if (id == 5) 
        {
            sceneControl.quzFiveAnswered = true;
            if (attempts == 0) sceneControl.quizFiveCorrect = true;
        }
        statUI.GetComponent<TeammateManager>().teamNumber += remainPeoples;
        setDefault();
        setCanvasActive(false);
        statUI.enabled = true;
    }

    void Incorrect()
    {
        playerStat.LossStress(5);
        attempts++;
    }


    void setDefault()
    {
        OptionA.onClick.RemoveAllListeners();
        OptionB.onClick.RemoveAllListeners();
        OptionC.onClick.RemoveAllListeners();

        resquestItemNum = 0;
        remainPeoples = 0;
        attempts = 0;

        hpToLoss = 0;
        stressToLoss = 0;
        TimeToLoss = 0;

        StopAllCoroutines();
        for (int i = 0; i < extraCountDown.Length; i++)
        {
            extraCountDown[i].enabled = true;
            extraPortrait[i].enabled = true;
        }
        otherPanel.SetActive(false);

    }

    public void setCanvasActive(bool active) 
    {
        canvas.enabled = active;
    }

    IEnumerator Time(double time , Text label)
    {
        while (time >= 0)
        {
            TimeSpan ts = TimeSpan.FromSeconds(time);
            label.text = ts.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1);
            time--;
        }
        remainPeoples--;
    }
}
