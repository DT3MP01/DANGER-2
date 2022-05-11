using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RayCast : MonoBehaviour
{
    /// <summary>
    /// Variables:
    /// 
    ///     angle: indica el ángulo de visión (hacia cada lado respecto del central)
    ///     step:  indica el número de divisiones de este ángulo para crear los distintos rayos
    ///     
    /// El objetivo es partiendo de un ángulo, p.e 30? con un step de 10 crearíamos 3 rayos en cada uno dse los lados.
    /// </summary>

    [SerializeField]
    private float angle;
    //[SerializeField]
    //private int fragments;
    [SerializeField]
    private float step;

    //public GameObject text;
    
    private Color tintColor = Color.blue;
    private float remainingAngle;

    private Ray[] rays;
    private RaycastHit[] raycasthits;
    private int numberOfRays;
    private AgentMovement am;
    private bool seeFire;
    private bool changingCourse;
    
    // Start is called before the first frame update
    void Start()
    {
        
        angle /= 100;
        step /= 100;
        remainingAngle = angle;
        numberOfRays = (int) ((angle / step)*2 ); // ej 30?con un step de 10 son 3 rayos para cada lado son 6 rayos +1 del rayo central
        rays = new Ray[numberOfRays+1];
        raycasthits = new RaycastHit[numberOfRays+1];
        am = GetComponent<AgentMovement>();
        changingCourse = false;
        Raycast();
       
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
    }

    private void Raycast() 
    
    {
        //Ray frontal:
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Vector3 auxiliar = new Vector3();
        double dirX = Math.Round(direction[0]);
        double dirY = Math.Round(direction[2]);

        //Debug.Log("dir0:"+Math.Round(direction[0])+"     dir1:"+Math.Round(direction[1])+"      );       
        Ray frontRay = new Ray(origin, direction );
        if (Physics.Raycast(frontRay, out RaycastHit frontraycasthit, 10f))
        {
            Debug.DrawRay(origin, direction * 10f, Color.blue);     //rayo central
            //frontraycasthit.collider.GetComponent<Renderer>().material.color = Color.blue;
            float distance = frontraycasthit.distance;
            //Debug.Log("Colisión a :" + frontraycasthit.distance.ToString());
            //if (frontraycasthit.collider.gameObject.tag == "Wall") { Debug.Log("Thats a WALL"); }
            if (frontraycasthit.collider.gameObject.tag == "Fire") { am.stressLv +=0.0001f; seeFire = true; StartCoroutine(RunFromFire()); }
        }
        //Debug.Log("X:"+dirX+" Y:"+dirY);
        //while(remainingAngle > 0)
        if ((dirX == 1 || dirX == -1) && dirY == 0)
        {

            //sumar y restar en componente Z
            //auxiliar.Set(0, 0, remainingAngle);
            //Debug.DrawRay(origin, (direction + auxiliar) * 15f, Color.red);

            for (int i = 0; i < numberOfRays; i = i + 2)
            {
                auxiliar.Set(0, 0, remainingAngle);

                rays[i] = new Ray(origin, (direction + auxiliar));
                if (Physics.Raycast(rays[i], out raycasthits[i], 10f))
                {
                    Debug.DrawRay(origin, (direction + auxiliar) * 10f, Color.red);
                    //raycasthits[i].collider.GetComponent<Renderer>().material.color = Color.red;
                    if (raycasthits[i].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { am.stressLv += 0.0001f; } seeFire = true; StartCoroutine(RunFromFire()); }
                }

                rays[i + 1] = new Ray(origin, (direction - auxiliar));
                if (Physics.Raycast(rays[i + 1], out raycasthits[i + 1], 10f))
                {
                    Debug.DrawRay(origin, (direction - auxiliar) * 10f, Color.red);
                    //raycasthits[i + 1].collider.GetComponent<Renderer>().material.color = Color.red;
                    if (raycasthits[i+1].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { am.stressLv += 0.0001f; } seeFire = true; StartCoroutine(RunFromFire()); }
                }
                remainingAngle -= step;


            }
            remainingAngle = angle;
        }
        else if(dirX ==0 && (dirY ==1 || dirY == -1)) 
        {
            for (int i = 0; i < numberOfRays; i = i + 2)
            {
                auxiliar.Set(remainingAngle, 0, 0);

                rays[i] = new Ray(origin, (direction + auxiliar));
                if (Physics.Raycast(rays[i], out raycasthits[i], 10f))
                {
                    Debug.DrawRay(origin, (direction + auxiliar) * 10f, Color.red);
                    //raycasthits[i].collider.GetComponent<Renderer>().material.color = Color.red;
                    if (raycasthits[i].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { am.stressLv += 0.0001f; } seeFire = true; StartCoroutine(RunFromFire()); }
                }

                rays[i + 1] = new Ray(origin, (direction - auxiliar));
                if (Physics.Raycast(rays[i + 1], out raycasthits[i + 1], 10f))
                {
                    Debug.DrawRay(origin, (direction - auxiliar) * 10f, Color.red);
                    //raycasthits[i + 1].collider.GetComponent<Renderer>().material.color = Color.red;
                    if (raycasthits[i + 1].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { am.stressLv += 0.0001f; } seeFire = true; StartCoroutine(RunFromFire()); }
                }
                remainingAngle -= step;

            }
            remainingAngle = angle;
        }

        //Dirección:
        //Debug.Log(direction);
        
        else if ((direction[0] > 0.1 & direction[2] > 0.1) || (direction[0] < -0.1 & direction[2] < -0.1)) // si est?en el primer o tercer cuadrante
        {
                //Debug.Log(direction);
                //while(remainingAngle > 0)
                for(int i = 0; i< numberOfRays; i=i+2) 
                {
                    auxiliar.Set(remainingAngle, 0, (-1)*remainingAngle);
                    
                    
                    


                    rays[i] = new Ray(origin, (direction + auxiliar));
                    if (Physics.Raycast(rays[i], out raycasthits[i], 10f))
                    {
                        Debug.DrawRay(origin, (direction + auxiliar) * 10f, Color.red);
                        //raycasthits[i].collider.GetComponent<Renderer>().material.color = Color.red;
                        if (raycasthits[i].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { am.stressLv += 0.0001f; } seeFire = true; StartCoroutine(RunFromFire()); }
                }

                    rays[i+1] = new Ray(origin, (direction - auxiliar));
                    if (Physics.Raycast(rays[i+1], out raycasthits[i+1], 10f))
                    {
                        Debug.DrawRay(origin, (direction - auxiliar) * 10f, Color.red);
                        //raycasthits[i+1].collider.GetComponent<Renderer>().material.color = Color.red;
                        if (raycasthits[i + 1].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { am.stressLv += 0.0001f; } seeFire = true; StartCoroutine(RunFromFire()); }
                }
                remainingAngle -= step;

                }
                remainingAngle = angle;
                
         }

        else if ((direction[0] > 0.1 & direction[2] < -0.1) || (direction[0] < -0.1 & direction[2] > 0.1)) // si est?en el primer o tercer cuadrante
        {
            //Debug.Log(direction);
            for (int i = 0; i < numberOfRays; i = i + 2)
            {
                auxiliar.Set(remainingAngle, 0, remainingAngle);
                
                



                rays[i] = new Ray(origin, (direction + auxiliar));
                if (Physics.Raycast(rays[i], out raycasthits[i], 10f))
                {
                    Debug.DrawRay(origin, (direction + auxiliar) * 10f, Color.red);
                    //raycasthits[i].collider.GetComponent<Renderer>().material.color = Color.red;
                    if (raycasthits[i].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { if (am.stressLv < 1) { am.stressLv += 0.0001f; } }; seeFire = true; StartCoroutine(RunFromFire()); }
                }

                rays[i + 1] = new Ray(origin, (direction - auxiliar));
                if (Physics.Raycast(rays[i+1], out raycasthits[i+1], 10f))
                {
                    Debug.DrawRay(origin, (direction - auxiliar) * 10f, Color.red);
                    //raycasthits[i + 1].collider.GetComponent<Renderer>().material.color = Color.red;
                    if (raycasthits[i + 1].collider.gameObject.tag == "Fire") { if (am.stressLv < 1) { if (am.stressLv < 1) { am.stressLv += 0.0001f; } }; seeFire = true; StartCoroutine(RunFromFire()); }


                }
                remainingAngle -= step;

            }
            remainingAngle = angle;

        }

        if (!seeFire) { am.stressLv -= 0.00005f; }
        if (seeFire) { seeFire = false;  }

        

        


        }
    private IEnumerator RunFromFire()
    {
        if (!changingCourse) 
        {
            
            //am.stressLv += 0.3f;
            changingCourse = true;
            //yield return new WaitForSeconds(0.2f);
            am.runFromFire();

            yield return new WaitForSeconds(0.1f);
            changingCourse = false;
        }
        
    }
}

