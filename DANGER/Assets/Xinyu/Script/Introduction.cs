using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Introduction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] paneles;
    public int currentPage;

    void Start()
    {
        currentPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownPage() {
        if (currentPage != paneles.Length - 1)
        {
            paneles[currentPage].SetActive(false);
            paneles[currentPage + 1].SetActive(true);
            currentPage += 1;
        }
        else {
            SceneManager.LoadScene("AdventureScene");
        }
    
    }
    
    public void ToEnd() {
        SceneManager.LoadScene("AdventureScene");
    }
}
