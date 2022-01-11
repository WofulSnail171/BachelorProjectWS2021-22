using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RefocusManager : MonoBehaviour
{
    public static RefocusManager _instance;
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
    public void OnApplicationFocus(bool focus)
    {
        if (focus == true && SceneManager.GetActiveScene().buildIndex == 1 && _instance == this)
        {
            if (DatabaseManager._instance.dungeonData.currentRun != null && DatabaseManager._instance.dungeonData.currentRun.valid == true)
            {
                DungeonManager._instance.RevalidateMaxStepsAndRandomNums();
                DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(true);
                DungeonManager._instance.CalculateRun(DungeonManager._instance.CurrentStep());
                //DungeonManager._instance.CalculateRun(0);
            }
            if (DatabaseManager._instance.activePlayerData.tradeStartDate != "" && DatabaseManager._instance.tradeData.ownOffers != null && DatabaseManager._instance.tradeData.ownOffers.Count > 0)
            {
                TradeManager._instance.FastForwardToStep(TradeManager._instance.GetCurrentStep());
            }
        }
    }
}