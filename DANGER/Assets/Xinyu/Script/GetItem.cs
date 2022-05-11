using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    // Start is called before the first frame update
    private bool actived = false;
    private AudioSource AS;
    private TeammateManager TeammateManager;

    private Canvas dialog;

    public Items.ItemType itemToGet;

    public GameObject[] itemDissapearFromScene;

    GameObject player;

    SceneOneControl sceneOne;

    void Start()
    {
        sceneOne = GameObject.FindGameObjectWithTag("SceneOneControl").GetComponent<SceneOneControl>();

        TeammateManager = GameObject.FindGameObjectWithTag("TeamUI").GetComponentInChildren<TeammateManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        AS = GetComponent<AudioSource>();

        dialog = GameObject.FindGameObjectWithTag("DialogUI").GetComponent<Canvas>();
    }

    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (actived == false)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (itemToGet == Items.ItemType.ALARM)
                    {
                        actived = true;
                        AS.Play();
                        player.GetComponent<CharacterStat>().addItem(Items.ItemType.ALARM);
                        TeammateManager.Activate(Items.ItemType.ALARM);

                        dialog.GetComponent<DialogController>().text.text = "!Has activar la alarma¡ Dentro de poco llegará los bomberos. [Tiempo + 60s] ";
                        dialog.enabled = true;

                        TeammateManager.GetComponent<TimeController>().setCurrentTime(60);

                        sceneOne.fireAlarmActived = true;
                    }

                    else if (itemToGet == Items.ItemType.TOWEL)
                    {
                        actived = true;
                        player.GetComponent<CharacterStat>().addItem(Items.ItemType.TOWEL);

                        dialog.GetComponent<DialogController>().text.text = "!Has encontrado una toalla! Puedes protejerte mejor si está mojada.";
                        itemDissapearFromScene[0].SetActive(false);
                        dialog.enabled = true;
                    }

                    else if (itemToGet == Items.ItemType.WET_TOWEL)
                    {
                        //Si no tiene toalla
                        if (!player.GetComponent<CharacterStat>().GetItemList().Contains(Items.ItemType.TOWEL))
                        {
                            dialog.GetComponent<DialogController>().text.text = "Un dispensador de agua.";
                            dialog.enabled = true;
                        }
                        else
                        {
                            actived = true;
                            player.GetComponent<CharacterStat>().addItem(Items.ItemType.WET_TOWEL);
                            TeammateManager.Activate(Items.ItemType.WET_TOWEL);
                            dialog.GetComponent<DialogController>().text.text = "!Has mojado la toalla y has tapado la nariz y la boca! Ahora tus puntos de la vida de baja más lento.";
                            dialog.enabled = true;

                            sceneOne.wetTowelGot = true;
                        }
                    }


                }
            }
            else
            {
                dialog.GetComponent<DialogController>().text.text = "Parece que ya no puedes hacer nada en aquí.";
                dialog.enabled = true;
            }
        }
    }
}
