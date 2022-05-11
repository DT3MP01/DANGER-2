using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SummaryLevel1 : MonoBehaviour
{
    public Sprite wrong;
    public Text text;

    public Image[] images;

    private SceneOneControl sceneOne;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        sceneOne = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();
        text.text = sceneOne.usedTime;
        if (sceneOne.avoidFire == false) {
            images[0].sprite = wrong;
        }
        if (sceneOne.fireAlarmActived == false)
        {
            images[1].sprite = wrong;
        }
        if (sceneOne.followSign == false)
        {
            images[2].sprite = wrong;
        }
        if (sceneOne.wetTowelGot == false)
        {
            images[3].sprite = wrong;
        }
        if (sceneOne.OpenDoorCarefully == false)
        {
            images[4].sprite = wrong;
        }
    }
}
