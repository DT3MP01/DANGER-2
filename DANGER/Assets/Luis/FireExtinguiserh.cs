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
        if(other.tag == "Fire")
        {
            other.GetComponent<FireScript>().stopFire();
        }
    }
}
