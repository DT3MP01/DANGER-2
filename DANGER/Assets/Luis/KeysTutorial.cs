using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeysTutorial : MonoBehaviour
{
    public Sensor playerStats;
    
    private bool hasSeenSmoke;
    private bool hasSeenExtinguisher;
    // Start is called before the first frame update
    void Start()
    {
        hasSeenSmoke = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasSeenSmoke && playerStats.nearbySmoke && !playerStats.isCrawling )
        {
            hasSeenSmoke= true;
            GetComponent<TMP_Text>().text = "Presiona Control para gatear";
            StartCoroutine(RemoveDialog());
        }
        else if(!hasSeenExtinguisher &&playerStats.extinguisherCapacity == 100)
        {
            GetComponent<TMP_Text>().text = "Presiona Space para usar el extintor";
            hasSeenExtinguisher = true;
            StartCoroutine(RemoveDialog());
        }
    }
    IEnumerator RemoveDialog()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<TMP_Text>().text = "";

    }

}
