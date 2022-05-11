using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalVar.stressBar <= 0.333) { GetComponent<TMPro.TextMeshProUGUI>().text = "Calm"; }
        if (GlobalVar.stressBar > 0.333 && GlobalVar.stressBar < 0.666) { GetComponent<TMPro.TextMeshProUGUI>().text = "Stressed"; }
        if (GlobalVar.stressBar >= 0.666) { GetComponent<TMPro.TextMeshProUGUI>().text = "Panic"; }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar.stressBar <= 0.333) { GetComponent<TMPro.TextMeshProUGUI>().text = "Calm"; }
        if (GlobalVar.stressBar > 0.333 && GlobalVar.stressBar < 0.666) { GetComponent<TMPro.TextMeshProUGUI>().text = "Stressed"; }
        if (GlobalVar.stressBar >= 0.666) { GetComponent<TMPro.TextMeshProUGUI>().text = "Panic"; }
    }
}
