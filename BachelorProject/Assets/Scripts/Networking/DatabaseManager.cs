using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager _instance;
    public static int maxInventorySize;
    public static bool CheckDatabaseValid()
    {
        if (_instance == null 
            || DatabaseManager._instance.dungeonData == null 
            || DatabaseManager._instance.defaultHeroData == null
            || DatabaseManager._instance.eventData == null)
            return false;
        return true;
    }

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
    //some logic to compare local and online data and do something according
    public void SaveGameDataLocally()
    {
        LocalSaveSystem.SaveLocaldata();
        //Push to server?
    }
    private GameData localSave;
    public void LoadLocalSave()
    {
        //just happens on opening the app (i guessssss)
        localSave = LocalSaveSystem.LoadLocalData();
        if(localSave == null)
        {
            //perform no local load event
        }
        else
        {
            if(activePlayerData.lastUpdate != "" && DateTime.Parse(activePlayerData.lastUpdate).CompareTo(DateTime.Parse(localSave.activePlayerData.lastUpdate)) < 0)
            {
                //online save is younger the local one -> played on an other device -> need to fire special event

                //dunno how that can happen lel, but i guess we want to save the current data instead of applying the old local one riiiiiiiiight?!
                SaveGameDataLocally();
            }
            else
            {
                //online save is older -> probably need to update online savefile
                //but we can savely apply the local savefile to the active data
                defaultHeroData = localSave.defaultHeroData;
                eventData = localSave.eventData;
                eventData.CreateDictionaries();
                activePlayerData = localSave.activePlayerData;
                dungeonData = localSave.dungeonData;
            }
        }
    }

    public IncomingHeroData defaultHeroData;
    public void UpdateDefaultHeroListFromServer(string _message)
    {
        defaultHeroData = JsonUtility.FromJson<IncomingHeroData>(_message);
        defaultHeroData.FillDictionary();
        LocalSaveSystem.SaveLocaldata();
        return;
    }

    public PlayerData activePlayerData;
    public void UpdateActivePlayerFromServer(string _message)
    {
        activePlayerData = JsonUtility.FromJson<PlayerData>(_message);
        //check for dates lol
        if(localSave == null || DateTime.Parse(activePlayerData.lastUpdate).CompareTo(DateTime.Parse(localSave.activePlayerData.lastUpdate)) < 0)
        {
            //online save is younger the local one -> played on an other device -> need to fire special event
            Debug.Log("online save is younger than the local one");
        }
        else
        {
            //online save is older -> probably need to update online savefile
            Debug.Log("online save is older than the local one");
        }
        return;
    }

    public EventData eventData;
    public void UpdateEventDataFromServer(string _message)
    {
        eventData = JsonUtility.FromJson<EventData>(_message);
        eventData.CreateDictionaries();
        LocalSaveSystem.SaveLocaldata();
    }

    public DungeonData dungeonData;
    public void UpdateDungeonDataFromServer(string _message)
    {
        dungeonData = JsonUtility.FromJson<DungeonData>(_message);
        LocalSaveSystem.SaveLocaldata();
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
    public GameData(DatabaseManager _manager)
    {
        defaultHeroData = _manager.defaultHeroData;
        activePlayerData = _manager.activePlayerData;
        eventData = _manager.eventData;
        dungeonData = _manager.dungeonData;
    }
    public IncomingHeroData defaultHeroData;
    public PlayerData activePlayerData;
    public EventData eventData;
    public DungeonData dungeonData;



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
    public int rewardTierBuff;

    public BlacklistEntry[] blacklist;
    public List<PlayerHero> inventory;
}

[System.Serializable]
public class BlacklistEntry
{
    public string playerId;
    public string heroId;
}

[System.Serializable]
public class EventData
{
    public void CreateDictionaries()
    {
        basicQuestDict = new Dictionary<string, DungeonEvent>();
        if (basicQuestDeck == null)
            return;
        foreach (var item in basicQuestDeck)
        {
            if (!basicQuestDict.ContainsKey(item.eventName))
            {
                basicQuestDict.Add(item.eventName, item);
            }
        }
        doomQuestDict = new Dictionary<string, DungeonEvent>();
        if (basicQuestDeck == null)
            return;
        foreach (var item in doomQuestDeck)
        {
            if (!doomQuestDict.ContainsKey(item.eventName))
            {
                doomQuestDict.Add(item.eventName, item);
            }
        }
    }

    public int GetNodeTypeIndex(string nodeType)
    {
        for (int i = 0; i < nodeTypes.Length; i++)
        {
            if (nodeType == nodeTypes[i])
                return i;
        }
        return -1;
    }
    public DungeonEvent[] basicQuestDeck;
    public Dictionary<string, DungeonEvent> basicQuestDict;
    public DungeonEvent[] doomQuestDeck;
    public Dictionary<string, DungeonEvent> doomQuestDict;
    public EventDeck[] eventDecks;
    public string[] nodeTypes;
    public string[] pathTypes;
}

[System.Serializable]
public class EventDeck
{
    public string deckName;
    public Event[] deck;
}

[System.Serializable]
public class Event
{
    public string eventName;
    public string statType;
    public string startText;
    public string endText;
}
[System.Serializable]
public class DungeonEvent
{
    public string eventName;
    public string dungeonType;
    public string startText;
    public string endText;
    public string description;
}

public enum DungeonType
{
    basic,
    doom
}