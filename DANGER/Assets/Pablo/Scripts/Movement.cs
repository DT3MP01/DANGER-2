using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public CharacterController chara;

    void Update()
    {
       // chara = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        if (GlobalVar.mapMovement) 
        
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            // movementDirection.Normalize();


            if (Input.GetKey(KeyCode.LeftShift)) { transform.Translate(movementDirection * speed * 2f * Time.deltaTime, Space.World); }
            else { transform.Translate(movementDirection * speed * Time.deltaTime, Space.World); }
            //transform.Translate(movementDirection * speed * Time.deltaTime);
            // chara.Move(movementDirection); 

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime * 1000);
            }

        }
        
    }

}



