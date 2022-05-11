using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class TeammateManager : MonoBehaviour
{
    public Image[] images;
    public Text teamNumberText;

    public int teamNumber;


    //public Dictionary<Items.ItemType, Image> imagesDict = new Dictionary<Items.ItemType, Image>();

    // Start is called before the first frame update
    void Start()
    {
        //++Protagonista
        teamNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Si desaparece hud, personaje no pude mover
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = GetComponent<Canvas>().isActiveAndEnabled;
        teamNumberText.text = teamNumber.ToString();
    }

    void Awake()
    {

    }

    public void Activate(Items.ItemType it)
    {
        if (it == Items.ItemType.ALARM)
        {
            images[0].enabled = true;
        }
        else if (it == Items.ItemType.WET_TOWEL)
        {
            images[1].enabled = true;
        }
    } 
}
