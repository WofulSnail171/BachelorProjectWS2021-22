using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroGrowthUI : MonoBehaviour
{
    #region vars
    [SerializeField] Transform HeroAmount;

    [SerializeField] GameObject SkipButton;
    [SerializeField] GameObject ContinueButton;

    //calc helper
    int child_1;
    int child_2;
    #endregion


    //init
    private void Start()
    {
        SkipButton.GetComponent<Button>().onClick.AddListener(() => { ClickedSkip(); });
        ContinueButton.GetComponent<Button>().onClick.AddListener(() => { ClickedContinue(); });
    }

    private void OnEnable()
    {
        //test
        UpdateHeroGrowthPopUp();

        //initially hidden
        ContinueButton.SetActive(false);
    }

    //update card
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void UpdateHeroGrowthPopUp()
    {
        //DatabaseManager._instance.dungeonData.currentRun.party

        //DungeonManager._instance.currentCalcRun.

        //catch
        if (DatabaseManager._instance.dungeonData.currentRun.party.Length < 0)
            Debug.LogWarning("Party is empty");

        //hide all
        for(int i = 0; i < HeroAmount.childCount; i++)
        {
            HeroAmount.GetChild(i).gameObject.SetActive(false);
        }


        //set active due to party size
        for (int i = 0; i < DatabaseManager._instance.dungeonData.currentRun.party.Length; i++)
        {
            if(i >= 4)
            {
                Debug.LogWarning("Party too big");
                break;
            }

            child_1 = i * 2;

            HeroAmount.GetChild(child_1).gameObject.SetActive(true);
            HeroAmount.GetChild(child_1).GetComponent<GrowthCard>().UpdateHero(DatabaseManager._instance.dungeonData.currentRun.party[i]);

            if(child_1 - 1 >= 0)
            {
                child_2 = child_1 - 1;
                HeroAmount.GetChild(child_2).gameObject.SetActive(true);
            }
        }

    }


    //buttons
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedSkip()
    {

    }    
    
    private void ClickedContinue()
    {

    }




    //do growth animations
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



}