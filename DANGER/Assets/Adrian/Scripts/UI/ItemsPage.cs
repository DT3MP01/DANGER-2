using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ItemsPage : MonoBehaviour, IPointerExitHandler,IPointerEnterHandler
{
    public ItemsButton itemButton;
    public ItemsGroup itemGroup;
    public Image background;
    public ObjectCreation objectCreation;

    public UnityEvent onItemSelected;
    public UnityEvent onItemDeselected;


    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
        objectCreation.editorEnabled = true;
        itemGroup.OnPageSelected(itemButton);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        objectCreation.editorEnabled = false;
    }

    void Start()
    {
        background = GetComponent<Image>();
    }

    public void Select()
    {
        if (onItemSelected != null)
        {
            onItemSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onItemSelected != null)
        {
            onItemDeselected.Invoke();
        }
    }
}
