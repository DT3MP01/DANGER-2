using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckDetection : ActionNode
{
    public string variableToCheck;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(context.gameObject.GetComponent<Sensor>().getDetections(variableToCheck)){
            return State.Success;
        }
        return State.Failure;
    }
}
