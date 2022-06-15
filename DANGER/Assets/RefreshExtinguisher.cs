using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshExtinguisher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("Player Detected");
            collider.gameObject.GetComponent<PlayerSensor>().extinguisherCapacity = 100f;
            Destroy(gameObject.transform.parent.parent.gameObject);
        }
        if(collider.gameObject.tag == "NPC"){

            collider.gameObject.GetComponent<AiSensor>().extinguisherCapacity = 100f;
            Destroy(gameObject.transform.parent.parent.gameObject);
        }
    }
}
