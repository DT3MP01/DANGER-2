using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ActiveSelector : CompositeNode
{
        protected int current;
        public int selected = 0;

        protected override void OnStart() {
            current = 0;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            // for (int i = current; i < children.Count; ++i) {
            //     current = i;
                
            //     switch (child.Update()) {
            //         case State.Running:
            //             return State.Running;
            //         case State.Success:
            //             return State.Success;
            //         case State.Failure:
            //             continue;
            //     }
            //     child.Abort();
            // }
            if(selected!=current){
                Debug.Log("Abort");
                children[current].Abort();
                current=selected;
            }
            var child = children[current];
            child.Update();
            switch (child.Update()) {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                default:
                    return State.Failure;
            }

        }
    
}
