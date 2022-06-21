using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovment : MonoBehaviour
{


    private NavMeshAgent agent; //Libreria AI, para encontrar camino
    private Animator animator;
    public string parameterHorizontal = "AxisX";
    public string parameterVertical = "AxisY";

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
        if(underControl){
            if (Input.GetMouseButtonDown(0))
            { //se traza un rayo desde el punto pulsado en la pantalla hasta el escenario
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {// si el rayo choca con algo actualiza el punto de intersección como destino
                    agent.destination = hit.point;
                    agent.stoppingDistance = 0.3f;
                    // agent.
                }
            }
            //actualizar la animación de locomoción con los parámetros de avance y giro del agente
            animator.SetFloat(parameterHorizontal, transform.InverseTransformDirection(agent.velocity).x);  
            animator.SetFloat(parameterVertical, transform.InverseTransformDirection(agent.velocity).z);
        }
        else{
            agent.destination = transform.position;
            agent.stoppingDistance = 0f;
        }


    }
}
