using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterStat : MonoBehaviour
{
    public GameObject TemperatureDetector;

    public double MaxHp = 100;
    public double CurrentHp = 100;
    private double HpLostPerFrame = 0.001;

    public double MaxStress = 100;
    public double CurrentStress = 100;
    private double StressGainPerFrame = 0.003;

    private List<Items.ItemType> ItemList = new List<Items.ItemType>();

    SceneOneControl sceneOne;

    void Start()
    {
        sceneOne = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Menu Principal");
        }
        //Check Game Over
        if (CurrentHp <= 0) {
            SceneManager.LoadScene("GameOver");
        }

        CurrentStress -= StressGainPerFrame;
        CurrentHp -= HpLostPerFrame;

        if (CurrentStress >= MaxStress)
        {
            CurrentHp -= 5;
            CurrentStress = 0;
        }
    }


    public double GetMaxHp() 
    {
        return MaxHp;
    }


    public double GetCurrentHp() 
    {
        return CurrentHp;
    }

    public double GetMaxStress()
    {
        return MaxStress;
    }

    public double GetCurrentStress()
    {
        return CurrentStress;
    }

    public List<Items.ItemType> GetItemList()
    {
        return ItemList;
    }


    public void AddHp(double i) 
    {
        CurrentHp += i;
    }

    public void LossHp(double i)
    {
        CurrentHp -= i;
    }

    public void AddStress(double i)
    {
        CurrentStress += i;
    }

    public void LossStress(double i) 
    {
        CurrentStress -= i;
    }

    public void addItem(Items.ItemType it)
    {
        ItemList.Add(it);
        if (it == Items.ItemType.WET_TOWEL)
        {
            HpLostPerFrame -= 0.005;
            StressGainPerFrame -= 0.005;
        }
    }

    public void LostItem(Items.ItemType it)
    {
        if(ItemList.Contains(it))
            ItemList.Remove(it);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si hemos entrado a zona peligro
        if (other.tag == "Fire")
        {
            TemperatureDetector.GetComponent<MeshRenderer>().enabled = true;
            TemperatureDetector.GetComponent<TemperatureDetector>().ChangeMaterialRed();
            HpLostPerFrame += 0.005;
            StressGainPerFrame += 0.003;

        }
        else if (other.tag == "DangerFire")
        {
            TemperatureDetector.GetComponent<MeshRenderer>().enabled = true;
            TemperatureDetector.GetComponent<TemperatureDetector>().ChangeMaterialOrange();
            HpLostPerFrame += 0.002;
            StressGainPerFrame += 0.002;

            sceneOne.avoidFire = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fire")
        {

            TemperatureDetector.GetComponent<TemperatureDetector>().ChangeMaterialOrange();
            HpLostPerFrame -= 0.005;
            StressGainPerFrame -= 0.003;

        }
        else if (other.tag == "DangerFire")
        {

            TemperatureDetector.GetComponent<MeshRenderer>().enabled = false;
            HpLostPerFrame -= 0.002;
            StressGainPerFrame -= 0.002;

        }
    }
}
