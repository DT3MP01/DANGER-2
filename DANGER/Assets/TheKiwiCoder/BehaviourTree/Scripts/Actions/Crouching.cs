using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Crouching : ActionNode
{
    protected override void OnStart() {
        
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(context.gameObject.GetComponent<AiSensor>().isSmokeNearby != context.animator.GetBool("isCrouching"))
        {
            if(context.gameObject.GetComponent<AiSensor>().isSmokeNearby == false){
                context.animator.SetBool("isCrouching", false);
            }
            else if (context.gameObject.GetComponent<AiSensor>().isSmokeNearby == true){
                context.animator.SetBool("isCrouching", true);
            }
        }
        
        return State.Running;
    }
}
