using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DungeonObserveHeader : MonoBehaviour
{
    public TextMeshProUGUI shardText;
    public TextMeshProUGUI buffText;

    public TextMeshProUGUI rewardTierText;
    public TextMeshProUGUI dialogText;

    [SerializeField] Image healthBar;
    [SerializeField] float animSpeed;

    //anim calc helper
    private int rewardTier;
    private int oldRewardTier;
    private float currentFill;

    private bool IsRunningCoroutine;
    private bool WasTriggered;

    void Awake()
    {
        DeleventSystem.DungeonStep += UpdateVisuals;
        DeleventSystem.DungeonStart += InitVisuals;
    }

    private void InitVisuals()
    {
        if (shardText != null && buffText != null && DatabaseManager.CheckDatabaseValid())
        {
            shardText.text = $"{DatabaseManager._instance.activePlayerData.shards} / 5 Shards";
            buffText.text = $"{DatabaseManager._instance.activePlayerData.rewardTierBuff} / 10 Shards";
        }
        //remove

        healthBar.fillAmount = 1;
        rewardTier = DatabaseManager._instance.dungeonData.currentRun.initialRewardTier  + 1;

        rewardTierText.text = $" {rewardTier} Lvl";
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        IsRunningCoroutine = false;
    }

    private void OnEnable()
    {
        CatchUpVisuals();
    }

    private void CatchUpVisuals()
    {
        rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 + 1;


        if (rewardTier != 1)
        {
            if (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10 != 0)
                healthBar.fillAmount = (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10) / 10f;


            else
                healthBar.fillAmount = 1;

            Debug.Log(healthBar.fillAmount.ToString());
        }

        rewardTierText.text = $"Lvl {rewardTier}";

    }

    private void UpdateVisuals()
    {
        //healthbar of reward
        if(rewardTierText != null && DatabaseManager.CheckDatabaseValid() && DungeonManager._instance.currentCalcRun != null)//catch
        {
            //reward tiers
            oldRewardTier = rewardTier;
            rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 + 1;



            //do not animate if inactive
            if (rewardTier != 1 && this.gameObject.activeSelf)
            {
                if(rewardTier == oldRewardTier && !IsRunningCoroutine && DungeonManager._instance.currentCalcRun.rewardHealthBar % 10 != 0)
                {

                    float start = healthBar.fillAmount;
                    float end = healthBar.fillAmount = (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10) / 10f;

                    StartCoroutine(AnimateOne(animSpeed, start, end));
                }
            }


            //catch
            if (oldRewardTier != rewardTier && !IsRunningCoroutine)
                rewardTierText.text = $"Lvl {rewardTier}";


            //text
            if (dialogText != null && DungeonManager._instance.currentCalcRun.dungeonLogArr != null && DungeonManager._instance.currentCalcRun.dungeonLogArr.Length >= 1)
            {
                dialogText.text = DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry;
            }

            else
                dialogText.text = "";
        }
    }


    IEnumerator AnimateOne(float time, float startValue, float endValue)
    {
        IsRunningCoroutine = true;



        healthBar.fillAmount = startValue;

        LeanTween.value(healthBar.gameObject,startValue, endValue, time)
            .setOnUpdate(setFillAmount)
            .setEaseInOutExpo();

        yield return new WaitForSeconds(time);


        if (healthBar.fillAmount == 0)
        {
            healthBar.fillAmount = 1;

            rewardTierText.text = $"Lvl {rewardTier}";
        }

        yield return new WaitForEndOfFrame();

        IsRunningCoroutine = false;
    }


    private void setFillAmount(float value)
    {
        healthBar.fillAmount = value;    
    }

}
