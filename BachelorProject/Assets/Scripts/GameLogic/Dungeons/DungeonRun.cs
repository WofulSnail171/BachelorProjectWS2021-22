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
    public int maxSteps;
    public List<RandomNum> randomNums;
}

[System.Serializable]
public class CalculatedDungeonRun
{
    public CalculatedDungeonRun(DungeonRun _dungeonRun)
    {
        dungeonRun = _dungeonRun;
        currentStep = 0;
        currentActivity = DungeonActivity.startQuest;
        startActivitySteps = 0;
        remainingActivitySteps = 0;
        rewardHealthBar = dungeonRun.initialRewardTier * 10;
        currentNode = dungeonRun.dungeon.dungeonLayout.startNode;

        pStatGrowth = 0;
        mStatGrowth = 0;
        sStatGrowth = 0;

        nextHero = 0;
        randomNums = new Dictionary<int, int>();
        foreach (RandomNum randNum in _dungeonRun.randomNums)
        {
            randomNums.Add(randNum.step, randNum.num);
        }
    }

    public int RandomNum(int min, int max)
    {
        if (randomNums.ContainsKey(currentStep))
        {
            return randomNums[currentStep];
        }
        else
        {
            // In theory this should only be the case when setting up with calcMaxStep
            int randomNum = UnityEngine.Random.Range(min, max);
            randomNums.Add(currentStep, randomNum);
            DatabaseManager._instance.dungeonData.currentRun.randomNums.Add(new RandomNum { num = randomNum, step = currentStep });
            return randomNum;
        }
    }

    public bool Finished()
    {
        return currentStep >= dungeonRun.maxSteps;
    }

    Dictionary<int, int> randomNums;

    DungeonRun dungeonRun;
    public int currentStep;
    public DungeonActivity currentActivity;
    public int startActivitySteps;
    public int remainingActivitySteps;
    public int rewardHealthBar; //initial rewardbuff *10 -> gets later devided by 10 to get reward tier again
    public DungeonNode currentNode;
    public int pStatGrowth;
    public int mStatGrowth;
    public int sStatGrowth;

    public int nextHero;

    List<LogEntry> dungeonLog = new List<LogEntry>();
    public LogEntry[] dungeonLogArr;
    public void UpdateLog(string _newEntry)
    {
        if (dungeonLog == null)
            dungeonLog = new List<LogEntry>();
        dungeonLog.Add(new LogEntry {step = currentStep, entry = _newEntry });
        dungeonLogArr = dungeonLog.ToArray();
        if(DeleventSystem.DungeonLog != null)
            DeleventSystem.DungeonLog();
    }
    public void SetActivitySteps(int _newSteps)
    {
        startActivitySteps = _newSteps;
        remainingActivitySteps = _newSteps;
    }
}

public enum DungeonActivity
{
    eventStart,
    eventEnd,
    eventHandling,
    pathHandling,
    pathChoosing,
    startQuest,
    endQuest
}

[System.Serializable]
public struct LogEntry
{
    public int step;
    public string entry;
}

[System.Serializable]
public struct RandomNum
{
    public int step;
    public int num;
}
