using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DungeonObserveHeader : MonoBehaviour
{
    public TextMeshProUGUI shardText;
    public TextMeshProUGUI buffText;

    public TextMeshProUGUI rewardTierText;
    public TextMeshProUGUI dialogText;
    public CheapProgressBar healthBar;

    void Awake()
    {
        DeleventSystem.DungeonStep += UpdateVis;
    }

    // Update is called once per frame
    void UpdateVis()
    {
        if(shardText != null && buffText != null && DatabaseManager.CheckDatabaseValid())
        {
            shardText.text = DatabaseManager._instance.activePlayerData.shards.ToString() + " / 5 Shards";
            buffText.text = DatabaseManager._instance.activePlayerData.rewardTierBuff.ToString() + " / 10 Shards";
        }

        if(rewardTierText != null && DatabaseManager.CheckDatabaseValid() && DungeonManager._instance.currentCalcRun != null)
        {
            int rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10;
            if (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10 != 0)
            {
                //integer division rounds down but we want it to go up
                rewardTier += 1;

                if (healthBar != null && DungeonManager._instance.currentCalcRun != null)
                {
                    healthBar.SetVal(DungeonManager._instance.currentCalcRun.rewardHealthBar % 10, 10);
                }
            }
            else
            {
                if (healthBar != null && DungeonManager._instance.currentCalcRun != null)
                {
                    healthBar.SetVal(10, 10);
                }
            }            
            rewardTierText.text = "Lvl " + rewardTier.ToString();
            if(dialogText != null && DungeonManager._instance.currentCalcRun.dungeonLogArr != null && DungeonManager._instance.currentCalcRun.dungeonLogArr.Length >= 1)
            {
                dialogText.text = DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry;
            }
        }
    }
}
