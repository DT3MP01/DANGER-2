using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{

    private Vector3 endPos;

    public int moveSpeed = 10;
    
    public NavMeshAgent agent;
    public int agentID;
    public float secsUntilWander;
    public GameObject text;
    private static List<WorldGenerator.generatorPoint> rooms;
    private static List<int> widths;
    private static List<int> lengths;
    private bool countStarted;
    private float start_time;
    public float stressLv;
    private Color originalColor;
    private int nextUpdate = 1;
    RayCast rc;
    public Animator animator;
    private bool selected;


    // Update is called once per frame
    private void Start()
    {

        endPos = transform.position;

        agentID = GlobalVar.agentCounter++;
        rooms = GlobalVar.rooms;
        widths = GlobalVar.widths;
        lengths = GlobalVar.lengths;
        countStarted = false;
        stressLv = Random.Range(0f, 1f);
        originalColor = GetComponent<Collider>().GetComponent<Renderer>().material.color;
        // No hace falta raycast de momento
        //rc = GetComponent<RayCast>();
        animator = GetComponent<Animator>();

        // - After 0 seconds, prints "Starting 0.0 seconds"
        // - After 0 seconds, prints "Coroutine started"
        // - After 2 seconds, prints "Coroutine ended: 2.0 seconds"
        //print("Starting " + Time.time + " seconds");

        // Start function WaitAndPrint as a coroutine.

        //coroutine = WaitAndPrint(2.0f);
        //StartCoroutine(Wander(secsUntilWander));




    }
    void FixedUpdate()
    {

        //Presionar bot¨®n izquierdo
        if (Input.GetMouseButtonDown(0)) 
        {
            //Posici¨®n bot¨®n
            Move();

        }

        if (endPos != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * moveSpeed);
        }

        
    }
    private void Move()
    {
        Debug.Log("Moving");
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition); //Un rayo de camara a cursos
        RaycastHit hit; //Cosa que vamos a chocar

        if (Physics.Raycast(ray, out hit, 1000)) {
            if (hit.collider.gameObject.tag == "Terrain") {
                endPos = hit.point;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Terrain") {
            Debug.Log("Auuu");
        }

    }

    private IEnumerator Wander(float secsUntilWander)
    {
        while (true)
        {

            print("Due to inactivity, I: " + agentID + "start wandering");

            Fisher_YatesShuffle(rooms);
            WorldGenerator.generatorPoint chosenRoom = rooms[0];

            agent.SetDestination(chosenRoom.coords);

            yield return new WaitForSeconds(secsUntilWander);
        }




    }
    public void incrementStress(float increment)
    {
        stressLv += increment;
    }

    void Fisher_YatesShuffle(List<WorldGenerator.generatorPoint> a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Número aleatorio entre 0 y i (de forma que i decrementa cada iteración)
            int rnd = UnityEngine.Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            WorldGenerator.generatorPoint temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posición aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }

    public void runFromFire() {

        countStarted = false;
        print("Due to seing fire, I: " + agentID + "change course");

        //Fisher_YatesShuffle(rooms);
        int roomIndex = Random.Range(0, rooms.Count - 1);
        WorldGenerator.generatorPoint chosenRoom = rooms[roomIndex];
        animator.SetBool("Walk", true);
        agent.SetDestination(chosenRoom.coords + new Vector3(Random.Range(-widths[roomIndex] / 2f, widths[roomIndex] / 2f), 0, Random.Range(-lengths[roomIndex] / 2f, lengths[roomIndex] / 2f)));
        
    }
    /*
    private IEnumerator Run()
    {
        
        yield return new WaitForSeconds(5);
        countStarted = false;
        print("Due to seing fire, I: " + agentID + "change course");

        //Fisher_YatesShuffle(rooms);
        int roomIndex = Random.Range(0, rooms.Count - 1);
        WorldGenerator.generatorPoint chosenRoom = rooms[roomIndex];

        agent.SetDestination(chosenRoom.coords + new Vector3(Random.Range(-widths[roomIndex] / 2f, widths[roomIndex] / 2f), 0, Random.Range(-lengths[roomIndex] / 2f, lengths[roomIndex] / 2f)));
        yield return new WaitForSeconds(5);
       

    }
     */
}
