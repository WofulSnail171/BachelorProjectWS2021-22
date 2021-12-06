using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum HubState
{
    HeroHub,
    TradeHub,
    DungeonHub
}

public enum ProgressState
{
    Empty,
    Pending,
    Done
}

public enum ButtonState
    {
        Focused,
        Unfocused,
        Finished,
    }

public class HubButtonActions : MonoBehaviour
{

    #region vars
    //actual buttons
    [SerializeField] GameObject tradeButton;
    [SerializeField] GameObject tradeFocusedButton;
    [SerializeField] GameObject tradeReadyButton;
    [SerializeField] GameObject dungeonButton;
    [SerializeField] GameObject dungeonFocusedButton;
    [SerializeField] GameObject dungeonReadyButton;
    [SerializeField] GameObject hubButton;
    [SerializeField] GameObject hubFocusedButton;//do nothing, just set active
    [Space]
    [SerializeField] GameObject ContinueEndTextButton;
    [SerializeField] GameObject ContinueHeroPullButton;
    [SerializeField] GameObject DiscardHeroPullButton;
    [SerializeField] GameObject ReleaseHeroPullButton;
    [SerializeField] GameObject ContinueShardButton;
    [SerializeField] GameObject ContinueHeroGrowthButton;
    [SerializeField] GameObject CloseWarningButton;
    [SerializeField] GameObject CancelReleaseButton;
    [SerializeField] GameObject ConfirmReleaseButton;
    [SerializeField] GameObject CannotConfirmReleaseButton;
    [Space]
    [SerializeField] GameObject Cancel1;
    [SerializeField] GameObject Cancel2;
    [Space]
    [SerializeField] GameObject tradeTextGroup;
    [SerializeField] GameObject tradeSungleTextGroup;
    [SerializeField] GameObject dungeonTextGroup;
    [SerializeField] GameObject dungeonSingleTextGroup;
    [Space]
    [SerializeField] Image tradeProgressBar;
    [SerializeField] Image dungeonProgressBar;
    [SerializeField] TextMeshProUGUI tradeProgressTime;
    [SerializeField] TextMeshProUGUI dungeonProgressTime;
    [Space]
    [SerializeField] Image tradeFocusProgressBar;
    [SerializeField] Image dungeonFocusProgressBar;
    [SerializeField] TextMeshProUGUI tradeFocusProgressTime;
    [SerializeField] TextMeshProUGUI dungeonFocusProgressTime;

    [HideInInspector]public HubState currentHubFocus;
    [Space]
    [Space]
    [SerializeField] InventoryUI InventoryUI;

    public bool isRewarding = false;

    private ProgressState tradeState;
    private ProgressState dungeonState;
    #endregion


