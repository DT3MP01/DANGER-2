using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireScript : MonoBehaviour
{
    public float fireExtinguisherTime;
    public bool reignite;
    private int minAncho;
    private int maxAlto;
    private int x;
    private int z;
    private FireGeneration fireData;
    private float firePropagationTime;
    void Start()
    {
        firePropagationTime = 5f;
        fireData =GameObject.FindGameObjectWithTag("GameController").GetComponent<FireGeneration>();
        fireExtinguisherTime = 8f;
        reignite = false;
        StartCoroutine(startFire());

    }

    // Start is called before the first frame update
    void Update()
    {
        if(reignite){
        fireExtinguisherTime -= Time.deltaTime;
        if (fireExtinguisherTime <= 0)
        {
            GetComponent<ParticleSystem>().Play();
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<NavMeshObstacle>().enabled = true;
            reignite=false;
        }
        }
        if(firePropagationTime>0){
            firePropagationTime-=Time.deltaTime;

        }


    }
    public void SetValues(int x, int z, int minAncho, int maxAlto)
    {
        this.x = x;
        this.z = z;
        this.minAncho = minAncho;
        this.maxAlto = maxAlto;
    }

    public void startCooldown()
    {
        GetComponent<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshObstacle>().enabled = false;
        fireExtinguisherTime = 8f;
        reignite = true;
    }
    void OnDestroy()
    {
        fireData.ClearFireCell(x, z,minAncho,maxAlto);
    }

    IEnumerator startFire(){
        yield return new WaitForSeconds(fireData.fireSpreadSpeed);
        StartCoroutine(fireData.generarFuego(x + 1, z, minAncho, maxAlto));
        Debug.Log("Fuego generado en " + x + " " + z);
        StartCoroutine(fireData.generarFuego(x - 1, z, minAncho, maxAlto));
        Debug.Log("Fuego generado en " + x + " " + z);
        StartCoroutine(fireData.generarFuego(x, z + 1, minAncho, maxAlto));
        Debug.Log("Fuego generado en " + x + " " + z);
        StartCoroutine(fireData.generarFuego(x, z - 1, minAncho, maxAlto));
        Debug.Log("Fuego generado en " + x + " " + z);
        

    }

}
