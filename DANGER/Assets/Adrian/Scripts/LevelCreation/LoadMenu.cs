using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class LoadMenu : MonoBehaviour {

    public Text PagesInfoText;
    public LoadPart PrefabToLoad;

    public List<GameObject> PointsToLoadPrefabs;

    public TMP_InputField inputField;

    public GameObject OverrideMenu;
    public CanvasGroup SaveMenuCanvasGroup;
    
    public GameObject EmptyMenu;

    public GameObject ControllerInput;
    private ObjectCreation objectCreation;
    
    private int actualPage, totalPages;
    private List<LoadPart> allLoadParts = new List<LoadPart>();
    private List<string> allSceneFiles;


    void OnEnable()
    {
        objectCreation=ControllerInput.GetComponent<ObjectCreation>();
        RefreshMemory();
        UpdatePage();
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
        
        Debug.Log("Saving file");
        objectCreation.SaveToFileRoom(text);
    }

    public void OverrideFile(){
        string text = inputField.text+ ".dataRoom";
        objectCreation.SaveToFileRoom(text);
    }

    public void EnableInput(){
        this.GetComponentInParent<CanvasGroup>().blocksRaycasts = true;
    }

    IEnumerator Refresh(){
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        RefreshMemory();
        UpdatePage();
    }

    public void RefreshMemory(){
        allSceneFiles = new List<string>(Directory.GetFiles(Application.persistentDataPath, "*.dataRoom"));

        totalPages = (int)Mathf.Ceil((float)this.allSceneFiles.Count / PointsToLoadPrefabs.Count);

        foreach (GameObject point in PointsToLoadPrefabs) {
            GameObject go = Instantiate<GameObject>(PrefabToLoad.gameObject, point.transform.position, Quaternion.identity,point.transform);
            allLoadParts.Add(go.GetComponent<LoadPart>());
        }
    }

    private void UpdatePage() {

        List<string> copyScenesFiles = new List<string>(this.allSceneFiles);

        copyScenesFiles.RemoveRange(0, actualPage * PointsToLoadPrefabs.Count);

        foreach (LoadPart item in allLoadParts) {
            if (copyScenesFiles.Count == 0) {
                item.gameObject.SetActive(false);
                continue;
            }

            item.gameObject.SetActive(true);

            string select = copyScenesFiles[0];
            copyScenesFiles.Remove(select);
            item.SetInfo(select);

        }

        int page = actualPage + 1;
        PagesInfoText.text = "Page " + page + " of " + totalPages;
    }

    public void Next() {
        if ((actualPage + 1) * PointsToLoadPrefabs.Count >= this.allSceneFiles.Count)
            return;

        actualPage++;
        UpdatePage();
    }

    public void Back() {
        if (actualPage <= 0)
            return;

        actualPage--;
        UpdatePage();
    }

    public void OpenLevel(int sceneBuildIndex) {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
