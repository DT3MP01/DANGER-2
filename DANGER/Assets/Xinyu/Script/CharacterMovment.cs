using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovment : MonoBehaviour
{


    private NavMeshAgent agent; //Libreria AI, para encontrar camino
    private Animator animator;

    public bool underControl;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (underControl)
        {

            if (!agent.hasPath)
                animator.SetBool("Walk", false);
            else
                animator.SetBool("Walk", true);

            if (Input.GetMouseButtonDown(0))
            {
                Move();
            }
        }
        else
        {
            //Quieto xd
            //agent.SetDestination(transform.position);

        }

    }

    private void Move()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition); //Un rayo de camara a cursos
        RaycastHit hit; //Cosa que vamos a chocar

        if (Physics.Raycast(ray, out hit))
        {
            //if (hit.collider.gameObject.tag == "Terrain" || hit.collider.gameObject.tag == "DangerFire" || hit.collider.gameObject.tag == "Fire")
            //{
                agent.SetDestination(hit.point);
                transform.LookAt(hit.point);
            //}

        }
    }


}
