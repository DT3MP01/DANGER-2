using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomPopulator : MonoBehaviour
{
    //public GameObject[] objectsToSpawn;

    // Start is called before the first frame update

    public GameObject doorPrefab;
    public GameObject wallPrefab;
    private int objWidth;
    private int objLength;
    private int objHeigth;

    
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    private List<ocuppiedArea> restrictedAreas;

    private List<Transform> doors;
    private Boolean fits;

    private Vector3 esqAI;
    private Vector3 esqBI;
    private Vector3 esqAD;
    private Vector3 esqBD;
    private ocuppiedArea room;
    private Dictionary<Vector3,ObjectTransform> doorsLocations;

    //public GameObject fire;



    //public GameObject m_fireIgniter;
    //public int margin;

    //private int[,] grid;
    /*
    private int remWidth;
    private int remLength;
    private bool newObject;
    
    private float[,] prevState;
    private int prevJ;
    private int prevK;
    private int[] objectsToSpawnIndexes;
    private int currentObject;
    */



    struct ocuppiedArea
    {
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;
        

        public ocuppiedArea(float minX, float maxX, float minZ, float maxZ)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minZ = minZ;
            this.maxZ = maxZ;
        }
    }



    public void RoomPopulate(List<Transform>generatedRoomList,List<WorldGenerator.generatorPoint> roomsList, List<int> width, List<int> length, GameObject[] objectsToSpawn, int margin) 
    {
        GameObject[] spawnObjects = objectsToSpawn;
        Fisher_YatesShuffle(spawnObjects);
        doorsLocations = new Dictionary<Vector3, ObjectTransform>();
        restrictedAreas = new List<ocuppiedArea>();
        //Debug.Log(objectsToSpawn[0]);
        objWidth = Mathf.CeilToInt(spawnObjects[0].GetComponent<Renderer>().bounds.size.x);
        Debug.Log("ObjectXsize: " + objWidth);
        objLength = Mathf.CeilToInt(spawnObjects[0].GetComponent<Renderer>().bounds.size.z);
        Debug.Log("ObjectZsize: " + objLength);
        objHeigth = Mathf.CeilToInt(spawnObjects[0].GetComponent<Renderer>().bounds.size.y);
        Vector3 position;
        //1) Calcular esquinas como centro + width (o length) /2
        for (int r = 0; r < roomsList.Count; r++) 
        
        {
            minX = roomsList[r].coords.x - width[r] / 2f;
            maxX = roomsList[r].coords.x + width[r] / 2f;
            minZ = roomsList[r].coords.z - length[r] / 2f;
            maxZ = roomsList[r].coords.z + length[r] / 2f;
            
            restrictedAreas.Add(new ocuppiedArea(roomsList[r].coords.x-1.5f, roomsList[r].coords.x + 1.5f, roomsList[r].coords.z-length[r]/2f, roomsList[r].coords.z + length[r] / 2f));
            restrictedAreas.Add(new ocuppiedArea(roomsList[r].coords.x - width[r]/2f, roomsList[r].coords.x +width[r]/2f, roomsList[r].coords.z - 1f, roomsList[r].coords.z + 1f));
            //restrictedAreas.Add(new ocuppiedArea(esqBI.x, esqAI.x, esqBI.z, esqAI.z));

            //Debug.Log("------------Rellenando sala: " + r+"-------------");
            room = new ocuppiedArea(minX, maxX, minZ, maxZ);
        
            position =new Vector3(minX, 1.35f, roomsList[r].coords.z);
            if(!doorsLocations.ContainsKey(position))
            {
               doorsLocations.Add(position, new ObjectTransform(Quaternion.Euler(0f, 90f, 0f), "wall","left"));
            }
            else {
                doorsLocations[position] = new ObjectTransform(Quaternion.Euler(0f, 90f, 0f), "door", "left");
            }

            position = new Vector3(maxX, 1.35f, roomsList[r].coords.z);
            if (!doorsLocations.ContainsKey(position))
            {
                doorsLocations.Add(position, new ObjectTransform(Quaternion.Euler(0f, 90f, 0f), "wall", "right"));
            }
            else
            {
                doorsLocations[position] = new ObjectTransform(Quaternion.Euler(0f, 90f, 0f), "door", "right");
            }

            position = new Vector3(roomsList[r].coords.x, 1.35f, minZ);
            if (!doorsLocations.ContainsKey(position))
            {
                doorsLocations.Add(position, new ObjectTransform(Quaternion.Euler(0f, 0f, 0f), "wall","bottom"));
            }
            else
            {
                doorsLocations[position] = new ObjectTransform(Quaternion.Euler(0f, 0f, 0f), "door", "bottom");
            }
            
            position = new Vector3(roomsList[r].coords.x, 1.35f, maxZ);

            if (!doorsLocations.ContainsKey(position))
            {
                doorsLocations.Add(position, new ObjectTransform(Quaternion.Euler(0f, 0f, 0f), "wall", "top"));
            }
            else
            {
                doorsLocations[position] = new ObjectTransform(Quaternion.Euler(0f, 0f, 0f), "door", "top");
            }



            for (int i = (int) Math.Ceiling(roomsList[r].coords.z - length[r] / 2f + 2 + objLength); i <= (int) Math.Floor(roomsList[r].coords.z + length[r] / 2f - 2 - objLength); i+= objLength+margin) // columnas
            {
                for (int j = (int) Math.Ceiling(roomsList[r].coords.x - width[r] / 2f + 2 + objWidth); j <= (int) Math.Floor(roomsList[r].coords.x + width[r] / 2f - 2 - objWidth); j+=objWidth+margin) // filas
                {
                    
                    //Debug.Log("-------X:" + j + "Y:" + i + "--------");
                    fits = true;
                    esqAI = new Vector3(j - (objWidth / 2f), 0, i + (objLength / 2f));
                    esqBI = new Vector3(j - (objWidth / 2f), 0, i - (objLength / 2f));
                    esqAD = new Vector3(j + (objWidth / 2f), 0, i + (objLength / 2f));
                    esqBD = new Vector3(j + (objWidth / 2f), 0, i - (objLength / 2f));

                    if ((esqAI.x >= room.minX & esqAI.x <= room.maxX & esqAI.z >= room.minZ & esqAI.z <= room.maxZ) &             // Si las esquinas del objeto est�n dentro de la sala
                        (esqBI.x >= room.minX & esqBI.x <= room.maxX & esqBI.z >= room.minZ & esqBI.z <= room.maxZ) &
                        (esqAD.x >= room.minX & esqAD.x <= room.maxX & esqAD.z >= room.minZ & esqAD.z <= room.maxZ) &
                        (esqBD.x >= room.minX & esqBD.x <= room.maxX & esqBD.z >= room.minZ & esqBD.z <= room.maxZ)
                        )
                    {
                        for (int k = 0; k < restrictedAreas.Count; k++)                                                 // Comprobamos si el objeto incumple alguna restricci�n
                        {
                            if ((esqAI.x > restrictedAreas[k].minX && esqAI.x < restrictedAreas[k].maxX && esqAI.z > restrictedAreas[k].minZ && esqAI.z < restrictedAreas[k].maxZ) ||
                                (esqBI.x > restrictedAreas[k].minX && esqBI.x < restrictedAreas[k].maxX && esqBI.z > restrictedAreas[k].minZ && esqBI.z < restrictedAreas[k].maxZ) ||
                                (esqAD.x > restrictedAreas[k].minX && esqAD.x < restrictedAreas[k].maxX && esqAD.z > restrictedAreas[k].minZ && esqAD.z < restrictedAreas[k].maxZ) ||
                                (esqBD.x > restrictedAreas[k].minX && esqBD.x < restrictedAreas[k].maxX && esqBD.z > restrictedAreas[k].minZ && esqBD.z < restrictedAreas[k].maxZ)
                            )

                            { fits = false; /*Debug.Log("Choca con otros objetos");*/ break; }
                            else { /*fits = false; Debug.Log("Choca con otros objetos"); break;*/ }
                        }
                    }
                    else
                    {
                        //Debug.Log("Fuera de la sala");
                        //Debug.Log("DEPURACION");
                        //Debug.Log(esqAI.x + ">=" + room.minX + " &&" + esqAI.x + " <=" + room.maxX + "&&" + esqAI.z + " >=" + room.minZ + "&&" + esqAI.z + "<=" + room.maxZ);
                        //Debug.Log(esqBI.x + ">=" + room.minX + " &&" + esqBI.x + " <=" + room.maxX + "&&" + esqBI.z + " >=" + room.minZ + "&&" + esqBI.z + "<=" + room.maxZ);
                        //Debug.Log(esqAD.x + ">=" + room.minX + " &&" + esqAD.x + " <=" + room.maxX + "&&" + esqAD.z + " >=" + room.minZ + "&&" + esqAD.z + "<=" + room.maxZ);
                        //Debug.Log(esqBD.x + ">=" + room.minX + " &&" + esqBD.x + " <=" + room.maxX + "&&" + esqBD.z + " >=" + room.minZ + "&&" + esqBD.z + "<=" + room.maxZ);

                        fits = false;
                    }

                    if (fits) 
                    { 
                        /*Debug.Log("CABE");*/ 
                        restrictedAreas.Add(new ocuppiedArea(esqBI.x, esqAI.x, esqBI.z, esqAI.z));
                        Instantiate(spawnObjects[0], new Vector3(j, objHeigth/2f, i), Quaternion.Euler(0f, 0f, 0f),generatedRoomList[r]); 
                        Fisher_YatesShuffle(spawnObjects);
                        objWidth = (int)spawnObjects[0].GetComponent<Renderer>().bounds.size.x;
                        //Debug.Log("ObjectXsize: " + objWidth);
                        objLength = (int)spawnObjects[0].GetComponent<Renderer>().bounds.size.z;
                        //Debug.Log("ObjectZsize: " + objLength);
                        objHeigth = (int)spawnObjects[0].GetComponent<Renderer>().bounds.size.y;

                    }



                }


            }
        }

        foreach(Vector3 prefabs in doorsLocations.Keys){
            if(doorsLocations[prefabs].prefabName == "door"){
                Instantiate(doorPrefab,prefabs,doorsLocations[prefabs].rotation);

            }
            else{
                Vector3 pos;
                GameObject wall;
                switch(doorsLocations[prefabs].position){
                    case "left":
                        pos = prefabs + new Vector3(-0.25f,-1.35f,0);
                        wall= Instantiate(wallPrefab,pos,doorsLocations[prefabs].rotation);
                        wall.transform.localScale = wall.transform.localScale + new Vector3(0.5f,0,0);
                        break;
                    case "right":
                        pos = prefabs + new Vector3(0.25f,-1.35f,0);
                        wall=Instantiate(wallPrefab,pos,doorsLocations[prefabs].rotation);
                        wall.transform.localScale = wall.transform.localScale + new Vector3(0.5f,0,0);
                        break;
                    case "top":
                        pos = prefabs + new Vector3(0,-1.35f,0.25f);
                        wall=Instantiate(wallPrefab,pos,doorsLocations[prefabs].rotation);
                        wall.transform.localScale = wall.transform.localScale + new Vector3(0.5f,0,0);
                        break;
                    case "bottom":
                        pos = prefabs + new Vector3(0,-1.35f,-0.25f);
                        wall=Instantiate(wallPrefab,pos,doorsLocations[prefabs].rotation);
                        wall.transform.localScale = wall.transform.localScale + new Vector3(0.5f,0,0);
                        break;
                    default:
                        break;
                }
                
            }
            

        }
        Debug.Log("Rooms created and populated");
    }

    void Fisher_YatesShuffle(GameObject[] a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Length - 1; i > 0; i--)
        {
            // Numero aleatorio entre 0 y i (de forma que i decrementa cada iteraci�n)
            int rnd = UnityEngine.Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            GameObject temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posici�n aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }
    // Update is called once per frame
}
