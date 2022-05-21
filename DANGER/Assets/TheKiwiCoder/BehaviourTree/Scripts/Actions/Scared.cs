using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Scared : ActionNode
{
        public float duration = 2;
        float startTime;
        protected override void OnStart() {
            context.animator.SetBool("isScared", context.gameObject.GetComponent<AiSensor>().isTerrified);
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (context.gameObject.GetComponent<AiSensor>().isTerrified==false) {
                context.animator.SetBool("isScared", false);
                return State.Success;
            }
            return State.Running;
        }
}
