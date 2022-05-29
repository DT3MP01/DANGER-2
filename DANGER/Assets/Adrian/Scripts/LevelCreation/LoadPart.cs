
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadPart : MonoBehaviour {

    public Text Title, Line1, Line2, Line3, Line4;
    public Image Screenshot;

    SaveRoom roomFile;
    string roomFilePath;

    public void SetInfo(string roomFilePath) {
        this.roomFilePath = roomFilePath;

        // ResetTexts();

        Title.text= Path.GetFileName(roomFilePath);

        System.DateTime date=File.GetLastWriteTime(roomFilePath);
        Line1.text = "Last Play: " +date.Day+"/"+date.Month+"/"+date.Year;
        Line2.text = "Hours: " + date.Hour + ":" + date.Minute+":"+date.Second;




        // if (roomFile.HasKeyQuickAccess("SceneName")) {
        //     Line2.text = "Scene: " + roomFile.SceneName;
        // } else {
        //     Line2.text = "Version: " + roomFile.FormatVersion;
        // }

        // if (roomFile.HasSaveIsEasyStatistics) {
        //     Line3.text = "Created: " + roomFile.StatisticsCreationDateAsDateTime.ToString("M/d/yyyy");

        //     if ((int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalDays == 0) {
        //         if ((int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalHours == 0) {
        //             if ((int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalMinutes == 0) {
        //                 Line4.text = "Total time: " + (int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalSeconds + " Seconds";
        //             } else {
        //                 Line4.text = "Total time: " + (int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalMinutes + " Minutes";
        //             }
        //         } else {
        //             Line4.text = "Total time: " + (int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalHours + " Hours";
        //         }
        //     } else {
        //         Line4.text = "Total time: " + (int)roomFile.StatisticsTotalTimeInSecondsAsTimeSpan.TotalDays + " Days";
        //     }
        // }


        // Sprite sprite = roomFile.StatisticsScreenshotAsSprite;

        // if (sprite != null) {
        //     Screenshot.rectTransform.gameObject.SetActive(true);
        //     Screenshot.sprite = sprite;
        // } else {
        //     Screenshot.rectTransform.gameObject.SetActive(false);
        // }
    }

    private void ResetTexts() {
        Title.text = "";
        Line1.text = "";
        Line2.text = "";
        Line3.text = "";
        Line4.text = "";
    }

    public void OnClick() {
        string saveFile=System.IO.File.ReadAllText(roomFilePath);
        Debug.Log(saveFile);
        roomFile = JsonUtility.FromJson<SaveRoom>(saveFile);
        ObjectCreation objectCreation= GameObject.FindObjectsOfType<ObjectCreation>()[0];
        objectCreation.LoadFileRoom(roomFile);
        objectCreation.enableInput();
        gameObject.transform.parent.parent.parent.gameObject.SetActive(false);

    }
}
