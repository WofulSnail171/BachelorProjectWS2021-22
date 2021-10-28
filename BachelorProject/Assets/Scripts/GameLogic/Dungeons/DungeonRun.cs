using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRun
{
    public bool valid = false; //gets set by DungeonManager when starting or proceeding a run
    public PlayerHero[] party;
    public string date;
    public int dungeonSeed;
    public DailyDungeon dungeon;
    public int initialRewardTier; //dungeon assigned rt + rewardbuff
}

public class CalculatedDungeonRun
{
    CalculatedDungeonRun(DungeonRun _dungeonRun)
    {
        dungeonRun = _dungeonRun;
    }
    DungeonRun dungeonRun;

    public int rewardHealthBar; //initial rewardbuff *10 -> gets later devided by 10 to get reward tier again
    public DungeonNode currentNode;
    public int pStatGrowth;
    public int mStatGrowth;
    public int sStatGrowth;
    public int stepCounter;

    public int nextHero;
}
