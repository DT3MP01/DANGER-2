using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour {

    public Text PagesInfoText;
    public LoadPart PrefabToLoad;

    public List<GameObject> PointsToLoadPrefabs;
    
    private int actualPage, totalPages;
    private List<LoadPart> allLoadParts = new List<LoadPart>();
    private List<string> allSceneFiles;


    void OnEnable()
    {
        RefreshMemory();
        UpdatePage();
    }

    public void RefreshMemory(){
        allSceneFiles = new List<string>(Directory.GetFiles(Application.persistentDataPath, "*.dataRoom"));

        totalPages = (int)Mathf.Ceil((float)this.allSceneFiles.Count / PointsToLoadPrefabs.Count);

        foreach (GameObject point in PointsToLoadPrefabs) {
            GameObject go = Instantiate<GameObject>(PrefabToLoad.gameObject, point.transform.position, Quaternion.identity);
            go.transform.SetParent(point.transform);
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
