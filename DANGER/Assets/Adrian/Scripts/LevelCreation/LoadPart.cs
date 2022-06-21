
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;

public class LoadPart : MonoBehaviour {

    public Text Title, Line1, Line2;
    public Image Screenshot;

    SaveRoom roomFile;
    string roomFilePath;

    public void SetInfo(string roomFilePath) {
        this.roomFilePath = roomFilePath;

        // ResetTexts();

        Title.text= Path.GetFileName(roomFilePath).Split('.')[0];
        Debug.Log(Title.text);
        System.DateTime date=File.GetLastWriteTime(roomFilePath);
        Line1.text = "Last Play: " +date.Day+"/"+date.Month+"/"+date.Year;
        

        string saveFile=System.IO.File.ReadAllText(roomFilePath);
        try{
        roomFile = JsonUtility.FromJson<SaveRoom>(saveFile);

        Line2.text = "Meters: " + roomFile.statsRoom.meters + " m";
        
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(Convert.FromBase64String(roomFile.image));
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        if (sprite != null) {
            Screenshot.rectTransform.gameObject.SetActive(true);
            Screenshot.sprite = sprite;
        }
        }
        catch(Exception e){
            Debug.Log(e);
        }
        

    
    }

    public void disableDot(){



    }
    private void ResetTexts() {
        Title.text = "";
        Line1.text = "";
        Line2.text = "";
    }

    public void OnClick() {


        ObjectCreation objectCreation= GameObject.FindObjectsOfType<ObjectCreation>()[0];
        objectCreation.LoadFileRoom(roomFile);
        objectCreation.enableInput();
        gameObject.transform.parent.parent.parent.gameObject.SetActive(false);

    }
}
