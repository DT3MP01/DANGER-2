using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsGroup : MonoBehaviour
{
    public List<ItemsButton> itemButtons;
    public List<GameObject> objectsToSwap;

    public Sprite itemIdle;
    public Sprite itemHover;
    public Sprite itemActive;

    public ItemsButton selectedItem;

    public void Subscribe(ItemsButton button)
    {
        if(itemButtons == null)
        {
            itemButtons = new List<ItemsButton>();
        }

        itemButtons.Add(button);
    }

    public void OnPageEnter(ItemsButton button)
    {
        ResetPages();
        if(selectedItem == null || button != selectedItem)
        {
            button.background.sprite = itemHover;
        }
    }

    public void OnPageExit(ItemsButton button)
    {
        ResetPages();
    }

    public void OnPageSelected(ItemsButton button)
    {
        if (selectedItem != null)
        {
            selectedItem.Deselect();
        }

        if(button == selectedItem)
        {
            selectedItem = null;

            ResetPages();

            if(button.tag != "Item") objectsToSwap[button.transform.GetSiblingIndex()].SetActive(false);

            return;
        }

        selectedItem = button;

        selectedItem.Select();
        
        ResetPages();
        
        button.background.sprite = itemActive;
        
        int index = button.transform.GetSiblingIndex();

        
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            } 
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetPages()
    {
        foreach(ItemsButton button in itemButtons)
        {
            if(selectedItem != null && button == selectedItem) { continue; }
            button.background.sprite = itemIdle;
        }
    }
}
