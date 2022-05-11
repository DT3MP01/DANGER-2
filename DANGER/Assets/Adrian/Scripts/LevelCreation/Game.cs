using System;
using UnityEngine;

[Serializable]
public class Game
{
    public string time;
    public int scannedTimes;
    public string playerName;
    public int meters;

   public Game(string time, int scannedTimes, string playerName, int meters)
    {
        this.time = time;
        this.scannedTimes = scannedTimes;
        this.playerName = playerName;
        this.meters = meters;
    }

    public Game()
    {

    }
}
