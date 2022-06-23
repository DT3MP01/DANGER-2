using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fire;
    //public GameObject smoke;
    public float fireSpreadSpeed;
    //public float smokeSpreadSpeed;

    List<WorldGenerator.generatorPoint> rooms;
    private int randomIndex ;
    bool started = false;
    WorldGenerator.generatorPoint room;
    private List<Vector3> firesPos;
    private Queue<Vector3> fireQ;
    private Vector3 newFire;
    private List<int> widths;
    private List<int> lengths;
    private bool first;
    public int maxAncho;
    public int minAncho;
    public int maxAlto;
    public int minAlto;
    public int indexX;
    public int indexZ;
    public bool start = false;
    public bool[,] matrix;



    void Start()
    {
        rooms = GlobalVar.rooms;
        firesPos = new List<Vector3>();
        fireQ = new Queue<Vector3>();
        first = true;
        //startFire();
        //rooms = GlobalVar.rooms;
        //Instantiate(fire, new Vector3(-3, 0.5f, -3), Quaternion.Euler(0f, 0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        rooms = GlobalVar.rooms;
        if (rooms != null && !started) { 
                started = true; startFire2();
                
        }
        
    }

    public void startFire2()
    {
        rooms = GlobalVar.rooms;
        widths = GlobalVar.widths;
        lengths = GlobalVar.lengths;

        maxAncho = 0;
        minAncho = 0;
        maxAlto = 0;
        minAlto = 0;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (Mathf.CeilToInt(rooms[i].coords.x + widths[i] / 2f) > maxAncho) { maxAncho = Mathf.CeilToInt(rooms[i].coords.x + widths[i] / 2f); }
            if (Mathf.FloorToInt(rooms[i].coords.x - widths[i] / 2f) < minAncho) { minAncho = Mathf.FloorToInt(rooms[i].coords.x - widths[i] / 2f); }
            if (Mathf.CeilToInt(rooms[i].coords.z + lengths[i] / 2f) > maxAlto) { maxAlto = Mathf.CeilToInt(rooms[i].coords.z + lengths[i] / 2f); }
            if (Mathf.FloorToInt(rooms[i].coords.z - lengths[i] / 2f) < minAlto) { minAlto = Mathf.FloorToInt(rooms[i].coords.z - lengths[i] / 2f); }

        }
        
        matrix = new bool[maxAlto - minAlto, maxAncho - minAncho];
        Debug.Log("Matrix: " + matrix.GetLength(0) + "," + matrix.GetLength(1));
        GlobalVar.sizeX = matrix.GetLength(1);
        GlobalVar.sizeZ = matrix.GetLength(0);

        GlobalVar.middleX = minAncho + matrix.GetLength(1) / 2f;
        GlobalVar.middleZ = maxAlto - matrix.GetLength(0) / 2f;

        for (int z = 0; z < matrix.GetLength(0); z++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                int ancho = minAncho + x;
                int alto = maxAlto - z;

                for (int room = 0; room < rooms.Count; room++)
                {


                    // ancho son las coordenadas reales de la x, alto son las coordenadas reales de la z
                    // lo que queremos ver es si estamos en uno de los 4 generadores para dejar pasar solo por ahi el fuego
                    // para ello ancho tendra que ser igual al roomcords.x - width/2 o +width/2   y alto = 0

                    if (
                        (Mathf.Abs(Mathf.Abs(ancho) - Mathf.Abs(rooms[room].coords.x - widths[room] / 2f+1)) <= 1 ||
                         Mathf.Abs(Mathf.Abs(ancho) - Mathf.Abs(rooms[room].coords.x + widths[room] / 2f-1)) <= 1) &&

                        (Mathf.Abs(Mathf.Abs(alto) - Mathf.Abs(rooms[room].coords.z)) <= 1)


                        ) { matrix[z, x] = true; break; }


                    else if (
                        (Mathf.Abs(Mathf.Abs(alto) - Mathf.Abs(rooms[room].coords.z - lengths[room] / 2f+1)) <= 1 ||
                         Mathf.Abs(Mathf.Abs(alto) - Mathf.Abs(rooms[room].coords.z + lengths[room] / 2f-1)) <= 1) &&

                        (Mathf.Abs(Mathf.Abs(ancho) - Mathf.Abs(rooms[room].coords.x)) <= 1)


                        ) { matrix[z, x] = true; break; }

                    else 
                    {
                        int minX = Mathf.FloorToInt(rooms[room].coords.x - widths[room] / 2f);
                        int maxX = Mathf.CeilToInt(rooms[room].coords.x + widths[room] / 2f);
                        int minZ = Mathf.FloorToInt(rooms[room].coords.z - lengths[room] / 2f);
                        int maxZ = Mathf.CeilToInt(rooms[room].coords.z + lengths[room] / 2f);

                        if (ancho > minX + 1 & ancho < maxX - 1 & alto > minZ + 1 & alto < maxZ - 1) { matrix[z, x] = true; break; }
                    }


                    //if ((Mathf.Abs( Mathf.Abs(ancho) - Mathf.Abs(rooms[room].coords.x ))) <= 1  && (Mathf.Abs(Mathf.Abs(alto) - Mathf.Abs(rooms[room].coords.z - lengths[room] / 2f) )) <= 1 || (Mathf.Abs(Mathf.Abs(alto) - Mathf.Abs(rooms[room].coords.z + lengths[room] / 2f))) <= 1) { matrix[z, x] = true; break; }


                        

                    

                    

                }

            }
            //averiguar coordenadas reales: x= minAncho + anchoRestante, y = maxAlto-altoAvanzado

        }
        //seleccionamos primera posici�n true:
        /*
        int indexZ=0;
        int indexX=0;
        for (int z = 0; z < matrix.GetLength(0); z++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                if (matrix[z, x] == true) { indexZ = z;indexX = x;break; }
            }
        }
        */
        indexZ = Random.Range(0, matrix.GetLength(0) - 1);
        indexX = Random.Range(0, matrix.GetLength(1) - 1);
        while (matrix[indexZ, indexX] != true) 
        {
            indexZ = Random.Range(0, matrix.GetLength(0) - 1);
            indexX = Random.Range(0, matrix.GetLength(1) - 1);
        }


        string aux = "";
        for (int z = 0; z < matrix.GetLength(0); z++)
        {
            //aux = "";
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                if(matrix[z, x]) aux +="*" ;
                else { aux += " "; }
            }
            aux += "\n";
            
        }
        Debug.Log(aux);
                //SmokeGeneration sg = new SmokeGeneration();
                //SmokeGeneration.startSmoke(indexX,indexZ,minAncho,maxAlto);

                GlobalVar.matrix = matrix;
        GlobalVar.smokeMatrix = (bool[,])matrix.Clone();
        GlobalVar.minAncho = minAncho;
        GlobalVar.minAncho = maxAlto;
        GlobalVar.indexX = indexX;
        GlobalVar.indexZ = indexZ;

        Debug.Log("X_F:" + indexZ);
        Debug.Log("F:" + (GlobalVar.matrix[indexZ, indexX] == true).ToString());

        StartCoroutine(generarFuego(indexX, indexZ, minAncho, maxAlto));
        //StartCoroutine(generarHumo(indexX, indexZ, minAncho, maxAlto));
        
        GlobalVar.start = true;
        //SmokeGeneration sg = new SmokeGeneration();
        


    }

    public void  ClearFireCell(int x, int z, int minAncho, int maxAlto)
    {
        GlobalVar.matrix[z, x] = true;
        GlobalVar.fireDict[new Vector2(z,x)] =null;
        GlobalVar.fireDict.Remove(new Vector2(z,x));
        if(GlobalVar.fireDict.ContainsKey(new Vector2(z,x+1)))
        {
            FireScript fire = GlobalVar.fireDict[new Vector2(z,x+1)];
            fire.startFire();
        }
        if (GlobalVar.fireDict.ContainsKey(new Vector2(z, x - 1)))
        {
            FireScript fire = GlobalVar.fireDict[new Vector2(z, x - 1)];
            fire.startFire();
        }
        if (GlobalVar.fireDict.ContainsKey(new Vector2(z+1, x)))
        {
            FireScript fire = GlobalVar.fireDict[new Vector2(z+1, x)];
            fire.startFire();
        }
        if (GlobalVar.fireDict.ContainsKey(new Vector2(z-1, x)))
        {
            FireScript fire = GlobalVar.fireDict[new Vector2(z-1, x)];
            fire.startFire();
        }
        
    }

    public IEnumerator generarFuego(int x, int z, int minAncho, int maxAlto) 
    {
        //Debug.Log("Gb" + GlobalVar.matrix.GetLength(0) + "," + GlobalVar.matrix.GetLength(1));
        //Debug.Log("x: " + x + "       Z: " + z);
        yield return new WaitForSeconds(1f);
        if (z<GlobalVar.matrix.GetLength(0) && x<GlobalVar.matrix.GetLength(1) && z>=0 && x>=0) 
        {
            //Debug.Log("Gb" + GlobalVar.matrix.GetLength(0) + "," + GlobalVar.matrix.GetLength(1));
            //Debug.Log("Z: " + z + "       X: " + x);
            if (GlobalVar.matrix[z, x] == true) 
            {
                GlobalVar.matrix[z, x] = false;
               
                newFire = new Vector3(minAncho + x, 0, maxAlto - z);
                GameObject firePoint = Instantiate(fire, newFire, Quaternion.Euler(0f, 0f, 0f));
                GlobalVar.fireDict.Add(new Vector2(z,x), firePoint.GetComponent<FireScript>());
                firePoint.GetComponent<FireScript>().SetValues(x, z, minAncho, maxAlto);

            }
            
        }
        


    }



    void Fisher_YatesShuffle(List<WorldGenerator.generatorPoint> a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Count - 1; i > 0; i--)
        {
            // N�mero aleatorio entre 0 y i (de forma que i decrementa cada iteraci�n)
            int rnd = UnityEngine.Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            WorldGenerator.generatorPoint temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posici�n aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
            //if ("1" in ["2","3"]){ }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Colision");
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(collision.gameObject);

        }
    }
}
