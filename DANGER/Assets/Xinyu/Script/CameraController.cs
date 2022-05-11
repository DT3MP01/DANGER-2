using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    //Distance between the target and the player
    public Vector3 cameraOffset;

    //Smooth factor for Camera rotation
    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        cameraOffset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = target.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position,newPosition,smoothFactor);
    }
}
