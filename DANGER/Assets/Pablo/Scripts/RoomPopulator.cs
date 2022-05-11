using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomPopulator : MonoBehaviour
{
    //public GameObject[] objectsToSpawn;

    // Start is called before the first frame update
    
    private int objWidth;
    private int objLength;
    private int objHeigth;

    
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    private List<ocuppiedArea> restrictedAreas;
    private Boolean fits;

    private Vector3 esqAI;
    private Vector3 esqBI;
    private Vector3 esqAD;
    private Vector3 esqBD;
    private ocuppiedArea room;

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



    void Start()
    {
        /*
        origObjWidth = (int) objectsToSpawn[0].GetComponent<Renderer>().bounds.size.x;
        origObjLength = (int) objectsToSpawn[0].GetComponent<Renderer>().bounds.size.z;

        remWidth = origObjWidth;
        remLength = origObjLength;
        newObject = true;
        //GameObject w = objectsToSpawn[0];
        */
        
       


        //
        /*
          for (int i=0; i< objectsToSpawn.Length; i++) 
        {
            objectsToSpawnIndexes[i] = i;
        }
        Fisher_YatesShuffle(objectsToSpawnIndexes);
        currentObject = 0;
        GameObject w=objectsToSpawn[0];
        Instantiate(w, new Vector3(0, 0, 0), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(w, new Vector3(0, 0, 0), Quaternion.Euler(0f, 90f, 0f));
        Instantiate(w, new Vector3(0, 0, 0), Quaternion.Euler(0f, 180f, 0f));
        Instantiate(w, new Vector3(0, 0, 0), Quaternion.Euler(0f, 270f, 0f));
        
        */
        //Instantiate(w, new Vector3(0, 0, 0), new Quaternion(0, , 0, 0));
    }
    //public void RoomPopulateST(List<WorldGenerator.generatorPoint> roomsList) { for (int i = 0; i < roomsList.Count; i++) { Debug.Log(roomsList[i].coords); } }
    public void RoomPopulate(List<WorldGenerator.generatorPoint> roomsList, List<int> width, List<int> length, GameObject[] objectsToSpawn, int margin) 
    {
        //fire = GetComponent<WorldGenerator>().fire;
        //for (int i = 0; i < roomsList.Count; i++) { Debug.Log(roomsList[i].coords); }
        //Debug.Log(roomsList[2].coords);
        GameObject[] spawnObjects = objectsToSpawn;
        Fisher_YatesShuffle(spawnObjects);
        restrictedAreas = new List<ocuppiedArea>();
        //Debug.Log(objectsToSpawn[0]);
        objWidth = Mathf.CeilToInt(spawnObjects[0].GetComponent<Renderer>().bounds.size.x);
        Debug.Log("ObjectXsize: " + objWidth);
        objLength = Mathf.CeilToInt(spawnObjects[0].GetComponent<Renderer>().bounds.size.z);
        Debug.Log("ObjectZsize: " + objLength);
        objHeigth = Mathf.CeilToInt(spawnObjects[0].GetComponent<Renderer>().bounds.size.y);

        



        //1) Calcular esquinas como centro + width (o length) /2
        for (int r = 0; r < roomsList.Count; r++) 
        
        {
            //if (r == 0) { Instantiate(fire, new Vector3(-3, 0.5f, -3), Quaternion.Euler(0f, 0f, 0f)); }
            minX = roomsList[r].coords.x - width[r] / 2f;
            maxX = roomsList[r].coords.x + width[r] / 2f;
            minZ = roomsList[r].coords.z - length[r] / 2f;
            maxZ = roomsList[r].coords.z + length[r] / 2f;

            restrictedAreas.Add(new ocuppiedArea(roomsList[r].coords.x-1.5f, roomsList[r].coords.x + 1.5f, roomsList[r].coords.z-length[r]/2f, roomsList[r].coords.z + length[r] / 2f));
            restrictedAreas.Add(new ocuppiedArea(roomsList[r].coords.x - width[r]/2f, roomsList[r].coords.x +width[r]/2f, roomsList[r].coords.z - 1f, roomsList[r].coords.z + 1f));
            //restrictedAreas.Add(new ocuppiedArea(esqBI.x, esqAI.x, esqBI.z, esqAI.z));

            Debug.Log("------------Rellenando sala: " + r+"-------------");
            room = new ocuppiedArea(minX, maxX, minZ, maxZ);
            /*
            Debug.Log("Center coords: " + roomsList[r].coords); 
            Debug.Log("Length " + length[r]);
            Debug.Log("Width " + width[r]);

            Debug.Log("i inicial: " + (int)Math.Ceiling(roomsList[r].coords.z - length[r] / 2f + 1.5f));
            Debug.Log("i final: " + (int)Math.Floor(roomsList[r].coords.z + length[r] / 2f - 1.5f));

            Debug.Log("j inicial: " + (int) Math.Ceiling(roomsList[r].coords.x - width[r] / 2f + 1.5f));
            Debug.Log("j final: " + (int) Math.Floor(roomsList[r].coords.x + width[r] / 2f - 1.5f));
            */

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

                    if ((esqAI.x >= room.minX & esqAI.x <= room.maxX & esqAI.z >= room.minZ & esqAI.z <= room.maxZ) &             // Si las esquinas del objeto están dentro de la sala
                        (esqBI.x >= room.minX & esqBI.x <= room.maxX & esqBI.z >= room.minZ & esqBI.z <= room.maxZ) &
                        (esqAD.x >= room.minX & esqAD.x <= room.maxX & esqAD.z >= room.minZ & esqAD.z <= room.maxZ) &
                        (esqBD.x >= room.minX & esqBD.x <= room.maxX & esqBD.z >= room.minZ & esqBD.z <= room.maxZ)
                        )
                    {
                        for (int k = 0; k < restrictedAreas.Count; k++)                                                 // Comprobamos si el objeto incumple alguna restricción
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
                        /*Debug.Log("CABEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");*/ 
                        restrictedAreas.Add(new ocuppiedArea(esqBI.x, esqAI.x, esqBI.z, esqAI.z));
                        Instantiate(spawnObjects[0], new Vector3(j, objHeigth/2f, i), Quaternion.Euler(0f, 0f, 0f)); 
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
        Debug.Log("Rooms created and populated");
        //FireGeneration fireGen = new FireGeneration(fire);
    }
    //Debug.Log(restrictedAreas[0]);




    /*
     * 
     remWidth = origObjWidth ;
    remLength = origObjLength ;
    newObject = true;
    //Instantiate(w, new Vector3(0, 0, 0), Quaternion.Euler(0f, 0f, 0f));
    //List<WorldGenerator.generatorPoint> generatedRooms = WorldGenerator.GetRoomsList();

    //for (int i = 0; i< roomsList.Count; i++) 
    //{
    //Debug.Log(roomsList[i].coords +"SIZE X:" +width+ "SIZE Y:"+length);
    grid = new float[width,length];
    for (int j = 0; j < width; j++) //verticales
    {
        for (int k = 0; k < length; k++) //horizontales
        {
            //Debug.Log("x:" + k + " y:" + j)
            Debug.Break();
            if (grid[j,k] != 1 &&  remWidth != 0) 
            {
                Debug.Log(remWidth);
                Debug.Log("grid[j,k] != 1 &&  remWidth!= 0");

                //Checkpoint : de este modo si el objeto no cabe podemos volver a este punto en el tiempo con otro objeto
                if (newObject) { newObject = false; prevState = grid; prevJ = j; prevK = k; }

                grid[j, k] = 1;
                remWidth--;
            }
            else if (grid[j,k]==1 && remWidth != 0) 
            {
                Debug.Log("grid[j,k]==1 && remWidth != 0");
                Aun me quedan trozo en X del objeto por colocar, reincio remainingX y remainingZ y probamos a partir de la siguiente celda
                remWidth = origObjWidth;
                remLength = origObjLength;
                j = prevJ;          // volvemos a la fila inicial, pero seguimos a artr de la x
                k = prevK + 1;
                grid = prevState;   //cargamos los valores antes de intentar introducir el nuevo objeto.
                newObject = true;


            }
            else if (grid[j, k] != 1 && remWidth == 0) 
            {
                Debug.Log("------------grid[j,k]==1 && remWidth != 0---------------");
                if (remLength == 0) 
                {   hemos terminado de introducir el objeto,CREAMOS EL OBJETO en la posición de la casilla+coordenadas reales en juego, restablecemos remX y remY y probamos a partir de prevX, break

                    Instantiate(objectsToSpawn[0], new Vector3(j+ roomsList[0].coords.x- width/2f, 0, k+ roomsList[0].coords.z-length/2f+0.5f), Quaternion.Euler(0f, 0f, 0f));
                    Debug.Log("orig" + origObjWidth);
                    //Instantiate(objectsToSpawn[0], new Vector3(j-(width / 2f) , 0f, k-(length/2f) ), Quaternion.Euler(0f, 0f, 0f));

                    remWidth = origObjWidth;
                    remLength = origObjLength;
                    j = prevJ;          // volvemos a la fila inicial, pero seguimos a artr de la x
                    k = prevK + 1;
                    prevState = grid;
                    //grid = prevState;   //cargamos los valores antes de intentar introducir el nuevo objeto.

                }
                else { k = prevK; remLength--; break; } //reinciamos en las x y pasamos a la siguiente fila


            }


        }

    //}
    /*
    for (float j = leftDown.x; j <= rightDown.x; j++)
    {
        for (float k = rightDown.z; k <= rightUp.z; k++)
        {
            //Debug.Log("X: " + j + "Y: " + k);
            if (j < ocuppiedAreas[i].maxX - 0.5f && j > ocuppiedAreas[i].minX - 0.5f && k < ocuppiedAreas[i].maxZ - 0.5f && k > ocuppiedAreas[i].minZ - 0.5f)
            { //Debug.Log("^^^^^^^^^^^^^NO FIT^^^^^^^^^^^^^^^^  X: "+j+"Y: "+k);
                ocuppied = true; break;
            }
        }
    }
    */





    void Fisher_YatesShuffle(GameObject[] a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Length - 1; i > 0; i--)
        {
            // Número aleatorio entre 0 y i (de forma que i decrementa cada iteración)
            int rnd = UnityEngine.Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            GameObject temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posición aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
