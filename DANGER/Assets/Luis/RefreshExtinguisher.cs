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
        if(collider.gameObject.tag == "Player" || collider.gameObject.tag == "NPC")
        {
            collider.gameObject.GetComponent<Sensor>().extinguisherCapacity = 100f;
            Destroy(gameObject.transform.parent.parent.gameObject);
        }
    }
}
