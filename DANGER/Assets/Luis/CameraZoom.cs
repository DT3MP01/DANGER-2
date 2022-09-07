using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private float scrollSpeed =15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView += Input.GetAxis("Mouse ScrollWheel")* scrollSpeed;
        
    
    
    }
}
