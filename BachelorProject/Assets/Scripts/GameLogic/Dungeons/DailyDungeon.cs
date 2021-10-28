using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyDungeon
{
    public int dailySeed;
    public string layoutId;
    public string date;
    public DungeonType type;
}

[System.Serializable]
public class DungeonData
{
    public DailyDungeon[] dailyDungeons;
    public DungeonRun currentRun;
}
