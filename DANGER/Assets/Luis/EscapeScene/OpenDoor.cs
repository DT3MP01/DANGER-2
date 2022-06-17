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
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(CloseDoor());
        }
    }
    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("isOpen", false);
        GetComponent<BoxCollider>().enabled = true;
    }
}
