using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public static bool SuccesfullTrades()
    {
        if(DatabaseManager.CheckDatabaseValid() && DatabaseManager._instance.tradeData != null && DatabaseManager._instance.tradeData.tradeOffers.Count > 0 )
        {
            //Do the check
            DatabaseManager._instance.tradeData.UpdateOwnOffers();
            foreach (var item in DatabaseManager._instance.tradeData.ownOffers)
            {
                if (item.available == DatabaseManager._instance.activePlayerData.playerId)
                    return true;
            }
        }
        return false;
    }

    private bool AutoPLay = true;
    public float AutoPlayWaitTimeSec = 1.0f;

    public void UploadOffer(PlayerHero heroOne, PlayerHero heroTwo = null, PlayerHero heroThree = null, PlayerHero heroFour = null)
    {
        if(heroFour != null)
            UploadOffer(new PlayerHero[] { heroOne, heroTwo, heroThree, heroFour });
        else if(heroThree != null)
            UploadOffer(new PlayerHero[] { heroOne, heroTwo, heroThree });
        else if(heroTwo != null)
            UploadOffer(new PlayerHero[] { heroOne, heroTwo});
        else if(heroOne != null)
            UploadOffer(new PlayerHero[] { heroOne});
    }

    public void UploadOffer(PlayerHero[] _heroes)
    {
        UploadOfferData _data = new UploadOfferData { heroes = _heroes, playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password } };
        _data.date = DateTime.Now.ToString("u");
        string message = JsonUtility.ToJson(_data);
        var test = JsonUtility.FromJson<PlayerHero[]>(message);

        ServerCommunicationManager._instance.GetInfo(Request.UploadOffer, message);
        PullTradeOffers();
    }
    [System.Serializable]
    public class UploadOfferData
    {
        public LoginInfo playerInfo;
        public PlayerHero[] heroes;
        public string date;
    }

    public void UpdateOffer(int _tradeOfferId, PlayerHero[] _ownTradeOfferIds)
    {
        //Can only happen when player has units in trade
        //Adds chosen OfferIds from own hero offers to current TradeOffer
        UploadUpdateOffer newUpdate = new UploadUpdateOffer { playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }, uniqueIds = new List<int>() };
        newUpdate.interestingId = _tradeOfferId;
        newUpdate.offerIdMode = false;
        foreach (var item in _ownTradeOfferIds)
        {
            newUpdate.uniqueIds.Add(item.uniqueId);
        }
        string message = JsonUtility.ToJson(newUpdate);
        ServerCommunicationManager._instance.GetInfo(Request.UpdateOffer, message);
    }

    public void UpdateOffer(int _tradeOfferId, int[] _ownTradeOfferIds)
    {
        //Can only happen when player has units in trade
        //Adds chosen OfferIds from own hero offers to current TradeOffer
        UploadUpdateOffer newUpdate = new UploadUpdateOffer { playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }, uniqueIds = new List<int>() };
        newUpdate.interestingId = _tradeOfferId;
        newUpdate.offerIdMode = true;
        foreach (var item in _ownTradeOfferIds)
        {
            newUpdate.uniqueIds.Add(item);
        }
        string message = JsonUtility.ToJson(newUpdate);
        ServerCommunicationManager._instance.GetInfo(Request.UpdateOffer, message);
    }

    [System.Serializable]
    public class UploadUpdateOffer
    {
        public bool offerIdMode;
        public LoginInfo playerInfo;
        public int interestingId;
        public List<int> uniqueIds;
    }


    public void PullTradeOffers()
    {
        // gets current status of the online backend concerning trades.
        // 1) when getting new trade offers for "swiping"
        // 2) when checking whether a trade was successful -> happens after x amount of time
        ServerCommunicationManager._instance.DoServerRequest(Request.PullTradeOffers, OnPullTradeOffers);
    }

    public void OnPullTradeOffers()
    {
        //Analyze and check for trades that have potentially happened
    }


    private void NextStepTrade()
    {

    }

    public void DeleteOffers(TradeOffer[] _tradeOffers, DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        UploadDeleteOffers uploadData = new UploadDeleteOffers { offersToDelete = new List<TradeOffer>(), playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password } };
        foreach (var item in _tradeOffers)
        {
            uploadData.offersToDelete.Add(item);
            DatabaseManager._instance.tradeData.tradeOffers.Remove(item);
        }
        DatabaseManager._instance.tradeData.UpdateOwnOffers();
        string message = JsonUtility.ToJson(uploadData);
        ServerCommunicationManager._instance.GetInfo(Request.DeleteOffers, message, _simpleEvent, _messageEvent);
    }

    [System.Serializable]
    public class UploadDeleteOffers
    {
        public LoginInfo playerInfo;
        public List<TradeOffer> offersToDelete;
    }

    IEnumerator AutoplayRoutine()
    {
        while (AutoPLay)
        {
            //Update Loop for dungeonRun
            if (DatabaseManager._instance.tradeData != null)
            {
                NextStepTrade();
            }
            yield return new WaitForSeconds(AutoPlayWaitTimeSec);
        }
    }


}
