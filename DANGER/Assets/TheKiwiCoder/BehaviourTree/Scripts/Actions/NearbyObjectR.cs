using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class NearbyObjectR : ActionNode
{
    public string objectTag;
    public GameObject nearestObject;
    protected override void OnStart() {
        nearestObject = null;
        float dist;
        float minDist = float.MaxValue;
        Debug.Log(context.gameObject.GetComponent<Sensor>().objectsInRange.Count);
        foreach(GameObject detection in context.gameObject.GetComponent<Sensor>().objectsInRange)
        {
            if (detection.tag == objectTag)
            {
                dist = Vector3.Distance(detection.transform.position, context.gameObject.transform.position);
                if(minDist > dist)
                {
                    minDist = dist;
                    nearestObject = detection;
                }
            }
        }

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        
        if (nearestObject != null)
        {
            Vector3 direction = nearestObject.transform.position - context.gameObject.transform.position;
            direction.Normalize();
            blackboard.moveToPosition = context.gameObject.transform.position + (Quaternion.AngleAxis(180, Vector3.up) * (2 * direction));
            return State.Success;
        }
        else return State.Failure;
    }
}
