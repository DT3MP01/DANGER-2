using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {

         slider = gameObject.GetComponentInParent<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value <= 0.333) { GetComponent<TMPro.TextMeshProUGUI>().text = "Calm"; }
        if (slider.value > 0.333 && slider.value < 0.666) { GetComponent<TMPro.TextMeshProUGUI>().text = "Stressed"; }
        if (slider.value >= 0.666) { GetComponent<TMPro.TextMeshProUGUI>().text = "Panic"; }
    }
}
