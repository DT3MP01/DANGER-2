using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ItemsPage : MonoBehaviour, IPointerExitHandler
{
    public ItemsButton itemButton;
    public ItemsGroup itemGroup;

    public Image background;

    public UnityEvent onItemSelected;
    public UnityEvent onItemDeselected;

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
        itemGroup.OnPageSelected(itemButton);
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
