using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    [ExecuteInEditMode]
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDrawGizmos()
    {

        Vector2 size = new Vector2(GetComponent<Renderer>().bounds.size.x, GetComponent<Renderer>().bounds.size.y);
        Vector2 halfSize = size * 0.5f;


        // Draw Forward Vector
        float lineLength = Mathf.Min(size.x, size.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(GetComponent<Renderer>().bounds.center , GetComponent<Renderer>().bounds.center + transform.forward * 1);

    }
}
