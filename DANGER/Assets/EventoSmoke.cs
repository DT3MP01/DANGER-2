using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class EventoSmoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Si contesta la pregunta, deja pasar
        if (GetComponent<Question>().answered == true)
        {
            GetComponent<BoxCollider>().isTrigger = GetComponent<Question>().answered;

            //Ignore raycast, no clicable
            this.gameObject.layer = 2;

        }
    }
}
