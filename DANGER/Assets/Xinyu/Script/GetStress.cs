using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStress : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject SomeOne;
    private CharacterStat Stat;

    private  Slider Scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        SomeOne = GameObject.FindGameObjectWithTag("Player");
        Stat = SomeOne.GetComponent<CharacterStat>();
        Scrollbar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Scrollbar.value = (float)Stat.GetCurrentStress() / (float)Stat.GetMaxStress();
    }

}
