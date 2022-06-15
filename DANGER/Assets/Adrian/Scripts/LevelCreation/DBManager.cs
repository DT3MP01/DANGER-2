using Firebase;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Net.Http;
using System.Linq;
using System.Text;

public class DBManager : MonoBehaviour
{

    public string idFile;
    public int id;
    public string text;

    public string partida;

    public ObjectCreation game;
    public timer timer;
    private FirebaseFirestore database;
    private FirebaseStorage storage;


    // Start is called before the first frame update
    void Start()
    {

        idFile = Application.dataPath + "/ID.txt";
        text = File.ReadAllText(idFile);
        
        // Get the root reference location of the database.
        //reference = FirebaseDatabase.DefaultInstance.RootReference;

        database = FirebaseFirestore.DefaultInstance;
        storage= FirebaseStorage.DefaultInstance;

    }

    IEnumerator SaveRoom()
    {
        yield return StartCoroutine(game.RecordFrame());

        // DocumentReference docData = database.Collection("GameData").Document();
        // docData.SetAsync(new Dictionary<string, object> {{"Raw",game.json}});

        DocumentReference docRefInfo = database.Collection("GameInfo").Document();

        StorageReference docData = storage.RootReference.Child("GameInfo-Data").Child(docRefInfo.Id+".dataRoom");
        docData.PutBytesAsync(Encoding.Unicode.GetBytes(game.json));

        StorageReference docDataImage = storage.RootReference.Child("GameInfo-Data").Child(docRefInfo.Id+"-Image.png");
        docDataImage.PutBytesAsync(Convert.FromBase64String(game.SaveRoomData.image));

        StatsRoom statsRoom = new StatsRoom(game.meters, game.extinguishers, game.windows, game.doors, game.countScans);
        Dictionary<string, object>data =  new Dictionary<string, object> {
            {"Image",docDataImage.Path},
            {"PlayerName", "PlayerExample"},
            {"RoomName", "ROOM"},
            {"Meters",game.SaveRoomData.statsRoom.meters},
            {"Extinguishers",game.SaveRoomData.statsRoom.extinguishers},
            {"Windows",game.SaveRoomData.statsRoom.windows},
            {"Doors",game.SaveRoomData.statsRoom.doors},
            {"CountScans",game.SaveRoomData.statsRoom.countScans},
            {"Reference",docData.Path}
            };
        docRefInfo.SetAsync(data);

    }


    public void inicializarBD()
    {

        // Debug.Log(text);
        // Debug.Log(this.game.json);
        StartCoroutine(SaveRoom());

        

        // reference.Child("User").Child(text).Child("Alias").SetValueAsync("DUDETE");
        // reference.Child("User").Child(text).Child("Games").Child(partida).SetRawJsonValueAsync(game.json);
        
        // this.id = int.Parse(text);
        // this.id++;

        // File.WriteAllText(idFile, id.ToString());
    }

    
}
