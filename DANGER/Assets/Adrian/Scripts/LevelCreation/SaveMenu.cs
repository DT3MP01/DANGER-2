using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class SaveMenu : MonoBehaviour {

    public TMP_InputField inputField;

    public GameObject OverrideMenu;
    public CanvasGroup SaveMenuCanvasGroup;
    
    public GameObject EmptyMenu;

    public GameObject ControllerInput;
    private ObjectCreation objectCreation;
    
    private List<string> allSceneFiles;


    void OnEnable()
    {
        objectCreation=ControllerInput.GetComponent<ObjectCreation>();
    }

    public void SaveFile(){
        objectCreation=ControllerInput.GetComponent<ObjectCreation>();
        allSceneFiles = new List<string>(Directory.GetFiles(Application.persistentDataPath, "*.dataRoom"));
        string text = inputField.text+ ".dataRoom";
        if(text==".dataRoom"){
                Debug.Log("Empty file name");
                SaveMenuCanvasGroup.blocksRaycasts = false;
                EmptyMenu.SetActive(true);
                return;
        }
        // Create unity dialog menu
        // https://docs.unity3d.com/ScriptReference/UnityEngine.UI.FileUtil.SaveFilePanel.html
        foreach(string file in allSceneFiles){
            
            if(file.Contains(text)){
                Debug.Log("File already exists");
                SaveMenuCanvasGroup.blocksRaycasts = false;
                OverrideMenu.SetActive(true);
                return;
            }
            
        }
        
        Debug.Log("Saving fileUWU");
        objectCreation.SaveToFileRoom(text);
        GameObject.Find("SaveMenu").SetActive(false);
        objectCreation.enableInput();

    }

    public void OverrideFile(){
        string text = inputField.text+ ".dataRoom";
        objectCreation.SaveToFileRoom(text);
        GameObject.Find("SaveMenu").SetActive(false);
        objectCreation.enableInput();
    }

    public void EnableInput(){
        this.GetComponentInParent<CanvasGroup>().blocksRaycasts = true;
    }
}
