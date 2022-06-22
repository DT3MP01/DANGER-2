using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PlayerPosition : ActionNode
{
    public GameObject player;

    protected override void OnStart() {
        player = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControlSelectedPlayer>().player;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(player == null) {
            return State.Failure;
        }
        blackboard.moveToPosition = player.transform.position;
        return State.Success;
    }
}
