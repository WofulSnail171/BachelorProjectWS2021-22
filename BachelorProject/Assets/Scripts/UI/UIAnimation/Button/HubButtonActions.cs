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

    private HubState currentHubFocus;
    [Space]
    [Space]
    [Space]
    [SerializeField] private ProgressState tradeState;
    [SerializeField] private ProgressState dungeonState;//wil be hidden later on


    [SerializeField] float alreadyPassedTime;
    [SerializeField] float maxTime;
    [SerializeField] float bufferTime;//helper for tests


    float pausedtime;

    #endregion


    private void Start()
    {
        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
        hubButton.GetComponent<Button>().onClick.AddListener(() => { ClickedHub(); });
        tradeFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedTrade(); });
        dungeonFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedDungeon(); });
        dungeonReadyButton.GetComponent<Button>().onClick.AddListener(() => { ClickedReadyDungeon(); });

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


        //animation test
        //AnimateTradeProgress(alreadyPassedTime,maxTime);
        //AnimateDungeonProgress(alreadyPassedTime, maxTime);
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


    //rewards or trade done
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ClickedReadyDungeon()
    {
        //split into more steps with pop ups:
        DungeonManager._instance.ApplyGrowth();
        DungeonManager._instance.EventRewardHandling();
        DungeonManager._instance.WrapUpDungeon();
        DatabaseManager._instance.SaveGameDataLocally();
        ServerCommunicationManager._instance.GetInfo(Request.PushPlayerData, JsonUtility.ToJson(DatabaseManager._instance.activePlayerData));
        UploadDungeonData dataDungeon = new UploadDungeonData { dungeonData = DatabaseManager._instance.dungeonData, playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password } };
        ServerCommunicationManager._instance.GetInfo(Request.PushDungeonData, JsonUtility.ToJson(dataDungeon));
        if(DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        //actually do pop up 
        //
        ///


        //change buttons and hub focus
        switch (currentHubFocus)
        {
            case HubState.HeroHub:
                break;
            case HubState.DungeonHub:
                UIEnablerManager.Instance.EnableCanvas();
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

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;
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




    //
    //animation progress
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /*private void AnimateTradeProgress( float start, float target)
    {
        //activate
        tradeProgressBar.gameObject.SetActive(true);
        tradeFocusProgressBar.gameObject.SetActive(true);
        tradeTextGroup.SetActive(true);
        tradeSungleTextGroup.SetActive(false);

        //calc
        float tweenTime = target - start;
        float progressStart = start / target;

        //animate
        StartCoroutine(WaitForTradeFinish(tweenTime));

        LeanTween.value(tradeProgressBar.gameObject, progressStart, 1f, tweenTime)
            .setOnUpdate((value) =>
            {
                tradeProgressBar.fillAmount = value;
                setTradeText(value,tradeProgressTime);
            });

        LeanTween.value(tradeFocusProgressBar.gameObject, progressStart, 1f, tweenTime)
            .setOnUpdate((value) =>
            {
                tradeFocusProgressBar.fillAmount = value;
                setTradeText(value, tradeFocusProgressTime);
            });
    }

    private void setTradeText(float value, TextMeshProUGUI textMesh)
    {
        int recalcValue = (int)(value * maxTime);

        textMesh.text = $"{recalcValue} sec";
    }*/

    /*private void AnimateDungeonProgress(float start,float target)
    {
        //activate
        dungeonProgressBar.gameObject.SetActive(true);
        dungeonFocusProgressBar.gameObject.SetActive(true);
        dungeonTextGroup.SetActive(true);
        dungeonSingleTextGroup.SetActive(false);

        //calc
        float tweenTime = target - start;
        float progressStart = start / target;


        //animate
        StartCoroutine(WaitForDungeonFinish(tweenTime));

        LeanTween.value(dungeonProgressBar.gameObject, progressStart, 1f, tweenTime)
            .setOnUpdate((value) =>
            {
                dungeonProgressBar.fillAmount = value;
                setDungeonText(value, dungeonProgressTime);
            });

        LeanTween.value(dungeonFocusProgressBar.gameObject, progressStart, 1f, tweenTime)
            .setOnUpdate((value) =>
            {
                dungeonFocusProgressBar.fillAmount = value;
                setDungeonText(value, dungeonFocusProgressTime);
            });
    }*/
    //helper coroutines
    /*IEnumerator WaitForTradeFinish(float time)
    {
        yield return new WaitForSeconds(time + bufferTime);

        UpdateTradeButton(ButtonState.Finished);

        tradeProgressBar.gameObject.SetActive(false);
        tradeFocusProgressBar.gameObject.SetActive(false);
        tradeTextGroup.SetActive(false);
        tradeSungleTextGroup.SetActive(true);
    }*/

    /*IEnumerator WaitForDungeonFinish(float time)
    {
        yield return new WaitForSeconds(time + bufferTime);

        UpdateDungeonButton(ButtonState.Finished);

        dungeonProgressBar.gameObject.SetActive(false);
        dungeonFocusProgressBar.gameObject.SetActive(false);
        dungeonTextGroup.SetActive(false);
        dungeonSingleTextGroup.SetActive(true);
    }*/

}

