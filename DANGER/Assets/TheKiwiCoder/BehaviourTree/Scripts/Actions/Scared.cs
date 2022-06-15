using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Scared : ActionNode
{
        public float duration = 2;
        float startTime;
        protected override void OnStart() {
            Debug.Log("Scared");
            context.animator.SetBool("isScared", true);
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            // if (context.gameObject.GetComponent<AiSensor>().isTerrified==false) {
            //     Debug.Log("ScaredEND");
            //     context.animator.SetBool("isScared", false);
            //     return State.Success;
            // }
            Debug.Log("ScaredR");
            return State.Running;
        }
}
