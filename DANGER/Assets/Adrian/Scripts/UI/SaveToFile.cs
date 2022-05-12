using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveToFile : MonoBehaviour
{
    // Start is called before the first frame updat

GameObject room;

private void  SaveToFileRoom()
{
    string path = Application.dataPath + "/Rooms/";
    Debug.Log(path);
    SaveObject hola = new SaveObject();

    string json = JsonUtility.ToJson(hola);
    Debug.Log(json);
    //System.IO.File.WriteAllText(filePath, json);
}

private class SaveObject {
    public int gold;

    public SaveObject() {
        gold = 0;
    }
}
}
