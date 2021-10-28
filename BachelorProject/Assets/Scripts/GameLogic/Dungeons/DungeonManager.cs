using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public CalculatedDungeonRun currentCalcRun;

    
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
