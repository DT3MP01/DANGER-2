using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{


    public GameObject particle;
    public Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        particle.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Clicked");
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            particle.SetActive(true);
            particle.transform.position = new Vector3(mousePos.x,mousePos .y + 20, mousePos.z);


        }

        if (Input.GetMouseButtonUp(0))
        {
            particle.SetActive(false);
        }
    }
}
