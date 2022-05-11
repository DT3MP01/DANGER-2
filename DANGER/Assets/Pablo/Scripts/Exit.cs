using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Yo ,"+gameObject.name+" colision con:"+collision.gameObject.name);
        if (collision.gameObject.name.Substring(0, 7) == "Capsule") { 
            
            collision.gameObject.SetActive(false);
            GlobalVar.remainingNPCs -= 1;
        }
        
        
    }
    
}
