using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAlarm : MonoBehaviour
{
    private AudioSource AS;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "NPC")
        {
            if(!GameObject.FindGameObjectWithTag("GameController").GetComponent<FireGeneration>().alarmMitigation)
            GameObject.FindGameObjectWithTag("GameController").GetComponent<FireGeneration>().alarmMitigation = true;
            AS.Play();
        }
    }

}
