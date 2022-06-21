using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


    }

    void OnMouseEnter()
    {
        if(GetComponent<CustomCursor>().isActiveAndEnabled)
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(1))
        {
            //Posici��n bot��n
            

        }
    }
}

