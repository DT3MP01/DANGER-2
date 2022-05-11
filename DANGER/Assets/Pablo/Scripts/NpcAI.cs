using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAI : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    private Animator animator;
    private bool followMainChara;

    private SceneOneControl sceneControl;
    public int NPCid;

    // Start is called before the first frame update
    void Start()
    {
        sceneControl = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();

        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        animator = GetComponent<Animator>();

        followMainChara = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.hasPath)
            animator.SetBool("Walk", false);
        else
            animator.SetBool("Walk", true);

        if (NPCid == 1 && sceneControl.quizTwoAnsewed)
        {
            followMainChara = true;
        }
        else if (NPCid == 2 && sceneControl.quizThreeAnswered) {
            followMainChara = true;
        }


        if (followMainChara) {
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= wanderTimer && followMainChara == false)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
