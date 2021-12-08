using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShardBuffHeader : MonoBehaviour
{
    public TextMeshProUGUI shardText;
    public TextMeshProUGUI buffText;

    // Update is called once per frame
    void Update()
    {
        if (shardText != null && buffText != null && DatabaseManager.CheckDatabaseValid())
        {
            shardText.text = DatabaseManager._instance.activePlayerData.shards.ToString() + " / 5 Shards";
            buffText.text = DatabaseManager._instance.activePlayerData.rewardTierBuff.ToString() + " / 9 Buff";
        }
    }
}
