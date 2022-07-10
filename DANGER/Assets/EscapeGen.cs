using System.Linq;
using UnityEngine;
using System.Collections.Generic;
public class EscapeGen : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> RoomList;
	public GameObject StartRoom;
    public int roomsToGenerate = 15;

    public List<WorldGenerator.ocuppiedArea> ocuppiedAreas;


    void Start()
	{
        Transform building = new GameObject("Building").transform;
        ocuppiedAreas = new List<WorldGenerator.ocuppiedArea>();
		RoomDetails startModule = Instantiate(StartRoom, transform.position, transform.rotation,building).GetComponent<RoomDetails>();;
        List<Doorway> pendingDoorways = startModule.GetDoorways();
        ocuppiedAreas.Add(startModule.getSizeRoom());
       for (int i = 0; i < roomsToGenerate-1; i++)
        {
            List<Doorway> newDoorways = new List<Doorway>();
            int randomRoom = Random.Range(0, RoomList.Count);
            GameObject newRoomPrefab = RoomList[randomRoom];
            //Instantiate a New Room
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
                    isInPlace= true;
                    pendingDoorways.AddRange(newRoomDoorways);
                    break;
                }
            }
            if (!isInPlace)
            {
                Debug.Log("-1");
                Destroy(newRoom.transform.gameObject);
            }
        }

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
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
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
