using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlSelectedPlayer : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update

    public void ChangeSelectedPlayer(GameObject player)
    {
        if(this.player != null)
        {
            Debug.Log("Player changed");
            this.player.GetComponent<Sensor>().isPlayer = false;
            Debug.Log(this.player.transform.position);
            
        }
        this.player = player;
        this.player.GetComponent<Sensor>().isPlayer = true;
        this.player.GetComponent<NavMeshAgent>().SetDestination(this.player.transform.position);
    }

}
