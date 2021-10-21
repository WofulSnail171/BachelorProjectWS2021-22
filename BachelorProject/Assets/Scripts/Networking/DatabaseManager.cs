using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager _instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
    }

    public IncomingHeroData defaultHeroData;
    public void UpdateDefaultHeroListFromServer(string _message)
    {
        defaultHeroData = JsonUtility.FromJson<IncomingHeroData>(_message);
        return;
    }

    public PlayerData activePlayerData;
    public void UpdateActivePlayerFromServer(string _message)
    {
        activePlayerData = JsonUtility.FromJson<PlayerData>(_message);
        return;
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

//Dates: Use DateTime to fetch, cast and compare dates and save them as strings
//date = System.DateTime.Now.ToString(),
//signUpDate = System.DateTime.Parse(System.DateTime.Now.ToString()),

[System.Serializable]
public class PlayerData
{
    public string playerId;
    public string password;
    public string joinDate;
    public string lastUpdate;
    public string profileDescription;
    public int mtdCounter;
    public int tradeCounter;
    public string lastDungeonDate;
    public string currentDungeonRun;

    public BlacklistEntry[] blacklist;
    public PlayerHero[] inventory;
}

[System.Serializable]
public class BlacklistEntry
{
    public string playerId;
    public string heroId;
}
