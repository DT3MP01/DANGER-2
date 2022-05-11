using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ItemsButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public ItemsGroup itemGroup;

    public Image background;

    public UnityEvent onItemSelected;
    public UnityEvent onItemDeselected;

    public void OnPointerClick(PointerEventData eventData)
    {
        itemGroup.OnPageSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemGroup.OnPageEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemGroup.OnPageExit(this);
    }

    void Start()
    {
        background = GetComponent<Image>();
        itemGroup.Subscribe(this);
    }

    public void Select()
    {
        if(onItemSelected != null)
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
