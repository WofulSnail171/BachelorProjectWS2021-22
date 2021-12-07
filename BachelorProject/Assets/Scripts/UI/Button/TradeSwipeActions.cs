using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeSwipeActions : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject matchButton;
    [SerializeField] GameObject unmatchButton;
    [SerializeField] GameObject cancelFetchDataButton;



    [SerializeField]SwipeInventory swipeInventory;
    [SerializeField]TradeInventoryUI tradeInventory;

    //hero list of trades

    #endregion


    private void Awake()
    {
        cancelButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
        matchButton.GetComponent<Button>().onClick.AddListener(() => { ClickedMatch(); });
        unmatchButton.GetComponent<Button>().onClick.AddListener(() => { ClickedUnmatch(); });
        sendButton.GetComponent<Button>().onClick.AddListener(() => { ClickedSend(); });
        nextButton.GetComponent<Button>().onClick.AddListener(() => { ClickedNext(); });
        cancelFetchDataButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
    }

    private void Start()
    {
        foreach (SwipeSlot swipeSlot in swipeInventory.swipeSlots)
            swipeSlot.OnClickEvent += Click; ;
    }

    private void OnPullTradeOffers()
    {
        UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);

        if (DatabaseManager._instance.tradeData.GetNumberOFOpenOffers() > 1)
        {
            nextButton.SetActive(true);
            sendButton.SetActive(false);
        }

        else
        {
            sendButton.SetActive(true);
            nextButton.SetActive(false);
        }

        matchButton.SetActive(false);
        unmatchButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    private void OnEnable()
    {
        UIEnablerManager.Instance.EnableElement("WaitingForTrade", true);
        //send data
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData);

        //send own data
        List<PlayerHero> playerHeroes = new List<PlayerHero>();
        foreach (TradeSlot slot in tradeInventory.tradeSlots)
        {
            if(slot.playerHero != null)
                playerHeroes.Add(slot.playerHero);
        }

        //push own offer data
        TradeManager._instance.UploadOffer(playerHeroes.ToArray());
        TradeManager._instance.PullTradeOffers(OnPullTradeOffers);
    }

    //all heroes unfocused
    private void ClickedCancel()
    {
        UIEnablerManager.Instance.SwitchElements( "TradeSwipe", "TradeSelect", true);
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);
        UIEnablerManager.Instance.EnableElement("HeroHub", true);

        UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);
    }

    private void ClickedNext()
    {
        //if hero list is last --> change next inactive and last to active
        //
        //


    }

    //final
    private void ClickedSend()
    {
        //go to observe
        UIEnablerManager.Instance.SwitchElements("TradeSwipe", "TradeObserve", true);
        UIEnablerManager.Instance.EnableElement("General", true);

        //update the buttons and start trading time animation
        //
        //
        //


        //send data, fetch data and match
        //
        //

    }


    //hero focused
    private void ClickedMatch()
    {
        if(swipeInventory.swipeIndex != -1)
        {
            matchButton.SetActive(false);
            unmatchButton.SetActive(true);

            //visuals
            swipeInventory.swipeSlots[swipeInventory.swipeIndex].matchHero();

            matchButton.SetActive(false);
            unmatchButton.SetActive(true);



        }
    }

    private void ClickedUnmatch()
    {
        if (swipeInventory.swipeIndex != -1)
        {
            matchButton.SetActive(true);
            unmatchButton.SetActive(false);

            //visuals
            swipeInventory.swipeSlots[swipeInventory.swipeIndex].unmatchHero();


        }
    }

    //focus hero
    private void FocuseHero(int index)
    {
        //assign hero
        swipeInventory.swipeIndex = index;

        //set focused
        cancelButton.SetActive(false);

        //check hero state
        if(swipeInventory.swipeSlots[index].IsMatched)
        {
            matchButton.SetActive(true);
            unmatchButton.SetActive(false);
        }

        else
        {
            matchButton.SetActive(false);
            unmatchButton.SetActive(true);
        }
        
    }


    private void UnfocusHero(int index)
    {
        //unassign hero
        swipeInventory.swipeIndex = -1;

        //set unfocused
        cancelButton.SetActive(true);

        //does not matter
        matchButton.SetActive(false);
        unmatchButton.SetActive(false);
    }

    private void Click(int i)
    {
        //check if already matched

        matchButton.SetActive(true);
    }

}
