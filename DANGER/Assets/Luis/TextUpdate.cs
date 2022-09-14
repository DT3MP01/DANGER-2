using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    private TMP_InputField text;
    void Start()
    {
        text = GetComponent<TMP_InputField>();
        SetValueSlider();

    }

    // Update is called once per frame
    public void SetValueSlider()
    {
        if(text.text == "")
        {
           text.text = ((int)slider.minValue).ToString();
        }
        else if(int.Parse(text.text) >= slider.maxValue)
        {
           text.text = ((int)slider.maxValue).ToString();

        }
        slider.value = int.Parse(text.text);
    }
    public void SetValueText()
    {
        text.text =  ((int)slider.value).ToString();
    }
}
