using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject door;
    public QuizExtraInfo extraInfo;

    private CharacterStat stat;
    private Canvas teamUI;
    private TimeController timeController;
    private Canvas dialogController;

    private bool activated;
    SceneOneControl sceneOne;

    void Start()
    {
        sceneOne = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();
        sceneOne = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();
        if (extraInfo != null)
        {
            stat = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStat>();
            teamUI = GameObject.FindGameObjectWithTag("TeamUI").GetComponent<Canvas>();
            timeController = GameObject.FindGameObjectWithTag("TeamUI").GetComponent<TimeController>();
            dialogController = GameObject.FindGameObjectWithTag("DialogUI").GetComponent<Canvas>();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !activated)
        {
                if (extraInfo != null)
                {
                    teamUI.enabled = false;
                    dialogController.GetComponent<DialogController>().text.text = extraInfo.infoToShowOnFail;

                    stat.LossHp(extraInfo.hearthToLoss);
                    stat.LossStress(extraInfo.stressToLoss);
                    timeController.LossTime(extraInfo.timeToLoss);

                    dialogController.GetComponent<DialogController>().enableDialog();
                    if (extraInfo.doorID == 1)
                    {
                        sceneOne.followSign = false;
                    }
                    else if (extraInfo.doorID == 2)
                    {
                        sceneOne.OpenDoorCarefully = false;
                    }
                }

            activated = true;

            }


    }

}
