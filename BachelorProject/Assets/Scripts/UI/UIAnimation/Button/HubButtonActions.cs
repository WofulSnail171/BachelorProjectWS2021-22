using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HubButtonActions : MonoBehaviour
{

    private enum HubState
    {
        HeroHub,
        TradeHub,
        DungeonHub
    }

    private enum HeroState
    {
        Empty,
        Pending,
        Done
    }


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

    [SerializeField] Image tradeProgressBar;
    [SerializeField] Image dungeonProgressBar;
    [SerializeField] TextMeshProUGUI tradeProgressTime;
    [SerializeField] TextMeshProUGUI dungeonProgressTime;

    private HubState currentHubFocus;

    private HeroState tradeState;
    private HeroState dungeonState;


    [SerializeField] float alreadyPassedTime;
    [SerializeField] float maxTime;
    [SerializeField] float bufferTime;
    #endregion


    private void Start()
    {
        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
        hubButton.GetComponent<Button>().onClick.AddListener(() => { ClickedHub(); });
        tradeFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedTrade(); });
        dungeonFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedDungeon(); });


        //animation test
        //AnimateTradeProgress(alreadyPassedTime,maxTime);
        //AnimateDungeonProgress(alreadyPassedTime, maxTime);
    }


    //click checks
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedTrade()
    {
        //check what state the trade is in
        switch (tradeState)
        {
            case HeroState.Empty:
                //empty
                //------------------------------------

                UIEnablerManager.Instance.SwitchElements("General", "TradeSelect", true);

                break;


            case HeroState.Pending:
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
                        UIEnablerManager.Instance.EnableElement("TradeObserve",true);

                        //change buttons
                        dungeonFocusedButton.SetActive(false);
                        dungeonButton.SetActive(true);
                        break;

                    case HubState.HeroHub:
                        //ui
                        UIEnablerManager.Instance.SwitchElements("HeroHub", "TradeObserve", true);

                        //buttons
                        hubFocusedButton.SetActive(false);
                        hubButton.SetActive(true);
                        break;

                    default:
                        break;
                }

                //do always
                tradeButton.SetActive(false);
                tradeFocusedButton.SetActive(true);

                //change hub focus state
                currentHubFocus = HubState.TradeHub;

                break;

            case HeroState.Done:
                //pending is done
                //------------------------------------
                //do pop up

                break;

            default:
                Debug.Log("no trade state");
                break;
        }


    }

    private void ClickedDungeon()
    {

        //check what state the dungeon is in
        switch (dungeonState)
        {
            case HeroState.Empty:
                //empty
                //------------------------------------

                UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);
                UIEnablerManager.Instance.DisableElement("General", true);
                UIEnablerManager.Instance.SwitchElements("HeroHub", "DungeonMapSelect", true);


                break;

            case HeroState.Pending:
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
                        hubFocusedButton.SetActive(false);
                        hubButton.SetActive(true);

                        break;
                    case HubState.TradeHub:
                        //disable background
                        UIEnablerManager.Instance.DisableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("ShardAndBuff", "DungeonObserve", true);
                        UIEnablerManager.Instance.DisableElement("TradeObserve", true);

                        //buttons
                        tradeFocusedButton.SetActive(false);
                        tradeButton.SetActive(true);

                        break;
                    default:
                        break;
                }
                
                //do always
                dungeonButton.SetActive(false);
                dungeonFocusedButton.SetActive(true);

                //change hub focus state
                currentHubFocus = HubState.DungeonHub;

                break;

            case HeroState.Done:
                //pending is done
                //------------------------------------
                //do pop up
                break;
            default:
                Debug.Log("no trade state");
                break;
        }

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
                dungeonFocusedButton.SetActive(false);
                dungeonButton.SetActive(true);

                break;
            case HubState.TradeHub:
                //ui
                UIEnablerManager.Instance.SwitchElements("TradeObserve", "HeroHub", true);

                //buttons
                tradeFocusedButton.SetActive(false);
                tradeButton.SetActive(true);

                break;
            default:
                break;
        }

        //always do
        hubButton.SetActive(false);
        hubFocusedButton.SetActive(true);
    }


    
    //focused
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedFocusedDungeon()
    {
        //wanna cancel?
        
        //cancel tween and coroutine
        StopAllCoroutines();
        LeanTween.cancelAll();
        
        
        //do pop up

    }

    private void ClickedFocusedTrade()
    {
        //wanna cancel?

        //cancel tween and coroutine
        StopAllCoroutines();
        LeanTween.cancelAll();
        

        //do pop up

    }





    //
    //
    //
    //maybe in different script?
    //animation progress
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void AnimateTradeProgress( float start, float target)
    {
        float tweenTime = target - start;
        float progressStart = start / target;

        StartCoroutine(WaitForTradeFinish(tweenTime));

        LeanTween.value(tradeProgressBar.gameObject, progressStart, 1f, tweenTime)
            .setOnUpdate((value) =>
            {
                tradeProgressBar.fillAmount = value;
                setTradeText(value,tradeProgressTime);
            });
    }

    private void setTradeText(float value, TextMeshProUGUI textMesh)
    {
        int recalcValue = (int)(value * maxTime);

        textMesh.text = $"{recalcValue} sec";
    }




    private void AnimateDungeonProgress(float start,float target)
    {
        float tweenTime = target - start;
        float progressStart = start / target;

        StartCoroutine(WaitForDungeonFinish(tweenTime));

        LeanTween.value(tradeProgressBar.gameObject, progressStart, 1f, tweenTime)
            .setOnUpdate((value) =>
            {
                dungeonProgressBar.fillAmount = value;
                setDungeonText(value, dungeonProgressTime);
            });
    }

    private void setDungeonText(float value, TextMeshProUGUI textMesh)
    {
        int recalcValue = (int)(value * 100f);

        textMesh.text = $"{recalcValue} %";
    }


    //helper coroutines
    IEnumerator WaitForTradeFinish(float time)
    {
        yield return new WaitForSeconds(time + bufferTime);

        tradeFocusedButton.SetActive(false);
        tradeReadyButton.SetActive(true);
    }

    IEnumerator WaitForDungeonFinish(float time)
    {
        yield return new WaitForSeconds(time + bufferTime);

        dungeonFocusedButton.SetActive(false);
        dungeonReadyButton.SetActive(true);
    }

    //do button animations
}

