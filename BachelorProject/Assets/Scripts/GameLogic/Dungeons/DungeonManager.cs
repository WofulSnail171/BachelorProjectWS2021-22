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
            layoutList.CreateDictionary();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    LayoutList layoutList;
    public CalculatedDungeonRun currentCalcRun;

    
    // Start is called before the first frame update
    void OnEnable()
    {
        DeleventSystem.eventDataDownloaded += CreateDailyDungeons;
    }
    private void OnDisable()
    {
        DeleventSystem.eventDataDownloaded -= CreateDailyDungeons;
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
        if (DatabaseManager._instance.dungeonData.dailyDungeons != null && DatabaseManager._instance.dungeonData.dailyDungeons.Length > 0)
        {
            //check if current daily dungeon data is older than 24 hours
            if (DateTime.Parse(DatabaseManager._instance.dungeonData.dailyDungeons[0].date).Date == DateTime.Now.Date)
            {
                Debug.Log("Current daily dungeons are still valid");
                return;
            }
        }
        List<DailyDungeon> tempList = new List<DailyDungeon>();
        int numBasicDungeons = 3;
        for (int i = 0; i < numBasicDungeons; i++)
        {
            DailyDungeon tempDungeon = new DailyDungeon
            {
                dailySeed = UnityEngine.Random.Range(1, 3000),
                layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
                date = DateTime.Now.ToString(),
                questName = DatabaseManager._instance.eventData.basicQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.basicQuestDeck.Length)].eventName,
                type = DungeonType.basic
            };
            tempList.Add(tempDungeon);
        }
        DailyDungeon tempDoomDungeon = new DailyDungeon
        {
            dailySeed = UnityEngine.Random.Range(1, 3000),
            layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
            date = DateTime.Now.ToString(),
            questName = DatabaseManager._instance.eventData.doomQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.doomQuestDeck.Length)].eventName,
            type = DungeonType.doom
        };
        tempList.Add(tempDoomDungeon);
        DatabaseManager._instance.dungeonData.dailyDungeons = tempList.ToArray();
        DatabaseManager._instance.SaveGameDataLocally();
        //probably try to save data online
    }
}
