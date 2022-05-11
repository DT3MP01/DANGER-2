using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Items
{
    // Start is called before the first frame update

    public enum ItemType {
        ALARM,
        TOWEL,
        WET_TOWEL
    } ;

    public Array getAllItem() {
        return Enum.GetValues(typeof(ItemType));
    }
}
