using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using static WorldGenerator;

public class EscapeGen : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> RoomList;
	public GameObject StartRoom;
    public NavMeshSurface navMesh;
    public GameObject npc;
    public GameObject exit;
    public int roomsToGenerate;
    [Range(0.0f, 1.0f)]
    public float variance = 0.8f;
    public string seedName;
    [Range(0.0f, 1.0f)]
    public float minRoonPercentage = 0.6f;
    public List<WorldGenerator.ocuppiedArea> ocuppiedAreas;
    private List<GameObject> smallRoomList;
    private List<GameObject> defaultRoomList;

    void Start()
    {
    }

    public void StartGenerator(string seed, int numRoom, int numNPC)
	{
        roomsToGenerate = numRoom;
        int roomCount;
        Random.InitState(seed.GetHashCode());
        smallRoomList = new List<GameObject>();
        defaultRoomList = new List<GameObject>();
        List<Doorway> usedDoors;
        List<Doorway> pendingDoorways;
        Transform building= null;

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
        do { 
        
        if (building != null)
        {
            Destroy(building.gameObject);
        }
        roomCount = 1;
        building = new GameObject("Building").transform;
        ocuppiedAreas = new List<WorldGenerator.ocuppiedArea>();
        usedDoors = new List<Doorway>();
        RoomDetails startModule = Instantiate(StartRoom, transform.position, transform.rotation, building).GetComponent<RoomDetails>(); ;
        pendingDoorways = startModule.GetDoorways();
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
                foreach (Doorway doorway in pendingDoorways)
                {
                    MatchExits(doorway, exitToMatch);

                    if (IsFree(newRoom.getSizeRoom()))
                    {
                        pendingDoorways.Remove(doorway);
                        doorway.gameObject.SetActive(false);
                        usedDoors.Add(doorway);
                        isInPlace = true;
                        roomCount++;
                        pendingDoorways.AddRange(newRoomDoorways.Where(e => e != exitToMatch));
                        break;
                    }
                }
                if (!isInPlace)
                {
                    Destroy(newRoom.transform.gameObject);
                }
            }
            //Debug.Log(roomCount);
        }while (roomsToGenerate*minRoonPercentage > roomCount);

        Doorway minDoorX = pendingDoorways[0];
        Doorway minDoorZ = pendingDoorways[0];
        Doorway maxDoorX = pendingDoorways[0];
        Doorway maxDoorZ = pendingDoorways[0];
        foreach(Doorway doorway in pendingDoorways)
        {
            if(doorway.transform.position.x > minDoorX.transform.position.x)
            {
                minDoorX = doorway;
            }
            else if(doorway.transform.position.x < maxDoorX.transform.position.x)
            {
                maxDoorX = doorway;
            }
            else if(doorway.transform.position.z > minDoorZ.transform.position.z)
            {
                minDoorZ = doorway;
            }
            else if(doorway.transform.position.z < maxDoorZ.transform.position.z)
            {
                maxDoorZ = doorway;
            }

        }
        List<Doorway> exitDoors = new List<Doorway>();
        exitDoors.Add(minDoorX);
        exitDoors.Add(minDoorZ);
        exitDoors.Add(maxDoorX);
        exitDoors.Add(maxDoorZ);

        
        Instantiate(exit, new Vector3(minDoorX.transform.position.x,0, minDoorX.transform.position.z), Quaternion.identity, building);
        Instantiate(exit, new Vector3(maxDoorX.transform.position.x,0, maxDoorX.transform.position.z), Quaternion.identity, building);
        Instantiate(exit, new Vector3(minDoorZ.transform.position.x, 0, minDoorZ.transform.position.z), Quaternion.identity, building);
        Instantiate(exit, new Vector3(maxDoorZ.transform.position.x, 0, maxDoorZ.transform.position.z), Quaternion.identity, building);

        foreach (Doorway doorway in pendingDoorways)
        {
            if(exitDoors.Contains(doorway)){
                doorway.transform.gameObject.SetActive(false);
            }
            else
            {
                doorway.ReplaceDoorWithWall();
            }
            
        }


        navMesh.BuildNavMesh();
        //NPC;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(new Vector3(0,0,0), out hit, 1.0f, NavMesh.AllAreas))
        {
            Instantiate(npc, new Vector3(0, 0, 0), Quaternion.identity);  
        }

        for(int i = 1; i < numNPC; i++)
        {
            WorldGenerator.ocuppiedArea spawnArea = ocuppiedAreas[Random.Range(1, ocuppiedAreas.Count)];
            float posX= Random.Range(spawnArea.minX, spawnArea.maxX);
            float posZ = Random.Range(spawnArea.minZ, spawnArea.maxZ);

            if (NavMesh.SamplePosition(new Vector3(posX, 0, posZ), out hit, 1.0f, NavMesh.AllAreas))
            {
                Instantiate(npc, new Vector3(posX, 0, posZ), Quaternion.identity);
            }
        }



        GlobalVar.totalNPCs = numNPC;
        GlobalVar.remainingNPCs = numNPC;
        GlobalVar.ocuppiedAreas = ocuppiedAreas;
        GlobalVar.doors = usedDoors;


    }
    void OnDestroy()
    {
        GlobalVar.start = false;
        GlobalVar.ocuppiedAreas = null;
        GlobalVar.doors = null;
        GlobalVar.rooms = null;
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
		var newRoom = newExit.transform.parent.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newRoom.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
        newRoom.transform.position = new Vector3(0, 0, 0);
        var correctiveTranslation = oldExit.GetComponent<Renderer>().bounds.center - newExit.GetComponent<Renderer>().bounds.center;
        newRoom.transform.position += correctiveTranslation;
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
