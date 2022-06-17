using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OpenDoor : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Detected");
            anim.SetBool("isOpen", true);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            GetComponent<CustomCursor>().enabled = false;
        }
    }
    void OnTriggerExit(Collider other){
        anim.SetBool("isOpen", false);
        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<CustomCursor>().enabled = true;

    }
}
