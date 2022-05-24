using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToPosition : ActionNode
{
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public string parameterHorizontal = "AxisX";
    public string parameterVertical = "AxisY";
    protected override void OnStart() {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.animator.SetFloat(parameterHorizontal, context.transform.InverseTransformDirection(context.agent.velocity).x);  
        context.animator.SetFloat(parameterVertical, context.transform.InverseTransformDirection(context.agent.velocity).z);
        if (context.agent.pathPending) {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            context.animator.SetFloat(parameterHorizontal,0);  
            context.animator.SetFloat(parameterVertical, 0);
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            return State.Failure;
        }

        return State.Running;
    }
}
