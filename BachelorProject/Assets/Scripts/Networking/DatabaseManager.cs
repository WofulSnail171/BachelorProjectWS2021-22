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

    public static ProgressState GetDungeonRunState()
    {
        ProgressState result = ProgressState.Empty;
        if(DatabaseManager.CheckDatabaseValid() && DatabaseManager._instance.dungeonData.currentRun != null && DatabaseManager._instance.dungeonData.currentRun.valid == true)
        {
            if(DungeonManager._instance.CheckCalcRun())
            {
                if (DungeonManager._instance.currentCalcRun.Finished())
                {
                    result = ProgressState.Done;
                }
                else
                    result = ProgressState.Pending;
            }
        }
        return result;
    }

    public static ProgressState GetTradeState()
    {
        ProgressState result = ProgressState.Empty;
        return result;
    }

    public static bool DoomDungeonAvailable()
    {
        if(_instance != null && CheckDatabaseValid())
        {
            if(_instance.activePlayerData != null)
            {
                if(_instance.activePlayerData.shards >= 3)
                {
                    return true;
                }
            }
        }
        return false;
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
        DatabaseManager._instance.activePlayerData.lastUpdate = DateTime.Now.ToString("u");
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
            if (activePlayerData.lastUpdate != "" && DateTime.Parse(localSave.activePlayerData.lastUpdate).CompareTo(DateTime.Parse(activePlayerData.lastUpdate)) < 0)
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
                ValidateInventory();
                dungeonData = localSave.dungeonData;
                rewardTable = localSave.rewardTable;
            }
        }
    }

    public static void ValidateInventory()
    {
        foreach (var item in _instance.activePlayerData.inventory)
        {
            ValidatePlayerHero(item);
        }
    }

    public static void ValidatePlayerHero(PlayerHero _hero)
    {        
        while (!CheckUniqueId(_hero, _hero.uniqueId) || _hero.uniqueId < 1000)
        {
            _hero.uniqueId = UnityEngine.Random.Range(1000, 10000);
        }
        if (!CheckUniqueInvIndex(_hero, _hero.invIndex))
        {
            _hero.invIndex = -1;
            while (!CheckUniqueInvIndex(_hero, _hero.invIndex) || _hero.invIndex < 0)
            {
                _hero.invIndex++;
            }
        }            
    }

    private static bool CheckUniqueId(PlayerHero _hero, int _uniqueId)
    {
        foreach (var item in DatabaseManager._instance.activePlayerData.inventory)
        {
            if (item != _hero && item.uniqueId == _uniqueId)
            {
                return false;
            }
        }
        return true;
    }

    private static bool CheckUniqueInvIndex(PlayerHero _hero, int _uniqueInvIndex)
    {
        foreach (var item in DatabaseManager._instance.activePlayerData.inventory)
        {
            if (item != _hero && item.invIndex == _uniqueInvIndex || _uniqueInvIndex < 0)
            {
                return false;
            }
        }
        return true;
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
        var bla = DateTime.Parse(localSave.activePlayerData.lastUpdate).CompareTo(DateTime.Parse(activePlayerData.lastUpdate));
        if (localSave == null || bla > 0)
        {
            //online save is younger the local one -> played on an other device -> need to fire special event
            SaveGameDataLocally();
            ServerCommunicationManager._instance.DoServerRequest(Request.DownloadDungeonData);
            Debug.Log("online save is younger than the local one");
            //ToDo maybe Also Pull DungeonData and investigate it
            //ServerCommunicationManager._instance.DoServerRequest(Request.DownloadDungeonData);
        }
        else
        {
            //online save is older -> probably need to update online savefile
            LoadLocalSave();
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

    public RewardTable rewardTable;
    public void UpdateRewardTableFromServer(string _message)
    {
        rewardTable = JsonUtility.FromJson<RewardTable>(_message);
        LocalSaveSystem.SaveLocaldata();
    }

    public DungeonData dungeonData;
    public void UpdateDungeonDataFromServer(string _message)
    {
        dungeonData = JsonUtility.FromJson<DungeonData>(_message);
        DungeonManager._instance.CreateDailyDungeons();

        if (DatabaseManager._instance.dungeonData.currentRun != null && (DatabaseManager._instance.dungeonData.currentRun.valid))
        {
            //this run should be getting continued
            //Probelm: Party is not there!
            DatabaseManager._instance.dungeonData.currentRun.party = new List<PlayerHero>();
            foreach (var heroUnit in DatabaseManager._instance.activePlayerData.inventory)
            {
                if(heroUnit.status == HeroStatus.Exploring)
                {
                    DatabaseManager._instance.dungeonData.currentRun.party.Add(heroUnit);
                }
            }
            if(DatabaseManager._instance.dungeonData.currentRun.party.Count == 0)
            {
                //Fallback: Get the first idling heroes to continue dungeon Run
                foreach (var heroUnit in DatabaseManager._instance.activePlayerData.inventory)
                {
                    if (heroUnit.status == HeroStatus.Idle)
                    {
                        heroUnit.status = HeroStatus.Exploring;
                        DatabaseManager._instance.dungeonData.currentRun.party.Add(heroUnit);
                    }
                    if (DatabaseManager._instance.dungeonData.currentRun.party.Count == 4)
                        break;
                }
            }
            if (DatabaseManager._instance.dungeonData.currentRun.party.Count == 0)
            {
                DatabaseManager._instance.dungeonData.currentRun = null;
                foreach (var heroUnit in DatabaseManager._instance.activePlayerData.inventory)
                {
                    if (heroUnit.status == HeroStatus.Exploring)
                        heroUnit.status = HeroStatus.Idle;
                }
            }
        }
        LocalSaveSystem.SaveLocaldata();
    }

    public TradeData tradeData;
    public void UpdateTradeDataFromServer(string _message)
    {
        tradeData = JsonUtility.FromJson<TradeData>(_message);
        tradeData.UpdateOwnOffers();
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
        rewardTable = _manager.rewardTable;
    }
    public IncomingHeroData defaultHeroData;
    public PlayerData activePlayerData;
    public EventData eventData;
    public DungeonData dungeonData;
    public RewardTable rewardTable;



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
//date = System.DateTime.Now.ToString("u"),
//signUpDate = System.DateTime.Parse(System.DateTime.Now.ToString("u")),

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
    public int shards;
    public string tradeStartDate;

    public BlacklistEntry[] blacklist;
    public List<PlayerHero> inventory;

    public void ReleaseHero(int _uniqueId, bool _syncOnline = true)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].uniqueId == _uniqueId)
            {
                inventory.RemoveAt(i);
                break;
            }
        }
        DatabaseManager.ValidateInventory();
        DatabaseManager._instance.SaveGameDataLocally();
        if (_syncOnline){
            ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData);
        }
    }
}

