using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

public class RandomPosition : ActionNode
{
    public Vector2 min = Vector2.one * -10;
    public Vector2 max = Vector2.one * 10;

    public float radius= 5;
    private Vector3 randomDirection;
    
    NavMeshHit hit;
    Vector3 finalPosition = Vector3.zero;



    protected override void OnStart() {
        randomDirection = Random.insideUnitSphere * radius;
        randomDirection += context.transform.position;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
             randomDirection = hit.position;
         }
         else{
                randomDirection = context.transform.position;
         }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        blackboard.moveToPosition.x = randomDirection.x;
        blackboard.moveToPosition.z = randomDirection.z;
        return State.Success;
    }
}
