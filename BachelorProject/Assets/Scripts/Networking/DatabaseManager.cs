using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager _databaseManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (_databaseManager == null)
        {
            _databaseManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
    }
    
    // data synced with online or fetched regularly
   //player daten
   //   ->stammdaten (pw, username, etc.)
   //   ->inventar current heroes
   //   ->trading spezifische infos
   //       ->blacklist
   //       ->next reward tier base
   //   ->timestamp of last push(!) to decide when to delete local data
   //defaultHero List
   //text events
   //   ->green, red and blue decks
   //   ->zwischentexte
   //trading info list
   //liveticker (for fun)

    //just local data (lost when playing on other device!)
    //daily dungeon maps
    //  ->node types
    //  ->path types
    //  ->dungeon layout selection
    //current dungeon run
    //  ->seeds or whatever to recreate status with time stamps
    //      ->event progression
    //  ->wich heroes are currently running
    //stat growth stuff
    //  ->current reward tier for dungeon
    //  ->statgrowth status
}

[System.Serializable]
public class GameData
{
    public GameData()
    {

    }
    //Online Data:
    //PlayerData
    // -> own object (reuse same as for the on com
    
    //defaultHero List -> same as on com

    //text events -> same as on com

    //trading info list -> same as the one gotten from the on com

    //liveticker -> same as from the on com

    //Offline Data:
    //daily and current dungeon maps (containing layout and path+node types)
    //information for current dungeon run
    //  ->time stamps
    //  ->events
    //  ->results of events and stat growth
    //  ->reward level
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public string password;
    public string date;
    public DateTime signUpDate;
    public string profileDescription;
    public int mtDoomCounter;
    public int tradeCounter;
    public string lastDungeonRun;
    public string currentDungeonRun;

    public BlacklistEntry[] blacklist;
    public HeroData[] inventory;
}

[System.Serializable]
public class BlacklistEntry
{
    public string playerid;
    public string heroid;
}
