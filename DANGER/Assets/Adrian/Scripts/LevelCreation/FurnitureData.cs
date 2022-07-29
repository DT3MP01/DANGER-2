using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FurnitureData {
    public List<string> prefabName;
    public List<WallData> wallObjects;
    public List<Vector3> positionFurniture;
    public List<Quaternion> rotationFurniture;

    public FurnitureData(Transform cube) {
        positionFurniture = new List<Vector3>();
        rotationFurniture = new List<Quaternion>();
        prefabName = new List<string>();
        wallObjects = new List<WallData>();
        foreach (Transform child in cube) {
            positionFurniture.Add(child.position);
            rotationFurniture.Add(child.rotation);
            prefabName.Add(child.name.Replace("(Clone)", ""));
            wallObjects.Add(new WallData(child));
        }
    }
}
