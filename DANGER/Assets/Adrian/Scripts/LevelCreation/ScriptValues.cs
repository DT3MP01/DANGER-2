using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
    public class ScriptValues  {
    public List<bool> exteriors;
    public List<bool> fires;
    public List<bool> extinguishers;
        public bool tables, aux, door, extinguisher;
        public int exteriorsCount, firesCount, extinguishersCount;
        public ScriptValues(CubeObjects cube) {
            exteriors = new List<bool>();
            extinguishers = new List<bool>();
            fires =new List<bool>();
            foreach(Vector3 key  in cube.exteriors.Keys) {
                exteriors.Add(cube.exteriors[key]);
                extinguishers.Add(cube.extinguishers[key]);
                fires.Add(cube.fires[key]);
            }
            tables = cube.tables;
            aux = cube.aux;
            door = cube.door;
            extinguisher = cube.extinguisher;

            exteriorsCount = cube.exteriorsCount;
            firesCount = cube.firesCount;
            extinguishersCount = cube.extinguishersCount;
        }
    }
