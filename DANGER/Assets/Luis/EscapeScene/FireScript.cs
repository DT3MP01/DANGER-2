using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireScript : MonoBehaviour
{
    public bool propagateFire;
    private int minAncho;
    private int maxAlto;
    private int x;
    private int z;
    private FireGeneration fireData;
    private float firePropagationTime;
    void Start()
    {
        if (propagateFire)
        {
            firePropagationTime = 5f;
            fireData = GameObject.FindGameObjectWithTag("GameController").GetComponent<FireGeneration>();
            StartCoroutine(startFire());
        }


    }

    // Start is called before the first frame update

    public void SetValues(int x, int z, int minAncho, int maxAlto)
    {
        this.x = x;
        this.z = z;
        this.minAncho = minAncho;
        this.maxAlto = maxAlto;
    }

    public  void stopFire()
    {
        GetComponent<ParticleSystem>().Stop();
    }
    void OnDestroy()
    {
        if (propagateFire)
        {
            fireData.ClearFireCell(x, z, minAncho, maxAlto);
        }
    }

    public IEnumerator startFire(){
        yield return new WaitForSeconds(fireData.fireSpreadSpeed);
        StartCoroutine(fireData.generarFuego(x + 1, z, minAncho, maxAlto));
        yield return new WaitForSeconds(fireData.fireSpreadSpeed);
        StartCoroutine(fireData.generarFuego(x - 1, z, minAncho, maxAlto));
        yield return new WaitForSeconds(fireData.fireSpreadSpeed);
        StartCoroutine(fireData.generarFuego(x, z + 1, minAncho, maxAlto));
        yield return new WaitForSeconds(fireData.fireSpreadSpeed);
        StartCoroutine(fireData.generarFuego(x, z - 1, minAncho, maxAlto));
        

    }

}
