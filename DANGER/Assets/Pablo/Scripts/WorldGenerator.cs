using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldGenerator : RoomPopulator
{
    // Start is called before the first frame update
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject npcPrefab;
    public GameObject roomParentPrefab;
    public NavMeshSurface navMesh;
    public GameObject exit;
    public bool enablePopulate;
    



    public int minLength =7;
    public int maxLength = 40;
    public int minWidth = 7;
    public int maxWidth = 40;
    public int roomsToGenerate; //= 6;
    public float doorSize = 3;
    public int margin;

    private float length;
    private float width;
    private float doorMargin;
    private generatorPoint genP;
    private int createdRooms;

    private ocuppiedArea[] ocuppiedAreas;
    private ocuppiedArea ocuArea;
    private bool ocuppied;
    private Vector3 roomCentre;
    private List<generatorPoint> generatedRooms;
    private List<int> generatedWidths;
    private List<int> generatedLengths;

    private int originWidth;                                      // Guardamos una copia del ancho original para crear el suelo
    private int originLength;

    public enum dir { left, up, right, down, centre };
    Queue<generatorPoint> genQ;
    private int[] queueOrder;
    public GameObject[] objectsToSpawn;
    public GameObject[] labItems;
    generatorPoint aux1;
    generatorPoint aux2;
    private bool started;

    public bool lab;


    public struct generatorPoint
    {
        
        public Vector3 coords;  
        public dir direction;

        public generatorPoint(Vector3 coordinates, dir direction)
        {
            this.coords = coordinates;
            this.direction = direction;
        }
        
        
    }

    struct ocuppiedArea 
    {

        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;

        public ocuppiedArea(float minX,float maxX, float minZ, float maxZ) 
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minZ = minZ;
            this.maxZ = maxZ;
        }



    }
    void Start()
    {
        genP = new generatorPoint(new Vector3(0, 0, 0), dir.centre);
        doorMargin = doorSize / 2f;
        genQ = new Queue<generatorPoint>(roomsToGenerate*roomsToGenerate * 4); //Cola de puntos generadores (situados en las puertas de las salas para generar una sala nueva)
        ocuppiedAreas = new ocuppiedArea[roomsToGenerate+1];
        createdRooms = 0;
        ocuArea = new ocuppiedArea();
        ocuppied = false;
        queueOrder = new int[] {1,2,3,4 };
        generatedRooms = new List<generatorPoint>();
        generatedWidths = new List<int>();
        generatedLengths = new List<int>();
        GlobalVar.totalNPCs = 0;

        worldGenerator();

  
        //queueOrder = queueOrder.Sort
          //  listOfThings = listOfThings.OrderBy(i => Guid.NewGuid()).ToList();



    }
    void worldGenerator() 
    {
        while(roomsToGenerate > 0)
        {
            roomGenerator(genP);


            //Debug.Log("Origin: "+genP.coords);

            
            if (!ocuppied) {
                //aux2 previo
                //aux1 ultimo

                
                //if (roomsToGenerate == 3) { aux1 = genP; }
                //if (roomsToGenerate == 2) { aux2 = genP; }
                roomsToGenerate = roomsToGenerate - 1; }  // añadimos a generatedRooms una sala si esta ha podido ser generada al no haber colisiones
            else { ocuppied = false; }
            genP = genQ.Dequeue();
            

        }
        while (genQ.Count>0 ) 
        
        {
            aux2 = aux1;
            aux1 = genQ.Dequeue();
        }
        
        
        
    
        // Generamos el Mesh para los agentes
        //Debug.Log(generatedRooms[0]);

        //RoomPopulateST(generatedRooms);
        
        GlobalVar.widths = generatedWidths;
        GlobalVar.lengths = generatedLengths;
        if (enablePopulate) {
            if (lab) { RoomPopulate(generatedRooms, generatedWidths, generatedLengths, labItems, margin); }
            else { RoomPopulate(generatedRooms, generatedWidths, generatedLengths, objectsToSpawn, margin); }
             
        
        }
        GlobalVar.rooms = generatedRooms;
        
        


    }

    void roomGenerator(generatorPoint genP)
    {
        
        
        //Debug.Log("Creating a room"+createdRooms);
        
        width = Random.Range(minWidth, maxWidth);                           // Obtenemos el ancho de la sala
        length = Random.Range(minLength, maxLength);                        // Obtenemos largo de la sala

        
        //Debug.Log(" GENERATED width=" + width + "length=" + length);
        originWidth = (int)width;                                          // Guardamos una copia del ancho original para crear el suelo
        originLength = (int)length;                                        // Guardamos una copia del largo original para crear el suelo
        
        length = (length / 2f) - doorMargin;                                       // La longitud que tendr?cada uno de los trozos de la pared que se utilizen en el eje vertical ser?la mitad de la longitud de la sala menos el margen para la puerta/2
        width = (width / 2f) - doorMargin;                                         // La longitud que tendr?cada uno de los trozos de la pared que se utilizen en el eje horizontal ser?la mitad de la anchura de la sala menos el margen para la puerta/2

        //Vector3 roomCentre = genP.coords + new Vector3(originWidth/2f, 0, originLength/2f);

        if(      genP.direction == dir.up) { roomCentre = genP.coords + new Vector3(0, 0, originLength / 2f); }
        else if (genP.direction == dir.down) { roomCentre = genP.coords + new Vector3(0, 0, -originLength / 2f); }
        else if (genP.direction == dir.left) { roomCentre = genP.coords + new Vector3(-originWidth / 2f, 0, 0); }
        else if (genP.direction == dir.right) { roomCentre = genP.coords + new Vector3(originWidth / 2f, 0, 0); }
        else { roomCentre = genP.coords; }

        
        ocuArea.maxX = roomCentre.x + originWidth / 2f;
        ocuArea.minX = roomCentre.x - originWidth / 2f;
        ocuArea.maxZ = roomCentre.z + originLength / 2f;
        ocuArea.minZ = roomCentre.z - originLength / 2f;

        ocuppiedAreas[createdRooms] = ocuArea;      //ocuppiedAreas[0]
        Vector3 leftUp   = new Vector3((-originWidth / 2.0f) + roomCentre.x, 0, (originLength / 2f) + roomCentre.z);
        Vector3 leftDown  = new Vector3((-originWidth / 2.0f) + roomCentre.x, 0, (-originLength / 2f) + roomCentre.z);
        Vector3 rightUp   = new Vector3((originWidth / 2.0f) + roomCentre.x, 0, (originLength / 2f) - 0.5f + roomCentre.z);
        Vector3 rightDown = new Vector3((originWidth / 2.0f) - 0.5f + roomCentre.x, 0, (-originLength / 2f) + roomCentre.z);

        if (createdRooms > 0) 
        {
            for (int i = 0; i < createdRooms; i++)
            {
                for (float j =leftDown.x; j<= rightDown.x; j++) 
                {
                    for (float k = rightDown.z; k <= rightUp.z; k++) 
                    {
                        //Debug.Log("X: " + j + "Y: " + k);
                        if (j < ocuppiedAreas[i].maxX - 0.5f && j > ocuppiedAreas[i].minX - 0.5f && k < ocuppiedAreas[i].maxZ - 0.5f && k > ocuppiedAreas[i].minZ - 0.5f) { //Debug.Log("^^^^^^^^^^^^^NO FIT^^^^^^^^^^^^^^^^  X: "+j+"Y: "+k);
                                                                                                                                                                            ocuppied = true; break; }
                    }
                }
                /*
                if ((roomCentre.x   <= ocuppiedAreas[i].maxX && roomCentre.x >= ocuppiedAreas[i].minX && roomCentre.z <= ocuppiedAreas[i].maxZ && roomCentre.z >= ocuppiedAreas[i].minZ)
                    || (leftUp.x    <= ocuppiedAreas[i].maxX - 1 && leftUp.x     >= ocuppiedAreas[i].minX - 1 && leftUp.z     <= ocuppiedAreas[i].maxZ - 1 && leftUp.z     >= ocuppiedAreas[i].minZ - 1)
                    || (leftDown.x  <= ocuppiedAreas[i].maxX - 1 && leftDown.x   >= ocuppiedAreas[i].minX - 1 && leftDown.z   <= ocuppiedAreas[i].maxZ - 1 && leftDown.z   >= ocuppiedAreas[i].minZ - 1)
                    || (rightUp.x   <= ocuppiedAreas[i].maxX - 1 && rightUp.x    >= ocuppiedAreas[i].minX - 1 && rightUp.z    <= ocuppiedAreas[i].maxZ - 1 && rightUp.z    >= ocuppiedAreas[i].minZ - 1)
                    || (rightDown.x <= ocuppiedAreas[i].maxX - 1 && rightDown.x  >= ocuppiedAreas[i].minX - 1 && rightDown.z  <= ocuppiedAreas[i].maxZ - 1 && rightDown.z  >= ocuppiedAreas[i].minZ - 1))
                { Debug.Log("^^^^^^^^^^^^^NO FIT^^^^^^^^^^^^^^^^"); ocuppied = true; break; }
                */


                /*else
                {
                    Debug.Log("-----------------------------fits----------------------------");
                    Debug.Log(createdRooms);
                    Debug.Log(roomCentre.x + "<=" + ocuppiedAreas[i].maxX);
                    Debug.Log(roomCentre.x + ">=" + ocuppiedAreas[i].minX);
                    Debug.Log(roomCentre.z + "<=" + ocuppiedAreas[i].maxZ);
                    Debug.Log(roomCentre.z + ">=" + ocuppiedAreas[i].minZ);
                    Debug.Log("-------------------------------------------------------------");
                }
                */
            }
        }
        
        
        //Debug.Log("OSCUUUUUUUUUUUUUUU: "+ocuppiedAreas[1].maxZ);

        if (!ocuppied)
        {
            Debug.Log("Creating room..." + createdRooms);
            createdRooms += 1;

            GameObject roomParent = Instantiate(roomParentPrefab, new Vector3(roomCentre.x, 0, roomCentre.z), Quaternion.identity);
            generatedRooms.Add(new generatorPoint( new Vector3(roomCentre.x,0,roomCentre.z), genP.direction));
            generatedWidths.Add(originWidth);
            generatedLengths.Add(originLength);
            /*
             * Situamos un cubo (pared) con dimensiones 1x1 a mitad camino entre cada una de las esquinas y el punto central de las esquinas
             * 
             *          ExxxxCxxxxxPGPxxxxxCxxxxE
             *          x                       x
             *          C                       C
             *          x                       x
             *          P                       P
             *          G                       G
             *          P                       P
             *          x                       x
             *          C                       C
             *          x                       x
             *          ExxxxCxxxxxPGPxxxxxCxxxxE
             * 
             * Siendo C el punto donde situamos cada uno de los cubos generadores del bloque de pared entre la esquina y la puerta. Las E son las esquinas, las P son las puertas, x los huecos que se ocuparan al escalar el cubo generador
             * y G los generadores de salas siguientes
             */
            GameObject leftDownHor = Instantiate(wallPrefab,  new Vector3((-width / 2.0f) - doorMargin + roomCentre.x, 0, -length - 1f + roomCentre.z), Quaternion.identity);
            GameObject leftUpHor = Instantiate(wallPrefab,    new Vector3((-width / 2.0f) - doorMargin + roomCentre.x, 0, length + 1f + roomCentre.z), Quaternion.identity);
            GameObject rightDownHor = Instantiate(wallPrefab, new Vector3(( width / 2.0f) + doorMargin + roomCentre.x, 0, -length - 1f + roomCentre.z), Quaternion.identity);
            GameObject rightUpHor = Instantiate(wallPrefab,   new Vector3(( width / 2.0f) + doorMargin + roomCentre.x, 0, length + 1f + roomCentre.z), Quaternion.identity);

            leftDownHor.transform.SetParent(roomParent.transform);
            leftUpHor.transform.SetParent(roomParent.transform);
            rightDownHor.transform.SetParent(roomParent.transform);
            rightUpHor.transform.SetParent(roomParent.transform);

            //Una vez generados los cubos generadores los escalamos para que ocupen todas las posiciones de las x
            leftDownHor.transform.localScale += new Vector3(width - 1, 1, 0);
            leftUpHor.transform.localScale += new Vector3(width - 1, 1, 0);
            rightDownHor.transform.localScale += new Vector3(width - 1, 1, 0);
            rightUpHor.transform.localScale += new Vector3(width - 1, 1, 0);



            GameObject leftDownVer = Instantiate(wallPrefab, new Vector3(-width - 1f + roomCentre.x, 0, (-length / 2f) - doorMargin + roomCentre.z), Quaternion.identity);
            GameObject leftUpVer = Instantiate(wallPrefab, new Vector3(-width - 1f + roomCentre.x, 0, (length / 2f) + doorMargin + roomCentre.z), Quaternion.identity);
            GameObject rightDownVer = Instantiate(wallPrefab, new Vector3(width + 1f + roomCentre.x, 0, (-length / 2f) - doorMargin + roomCentre.z), Quaternion.identity);
            GameObject rightUpVer = Instantiate(wallPrefab, new Vector3(width + 1f + roomCentre.x, 0, (length / 2f) + doorMargin + roomCentre.z), Quaternion.identity);

            leftDownVer.transform.SetParent(roomParent.transform);
            leftUpVer.transform.SetParent(roomParent.transform);
            rightDownVer.transform.SetParent(roomParent.transform);
            rightUpVer.transform.SetParent(roomParent.transform);

            leftDownVer.transform.localScale += new Vector3(0, 1, length - 1);
            leftUpVer.transform.localScale += new Vector3(0, 1, length - 1);
            rightDownVer.transform.localScale += new Vector3(0, 1, length - 1);
            rightUpVer.transform.localScale += new Vector3(0, 1, length - 1);
            Instantiate(npcPrefab, new Vector3(roomCentre.x, -2f, roomCentre.z), Quaternion.identity);  // Añadimos un primer agente en el centro de la sala
            GlobalVar.totalNPCs += 1;


            /*Una vez creadas las paredes, creamos un plano con el tamaño original de anchura y largo, que se corresponde con las dimensiones de la habitación. Como los planos en Unity se crean por defecto con dimensiones 10x10,
             * he creado un prefab que lo escala a 1x1 poniendole como escala 0.1 en los ejes X y Z. Para ampliarlo (al no ser sobre uno )    
             * 
             */

            GameObject plane = Instantiate(floorPrefab, new Vector3(roomCentre.x, 0, roomCentre.z), Quaternion.identity);
            //Debug.Log("Floor Scale X: " + plane.transform.localScale.x * originWidth + " Y:" + plane.transform.localScale.z * originLength);
            plane.transform.SetParent(roomParent.transform);
            plane.transform.localScale += new Vector3(plane.transform.localScale.x * originWidth - 0.1f, 0, plane.transform.localScale.z * originLength - 0.1f);

            //Debug.Log(queueOrder[0]);
            Fisher_YatesShuffle(queueOrder);
            
            for (int i = 0; i < queueOrder.Length; i++) 
            {
                if (queueOrder[i] == 1) 
                {
                    genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x - (originWidth / 2f), 0, roomCentre.z), dir.left));
                    //Instantiate(wallPrefab, new Vector3(roomCentre.x - (originWidth / 2f), 0, roomCentre.z), Quaternion.identity);
                }

                if (queueOrder[i] == 2)
                {
                    genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x, 0, roomCentre.z + (originLength / 2f)), dir.up));
                    //Instantiate(wallPrefab, new Vector3(roomCentre.x, 0, roomCentre.z + (originLength / 2f)), Quaternion.identity);
                }

                if (queueOrder[i] == 3)
                {
                    genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x, 0, roomCentre.z - (originLength / 2f)), dir.down));
                    //Instantiate(wallPrefab, new Vector3(roomCentre.x, 0, roomCentre.z - (originLength / 2f)), Quaternion.identity);
                }

                if (queueOrder[i] == 4)
                {
                    genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x + (originWidth / 2f), 0, roomCentre.z), dir.right));
                    //Instantiate(wallPrefab, new Vector3(roomCentre.x + (originWidth / 2f), 0, roomCentre.z), Quaternion.identity);
                }
                
            }
            /*
            genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x - (originWidth / 2f), 0, roomCentre.z), dir.left));
            Instantiate(wallPrefab, new Vector3(roomCentre.x - (originWidth / 2f), 0, roomCentre.z), Quaternion.identity);

            genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x, 0, roomCentre.z + (originLength / 2f)), dir.up));
            Instantiate(wallPrefab,         new Vector3(roomCentre.x, 0, roomCentre.z + (originLength / 2f)), Quaternion.identity);

            genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x, 0, roomCentre.z - (originLength / 2f)), dir.down));
            Instantiate(wallPrefab,         new Vector3(roomCentre.x, 0, roomCentre.z - (originLength / 2f)), Quaternion.identity);


            genQ.Enqueue(new generatorPoint(new Vector3(roomCentre.x + (originWidth / 2f), 0, roomCentre.z), dir.right));
            Instantiate(wallPrefab,         new Vector3(roomCentre.x + (originWidth / 2f), 0, roomCentre.z), Quaternion.identity);
            
            */
            
        }
    }


    void Fisher_YatesShuffle(int[] a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Length-1;  i > 0; i--)
        {
            // Número aleatorio entre 0 y i (de forma que i decrementa cada iteración)
            int rnd = Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            int temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posición aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }

    //public static List<generatorPoint> GetRoomsList() 
    //{
    //    return generatedRooms;
    //}



    // Update is called once per frame
    void Update()
    {
        if (!started) 
        {
            if (GlobalVar.middleX > -9000 && GlobalVar.middleZ > -9000)
            {
                started = true;
                Debug.Log("1:     " + aux1.coords);
                Debug.Log("2:     " + aux2.coords);
                Debug.Log("middle: " + GlobalVar.middleX + " " + GlobalVar.middleZ);
                //aux1.coords += new Vector3(0, 0.55f, 0); // para que no este en contacto con el suelo sino levitando sobre el y n haya colisión
                //aux2.coords += new Vector3(0, 0.55f, 0);
                //Cogemos los 2 ultimos generadores, calculamos la distrancia con el centro del mapa y escogemos aquel que est?mas alejado
                if (Mathf.Sqrt(
                                    Mathf.Pow((GlobalVar.middleX - aux1.coords.x), 2) + Mathf.Pow((GlobalVar.middleZ - aux1.coords.z), 2)) >


                        Mathf.Sqrt(
                                    Mathf.Pow((GlobalVar.middleX - aux2.coords.x), 2) + Mathf.Pow((GlobalVar.middleZ - aux2.coords.z), 2))


                                    ) { Debug.Log("escogido1"); Instantiate(exit, aux1.coords, Quaternion.identity); }
                else { Debug.Log("escogido2"); Instantiate(exit, aux2.coords, Quaternion.identity); }
                navMesh.BuildNavMesh();
                GlobalVar.remainingNPCs = GlobalVar.totalNPCs;
            }
        }
        
    }
}
