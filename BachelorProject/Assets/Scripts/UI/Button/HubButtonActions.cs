using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubButtonActions : MonoBehaviour
{
    [SerializeField] GameObject tradeButton;
    [SerializeField] GameObject dungeonButton;

    private void Start()
    {
        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
    }

    private void ClickedTrade()
    {
        //check what state the trade is in

        Debug.Log("trade");

        UIEnablerManager.Instance.EnableElement("TradeSelect");
    }

    private void ClickedDungeon()
    {
        //check what state the dungeon is in

        Debug.Log("explore");

        UIEnablerManager.Instance.EnableElement("ExploreSelect");
    }
}
