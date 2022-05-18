using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguiserh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<ParticleSystem>().Play();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other)
    {
    // var ps = other.GetComponent<ParticleSystem>();
    // var emission = ps.emission;
    
    // emission.rateOverTime = Mathf.Min(0.0f,(float)emission.rateOverTime);

    other.GetComponent<FireScript>().startCooldown();


    }
}
