using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubButtonActions : MonoBehaviour
{
    [SerializeField] GameObject tradeButton;
    [SerializeField] GameObject dungeonButton;

    private void OnEnable()
    {
        tradeButton.GetComponent<Button>().interactable = true;
        dungeonButton.GetComponent<Button>().interactable = true;
    }

    private void Start()
    {
        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
    }

    private void ClickedTrade()
    {
        //check what state the trade is in
        

        tradeButton.GetComponent<Button>().interactable = false;
        dungeonButton.GetComponent<Button>().interactable = false;

        UIEnablerManager.Instance.SwitchElements("General", "TradeSelect", true);
    }

    private void ClickedDungeon()
    {
        //UIEnablerManager.Instance.DisableElement("DungeonSelect",false);

        tradeButton.GetComponent<Button>().interactable = false;
        dungeonButton.GetComponent<Button>().interactable = false;

        UIEnablerManager.Instance.SwitchElements("HeroHub", "DungeonSelect", true);
    }
}
