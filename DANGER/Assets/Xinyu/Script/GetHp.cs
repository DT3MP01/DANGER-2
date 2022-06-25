using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHp : MonoBehaviour
{
    private GameObject SomeOne;
    private Sensor Stat;

    private Slider Scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        SomeOne = GameObject.FindGameObjectWithTag("Player");
        Scrollbar = GetComponent<Slider>();
        Stat = SomeOne.GetComponent<Sensor>();
    }

    // Update is called once per frame
    void Update()
    {
        Scrollbar.value = (float)Stat.GetCurrentHp() / (float)Stat.GetMaxHp();
    }

}
