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

        
        
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other)
    {
    // var ps = other.GetComponent<ParticleSystem>();
    // var emission = ps.emission;
    
    // emission.rateOverTime = Mathf.Min(0.0f,(float)emission.rateOverTime);
    if(other.tag == "Fire")
    {
        other.GetComponent<FireScript>().startCooldown();
    }
    }
}
