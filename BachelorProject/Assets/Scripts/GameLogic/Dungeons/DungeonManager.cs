using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            layoutList = Resources.Load<LayoutList>("LayoutList");
        }
        else
        {
            Destroy(this);
        }
    }

    LayoutList layoutList;
    public CalculatedDungeonRun currentCalcRun;

    
    // Start is called before the first frame update
    void Start()
    {
        CreateDailyDungeons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDungeonRun()
    {
        //utilizes one of the daily dungeons. creates a dungeonRun and a calculated dungeonRun
    }

    public void ProceedDungeonRun()
    {
        //utilizes saved dungeonRun object to recreate dungeonRun
    }

    public void CreateDailyDungeons()
    {
        //create a new set of dailyDungeons
        if(DatabaseManager._instance.dungeonData == null)
        {
            DatabaseManager._instance.dungeonData = new DungeonData();
            DatabaseManager._instance.dungeonData.currentRun = new DungeonRun { valid = false };
        }
        List<DailyDungeon> tempList = new List<DailyDungeon>();
        int numBasicDungeons = 3;
        for (int i = 0; i < numBasicDungeons; i++)
        {
            DailyDungeon tempDungeon = new DailyDungeon
            {
                dailySeed = UnityEngine.Random.Range(1, 3000),
                layoutId = layoutList.layoutNames[UnityEngine.Random.Range(0, layoutList.layoutNames.Length)],
                date = DateTime.Now.ToString(),
                type = DungeonType.basic
            };
            tempList.Add(tempDungeon);
        }
        DailyDungeon tempDoomDungeon = new DailyDungeon
        {
            dailySeed = UnityEngine.Random.Range(1, 3000),
            layoutId = layoutList.layoutNames[UnityEngine.Random.Range(0, layoutList.layoutNames.Length)],
            date = DateTime.Now.ToString(),
            type = DungeonType.doom
        };
        tempList.Add(tempDoomDungeon);
        DatabaseManager._instance.dungeonData.dailyDungeons = tempList.ToArray();
        //probably try to save data online
    }
}
