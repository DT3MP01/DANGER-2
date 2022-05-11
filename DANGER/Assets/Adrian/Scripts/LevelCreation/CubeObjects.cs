using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObjects : MonoBehaviour
{
    public Dictionary<Vector3, bool> exteriors;
    public Dictionary<Vector3, bool> fires;
    public Dictionary<Vector3, bool> extinguishers;
    public Dictionary<Vector3, bool> cubes;
    public bool tables, aux, door, extinguisher;

    public int exteriorsCount, firesCount, extinguishersCount;

    void Start()
    {
        aux = false;
        tables = true;
        door = false;
        extinguisher = false;
        exteriorsCount = 0;
        firesCount = 0;
        extinguishersCount = 0;

        extinguishers = new Dictionary<Vector3, bool>()
        {
            {new Vector3(0f, 1.5f, 0.35f), true},
            {new Vector3(0f, 1.5f, -0.35f), true},
            {new Vector3(0.35f, 1.5f, 0f), true},
            {new Vector3(-0.35f, 1.5f, 0f), true}
        };

        fires = new Dictionary<Vector3, bool>()
        {
            {new Vector3(0f, 1.5f, 0.35f), true},
            {new Vector3(0f, 1.5f, -0.35f), true},
            {new Vector3(0.35f, 1.5f, 0f), true},
            {new Vector3(-0.35f, 1.5f, 0f), true}
        };

        exteriors = new Dictionary<Vector3, bool>()
        {
            {new Vector3(0f, 1.5f, 0.35f), true},
            {new Vector3(0f, 1.5f, -0.35f), true},
            {new Vector3(0.35f, 1.5f, 0f), true},
            {new Vector3(-0.35f, 1.5f, 0f), true}
        };
    }

    public bool putItem(string tag, Vector3 item)
    {
        if ((tag == "Wall" || tag == "Window" || tag == "Door") && exteriors.ContainsKey(item))
        {
            if (door) return false;
            if (tag == "Door")
            {
                if (exteriorsCount > 0) return false;
                if (!tables) return false;
                door = true;
            }

            aux = exteriors[item];
            exteriors.Remove(item);
            exteriors.Add(item, false);
            if (aux) exteriorsCount++;
        }
        else if (tag == "Extinguisher" && extinguishers.ContainsKey(item))
        {
            if (exteriorsCount == 0) return false;
            else if (extinguishersCount >= exteriorsCount) return false;
            if (extinguisher) return false;
            if (!tables) return false;

            extinguisher = true;
            aux = extinguishers[item];
            extinguishers.Remove(item);
            extinguishers.Add(item, false);
            if (aux) extinguishersCount++;
        }
        else if (tag == "Table")
        {
            if (extinguisher || door) return false;
            aux = tables;
            if (aux) tables = false;
        }
        return aux;  
    }

    public void deleteItem(string tag, Vector3 item)
    {
        if ((tag == "Wall" || tag == "Window" || tag == "Door") && exteriors.ContainsKey(item))
        {
            exteriors.Remove(item);
            exteriors.Add(item, true);
            aux = exteriors[item];
            if (tag == "Door") door = false;
            if(aux) exteriorsCount--;
        }
        else if (tag == "Fire" && fires.ContainsKey(item))
        {
            fires.Remove(item);
            fires.Add(item, true);
            aux = fires[item];
            if(aux) firesCount--;
        }
        else if (tag == "Extinguisher" && extinguishers.ContainsKey(item))
        {
            extinguishers.Remove(item);
            extinguishers.Add(item, true);
            aux = extinguishers[item];
            extinguisher = false;
            if(aux) extinguishersCount--;
        }
        else if (tag == "Table") 
        {
            tables = true;
        }
    }
}
