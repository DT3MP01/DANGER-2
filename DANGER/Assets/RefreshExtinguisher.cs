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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Detected");
            collision.gameObject.GetComponent<PlayerSensor>().extinguisherCapacity = 100f;
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
