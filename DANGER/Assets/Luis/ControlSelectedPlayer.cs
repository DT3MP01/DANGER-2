using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (player != null)
        {
            playerHealthSlider.value = (float)player.GetComponent<Sensor>().playerHealth / 100.0f;
            palyerStressSlider.value = (float)player.GetComponent<Sensor>().playerStress / 100.0f;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu Principal");
        }
    }



}
