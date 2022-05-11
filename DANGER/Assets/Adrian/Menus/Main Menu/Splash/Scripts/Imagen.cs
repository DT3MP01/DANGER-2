using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Imagen : MonoBehaviour
{
    RawImage imagen;
    [SerializeField]
    float angulo, velAng;
    // Start is called before the first frame update
    void Start()
    {
        imagen = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float seno = Mathf.Sin(angulo);
        angulo += velAng * Time.deltaTime;
    }
}
