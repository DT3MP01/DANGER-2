using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVar
{
    public static int selectedAgent = -1;
    public static int prevAgent = -1;
    public static int agentCounter = 0;
    public static float stressBar = 0;
    public static List<WorldGenerator.generatorPoint> rooms = null; //= new List<generatorPoint>();
    public static List<int> widths;
    public static List<int> lengths;
    public static bool[,] matrix;
    public static bool[,] smokeMatrix;
    public static bool start = false;
    public static int indexX = -1;
    public static int indexZ = -1;
    public static int minAncho = -1;
    public static int maxAlto = -1;
    public static float middleX = -9999;
    public static float middleZ = -9999;
    public static float sizeX;
    public static float sizeZ;
    public static bool mapMovement = true;
    public static int totalNPCs;
    public static int remainingNPCs;
    public enum dir { left, up, right, down, centre };

    
    // Start is called before the first frame update

    /*
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}

