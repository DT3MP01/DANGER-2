using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject smoke;
    public float smokeSpreadSpeed;

    List<WorldGenerator.generatorPoint> rooms;
    private int randomIndex;
    bool started = false;
    WorldGenerator.generatorPoint room;
    private List<Vector3> firesPos;
    private Queue<Vector3> fireQ;
    private Vector3 newFire;
    private List<int> widths;
    private List<int> lengths;
    private bool first;
    private bool start;
    private FireGeneration fg;



    void Start()
    {
        start = true;
        fg = GetComponent<FireGeneration>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar.start) { GlobalVar.start = false; StartCoroutine(generarHumo(fg.indexX, fg.indexZ, fg.minAncho, fg.maxAlto)); }
        //else { Debug.Log("Waiting..."); }

    }

   

    
    
    private IEnumerator generarHumo(int x, int z, int minAncho, int maxAlto)
    {
        //Debug.Log("------------------------HUMO--------------------------");
        //Debug.Log("Gb" + GlobalVar.smokeMatrix.GetLength(0) + "," + GlobalVar.smokeMatrix.GetLength(1));
        //Debug.Log("x: " + x + "       Z: " + z);

        if (z < GlobalVar.smokeMatrix.GetLength(0) && x < GlobalVar.smokeMatrix.GetLength(1) && z >= 0 && x >= 0)
        {
            //Debug.Log("Gb" + GlobalVar.smokeMatrix.GetLength(0) + "," + GlobalVar.smokeMatrix.GetLength(1));
            //Debug.Log("Z: " + z + "       X: " + x);
            //Debug.Log(GlobalVar.smokeMatrix[z, x] == true);
            if (GlobalVar.smokeMatrix[z, x] == true)
            {
                //Debug.Log("alive------------------------------------------------------------");
                GlobalVar.smokeMatrix[z, x] = false;
                newFire = new Vector3(minAncho + x, 0, maxAlto - z);
                Instantiate(smoke, newFire, Quaternion.Euler(0f, 0f, 0f));
                 //if(smoke.GetComponent<Collider>().)
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x + 1, z, minAncho, maxAlto));
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x - 1, z, minAncho, maxAlto));
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x, z + 1, minAncho, maxAlto));
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x, z - 1, minAncho, maxAlto));

                /*
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x + 2, z, minAncho, maxAlto));
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x - 2, z, minAncho, maxAlto));
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x, z + 2, minAncho, maxAlto));
                yield return new WaitForSeconds(smokeSpreadSpeed);
                StartCoroutine(generarHumo(x, z - 2, minAncho, maxAlto));
                */
            }

        }



    }




    void Fisher_YatesShuffle(List<WorldGenerator.generatorPoint> a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Número aleatorio entre 0 y i (de forma que i decrementa cada iteración)
            int rnd = UnityEngine.Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            WorldGenerator.generatorPoint temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posición aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
            //if ("1" in ["2","3"]){ }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(collision.gameObject);
            
        }
    }
}
