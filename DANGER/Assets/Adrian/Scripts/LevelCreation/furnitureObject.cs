using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class furnitureObject {
    public List<string> prefabTag;
    public List<wallFurniture> wallObjects;
    public List<Vector3> positionFurniture;
    public List<Quaternion> rotationFurniture;

    public furnitureObject(Transform cube) {
        positionFurniture = new List<Vector3>();
        rotationFurniture = new List<Quaternion>();
        prefabTag = new List<string>();
        wallObjects = new List<wallFurniture>();
        foreach (Transform child in cube) {
            positionFurniture.Add(child.position);
            rotationFurniture.Add(child.rotation);
            prefabTag.Add(child.name.Replace("(Clone)", ""));
            wallObjects.Add(new wallFurniture(child));
        }
    }
}
