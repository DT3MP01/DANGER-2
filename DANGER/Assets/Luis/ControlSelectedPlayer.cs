using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlSelectedPlayer : MonoBehaviour
{
    public GameObject player;

    public Slider playerHealthSlider;
    public Slider palyerStressSlider;

    // Start is called before the first frame update

    public void ChangeSelectedPlayer(GameObject player)
    {
        if(this.player != null)
        {
            Debug.Log("Player changed");
            this.player.GetComponent<Sensor>().isPlayer = false;
            
        }
        playerHealthSlider.gameObject.SetActive(true);
        palyerStressSlider.gameObject.SetActive(true);
        this.player = player;
        this.player.GetComponent<Sensor>().isPlayer = true;
        this.player.GetComponent<NavMeshAgent>().SetDestination(this.player.transform.position);
    }
    public void Update()
    {
        if(player != null)
        {
            Debug.Log(player.GetComponent<Sensor>().playerHealth);
            playerHealthSlider.value = player.GetComponent<Sensor>().playerHealth / 100.0f;
            palyerStressSlider.value = player.GetComponent<Sensor>().playerStress / 100.0f;
        }
    }



}
