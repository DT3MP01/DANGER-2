using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransform
{
   public Quaternion rotation;
   public string prefabName;
   public string position;
   public Transform roomParent;

    public ObjectTransform(Quaternion rotation, string prefabName,string position,Transform roomParent)
    {
        this.rotation = rotation;
        this.prefabName = prefabName;
        this.position = position;
        this.roomParent = roomParent;
    }


}
