using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCancelActions : MonoBehaviour
{
    [SerializeField] Button yes;
    [SerializeField] Button no;

    [SerializeField] HubButtonActions hub;
    [SerializeField] ExploreInventoryUI ExploreInventoryUI;
    [SerializeField] InventoryUI inventory;


    private void Start()
    {
       yes.GetComponent<Button>().onClick.AddListener(() => { ClickedYes(); });
       no.GetComponent<Button>().onClick.AddListener(() => { ClickedNo(); });
    }

    private void ClickedYes()
    {
        //do cancel


        ExploreInventoryUI.RemoveAllHeroesFromExplore();
        inventory.UpdateInventory();

        hub.UpdateHubState(HubState.HeroHub);

        UIEnablerManager.Instance.DisableElement("DungeonCancel", true);
        UIEnablerManager.Instance.SwitchElements("DungeonObserve", "HeroHub", false);
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", false);
    }


    private void ClickedNo()
    {
        UIEnablerManager.Instance.DisableElement("DungeonCancel", true);
    }
}
