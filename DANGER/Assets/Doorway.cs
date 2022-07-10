using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    [ExecuteInEditMode]
    // Start is called before the first frame update
    void Start()
    {
        // draw a 5-unit white line from the origin for 2.5 seconds
      
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void OnDrawGizmos()
    {

        Vector2 size = new Vector2(1, 1);
        Vector2 halfSize = size * 0.5f;


        // Draw Forward Vector
        float lineLength = Mathf.Min(size.x, size.y);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + transform.up * halfSize.y, transform.position + transform.up * halfSize.y + transform.forward * lineLength);

    }
}
