using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private Question question;
    void Start()
    {
        question = GetComponent<Question>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (question.answered)
        {
            GetComponent<Sensor>().followPlayer = true;
        }
    }
}
