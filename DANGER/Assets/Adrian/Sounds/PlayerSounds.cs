using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip walk;
    private bool walkBool=false;
    public AudioClip running;
    private bool runningBool=false;
    public AudioClip beam;
    public AudioSource source { get { return GetComponent<AudioSource>(); } }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!walkBool && !runningBool)
            {
                runningBool = true;
                source.PlayOneShot(running);
            }
            else { if (!source.isPlaying) { runningBool = false; } }
        }
        else
        {
            if (source.isPlaying && runningBool) { runningBool = false; source.Stop(); } 

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (!walkBool && !runningBool)
                {
                    walkBool = true;
                    source.PlayOneShot(walk);
                }
                else { if (!source.isPlaying) { walkBool = false; } }
            }
            else
            {
                if (source.isPlaying && walkBool) { walkBool = false; source.Stop(); }


            }
        }

        //Walk
        if (Input.GetKey(KeyCode.E))
        {
            if (!source.isPlaying) { source.PlayOneShot(beam); }
        
        }





    }
}
