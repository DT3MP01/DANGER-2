using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ActiveSelector : CompositeNode
{
        protected int current;
        public int selected = 0;
        enum Direction {North, East, South, West};

        protected override void OnStart() {
            current = 0;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            

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
