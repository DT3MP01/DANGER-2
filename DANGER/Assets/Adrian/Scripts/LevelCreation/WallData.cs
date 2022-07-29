using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallData {
    public List<string> prefabName;
    public List<Vector3> positionFurniture;
    public List<Quaternion> rotationFurniture;
    public WallData(Transform cube) {
        positionFurniture = new List<Vector3>();
        rotationFurniture = new List<Quaternion>();
        prefabName = new List<string>();
        foreach (Transform child in cube) {
            if(child.tag != cube.tag){
            positionFurniture.Add(child.position);
            rotationFurniture.Add(child.rotation);
            prefabName.Add(child.name.Replace("(Clone)", ""));
            }
        }
    }
}
