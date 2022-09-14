using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public Camera mapCamera;
    public GameObject canvas;
    private bool started;
    void Start()
    {
        mainCamera.enabled = true;
        mapCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) 
        {
            //Debug.Log(GlobalVar.middleX);
            if (GlobalVar.middleX > -9000 && GlobalVar.middleZ > -9000) { started = true;}
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                //GlobalVar.mapMovement = !GlobalVar.mapMovement;
                mainCamera.enabled = !mainCamera.enabled;
                mapCamera.enabled = !mapCamera.enabled;
                canvas.SetActive(!canvas.activeSelf);
            }
            

        }
        
    }
}
