using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Material Orange;
    public Material Red;
    private Sensor fireDetector;
    void Start()
    {
       fireDetector = GetComponentInParent<Sensor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireDetector.nearbyFire)
        {
            GetComponent<MeshRenderer>().enabled = true;
            if (fireDetector.touchingFire)
            {
                ChangeMaterialRed();
            }
            else
            {
                ChangeMaterialOrange();
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void ChangeMaterialOrange() 
    {
        GetComponent<MeshRenderer>().material = Orange;
    }

    public void ChangeMaterialRed()
    {
        GetComponent<MeshRenderer>().material = Red;
    }
}
