using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ObjectCreation : MonoBehaviour
{
    Ray rayCast;
    RaycastHit rayCastHit;
    float rayCastMax = 100.0f;
    float sens = 5f;
    float nearZoom = 1.5f;
    float farZoom = 1000;
    bool windowsSecure, doorsSecure, extinguishersSecure, reduced, redActive;

    public int meters, extinguishers, windows, doors, countScans;

    Dictionary<Vector3, Quaternion> faces;
    Dictionary<Vector3, GameObject> wallsRoom, extinguishersRoom;
         
    CubeObjects clonedCube, cube;
    GameObject referenceCube;

    Quaternion camRotation = Quaternion.identity;
    Quaternion rotation;
    Vector3 camPosition, focusPosition, dif, lastPos, mouseD, auxPos, prefabPosition;
    Vector3 amount = new Vector3(-137, -30, 75);

    [SerializeField] DBManager dbManager;

    [SerializeField] GameObject UIBorder;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject selectedItemText;
    [SerializeField] GameObject MetersText;
    [SerializeField] GameObject DoorsText;
    [SerializeField] GameObject WindowsText;
    [SerializeField] GameObject ExtinguishersText;
    [SerializeField] GameObject roomSecuredText;

    [SerializeField] GameObject doorsAdvise;
    [SerializeField] GameObject windowsAdvise;
    [SerializeField] GameObject extinguishersAdvise;

    [SerializeField] GameObject select;
    [SerializeField] GameObject selectBubble;
    [SerializeField] GameObject selectExterior;
    [SerializeField] GameObject selectExtinguisher;

    [SerializeField] GameObject selectRed;
    [SerializeField] GameObject selectBubbleRed;
    [SerializeField] GameObject selectExteriorRed;
    [SerializeField] GameObject selectExtinguisherRed;

    [SerializeField] GameObject prefab;

    [SerializeField] GameObject initialCube;

    [SerializeField] GameObject cubes;
    [SerializeField] GameObject prefabCube;
    [SerializeField] GameObject prefabCubeBlackPattern;
    [SerializeField] GameObject prefabCubeBlackWood;
    [SerializeField] GameObject prefabCubeGreyWood;
    [SerializeField] GameObject prefabCubeOrangeWood;
    [SerializeField] GameObject prefabCubeRedPattern;
    [SerializeField] GameObject prefabCubeWhitePattern;
    [SerializeField] GameObject prefabCubeWhiteWood;

    [SerializeField] GameObject prefabWall;
    [SerializeField] GameObject prefabDoor;
    [SerializeField] GameObject prefabExtinguisher;
    [SerializeField] GameObject prefabTable;
    [SerializeField] GameObject prefabWindow;

    void Start()
    {
        meters = 1;
        extinguishers = 0;
        windows = 0;
        countScans = 0;
        doors = 0;
        extinguishersSecure = false;
        reduced = false;
        windowsSecure = false;
        doorsSecure = false;
        redActive = false;

        rotation = Quaternion.identity;

        Rotation();

        wallsRoom = new Dictionary<Vector3, GameObject>();
        extinguishersRoom = new Dictionary<Vector3, GameObject>();

        faces = new Dictionary<Vector3, Quaternion>()
        {
            {new Vector3(0, 0, -1.0f), Quaternion.identity},
            {new Vector3(0, 1.0f, 0), Quaternion.Euler(90,0,0)},
            {new Vector3(-1.0f, 0, 0), Quaternion.Euler(0,90,0)},
            {new Vector3(0, 0, 1.0f), Quaternion.Euler(0,180,0)},
            {new Vector3(0, -1.0f, 0), Quaternion.Euler(270,0,0)},
            {new Vector3(1.0f, 0, 0), Quaternion.Euler(0,270,0)}
        };
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu Principal");
        }

        rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayCast.origin, rayCast.direction, out rayCastHit, rayCastMax))
        {
            dif = (rayCastHit.point - rayCastHit.transform.position) * 2;

            if (Mathf.Abs(dif.x) < 0.95f) { dif.x = 0; }
            if (Mathf.Abs(dif.y) < 0.95f) { dif.y = 0; }
            if (Mathf.Abs(dif.z) < 0.95f) { dif.z = 0; }

            if(rayCastHit.transform.tag == "Cube")
            {
                if (faces.ContainsKey(dif))
                {
                    select.SetActive(true);
                    selectBubble.SetActive(false);
                    selectExtinguisher.SetActive(false);
                    selectExterior.SetActive(false);
                    select.transform.position = rayCastHit.transform.position;
                    select.transform.rotation = faces[dif];
                }

                if ((prefab != null) && (prefab.transform.tag == "Wall" || prefab.transform.tag == "Window" || prefab.transform.tag == "Door") && dif == new Vector3(0, 1.0f, 0))
                {
                    if (meters == 1)
                    {
                        cube = initialCube.GetComponent(typeof(CubeObjects)) as CubeObjects;
                    }
                    else
                    {
                        cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;
                    }

                    if (Vector3.Dot(Camera.main.transform.forward, Vector3.forward) > 0.4 && cube.exteriors[new Vector3(0f, 1.5f, 0.35f)])
                    {
                        selectExterior.transform.rotation = Quaternion.identity;
                    }
                    else if (Vector3.Dot(Camera.main.transform.forward, Vector3.back) > 0.4 && cube.exteriors[new Vector3(0f, 1.5f, -0.35f)])
                    {
                        selectExterior.transform.rotation = Quaternion.Euler(0, 180, 0);
                        
                    }
                    else if (Vector3.Dot(Camera.main.transform.forward, Vector3.right) > 0.4 && cube.exteriors[new Vector3(0.35f, 1.5f, 0f)])
                    {
                        selectExterior.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (Vector3.Dot(Camera.main.transform.forward, Vector3.left) > 0.4 && cube.exteriors[new Vector3(-0.35f, 1.5f, 0f)])
                    {
                        selectExterior.transform.rotation = Quaternion.Euler(0, 270, 0);
                    }
                    else if (cube.exteriors[new Vector3(0f, 1.5f, 0.35f)])
                    {
                        selectExterior.transform.rotation = Quaternion.identity;
                    }
                    else if (cube.exteriors[new Vector3(0f, 1.5f, -0.35f)])
                    {
                        selectExterior.transform.rotation = Quaternion.Euler(0, 180, 0);

                    }
                    else if (cube.exteriors[new Vector3(0.35f, 1.5f, 0f)])
                    {
                        selectExterior.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (cube.exteriors[new Vector3(-0.35f, 1.5f, 0f)])
                    {
                        selectExterior.transform.rotation = Quaternion.Euler(0, 270, 0);
                    }

                    select.SetActive(false);
                    selectBubble.SetActive(false);
                    selectExterior.SetActive(true);
                    selectExterior.transform.position = new Vector3(rayCastHit.transform.position.x, selectExterior.transform.position.y, rayCastHit.transform.position.z);
                }
                else 
                {
                    selectExterior.SetActive(false);
                }
            }
            else
            {    
                if (rayCastHit.transform.tag == "Wall")
                {
                    if (prefab.transform.tag == "Extinguisher" && !reduced)
                    {
                        auxPos = rayCastHit.transform.position;
                        prefabPosition = FindParentWithTag(rayCastHit.transform.gameObject, "Cube").transform.position;

                        if (meters == 1)
                        {
                            cube = initialCube.GetComponent(typeof(CubeObjects)) as CubeObjects;
                        }
                        else
                        {
                            cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;
                        }

                        if (cube.extinguishers[new Vector3(0f, 1.5f, 0.35f)] && (rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.5f, 0.35f))
                        {
                            selectExtinguisher.transform.rotation = Quaternion.identity;
                            selectExtinguisher.transform.position = new Vector3(auxPos.x, 0f, auxPos.z - 0.35f);
                            selectExtinguisher.SetActive(true);
                            select.SetActive(false);
                            selectBubble.SetActive(false);
                            selectExterior.SetActive(false);
                        }
                        else if (cube.extinguishers[new Vector3(0f, 1.5f, -0.35f)] && (rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.5f, -0.35f))
                        {
                            selectExtinguisher.transform.rotation = Quaternion.Euler(0, 180, 0);
                            selectExtinguisher.transform.position = new Vector3(auxPos.x, 0f, auxPos.z + 0.35f);
                            selectExtinguisher.SetActive(true);
                            select.SetActive(false);
                            selectBubble.SetActive(false);
                            selectExterior.SetActive(false);

                        }
                        else if (cube.extinguishers[new Vector3(0.35f, 1.5f, 0f)] && (rayCastHit.transform.position - prefabPosition) == new Vector3(0.35f, 1.5f, 0f))
                        {
                            selectExtinguisher.transform.rotation = Quaternion.Euler(0, 90, 0);
                            selectExtinguisher.transform.position = new Vector3(auxPos.x - 0.35f, 0f, auxPos.z);
                            selectExtinguisher.SetActive(true);
                            select.SetActive(false);
                            selectBubble.SetActive(false);
                            selectExterior.SetActive(false);
                        }
                        else if (cube.extinguishers[new Vector3(-0.35f, 1.5f, 0f)] && (rayCastHit.transform.position - prefabPosition) == new Vector3(-0.35f, 1.5f, 0f))
                        {
                            selectExtinguisher.transform.rotation = Quaternion.Euler(0, 270, 0);
                            selectExtinguisher.transform.position = new Vector3(auxPos.x + 0.35f, 0f, auxPos.z);
                            selectExtinguisher.SetActive(true);
                            select.SetActive(false);
                            selectBubble.SetActive(false);
                            selectExterior.SetActive(false);
                        }
                        else 
                        {
                            selectBubble.transform.position = new Vector3(auxPos.x, 1.5f, auxPos.z);
                            selectBubble.SetActive(true);
                            selectExtinguisher.SetActive(false);
                            select.SetActive(false);
                            selectExterior.SetActive(false);
                        }
                    }
                    else
                    {
                        auxPos = rayCastHit.transform.position;
                        selectBubble.transform.position = new Vector3(auxPos.x, 1.5f, auxPos.z);
                        selectBubble.SetActive(true);
                        selectExtinguisher.SetActive(false);
                        selectExterior.SetActive(false);
                        select.SetActive(false);
                    }
                }
                else
                {
                    select.SetActive(false);
                    selectExtinguisher.SetActive(false);
                    auxPos = rayCastHit.transform.position;
                    selectBubble.transform.position = new Vector3(auxPos.x, 1.5f, auxPos.z);
                    selectBubble.SetActive(true);
                }
            }
        }
        else
        {
            select.SetActive(false);
            selectBubble.SetActive(false);
            selectExterior.SetActive(false);
            selectExtinguisher.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (rayCastHit.transform == null) return;

            if (!Input.GetKey(KeyCode.LeftControl) && rayCastHit.transform.tag != "Untagged")
            {
                lastPos = Vector3.zero;

                if (rayCastHit.transform.tag == "Cube")
                {
                    if (rayCastHit.transform.position == Vector3.zero)
                    {
                        if(select.transform.rotation == Quaternion.Euler(90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0.00011f, 0f);
                        else if (select.transform.rotation == Quaternion.Euler(-90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, -0.00011f, 0f);
                        else if (select.transform.rotation == Quaternion.Euler(0, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, -0.00011f);
                        else if (select.transform.rotation == Quaternion.Euler(0, -90, 0)) selectRed.transform.position = select.transform.position + new Vector3(0.00111f, 0f, 0f);
                        else if (select.transform.rotation == Quaternion.Euler(0, 90, 0)) selectRed.transform.position = select.transform.position + new Vector3(-0.00011f, 0f, 0f);
                        else if (select.transform.rotation == Quaternion.Euler(0, -180, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, 0.1f);

                        selectRed.transform.rotation = select.transform.rotation;
                        select.SetActive(false);
                        selectRed.SetActive(true);

                        StartCoroutine(disableSelectors());

                        return;
                    }
                    else
                    {
                        foreach (Transform child in rayCastHit.transform)
                        {
                            if (child.tag == "Wall")
                            {
                                wallsRoom.Remove(child.transform.position);

                                if(child.transform.childCount > 0)
                                {
                                    extinguishers--;
                                }
                            }
                            else if (child.tag == "Door")
                            {
                                wallsRoom.Remove(child.transform.position);

                                doors--;
                            }
                            else if (child.tag == "Window")
                            {
                                wallsRoom.Remove(child.transform.position);

                                windows--;
                            }
                        }

                        Destroy(rayCastHit.transform.gameObject, 0.1f);

                        meters--;
                    }
                }
                else if (rayCastHit.transform.tag == "Wall")
                {
                    prefabPosition = FindParentWithTag(rayCastHit.transform.gameObject, "Cube").transform.position;
                    cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;

                    if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.5f, 0.35f) || (rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1f, 0.35f))
                    {

                        cube.deleteItem("Wall", new Vector3(0f, 1.5f, 0.35f));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        
                        if (cube.extinguishers[new Vector3(0f, 1.5f, 0.35f)] == false)
                        {
                            cube.deleteItem("Extinguisher", new Vector3(0f, 1.5f, 0.35f));

                            foreach (Transform child in rayCastHit.transform)
                            {
                                if (child.tag == "Extinguisher")
                                    extinguishersRoom.Remove(child.transform.position);
                            }

                            extinguishers--;
                        }
                    }
                    else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0.0f, 1.5f, -0.35f) || (rayCastHit.transform.position - prefabPosition) == new Vector3(0.0f, 1f, -0.35f))
                    {

                        cube.deleteItem("Wall", new Vector3(0f, 1.5f, -0.35f));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        
                        if (cube.extinguishers[new Vector3(0f, 1.5f, -0.35f)] == false)
                        {
                            cube.deleteItem("Extinguisher", new Vector3(0.0f, 1.5f, -0.35f));

                            foreach (Transform child in rayCastHit.transform)
                            {
                                if (child.tag == "Extinguisher")
                                    extinguishersRoom.Remove(child.transform.position);
                            }

                            extinguishers--;
                        }
                    }
                    else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0.35f, 1.5f, 0) || (rayCastHit.transform.position - prefabPosition) == new Vector3(0.35f, 1f, 0))
                    {

                        cube.deleteItem("Wall", new Vector3(0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);

                        if (cube.extinguishers[new Vector3(0.35f, 1.5f, 0)] == false)
                        {
                            cube.deleteItem("Extinguisher", new Vector3(0.35f, 1.5f, 0));

                            foreach (Transform child in rayCastHit.transform)
                            {
                                if (child.tag == "Extinguisher")
                                    extinguishersRoom.Remove(child.transform.position);
                            }

                            extinguishers--;
                        }
                    }
                    else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(-0.35f, 1.5f, 0) || (rayCastHit.transform.position - prefabPosition) == new Vector3(-0.35f, 1f, 0))
                    {
                        cube.deleteItem("Wall", new Vector3(-0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        
                        if (cube.extinguishers[new Vector3(-0.35f, 1.5f, 0)] == false)
                        {
                            cube.deleteItem("Extinguisher", new Vector3(-0.35f, 1.5f, 0));

                            foreach (Transform child in rayCastHit.transform)
                            {
                                if (child.tag == "Extinguisher")
                                    extinguishersRoom.Remove(child.transform.position);
                            }
                            
                            extinguishers--;
                        }
                    }
                    else
                    {
                        selectBubbleRed.transform.position = selectBubble.transform.position;
                        selectBubbleRed.transform.rotation = selectBubble.transform.rotation;
                        selectBubble.SetActive(false);
                        selectBubbleRed.SetActive(true);

                        StartCoroutine(disableSelectors());

                        return;
                    }

                    wallsRoom.Remove(rayCastHit.transform.position);
                }
                else if (rayCastHit.transform.tag == "Door")
                {
                    cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;
                    prefabPosition = cube.transform.position;

                    if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.49f, 0.26f) || (rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 0.995f, 0.26f))
                    {
                        cube.deleteItem("Door", new Vector3(0f, 1.5f, 0.35f));
                        Destroy(rayCastHit.transform.gameObject, 0.1f);
                        doors--;
                    }
                    else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.49f, -0.26f) || (rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 0.995f, -0.26f))
                    {
                        cube.deleteItem("Door", new Vector3(0f, 1.5f, -0.35f));
                        Destroy(rayCastHit.transform.gameObject, 0.1f);
                        doors--;
                    }
                    else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0.26f, 1.49f, 0f) || (rayCastHit.transform.position - prefabPosition) == new Vector3(0.26f, 0.995f, 0f))
                    {
                        cube.deleteItem("Door", new Vector3(0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.gameObject, 0.1f);
                        doors--;
                    }
                    else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(-0.26f, 1.49f, 0f) || (rayCastHit.transform.position - prefabPosition) == new Vector3(-0.26f, 0.995f, 0f))
                    {
                        cube.deleteItem("Door", new Vector3(-0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.gameObject, 0.1f);
                        doors--;
                    }
                    else
                    {
                        selectBubbleRed.transform.position = selectBubble.transform.position;
                        selectBubbleRed.transform.rotation = selectBubble.transform.rotation;
                        selectBubble.SetActive(false);
                        selectBubbleRed.SetActive(true);

                        StartCoroutine(disableSelectors());

                        return;
                    }

                    wallsRoom.Remove(rayCastHit.transform.position);
                }
                else if (rayCastHit.transform.tag == "Window")
                {
                    cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;
                    prefabPosition = cube.transform.position;

                    if ((rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(0f, 1f, 0.349f) || (rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(0f, 0.75f, 0.349f))
                    {
                        cube.deleteItem("Window", new Vector3(0f, 1.5f, 0.35f));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        windows--;
                    }
                    else if ((rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(0f, 1f, -0.349f) || (rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(0f, 0.75f, -0.349f))
                    {
                        cube.deleteItem("Window", new Vector3(0f, 1.5f, -0.35f));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        windows--;
                    }
                    else if ((rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(0.349f, 1f, 0f) || (rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(0.349f, 0.75f, 0f))
                    {
                        cube.deleteItem("Window", new Vector3(0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        windows--;
                    }
                    else if ((rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(-0.349f, 1f, 0f) || (rayCastHit.transform.parent.transform.position - prefabPosition) == new Vector3(-0.349f, 0.75f, 0f))
                    {
                        cube.deleteItem("Window", new Vector3(-0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        windows--;
                    }
                    else
                    {
                        selectBubbleRed.transform.position = selectBubble.transform.position;
                        selectBubbleRed.transform.rotation = selectBubble.transform.rotation;
                        selectBubble.SetActive(false);
                        selectBubbleRed.SetActive(true);

                        StartCoroutine(disableSelectors());

                        return;
                    }

                    wallsRoom.Remove(rayCastHit.transform.position);
                }
                else if (rayCastHit.transform.tag == "Extinguisher")
                {
                    cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;
                    prefabPosition = FindParentWithTag(rayCastHit.transform.gameObject, "Cube").transform.position;

                    if ((rayCastHit.transform.parent.gameObject.transform.position - prefabPosition) == new Vector3(-3.43f, 0.11f, 2.95f))
                    {
                        cube.deleteItem("Extinguisher", new Vector3(0f, 1.5f, 0.35f));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        extinguishers--;
                    }
                    else if ((rayCastHit.transform.parent.gameObject.transform.position - prefabPosition) == new Vector3(2.95f, 0.11f, 3.430001f))
                    {
                        cube.deleteItem("Extinguisher", new Vector3(0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        extinguishers--;
                    }
                    else if ((rayCastHit.transform.parent.gameObject.transform.position - prefabPosition) == new Vector3(-3.43f, 0.11f, 2.75f))
                    {
                        cube.deleteItem("Extinguisher", new Vector3(0f, 1.5f, -0.35f));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        extinguishers--;
                    }
                    else if ((rayCastHit.transform.parent.gameObject.transform.position - prefabPosition) == new Vector3(2.750001f, 0.11f, 3.430001f))
                    {
                        cube.deleteItem("Extinguisher", new Vector3(-0.35f, 1.5f, 0));
                        Destroy(rayCastHit.transform.parent.gameObject, 0.1f);
                        extinguishers--;
                    }
                    else
                    {
                        selectBubbleRed.transform.position = selectBubble.transform.position;
                        selectBubbleRed.transform.rotation = selectBubble.transform.rotation;
                        selectBubble.SetActive(false);
                        selectBubbleRed.SetActive(true);

                        StartCoroutine(disableSelectors());

                        return;
                    }
                    extinguishersRoom.Remove(rayCastHit.transform.position);
                }
                else if (rayCastHit.transform.tag == "Table")
                {
                    cube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;

                    cube.deleteItem("Table", new Vector3(0f, 0f, 0f));
                    Destroy(rayCastHit.transform.gameObject, 0.1f);
                }
                else
                {
                    selectBubbleRed.transform.position = selectBubble.transform.position;
                    selectBubbleRed.transform.rotation = selectBubble.transform.rotation;
                    selectBubble.SetActive(false);
                    selectBubbleRed.SetActive(true);

                    StartCoroutine(disableSelectors());

                    return;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if ((select.activeSelf || selectExterior.activeSelf || selectBubble) && !Input.GetKey(KeyCode.LeftControl) && !selectExtinguisher.activeSelf)
            {
                if (rayCastHit.transform == null) return;

                if (prefab == null || (prefab.transform.tag == "Cube" && (dif == new Vector3(0, -1.0f, 0) || dif == new Vector3(0, 1.0f, 0))) || (rayCastHit.transform.tag == "Cube" && dif != new Vector3(0, 1.0f, 0) && prefab.transform.tag != "Cube") || (rayCastHit.transform.tag == "Cube" && dif == new Vector3(0, 1.0f, 0) && prefab.transform.tag == "Extinguisher"))
                {
                    if (select.transform.rotation == Quaternion.Euler(90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0.00011f, 0f);
                    else if (select.transform.rotation == Quaternion.Euler(-90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, -0.00011f, 0f);
                    else if (select.transform.rotation == Quaternion.Euler(0, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, -0.00011f);
                    else if (select.transform.rotation == Quaternion.Euler(0, -90, 0)) selectRed.transform.position = select.transform.position + new Vector3(0.00111f, 0f, 0f);
                    else if (select.transform.rotation == Quaternion.Euler(0, 90, 0)) selectRed.transform.position = select.transform.position + new Vector3(-0.00011f, 0f, 0f);
                    else if (select.transform.rotation == Quaternion.Euler(0, -180, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, 0.1f);

                    selectRed.transform.rotation = select.transform.rotation;
                    select.SetActive(false);
                    selectRed.SetActive(true);

                    StartCoroutine(disableSelectors());

                    return;
                }

                if(rayCastHit.transform.tag != "Cube")
                {
                    selectBubbleRed.transform.position = selectBubble.transform.position;
                    selectBubbleRed.transform.rotation = selectBubble.transform.rotation;
                    selectBubble.SetActive(false);
                    selectBubbleRed.SetActive(true);

                    StartCoroutine(disableSelectors());

                    return;
                }

                if (rayCastHit.transform.tag == "Cube")
                {

                    lastPos = dif + rayCastHit.transform.position;

                    if (dif == new Vector3(0, 1.0f, 0))
                    {
                        clonedCube = rayCastHit.transform.gameObject.GetComponentInParent(typeof(CubeObjects)) as CubeObjects;
                        if (clonedCube == null) return;

                        if (prefab.transform.tag.Equals("Door"))
                        {
                            if ((Vector3.Dot(Camera.main.transform.forward, Vector3.forward) > 0.4) && (clonedCube.putItem("Door", new Vector3(0f, 1.5f, 0.35f)))) 
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.49f, 0.26f);
                                rotation = Quaternion.identity;
                                
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.back) > 0.4) && (clonedCube.putItem("Door", new Vector3(0f, 1.5f, -0.35f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.49f, -0.26f);
                                rotation = Quaternion.Euler(0, 180, 0);       
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.right) > 0.4) && (clonedCube.putItem("Door", new Vector3(0.35f, 1.5f, 0f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0.26f, 1.49f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.left) > 0.4) && (clonedCube.putItem("Door", new Vector3(-0.35f, 1.5f, 0f))))
                            {
                                    lastPos = rayCastHit.transform.position + new Vector3(-0.26f, 1.49f, 0f);
                                    rotation = Quaternion.Euler(0, -90, 0);
                            }
                            else if (clonedCube.putItem("Door", new Vector3(0f, 1.5f, 0.35f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.49f, 0.26f);
                                rotation = Quaternion.identity;

                            }
                            else if (clonedCube.putItem("Door", new Vector3(0f, 1.5f, -0.35f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.49f, -0.26f);
                                rotation = Quaternion.Euler(0, 180, 0);
                            }
                            else if (clonedCube.putItem("Door", new Vector3(0.35f, 1.5f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0.26f, 1.49f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else if (clonedCube.putItem("Door", new Vector3(-0.35f, 1.5f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(-0.26f, 1.49f, 0f);
                                rotation = Quaternion.Euler(0, -90, 0);
                            }
                            else
                            {
                                selectExteriorRed.transform.position = selectExterior.transform.position;
                                selectExteriorRed.transform.rotation = selectExterior.transform.rotation;
                                selectExterior.SetActive(false);
                                selectExteriorRed.SetActive(true);

                                StartCoroutine(disableSelectors());

                                return;
                            }

                            doors++;
                        }
                        else if (prefab.transform.tag.Equals("Wall"))
                        {
                            if ((Vector3.Dot(Camera.main.transform.forward, Vector3.forward) > 0.4) && (clonedCube.putItem("Wall", new Vector3(0f, 1.5f, 0.35f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.5f, 0.35f);
                                rotation = Quaternion.identity;
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.back) > 0.4) && (clonedCube.putItem("Wall", new Vector3(0f, 1.5f, -0.35f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.5f, -0.35f);
                                rotation = Quaternion.identity;
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.right) > 0.4) && (clonedCube.putItem("Wall", new Vector3(0.35f, 1.5f, 0f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0.35f, 1.5f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);

                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.left) > 0.4) && (clonedCube.putItem("Wall", new Vector3(-0.35f, 1.5f, 0f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(-0.35f, 1.5f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else if (clonedCube.putItem("Wall", new Vector3(0f, 1.5f, 0.35f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.5f, 0.35f);
                                rotation = Quaternion.identity;
                            }
                            else if (clonedCube.putItem("Wall", new Vector3(0f, 1.5f, -0.35f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.5f, -0.35f);
                                rotation = Quaternion.identity;
                            }
                            else if (clonedCube.putItem("Wall", new Vector3(0.35f, 1.5f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0.35f, 1.5f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);

                            }
                            else if (clonedCube.putItem("Wall", new Vector3(-0.35f, 1.5f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(-0.35f, 1.5f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else
                            {
                                selectExteriorRed.transform.position = selectExterior.transform.position;
                                selectExteriorRed.transform.rotation = selectExterior.transform.rotation;
                                selectExterior.SetActive(false);
                                selectExteriorRed.SetActive(true);

                                StartCoroutine(disableSelectors());

                                return;
                            }
                        }
                        else if (prefab.transform.tag.Equals("Window"))
                        {
                            if ((Vector3.Dot(Camera.main.transform.forward, Vector3.forward) > 0.4) && (clonedCube.putItem("Window", new Vector3(0f, 1.5f, 0.35f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1f, 0.349f);
                                rotation = Quaternion.identity;
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.back) > 0.4) && (clonedCube.putItem("Window", new Vector3(0f, 1.5f, -0.35f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1f, -0.349f);
                                rotation = Quaternion.identity;
                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.right) > 0.4) && (clonedCube.putItem("Window", new Vector3(0.35f, 1.5f, 0f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0.349f, 1f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);

                            }
                            else if ((Vector3.Dot(Camera.main.transform.forward, Vector3.left) > 0.4) && (clonedCube.putItem("Window", new Vector3(-0.35f, 1.5f, 0f))))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(-0.349f, 1f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else if (clonedCube.putItem("Window", new Vector3(0f, 1.5f, 0.35f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1f, 0.349f);
                                rotation = Quaternion.identity;
                            }
                            else if (clonedCube.putItem("Window", new Vector3(0f, 1.5f, -0.35f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1f, -0.349f);
                                rotation = Quaternion.identity;
                            }
                            else if (clonedCube.putItem("Window", new Vector3(0.35f, 1.5f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0.349f, 1f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);

                            }
                            else if (clonedCube.putItem("Window", new Vector3(-0.35f, 1.5f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(-0.349f, 1f, 0f);
                                rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else
                            {
                                selectExteriorRed.transform.position = selectExterior.transform.position;
                                selectExteriorRed.transform.rotation = selectExterior.transform.rotation;
                                selectExterior.SetActive(false);
                                selectExteriorRed.SetActive(true);

                                StartCoroutine(disableSelectors());

                                return;
                            }

                            windows++;
                        }
                        else if (prefab.transform.tag.Equals("Table"))
                        {
                            if (clonedCube.putItem("Table", new Vector3(0f, 0f, 0f)))
                            {
                                lastPos = rayCastHit.transform.position + new Vector3(0f, 1.149f, 0f);
                                rotation = Quaternion.identity;
                            }
                            else
                            {
                                if (select.transform.rotation == Quaternion.Euler(90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0.00011f, 0f);
                                else if (select.transform.rotation == Quaternion.Euler(-90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, -0.00011f, 0f);
                                else if (select.transform.rotation == Quaternion.Euler(0, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, -0.00011f);
                                else if (select.transform.rotation == Quaternion.Euler(0, -90, 0)) selectRed.transform.position = select.transform.position + new Vector3(0.00111f, 0f, 0f);
                                else if (select.transform.rotation == Quaternion.Euler(0, 90, 0)) selectRed.transform.position = select.transform.position + new Vector3(-0.00011f, 0f, 0f);
                                else if (select.transform.rotation == Quaternion.Euler(0, -180, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, 0.1f);

                                selectRed.transform.rotation = select.transform.rotation;
                                select.SetActive(false);
                                selectRed.SetActive(true);

                                StartCoroutine(disableSelectors());

                                return;
                            }
                        }
                        else
                        {
                            if (select.transform.rotation == Quaternion.Euler(90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0.00011f, 0f);
                            else if (select.transform.rotation == Quaternion.Euler(-90, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, -0.00011f, 0f);
                            else if (select.transform.rotation == Quaternion.Euler(0, 0, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, -0.00011f);
                            else if (select.transform.rotation == Quaternion.Euler(0, -90, 0)) selectRed.transform.position = select.transform.position + new Vector3(0.00111f, 0f, 0f);
                            else if (select.transform.rotation == Quaternion.Euler(0, 90, 0)) selectRed.transform.position = select.transform.position + new Vector3(-0.00011f, 0f, 0f);
                            else if (select.transform.rotation == Quaternion.Euler(0, -180, 0)) selectRed.transform.position = select.transform.position + new Vector3(0f, 0f, 0.1f);

                            selectRed.transform.rotation = select.transform.rotation;
                            select.SetActive(false);
                            selectRed.SetActive(true);

                            StartCoroutine(disableSelectors());

                            return;
                        }
                    }

                    if(prefab.transform.tag.Equals("Cube"))
                    {
                        meters++;
                    }

                    GameObject clone = Instantiate(prefab) as GameObject;
                    clone.transform.position = lastPos;
                    clone.transform.rotation = rotation;

                    if (clone.transform.tag == "Wall" || clone.transform.tag == "Door" || clone.transform.tag == "Window") 
                    {
                        if (reduced)
                        {
                            if (clone.transform.tag == "Window")
                            {
                                clone.transform.localScale = new Vector3(clone.transform.localScale.x, 0.5f, clone.transform.localScale.z);
                                clone.transform.position = new Vector3(clone.transform.position.x, 0.75f, clone.transform.position.z);
                            }
                            else if (clone.transform.tag == "Wall")
                            {
                                clone.transform.localScale = new Vector3(clone.transform.localScale.x, 0.5f, clone.transform.localScale.z);
                                clone.transform.position = new Vector3(clone.transform.position.x, 1f, clone.transform.position.z);
                            }
                            else if (clone.transform.tag == "Door")
                            {
                                clone.transform.localScale = new Vector3(clone.transform.localScale.x, 0.48f, clone.transform.localScale.z);
                                clone.transform.position = new Vector3(clone.transform.position.x, 0.995f, clone.transform.position.z);
                            }
                        }

                        if (!wallsRoom.ContainsKey(clone.transform.position))
                        {
                            wallsRoom.Add(clone.transform.position, clone);
                        }
                    }

                    if (prefab.transform.tag != "Cube") clone.transform.parent = rayCastHit.transform;
                    
                    clone.SetActive(true);
                    
                    if (clone.transform.tag == "Cube")
                    {
                        foreach (Transform child in clone.transform)
                        {
                            Destroy(child.gameObject);
                        }

                        clone.transform.parent = cubes.transform;
                    }

                    StartCoroutine(waitASec());
                }
            }
            else if (selectExtinguisher.activeSelf)
            {
                referenceCube = FindParentWithTag(rayCastHit.transform.gameObject, "Cube");
                clonedCube = referenceCube.GetComponent(typeof(CubeObjects)) as CubeObjects;

                if (clonedCube == null || referenceCube == null) return;

                if (rayCastHit.transform.tag == "Wall")
                {
                    if (prefab.transform.tag.Equals("Extinguisher"))
                    {
                        lastPos = new Vector3();
                        rotation = rayCastHit.transform.rotation;

                        if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.5f, 0.35f) && (clonedCube.putItem("Extinguisher", new Vector3(0f, 1.5f, 0.35f))))
                        {
                            lastPos = rayCastHit.transform.parent.transform.position + new Vector3(-3.43f, -1.39f, 2.6f);
                            extinguishers++;
                        }
                        else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0.35f, 1.5f, 0f) && (clonedCube.putItem("Extinguisher", new Vector3(0.35f, 1.5f, 0f))))
                        {
                            lastPos = rayCastHit.transform.parent.transform.position + new Vector3(2.6f, -1.39f, 3.43f);
                            extinguishers++;
                        }
                        else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(0f, 1.5f, -0.35f) && (clonedCube.putItem("Extinguisher", new Vector3(0f, 1.5f, -0.35f))))
                        {
                            lastPos = rayCastHit.transform.parent.transform.position + new Vector3(-3.43f, -1.39f, 3.1f);
                            extinguishers++;
                        }
                        else if ((rayCastHit.transform.position - prefabPosition) == new Vector3(-0.35f, 1.5f, 0f) && (clonedCube.putItem("Extinguisher", new Vector3(-0.35f, 1.5f, 0f))))
                        {
                            lastPos = rayCastHit.transform.parent.transform.position + new Vector3(3.1f, -1.39f, 3.43f);
                            extinguishers++;
                        }
                        else
                        {
                            selectExtinguisherRed.transform.position = selectExtinguisher.transform.position;
                            selectExtinguisherRed.transform.rotation = selectExtinguisher.transform.rotation;
                            selectExtinguisher.SetActive(false);
                            selectExtinguisherRed.SetActive(true);

                            StartCoroutine(disableSelectors());

                            return;
                        }

                        GameObject clone = Instantiate(prefab) as GameObject;
                        clone.transform.position = lastPos;
                        clone.transform.rotation = rotation;
                        clone.transform.parent = rayCastHit.transform.parent;
                        clone.SetActive(true);

                        if (!extinguishersRoom.ContainsKey(clone.transform.position))
                        {
                            extinguishersRoom.Add(clone.transform.position, clone);
                        }
                        StartCoroutine(waitASec());
                    }
                    else return;
                }
                else return; 
            }
        }

        if(redActive)
        {
            select.SetActive(false);
            selectBubble.SetActive(false);
            selectExterior.SetActive(false);
            selectExtinguisher.SetActive(false);
        }

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
                mouseD.Set(-Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"), -Input.GetAxisRaw("Mouse ScrollWheel"));

                focusPosition += (Camera.main.transform.right * mouseD.x + Camera.main.transform.up * mouseD.y) * sens * 0.1f;

                CameraRecolocation();
            
        }

        if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))
        {
                Rotation();

        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            mouseD.Set(0, 0, -Input.GetAxisRaw("Mouse ScrollWheel"));

            amount += mouseD * 50f;

            CameraRecolocation();
        }

        MetersText.GetComponent<TMPro.TextMeshProUGUI>().text = meters.ToString();
        DoorsText.GetComponent<TMPro.TextMeshProUGUI>().text = doors.ToString();
        ExtinguishersText.GetComponent<TMPro.TextMeshProUGUI>().text = extinguishers.ToString();
        WindowsText.GetComponent<TMPro.TextMeshProUGUI>().text = windows.ToString();
    }

    public void scan()
    {
        countScans++;

        if (meters > 0)
        {
            if (meters / 15 < extinguishers)
            {
                extinguishersAdvise.SetActive(false);
                extinguishersSecure = true;
            }
            else
            {
                extinguishersAdvise.SetActive(true);
                extinguishersSecure = false;
            }

            if (meters / 15 < doors)
            {
                doorsAdvise.SetActive(false);
                doorsSecure = true;  
            }
            else
            {
                doorsAdvise.SetActive(true);
                doorsSecure = false;
            }

            if (meters / 15 < windows)
            {
                windowsAdvise.SetActive(false);
                windowsSecure = true;
            }
            else
            {
                windowsAdvise.SetActive(true);
                windowsSecure = false;
            }
        }
        else
        {
            extinguishersAdvise.SetActive(false);
            windowsAdvise.SetActive(false);
            doorsAdvise.SetActive(false);
        }

        if (windowsSecure && doorsSecure && extinguishersSecure)
        {
            timer.GetComponent<timer>().isActive = false;
            roomSecuredText.SetActive(true);
            dbManager.inicializarBD();
        }
        else
        {
            UIBorder.SetActive(true);
        }

        StartCoroutine(disable());
    }

    public void reduceWalls()
    {
        if (wallsRoom.Values == null) return;
        if (extinguishersRoom.Values == null) return;

        Dictionary<Vector3, GameObject>.ValueCollection exteriorValues = wallsRoom.Values;
        Dictionary<Vector3, GameObject>.ValueCollection extinguisherValues = extinguishersRoom.Values;

        if (!reduced)
        {
            foreach (GameObject exterior in exteriorValues)
            {
                if (exterior != null)
                {
                    if(exterior.transform.tag == "Window")
                    {
                        exterior.transform.localScale = new Vector3(exterior.transform.localScale.x, 0.5f, exterior.transform.localScale.z);
                        exterior.transform.position = new Vector3(exterior.transform.position.x, 0.75f, exterior.transform.position.z);
                    }
                    else if (exterior.transform.tag == "Wall")
                    {
                        exterior.transform.localScale = new Vector3(exterior.transform.localScale.x, 0.5f, exterior.transform.localScale.z);
                        exterior.transform.position = new Vector3(exterior.transform.position.x, 1f, exterior.transform.position.z);
                    }
                    else if (exterior.transform.tag == "Door")
                    {
                        exterior.transform.localScale = new Vector3(exterior.transform.localScale.x, 0.48f, exterior.transform.localScale.z);
                        exterior.transform.position = new Vector3(exterior.transform.position.x, 0.995f, exterior.transform.position.z);
                    }
                }
            }
            
            if(extinguishers > 0)
            {
                foreach (GameObject token in extinguisherValues)
                {
                    if (token != null)
                    {
                        token.SetActive(false);
                    } 
                }
            }

            StartCoroutine(disableSelectors());

            reduced = true;
        } 
        else
        {
            foreach (GameObject exterior in exteriorValues)
            {
                if (exterior != null) 
                {
                    if (exterior.transform.tag == "Window")
                    {
                        exterior.transform.localScale = new Vector3(exterior.transform.localScale.x, 1f, exterior.transform.localScale.z);
                        exterior.transform.position = new Vector3(exterior.transform.position.x, 1f, exterior.transform.position.z);
                    }
                    else if (exterior.transform.tag == "Wall")
                    {
                        exterior.transform.localScale = new Vector3(exterior.transform.localScale.x, 1f, exterior.transform.localScale.z);
                        exterior.transform.position = new Vector3(exterior.transform.position.x, 1.5f, exterior.transform.position.z);
                    }
                    else if (exterior.transform.tag == "Door")
                    {
                        exterior.transform.localScale = new Vector3(exterior.transform.localScale.x, 0.98f, exterior.transform.localScale.z);
                        exterior.transform.position = new Vector3(exterior.transform.position.x, 1.493f, exterior.transform.position.z);
                    }
                }
            }

            if(extinguishers > 0)
            {
                foreach (GameObject token in extinguisherValues)
                {
                    if (token != null)
                    {
                        token.SetActive(true);
                    }
                }
            }

            StartCoroutine(disableSelectors());

            reduced = false;
        }
    }

    void CameraRecolocation()
    {
        camPosition = camRotation * Vector3.forward;
        camPosition *= amount.z * 0.1f;
        camPosition += focusPosition;

        Camera.main.transform.position = camPosition;
        Camera.main.transform.LookAt(focusPosition);
    }

    void Rotation()
    {
        mouseD.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), -Input.GetAxisRaw("Mouse ScrollWheel"));

        amount += mouseD * sens;
        amount.z = Mathf.Clamp(amount.z, nearZoom, farZoom);
        amount.y = Mathf.Clamp(amount.y, -89, 89);

        camRotation = Quaternion.AngleAxis(amount.x, Vector3.up) * Quaternion.AngleAxis(amount.y, Vector3.right);

        CameraRecolocation();
    }

    public void ChangePrefab(string newPrefab)
    {
        if (prefab != null)
        {
            if (newPrefab == prefab.transform.tag)
            {
                prefab = null;
                selectedItemText.GetComponent<TMPro.TextMeshProUGUI>().text = "None";
                return;
            }

            if (newPrefab == "CubeRedWood" && prefab.transform.tag != "Cube")
            {
                prefab = prefabCube;
            }
            else if (newPrefab == "CubeBlackPattern" && prefab.transform.tag != "Cube")
            {
                prefab = prefabCubeBlackPattern;
            }
            else if (newPrefab == "CubeBlackWood" && prefab.name != "Black Wood")
            {
                prefab = prefabCubeBlackWood;
            }
            else if (newPrefab == "CubeGreyWood" && prefab.name != "Grey Wood")
            {
                prefab = prefabCubeGreyWood;
            }
            else if (newPrefab == "CubeOrangeWood" && prefab.name != "Orange Wood")
            {
                prefab = prefabCubeOrangeWood;
            }
            else if (newPrefab == "CubeRedPattern" && prefab.name != "Red Pattern")
            {
                prefab = prefabCubeRedPattern;
            }
            else if (newPrefab == "CubeWhitePattern" && prefab.name != "White Pattern")
            {
                prefab = prefabCubeWhitePattern;
            }
            else if (newPrefab == "CubeWhiteWood" && prefab.name != "White Wood")
            {
                prefab = prefabCubeWhiteWood;
            }
            else if (newPrefab == "Wall" && prefab.transform.tag != "Wall")
            {
                prefab = prefabWall;
            }
            else if (newPrefab == "Door" && prefab.transform.tag != "Door")
            {
                prefab = prefabDoor;
            }
            else if (newPrefab == "Extinguisher" && prefab.transform.tag != "Extinguisher")
            {
                prefab = prefabExtinguisher;
            }
            else if (newPrefab == "Table" && prefab.transform.tag != "Table")
            {
                prefab = prefabTable;
            }
            else if (newPrefab == "Window" && prefab.transform.tag != "Window")
            {
                prefab = prefabWindow;
            } 
        }
        else
        {
            if (newPrefab == "CubeRedWood")
            {
                prefab = prefabCube;
            }
            else if (newPrefab == "CubeBlackPattern")
            {
                prefab = prefabCubeBlackPattern;
            }
            else if (newPrefab == "CubeBlackWood")
            {
                prefab = prefabCubeBlackWood;
            }
            else if (newPrefab == "CubeGreyWood")
            {
                prefab = prefabCubeGreyWood;
            }
            else if (newPrefab == "CubeOrangeWood")
            {
                prefab = prefabCubeOrangeWood;
            }
            else if (newPrefab == "CubeRedPattern")
            {
                prefab = prefabCubeRedPattern;
            }
            else if (newPrefab == "CubeWhitePattern")
            {
                prefab = prefabCubeWhitePattern;
            }
            else if (newPrefab == "CubeWhiteWood")
            {
                prefab = prefabCubeWhiteWood;
            }
            else if (newPrefab == "Wall")
            {
                prefab = prefabWall;
            }
            else if (newPrefab == "Door")
            {
                prefab = prefabDoor;
            }
            else if (newPrefab == "Extinguisher" )
            {
                prefab = prefabExtinguisher;
            }
            else if (newPrefab == "Table")
            {
                prefab = prefabTable;
            }
            else if (newPrefab == "Window")
            {
                prefab = prefabWindow;
            }
        }

        selectedItemText.GetComponent<TMPro.TextMeshProUGUI>().text = newPrefab;
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null;
    }

    IEnumerator disable()
    {
        yield return new WaitForSeconds(1.5f);

        doorsAdvise.SetActive(false);
        extinguishersAdvise.SetActive(false);
        windowsAdvise.SetActive(false);
        roomSecuredText.SetActive(false);
        UIBorder.SetActive(false);
    }

    IEnumerator waitASec()
    {
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator disableSelectors()
    {
        redActive = true;
        select.SetActive(false);
        selectBubble.SetActive(false);
        selectExterior.SetActive(false);
        selectExtinguisher.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        selectRed.SetActive(false);
        selectBubbleRed.SetActive(false);
        selectExteriorRed.SetActive(false);
        selectExtinguisherRed.SetActive(false);
        redActive = false;
    }
}
