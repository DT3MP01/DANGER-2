using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class wallFurniture {
    public List<string> prefabTag;
    public List<Vector3> positionFurniture;
    public List<Quaternion> rotationFurniture;
    public wallFurniture(Transform cube) {
        positionFurniture = new List<Vector3>();
        rotationFurniture = new List<Quaternion>();
        prefabTag = new List<string>();
        foreach (Transform child in cube) {
            if(child.tag != cube.tag){
            positionFurniture.Add(child.position);
            rotationFurniture.Add(child.rotation);
            prefabTag.Add(child.name.Replace("(Clone)", ""));
            }
        }
    }
}
