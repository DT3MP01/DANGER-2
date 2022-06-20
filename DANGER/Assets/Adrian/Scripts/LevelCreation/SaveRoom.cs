using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

[System.Serializable]
    public class SaveRoom {
        public List<string> floorName;
        public List<furnitureObject> furnitureObjects;
        public List<Vector3> floorPositions;
        public List<ScriptValues> floorscriptValues;
        public StatsRoom statsRoom;
        public string image;
        public SaveRoom(GameObject room,StatsRoom statsRoom,byte[] image) {
            this.statsRoom = statsRoom;
            this.image =  Convert.ToBase64String(image);
            floorPositions = new List<Vector3>();
            floorName = new List<string>();
            floorscriptValues = new List<ScriptValues>();
            furnitureObjects = new List<furnitureObject>();
            foreach (Transform cube in room.transform) {
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
