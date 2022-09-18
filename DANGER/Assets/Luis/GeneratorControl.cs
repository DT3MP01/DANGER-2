using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorControl : MonoBehaviour
{
    public EscapeGen escapeGenerator;
    public WorldGenerator worldGenerator;
    public Toggle Legacy;
    public TMPro.TMP_InputField seed;
    public Slider Rooms;
    public Slider Npc;
    // Start is called before the first frame update
    void Start()
    {
        List<string> words = new List<string> { "Llamas", "Incendio", "Fuego","Humo","Extintor","Alarma" };
        int randomNumber = Random.Range(0, words.Count);
        string seedName = words[randomNumber].ToString();
        seed.text = seedName;
    }

    public void GenerateRoom()
    {
        if (Legacy.isOn)
        {
            worldGenerator.StartGenerator(seed.text, (int)Rooms.value, (int)Npc.value);
        }
        else
        {
            escapeGenerator.StartGenerator(seed.text, (int)Rooms.value, (int)Npc.value);
        }



    }
}
