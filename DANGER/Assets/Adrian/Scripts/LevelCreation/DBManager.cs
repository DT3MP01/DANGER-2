using Firebase;
using Firebase.Database;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class DBManager : MonoBehaviour
{
    DatabaseReference reference;

    string idFile;
    int id;
    string text;

    public ObjectCreation game;
    public timer timer;


    // Start is called before the first frame update
    void Start()
    {

        idFile = Application.dataPath + "/ID.txt";
        text = File.ReadAllText(idFile);
        
        // Get the root reference location of the database.
        //reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://danger-b0add-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = database.RootReference;
    }

    public void inicializarBD()
    {

        Game game = new Game(this.timer.timerString, this.game.countScans, "Test User", this.game.meters);
        string json = JsonUtility.ToJson(game);
        Debug.Log(text);
        reference.Child("Games").Child(text).SetRawJsonValueAsync(json);

        this.id = int.Parse(text);
        this.id++;

        File.WriteAllText(idFile, id.ToString());
    }
}