[System.Serializable]
public class UploadPlayerData
{
    public UploadPlayerData(PlayerData _playerData)
    {
        playerId = _playerData.playerId;
        password = _playerData.password;
        joinDate = _playerData.joinDate;
        lastUpdate = _playerData.lastUpdate;
        profileDescription = _playerData.profileDescription;
        mtdCounter = _playerData.mtdCounter;
        tradeCounter = _playerData.tradeCounter;
        lastDungeonDate = _playerData.lastDungeonDate;
        currentDungeonRun = _playerData.currentDungeonRun;
        rewardTierBuff = _playerData.rewardTierBuff;
        shards = _playerData.shards;
        tradeStartDate = _playerData.tradeStartDate;
    }
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
    public int shards;
    public string tradeStartDate;
}

[System.Serializable]
public class UploadInventory
{
    public LoginInfo loginInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password };
    public List<UploadInventoryEntry> inventorySegment = new List<UploadInventoryEntry>();
}

[System.Serializable]
public class UploadInventoryEntry
{
    public int index;
    public PlayerHero entry;
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

    //FlavourTexts
    public TextFlavours textFlavours;
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
    public string shortEndText;
    public string description;
}

public enum DungeonType
{
    basic,
    doom
}

[System.Serializable]
public class LoginInfo
{
    public string playerId = "name";
    public string password = "pw";
}

[System.Serializable]
public class UploadDungeonData
{
    public LoginInfo playerInfo;
    public DungeonData dungeonData;
}

[System.Serializable]
public class RewardTable
{
    public List<RewardTier> rewardTiers = new List<RewardTier>();
    public List<DungeonDifficulty> dungeonDifficulties = new List<DungeonDifficulty>();
}
[System.Serializable]
public class RewardTier
{
    public List<int> chances = new List<int>();
}

[System.Serializable]
public class DungeonDifficulty
{
    public int minLvl;
    public int maxLvl;
    public int medianLvl;
}

[System.Serializable]
public class TradeData
{
    public List<TradeOffer> ownOffers;
    public List<TradeOffer> tradeOffers;

    //
    //TODO
    public int GetNumberOFOpenOffers()
    {
        return 12;
    }

    public void UpdateOwnOffers()
    {
        ownOffers = new List<TradeOffer>();
        if (tradeOffers == null)
            return;
        foreach (var item in tradeOffers)
        {
            if(item.heroId == DatabaseManager._instance.activePlayerData.playerId || item.available == DatabaseManager._instance.activePlayerData.playerId)
            {
                ownOffers.Add(item);
            }
        }

        bool valid = false;
        List<TradeOffer> tradeOffersToDelete = new List<TradeOffer>();
        foreach (var offer in ownOffers)
        {
            valid = false;
            foreach (var plHero in DatabaseManager._instance.activePlayerData.inventory)
            {
                if(plHero.uniqueId == offer.uniqueId)
                {
                    valid = true; //hero connected to offer still exists
                    if (plHero.status != HeroStatus.Trading)
                    {
                        Debug.LogError("Hero is not in trading status but is referenced by tradeoffer");
                    }
                    break;                    
                }
            }
            if(valid == false)
            {
                tradeOffersToDelete.Add(offer);
            }
        }
        if(tradeOffersToDelete.Count > 0)
        {
            TradeManager._instance.DeleteOffers(tradeOffersToDelete.ToArray());
        }
    }
}

[System.Serializable]
public class TradeOffer
{
    public string available;
    public int offerId;
    public string date;
    public int playerId;
    public string heroId;
    public int uniqueId;
    public string lastOwner;
    public string origOwner;
    public int traded;
    public int runs;

    public List<int> interestedOffers;
}