using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradSelectActions : MonoBehaviour
{
    #region vars
    [SerializeField] TradeInventoryUI tradeInventory;

    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject confirmButton;

    [SerializeField] HubButtonActions hub;

    #endregion

    private void Start()
    {
        cancelButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
        confirmButton.GetComponent<Button>().onClick.AddListener(() => { ClickedConfirm(); });
    }

    private void ClickedCancel()
    {
        //revert
        UIEnablerManager.Instance.SwitchElements( "TradeSelect", "General", true);


        //do actual logic
        tradeInventory.RemoveAllHeroesFromTrade();

        hub.UpdateHubState(HubState.HeroHub);
        hub.UpdateTradeButton(ButtonState.Unfocused);
        hub.UpdateHubButton(ButtonState.Focused);
    }

    private void ClickedConfirm()
    {
        bool confirmed = tradeInventory.ConfirmAllHeroesForTrade();

        if(confirmed)
        {

            //go to swipe
            UIEnablerManager.Instance.SwitchElements("TradeSelect","TradeSwipe",true);
            UIEnablerManager.Instance.DisableElement("HeroHub", true);
            UIEnablerManager.Instance.DisableElement("ShardsAndBuff", true);

        }


    }
}
