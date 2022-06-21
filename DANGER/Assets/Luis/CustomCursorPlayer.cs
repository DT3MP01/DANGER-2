using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CustomCursorPlayer : MonoBehaviour
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
        if(GetComponent<CustomCursorPlayer>().isActiveAndEnabled && !GetComponent<Sensor>().isPlayer){
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
         
        
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ControlSelectedPlayer>().ChangeSelectedPlayer(gameObject);
        }
    }
}
