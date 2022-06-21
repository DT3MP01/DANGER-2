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
        if(context.gameObject.GetComponent<Sensor>().nearbySmoke != context.animator.GetBool("isCrouching"))
        {
            if(context.gameObject.GetComponent<Sensor>().nearbySmoke == false){
                context.animator.SetBool("isCrouching", false);
            }
            else if (context.gameObject.GetComponent<Sensor>().nearbySmoke == true){
                context.animator.SetBool("isCrouching", true);
            }
        }
        
        return State.Running;
    }
}
