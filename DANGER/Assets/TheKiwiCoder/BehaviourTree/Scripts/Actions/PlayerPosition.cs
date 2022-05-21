using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PlayerPosition : ActionNode
{
    public GameObject player;

    protected override void OnStart() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        blackboard.moveToPosition = player.transform.position;
        return State.Success;
    }
}
