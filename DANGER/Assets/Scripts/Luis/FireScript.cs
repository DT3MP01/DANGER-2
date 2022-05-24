using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public float fireExtinguisherTime;
    public bool reignite;
    void Start()
    {
        fireExtinguisherTime = 8f;
        reignite = false;
    }

    // Start is called before the first frame update
    void Update()
    {
        if(reignite){
        fireExtinguisherTime -= Time.deltaTime;
        if (fireExtinguisherTime <= 0)
        {
            GetComponent<ParticleSystem>().Play();
            reignite=false;
        }
        }

    }

    public void startCooldown()
    {
        GetComponent<ParticleSystem>().Stop();
        fireExtinguisherTime = 8f;
        reignite = true;
    }

}
