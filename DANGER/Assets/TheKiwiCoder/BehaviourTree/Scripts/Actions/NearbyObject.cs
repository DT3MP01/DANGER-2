using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

public class NearbyObject : ActionNode
{
    public GameObject[] objects;
    public GameObject nearestObject;
    private NavMeshPath path;
    public string objectTag;
    private float minDistance;


    protected override void OnStart() {
        //inicializar las variables para el pathfinding

        minDistance = float.MaxValue;
        
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        objects = GameObject.FindGameObjectsWithTag(objectTag);
        if (objects == null)
        {
            return State.Failure;
        }
        foreach (GameObject points in objects)
        {
            path = new NavMeshPath();
            if (context.agent.CalculatePath(points.transform.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                float distance = Vector3.Distance(context.transform.position, path.corners[0]);
                for (int i = 1; i < path.corners.Length; i++)
                {
                    distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = points;
                }
            }
        }
        if (minDistance ==float.MaxValue){
                return State.Failure;
        }

        blackboard.moveToPosition = nearestObject.transform.position;
        return State.Success;
    }
}