    private void Start()
    {
        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
        hubButton.GetComponent<Button>().onClick.AddListener(() => { ClickedHub(); });
        tradeFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedTrade(); });
        dungeonFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedDungeon(); });
        dungeonReadyButton.GetComponent<Button>().onClick.AddListener(() => { ClickedReadyDungeon(); });

        ContinueHeroGrowthButton.GetComponent<Button>().onClick.AddListener(() => { ContinueHeroGrowth(); }); 
        ContinueEndTextButton.GetComponent<Button>().onClick.AddListener(() => { ContinueEndText(); }); 
        ContinueShardButton.GetComponent<Button>().onClick.AddListener(() => { ContinueShards(); });
        ReleaseHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { ReleaseHeroReward(); });
        DiscardHeroPullButton.GetComponent<Button>().onClick.AddListener(() => {DiscardHeroReward(); });
        ContinueHeroPullButton.GetComponent<Button>().onClick.AddListener(() => {ContinueHeroReward(); });

        CancelReleaseButton.GetComponent<Button>().onClick.AddListener(() => {CancelRelease(); });
        Cancel1.GetComponent<Button>().onClick.AddListener(() => {CancelRelease(); });
        Cancel2.GetComponent<Button>().onClick.AddListener(() => {CancelRelease(); });


        ConfirmReleaseButton.GetComponent<Button>().onClick.AddListener(() => {ConfirmRelease(); });
        CloseWarningButton.GetComponent<Button>().onClick.AddListener(() => {CloseWarningPopUp(); });
        CannotConfirmReleaseButton.GetComponent<Button>().onClick.AddListener(() => {CannotConfirmRelease(); });

        DeleventSystem.DungeonStep += UpdateStates;
        DeleventSystem.DungeonStart += UpdateStates;
        DeleventSystem.DungeonEnd += UpdateStates;
        DeleventSystem.DungeonEvent += UpdateStates;
        DeleventSystem.DungeonRewardFinished += UpdateStates;
        //dungeon cancel? connected to seperate cancel func?

        DeleventSystem.TradeStart += UpdateStates;
        DeleventSystem.TradeEnd += UpdateStates;

        //maybe connect to a seperate cancel func?
        DeleventSystem.TradeCancel += UpdateStates;
    }


    private void UpdateStates()
    {
        tradeState = DatabaseManager.GetTradeState();
        dungeonState = DatabaseManager.GetDungeonRunState();


        //dont need switch
        if(dungeonState == ProgressState.Done)
        {
            dungeonProgressBar.gameObject.SetActive(false);
            dungeonTextGroup.SetActive(false);
            dungeonSingleTextGroup.SetActive(true);

            UpdateDungeonButton(ButtonState.Finished);
        }

        if(tradeState == ProgressState.Done)
        {
            //do stuff
            //
            //
        }
    }

    void Update()
    {
        if(DungeonManager._instance.currentCalcRun != null && dungeonState == ProgressState.Pending)
        {
            //set active progress bar
            dungeonProgressBar.gameObject.SetActive(true);
            dungeonTextGroup.SetActive(true);
            dungeonSingleTextGroup.SetActive(false);

            float value = (float)DungeonManager._instance.currentCalcRun.currentStep / (float)DatabaseManager._instance.dungeonData.currentRun.maxSteps;
            dungeonFocusProgressBar.fillAmount = value;
            dungeonProgressBar.fillAmount = value;
            setDungeonText(value, dungeonFocusProgressTime);
            setDungeonText(value, dungeonProgressTime);
            
        }
    }



    //click checks
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedTrade()
    {
        //check what state the trade is in
        switch (tradeState)
        {
            case ProgressState.Empty:
                //empty
                //------------------------------------

                //check where you are coming from
                switch(currentHubFocus)
                {
                    case HubState.DungeonHub:
                        UIEnablerManager.Instance.EnableCanvas();
                        UIEnablerManager.Instance.SwitchElements("DungeonObserve","HeroHub", true);
                        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);

                        //button
                        if (tradeState != ProgressState.Done)
                            UpdateDungeonButton(ButtonState.Unfocused);

                        break;
                    case HubState.HeroHub:
                        UpdateHubButton(ButtonState.Unfocused);
                        break;
                    default:
                        break;
                }

                //always
                UIEnablerManager.Instance.SwitchElements("General", "TradeSelect", true);
                UpdateTradeButton(ButtonState.Focused);


                break;


            case ProgressState.Pending:
                //pending
                //------------------------------------
                //check where you are coming from
                switch (currentHubFocus)
                {
                    case HubState.DungeonHub:

                        //enable background
                        UIEnablerManager.Instance.EnableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("DungeonObserve", "TradeObserve", true);

                        //change buttons
                        UpdateDungeonButton(ButtonState.Unfocused);
                        break;

                    case HubState.HeroHub:
                        //ui
                        UIEnablerManager.Instance.SwitchElements("HeroHub", "TradeObserve", true);
                        UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);

                        //buttons
                        UpdateHubButton(ButtonState.Unfocused);
                        break;

                    default:
                        break;
                }

                //do always
                UpdateTradeButton(ButtonState.Focused);

                //change hub focus state
                currentHubFocus = HubState.TradeHub;

                break;
            default:
                Debug.Log("no trade state");
                break;
        }

        //change focus state
        currentHubFocus = HubState.TradeHub;
    }

    private void ClickedDungeon()
    {

        //check what state the dungeon is in
        switch (dungeonState)
        {
            case ProgressState.Empty:
                //empty
                //------------------------------------
                //check where you coming from

                switch(currentHubFocus)
                {
                    case HubState.HeroHub:
                        UIEnablerManager.Instance.SwitchElements("HeroHub", "DungeonMapSelect", true);
                        UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);
                        break;
                    case HubState.TradeHub:
                        UIEnablerManager.Instance.SwitchElements("TradeObserve", "DungeonMapSelect", true);
                        break;

                    default:
                        break;
                }

                //always
                UIEnablerManager.Instance.DisableElement("General", true);
                
                break;

            case ProgressState.Pending:
                //pending
                //------------------------------------
                //check where you are coming from
                switch (currentHubFocus)
                {
                    case HubState.HeroHub:
                        //disable background
                        UIEnablerManager.Instance.DisableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("ShardAndBuff", "DungeonObserve", true);
                        UIEnablerManager.Instance.DisableElement("HeroHub", true);

                        //buttons
                        UpdateHubButton(ButtonState.Unfocused);

                        break;
                    case HubState.TradeHub:
                        //disable background
                        UIEnablerManager.Instance.DisableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("TradeObserve", "DungeonObserve", true);

                        //buttons
                        if(tradeState != ProgressState.Done)
                            UpdateTradeButton(ButtonState.Unfocused);

                        break;
                    default:
                        break;
                }

                //do always
                UpdateDungeonButton(ButtonState.Focused);

                break;
            default:
                Debug.Log("no trade state");
                break;
        }

        //change focus state
        currentHubFocus = HubState.DungeonHub;
    }

    private void ClickedHub()
    {
        InventoryUI.UpdateInventory();


        //check where you are coming from
        switch (currentHubFocus)
        {
            case HubState.DungeonHub:
                //enable background
                UIEnablerManager.Instance.EnableCanvas();

                //
                //ui
                UIEnablerManager.Instance.SwitchElements("DungeonObserve", "ShardAndBuff", true);
                UIEnablerManager.Instance.EnableElement("HeroHub", true);

                //buttons
                if(dungeonState != ProgressState.Done)
                    UpdateDungeonButton(ButtonState.Unfocused);


                break;
            case HubState.TradeHub:
                //ui
                UIEnablerManager.Instance.SwitchElements("TradeObserve", "ShardAndBuff", true);
                UIEnablerManager.Instance.EnableElement("HeroHub", true);

                //buttons
                if (tradeState != ProgressState.Done)
                    UpdateTradeButton(ButtonState.Unfocused);


                break;
            default:
                break;
        }

        //always do
        currentHubFocus = HubState.HeroHub;
        UpdateHubButton(ButtonState.Focused);
    }


    
    //focused
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedFocusedDungeon()
    {
        //wanna cancel?

        //do pop up
        UIEnablerManager.Instance.EnableElement("DungeonCancel", true);
    }

    private void ClickedFocusedTrade()
    {
        //wanna cancel?

        //do pop up
        UIEnablerManager.Instance.EnableElement("DungeonCancel", true);
    }


    //rewards done
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ClickedReadyDungeon()
    {
        isRewarding = true;

        //split into more steps with pop ups:
        DungeonManager._instance.EventRewardHeroHandling();
        DungeonManager._instance.EventRewardShardHandling();



        //actually start pop up flow
        UIEnablerManager.Instance.EnableElement("DungeonEnd", true);


        //change buttons and hub focus in bg
        switch (currentHubFocus)
        {
            case HubState.HeroHub:
                break;
            case HubState.DungeonHub:
                UIEnablerManager.Instance.EnableCanvas();
                UIEnablerManager.Instance.EnableElement("ShardAndBuff",true);
                UIEnablerManager.Instance.SwitchElements("DungeonObserve", "HeroHub", true);

                UpdateDungeonButton(ButtonState.Unfocused);
                UpdateHubButton(ButtonState.Focused);
                break;
            case HubState.TradeHub:
                UIEnablerManager.Instance.SwitchElements("TradeObserve", "ShardAndBuff", true);
                UIEnablerManager.Instance.EnableElement("HeroHub", true);

                //buttons
                if (tradeState == ProgressState.Pending)
                    UpdateTradeButton(ButtonState.Unfocused);

                else
                    UpdateTradeButton(ButtonState.Finished);
                break;
            default:
                break;
        }
       
    }


    //
    //reward flow pop ups
    private void ContinueEndText()
    {
        UIEnablerManager.Instance.DisableElement("DungeonEnd", true);

        if (DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
            UIEnablerManager.Instance.EnableElement("ShardReward", true);

        else
            UIEnablerManager.Instance.EnableElement("HeroGrowth", true);
    }

    private void ContinueShards()
    {
        UIEnablerManager.Instance.EnableBlur();
        UIEnablerManager.Instance.SwitchElements("ShardReward","HeroGrowth", true);
    }

    private void ContinueHeroGrowth()
    {
        UIEnablerManager.Instance.EnableBlur();

        UIEnablerManager.Instance.SwitchElements( "HeroGrowth","HeroPull", true);
    }

    private void ContinueHeroReward()
    {
        DungeonManager._instance.AddRewardHeroToInventory();

        //update inventory UI
        InventoryUI.ResetExploring();
        InventoryUI.UpdateInventory();

        //close pop up flow
        UIEnablerManager.Instance.DisableElement("HeroPull", true);

        //wrap up
        UIEnablerManager.Instance.DisableBlur();

        DungeonManager._instance.WrapUpDungeon();
        if (DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;

        isRewarding = false;
    }

    private void ReleaseHeroReward()
    {
        InventoryUI.DoRelease = true;

        UIEnablerManager.Instance.DisableBlur();
        UIEnablerManager.Instance.DisableElement("HeroPull", true);
        UIEnablerManager.Instance.SwitchElements("General","ReleaseCancel", false);   
    }  

    private void DiscardHeroReward()
    {
        DungeonManager._instance.DiscardRewardHero();
        UIEnablerManager.Instance.DisableElement("HeroPull", true);

        //update inventory UI
        InventoryUI.ResetExploring();
        InventoryUI.UpdateInventory();

        //wrap up
        UIEnablerManager.Instance.DisableBlur();


        DungeonManager._instance.WrapUpDungeon();
        if (DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;

        isRewarding = false;

    }




    //
    //release flow pop ups
    private void CancelRelease()
    {
        UIEnablerManager.Instance.EnableBlur();

        InventoryUI.DoRelease = false;


        UIEnablerManager.Instance.SwitchElements("ReleaseCancel", "General",  true);
        UIEnablerManager.Instance.EnableElement("HeroPull", true);
    }

    private void ConfirmRelease()
    {
        InventoryUI.DoRelease = false;     

        //switch old hero with pulled hero
        DatabaseManager._instance.activePlayerData.ReleaseHero(InventoryUI.releaseHero.uniqueId, false);
        DungeonManager._instance.AddRewardHeroToInventory();


        //update inventory UI
        InventoryUI.ResetExploring();
        InventoryUI.UpdateInventory();


        UIEnablerManager.Instance.SwitchElements("ReleaseSubmit", "General", true);

        //wrap up
        UIEnablerManager.Instance.DisableBlur();


        DungeonManager._instance.WrapUpDungeon();
        if (DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;

        isRewarding = false;

    }

    private void CannotConfirmRelease()
    {
        UIEnablerManager.Instance.EnableElement("ReleaseWarning", true);
    }

    private void CloseWarningPopUp()
    {
        UIEnablerManager.Instance.DisableElement("ReleaseWarning", true);
    }



    //update buttons
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //
    private void setDungeonText(float value, TextMeshProUGUI textMesh)
    {
        int recalcValue = (int)(value * 100f);

        textMesh.text = $"{recalcValue} %";
    }
    

    public void UpdateTradeButton(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Unfocused:
                tradeFocusedButton.SetActive(false);
                tradeReadyButton.SetActive(false);
                tradeButton.SetActive(true);
                break;
            case ButtonState.Focused:
                tradeReadyButton.SetActive(false);
                tradeButton.SetActive(false);
                tradeFocusedButton.SetActive(true);
                break;
            case ButtonState.Finished:
                tradeFocusedButton.SetActive(false);
                tradeButton.SetActive(false);
                tradeReadyButton.SetActive(true);
                break;
            default:
                Debug.Log("using unknown button state to update trade button");
                break;
        }
    }

    public void UpdateDungeonButton(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Unfocused:
                dungeonReadyButton.SetActive(false);
                dungeonFocusedButton.SetActive(false);
                dungeonButton.SetActive(true);
                break;
            case ButtonState.Focused:
                dungeonReadyButton.SetActive(false);
                dungeonButton.SetActive(false);
                dungeonFocusedButton.SetActive(true);
                break;
            case ButtonState.Finished:
                dungeonFocusedButton.SetActive(false);
                dungeonButton.SetActive(false);
                dungeonReadyButton.SetActive(true);
                break;
            default:
                Debug.Log("using unknown button state to update dungeon button");
                break;
        }
    }

    public void UpdateHubButton(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Unfocused:
                hubFocusedButton.SetActive(false);
                hubButton.SetActive(true);
                break;
            case ButtonState.Focused:
                hubButton.SetActive(false);
                hubFocusedButton.SetActive(true);
                break;
            default:
                Debug.Log("using unknown button state to update trade button");
                break;
        }
    }
    
    public void UpdateHubState(HubState state)
    {
        currentHubFocus = state;
    }

}

