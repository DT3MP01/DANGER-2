using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class EscapeGen : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> RoomList;
	public GameObject StartRoom;
    public NavMeshSurface navMesh;
    public int roomsToGenerate = 15;
    [Range(0.0f, 1.0f)]
    public float variance = 0.8f;
    public string seedName;
    public List<WorldGenerator.ocuppiedArea> ocuppiedAreas;
    private List<string> words;
    private List<GameObject> smallRoomList;
    private List<GameObject> defaultRoomList;

    void Start()
	{

        int roomCount = roomsToGenerate;
        //words = new List<string> { "Llamas", "Incendio", "Fuego","Humo" };
        words = new List<string> { "Incendio"};
        int randomNumber = Random.Range(0, words.Count);
        seedName = words[randomNumber].ToString();

        Random.InitState(words[randomNumber].GetHashCode());


        smallRoomList = new List<GameObject>();
        defaultRoomList = new List<GameObject>();

        foreach (GameObject Room in RoomList)
        {
            if (Room.GetComponent<RoomDetails>().GetDoorways().Count >= 2)
            {
                smallRoomList.Add(Room);

            }
            else
            {
                defaultRoomList.Add(Room);
            }

        }

        
        Transform building = new GameObject("Building").transform;
        ocuppiedAreas = new List<WorldGenerator.ocuppiedArea>();
        RoomDetails startModule = Instantiate(StartRoom, transform.position, transform.rotation, building).GetComponent<RoomDetails>(); ;
        List<Doorway> pendingDoorways = startModule.GetDoorways();
        ocuppiedAreas.Add(startModule.getSizeRoom());
        GameObject newRoomPrefab;

        for (int i = 0; i < roomsToGenerate - 1; i++)
        {
            List<Doorway> newDoorways = new List<Doorway>();
            //Instantiate a New Room
            if (Random.value > variance)
            {
                int randomRoom = Random.Range(0, defaultRoomList.Count);
                newRoomPrefab = defaultRoomList[randomRoom];
            }
            else
            {
                int randomRoom = Random.Range(0, smallRoomList.Count);
                newRoomPrefab = smallRoomList[randomRoom];
            }
            RoomDetails newRoom = Instantiate(newRoomPrefab, transform.position, Quaternion.identity, building).GetComponent<RoomDetails>();

            List<Doorway> newRoomDoorways = newRoom.GetDoorways();
            //Get Random DoorWay
            int randomDoorway = Random.Range(0, newRoomDoorways.Count);
            Doorway exitToMatch = newRoomDoorways[randomDoorway];
            bool isInPlace = false;
            foreach (Doorway doorway in Fisher_YatesShuffle(pendingDoorways))
            {
                MatchExits(doorway, exitToMatch);

                if (IsFree(newRoom.getSizeRoom()))
                {
                    pendingDoorways.Remove(doorway);
                    isInPlace = true;
                    pendingDoorways.AddRange(newRoomDoorways.Where(e => e != exitToMatch));
                    break;
                }
            }
            if (!isInPlace)
            {
                Debug.Log("-1");

                Destroy(newRoom.transform.gameObject);
            }
        }



        foreach (Doorway doorway in pendingDoorways)
        {
            doorway.transform.gameObject.SetActive(false);
        }


        //navMesh.BuildNavMesh();



        //for (int iteration = 0; iteration < 1; iteration++){
        //    List<Doorway> newDoorways = new List<Doorway>();
        //    foreach (Doorway doorway in pendingDoorways){
        //        int randomRoom = Random.Range(0,RoomList.Count);
        //        GameObject newRoomPrefab = RoomList[randomRoom];
        //        newRoomPrefab.GetComponent<RoomDetails>().getSizeRoom();
        //        RoomDetails newRoom = Instantiate(newRoomPrefab, transform.position, Quaternion.identity).GetComponent<RoomDetails>();
        //        List<Doorway> newRoomDoorways = newRoom.GetDoorways();
        //        int randomDoorway = Random.Range(0, newRoomDoorways.Count);
        //        Doorway exitToMatch = newRoomDoorways[randomDoorway];
        //        MatchExits(doorway, exitToMatch);
        //        newDoorways.AddRange(newRoomDoorways.Where(e => e != exitToMatch));
        //    }
        //    pendingDoorways = newDoorways;
        //}
    }

    private bool IsFree(WorldGenerator.ocuppiedArea newArea)
    {
        foreach(WorldGenerator.ocuppiedArea area in ocuppiedAreas)
        {
            if(area.minX < newArea.maxX 
                && area.maxX > newArea.minX 
                && area.maxZ > newArea.minZ 
                && area.minZ < newArea.maxZ)
            {
                return false;
            }
        }
        ocuppiedAreas.Add(newArea);
        return true;
    }
    private void MatchExits(Doorway oldExit, Doorway newExit)
	{
		var newRoom = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newRoom.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
        newRoom.transform.position = new Vector3(0, 0, 0);
        var correctiveTranslation = oldExit.GetComponent<Renderer>().bounds.center - newExit.GetComponent<Renderer>().bounds.center;
        newRoom.transform.position += correctiveTranslation;
    }

    List<Doorway> Fisher_YatesShuffle(List<Doorway> a)
    {
        // Recorremos la lista {1,2,3,4}
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Numero aleatorio entre 0 y i (de forma que i decrementa cada iteraci?n)
            int rnd = Random.Range(0, i);

            // Guardamos el valor que hay en a[i] 
            Doorway temp = a[i];

            // intercambiamos el valor de a[i] con el valor de que hay en la posici?n aleatoria
            a[i] = a[rnd];
            a[rnd] = temp;
        }
        return a;
    }
    private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[Random.Range(0, array.Length)];
	}


	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
}
