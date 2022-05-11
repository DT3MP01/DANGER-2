using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Material Orange;
    public Material Red;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
