using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

public class NearbyFireExtinguisher : ActionNode
{
    private GameObject[] extinguishers;
    private GameObject nearestExtinguisher;
    private NavMeshPath path;
    private float minDistance;


    protected override void OnStart() {
        //inicializar las variables para el pathfinding

        minDistance = float.MaxValue;
        extinguishers = GameObject.FindGameObjectsWithTag("Extinguisher");

        foreach (GameObject fire in extinguishers) {
            path = new NavMeshPath();
            if (context.agent.CalculatePath(fire.transform.position, path) && path.status == NavMeshPathStatus.PathComplete) {
                float distance = Vector3.Distance(context.transform.position, path.corners[0]);
                for (int i = 1; i < path.corners.Length; i++) {
                    distance += Vector3.Distance(path.corners[i-1], path.corners[i]);
                }

                if (distance < minDistance) {
                        minDistance = distance;
                        nearestExtinguisher = fire;
                    }
            }
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(nearestExtinguisher ==null){
                return State.Failure;
        }

        blackboard.moveToPosition = nearestExtinguisher.transform.position;
        return State.Success;
    }
}
