using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public float scrollSpeed =15f;
    public float maxZoom = 60;
    public float minZoom = 40;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView -= Input.GetAxis("Mouse ScrollWheel")* scrollSpeed;
        if(GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView >= maxZoom)
        {

            GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = maxZoom;
        }
        else if(GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView <= minZoom)
        {
            GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = minZoom;
        }

}
}
