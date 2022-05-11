using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Image fill;
    void Start()
    {
        FillSlider();

    }

    // Update is called once per frame
    void Update()
    {
        FillSlider();
    }

    public void FillSlider()
    {
        fill.fillAmount = 0.2f;
    }
}
