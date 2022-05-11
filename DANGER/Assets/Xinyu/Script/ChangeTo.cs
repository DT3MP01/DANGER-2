using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTo : MonoBehaviour
{
    private Button button;
    public GameObject character;
    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Change);
    }

    // Update is called once per frame
    void Update()
    {

        selected = character.GetComponent<CharacterMovment>().underControl;
    }

    public void Change()
    {
        Debug.Log("You have clicked the button!");
        if (!selected) 
        {
            character.GetComponent<CharacterMovment>().underControl = true;
            GameObject []players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (player != character) 
                {
                    player.GetComponent<CharacterMovment>().underControl = false;
                }
            
            }


        }
    }

   

    
}
