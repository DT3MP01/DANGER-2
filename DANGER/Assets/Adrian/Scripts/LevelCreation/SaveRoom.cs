using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class SaveRoom {
        public List<string> floorName;
        public List<furnitureObject> furnitureObjects;
        public List<Vector3> floorPositions;
        public List<ScriptValues> floorscriptValues;
        public StatsRoom statsRoom;
        public SaveRoom(GameObject rooms,StatsRoom statsRoom) {
            this.statsRoom = statsRoom;
            floorPositions = new List<Vector3>();
            floorName = new List<string>();
            floorscriptValues = new List<ScriptValues>();
            furnitureObjects = new List<furnitureObject>();
            foreach (Transform cube in rooms.transform) {
                floorPositions.Add(cube.position);
                floorName.Add(cube.name.Replace("(Clone)", ""));
                floorscriptValues.Add(new ScriptValues(cube.GetComponent<CubeObjects>()));
                furnitureObjects.Add(new furnitureObject(cube));
                }
        }
    }

[System.Serializable]
    public class StatsRoom {
        public int meters, extinguishers, windows, doors, countScans;
        public StatsRoom(int meters,int extinguisher, int windows, int doors, int countScans) {
            this.meters = meters;
            this.extinguishers = extinguisher;
            this.windows = windows;
            this.doors = doors;
            this.countScans = countScans;
        }
            
        }
