using Firebase;
using Firebase.Database;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class DBManager : MonoBehaviour
{
    DatabaseReference reference;

    public string idFile;
    public int id;
    public string text;

    public ObjectCreation game;
    public timer timer;


    // Start is called before the first frame update
    void Start()
    {

        idFile = Application.dataPath + "/ID.txt";
        text = File.ReadAllText(idFile);
        
        // Get the root reference location of the database.
        //reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://dangergame-d95dc-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = database.RootReference;
    }

    public void inicializarBD()
    {

        Debug.Log(text);
        Debug.Log(this.game.json);
        reference.Child("Games").Child(text).SetRawJsonValueAsync(game.json);

        this.id = int.Parse(text);
        this.id++;

        File.WriteAllText(idFile, id.ToString());
    }
}
