using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustumeDoor2 : MonoBehaviour
{
    private SceneOneControl sceneControl;
    private Canvas dialogController;
    public string textToShow;
    public Sprite imagetoShow;

    public DoorManager nextToDo;
    public GameObject dissAPear;


    // Start is called before the first frame update
    void Start()
    {
        sceneControl = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();
        dialogController = GameObject.FindGameObjectWithTag("DialogUI").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneControl.quizFourAnswered)
        {
            this.enabled = false;
            nextToDo.enabled = true;
            dissAPear.gameObject.SetActive(false);
        }

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && this.isActiveAndEnabled)
        {
            dialogController.GetComponent<DialogController>().text.text = textToShow;
            dialogController.GetComponent<DialogController>().image.sprite = imagetoShow;

            dialogController.GetComponent<DialogController>().enableDialog();
        }
    }
}
