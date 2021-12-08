using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeCancelActions : MonoBehaviour
{
    [SerializeField] Button yes;
    [SerializeField] Button no;

    [SerializeField] HubButtonActions hub;
    [SerializeField] TradeInventoryUI tradeInventoryUI;
    [SerializeField] InventoryUI inventory;


    private void Start()
    {
        yes.GetComponent<Button>().onClick.AddListener(() => { ClickedYes(); });
        no.GetComponent<Button>().onClick.AddListener(() => { ClickedNo(); });
    }

    private void ClickedYes()
    {
        //do cancel
        //TradeManager._instance.

        tradeInventoryUI.RemoveAllHeroesFromTrade();
        inventory.UpdateInventory();

        hub.UpdateHubState(HubState.HeroHub);

        UIEnablerManager.Instance.DisableElement("TradeCancel", true);
        UIEnablerManager.Instance.SwitchElements("TradeObserve", "HeroHub", false);
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", false);
    }


    private void ClickedNo()
    {
        UIEnablerManager.Instance.DisableElement("TradeCancel", true);
    }
}
