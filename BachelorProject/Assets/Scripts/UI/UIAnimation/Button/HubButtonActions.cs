using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubButtonActions : MonoBehaviour
{

    public enum HubState
    {
        HeroHub,
        TradeHub,
        DungeonHub
    }


    #region vars
    //actual buttons
    [SerializeField] GameObject tradeButton;
    [SerializeField] GameObject tradeFocusedButton;
    [SerializeField] GameObject dungeonButton;
    [SerializeField] GameObject dungeonFocusedButton;
    [SerializeField] GameObject hubButton;
    [SerializeField] GameObject hubFocusedButton;//do nothing, just set active


    //animation buttons
    [SerializeField] GameObject animtradeButton;
    [SerializeField] GameObject animtradeFocusedButton;
    [SerializeField] GameObject animdungeonButton;
    [SerializeField] GameObject animdungeonFocusedButton;
    [SerializeField] GameObject animhubButton;
    [SerializeField] GameObject animhubFocusedButton;


    private HubState currentHubFocus;
    #endregion



    private void OnEnable()
    {
        tradeButton.GetComponent<Button>().interactable = true;
        dungeonButton.GetComponent<Button>().interactable = true;
    }

    private void Start()
    {
        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
        hubButton.GetComponent<Button>().onClick.AddListener(() => { ClickedHub(); });
        tradeFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedTrade(); });
        dungeonFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedDungeon(); });
    }

    private void ClickedTrade()
    {
        //check what state the trade is in

        //empty
        //------------------------------------
        tradeButton.GetComponent<Button>().interactable = false;
        dungeonButton.GetComponent<Button>().interactable = false;

        UIEnablerManager.Instance.SwitchElements("General", "TradeSelect", true);


        //pending
        //------------------------------------


        //pending is done
        //------------------------------------


    }

    private void ClickedDungeon()
    {
        //check what state the dungeon is in

        //empty
        //------------------------------------
        tradeButton.GetComponent<Button>().interactable = false;
        dungeonButton.GetComponent<Button>().interactable = false;

        UIEnablerManager.Instance.SwitchElements("HeroHub", "DungeonSelect", true);


        //pending
        //------------------------------------



        //pending is done
        //------------------------------------


    }

    private void ClickedHub()
    {
        //check where you are coming from
        switch(currentHubFocus)
        {
            case HubState.DungeonHub:
                break;
            case HubState.TradeHub:
                break;
            default:
                break;
        }
    }


    //focused
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedFocusedDungeon()
    {
        //wanna cancel
    }

    private void ClickedFocusedTrade()
    {
        //wanna cancel
    }

}
