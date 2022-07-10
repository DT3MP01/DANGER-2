using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RoomDetails : MonoBehaviour
{
    public List<Doorway> doors;
    // Start is called before the first frame update
    void Start()
    {



    }
    public List<Doorway> GetDoorways()
    {
        return doors;
    }
    public WorldGenerator.ocuppiedArea getSizeRoom()
    {
        WorldGenerator.ocuppiedArea area = new WorldGenerator.ocuppiedArea(Mathf.Infinity, Mathf.NegativeInfinity, Mathf.Infinity, Mathf.NegativeInfinity);
        foreach (Transform child in gameObject.transform)
        {
            if(child.name == "Structure")
            {
                foreach (Transform bound in child)
                {
                    var posX = (float)Math.Round(bound.transform.position.x, 2);
                    var posZ = (float)Math.Round(bound.transform.position.z,2);
                    if (posX < area.minX)
                    {
                        area.minX = posX;
                    }
                    else if(posX > area.maxX)
                    {
                        area.maxX = posX;
                    }
                    else if (posZ < area.minZ)
                    {
                        area.minZ = posZ;
                    }
                    else if (posZ > area.maxZ)
                    {
                        area.maxZ = posZ;
                    }
                }
            }
        }
        
        Debug.Log(area.minX + " " + area.maxZ + " " +area.maxX+ " "+ area.minZ);
        return area;
    }
}
