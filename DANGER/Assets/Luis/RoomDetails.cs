using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RoomDetails : MonoBehaviour
{
    public List<Doorway> doors;
    WorldGenerator.ocuppiedArea area;
    // Start is called before the first frame update
    void Start()
    {
    }

    public WorldGenerator.generatorPoint GeneratorPoint()
    {
        return new WorldGenerator.generatorPoint(gameObject.transform.position);
    }

    public List<Doorway> GetDoorways()
    {
        return doors;
    }
    public WorldGenerator.ocuppiedArea getSizeRoom()
    {
        
        Bounds hola = GetBounds();
        //Debug.Log("BOUNDS "+ Mathf.Round(hola.min.x)+ " " + Mathf.Round(hola.max.z) + " " + Mathf.Round(hola.max.x) + " " + Mathf.Round(hola.min.z));
        area = new WorldGenerator.ocuppiedArea(Mathf.Round(hola.min.x), Mathf.Round(hola.max.x), Mathf.Round(hola.min.z), Mathf.Round(hola.max.z));
        return area;
    }

    public  Bounds GetBounds()

    {
        Bounds bounds = new Bounds();
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            //Find first enabled renderer to start encapsulate from it
            foreach (Renderer renderer in renderers)
            {
                if (renderer.enabled)
                {
                    bounds = renderer.bounds;
                    break;
                }
            }
            //Encapsulate for all renderers
            foreach (Renderer renderer in renderers)
            {
                if (renderer.enabled)

                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }
        return bounds;

    }
}
