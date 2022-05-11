using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressController : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider[] stressSliders;

    private GameObject SomeOne;
    private CharacterStat Stat;
    private int CurrentState;

    public enum StressLevel
    {
        
    }
    void Start()
    {
        SomeOne = GameObject.FindGameObjectWithTag("Player");
        Stat = SomeOne.GetComponent<CharacterStat>();
        double totalStress = Stat.GetMaxStress();
        double eachSection = totalStress / stressSliders.Length;

        double currentMax = totalStress;
        double currentMin = totalStress - eachSection;
        for (int i = 0; i < stressSliders.Length; i++) {
            stressSliders[i].maxValue = (float)currentMax;
            stressSliders[i].minValue = (float)currentMin;

            currentMax = currentMin;
            currentMin -= eachSection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stressSliders.Length; i++)
        {
            stressSliders[i].value = (float)Stat.GetCurrentStress();
            if (Stat.GetCurrentStress() <= stressSliders[i].maxValue &&
               Stat.GetCurrentStress() >= stressSliders[i].minValue)
            {
                CurrentState = i;
            }

        }
    }
}
