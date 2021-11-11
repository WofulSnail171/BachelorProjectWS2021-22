using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSwipeActions : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject matchButton;
    [SerializeField] GameObject unmatchButton;
    [SerializeField] GameObject detailButton;

    //swipeslot focused
    int swipeIndex;

    [SerializeField]SwipeInventory swipeInventory;

    //hero list of trades

    #endregion


    //all heroes unfocused
    private void ClickedCancel()
    {
        UIEnablerManager.Instance.SwitchElements( "TradeSwipe", "TradeSelect", true);
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
        matchButton.SetActive(false);
        unmatchButton.SetActive(true);

        //visuals
        swipeInventory.swipeSlots[swipeIndex].matchHero();

        //
        //actual logic
        //
        //
    }

    private void ClickedUnmatch()
    {
        matchButton.SetActive(true);
        unmatchButton.SetActive(false);

        //visuals
        swipeInventory.swipeSlots[swipeIndex].unmatchHero();

        //
        //actual logic
        //
        //
    }


    private void ClickedDetail()
    {
        //do pop up
        //
        //
    }


    //focus hero
    private void FocuseHero(int index)
    {
        //assign hero
        swipeIndex = index;

        //set focused
        cancelButton.SetActive(false);

        detailButton.SetActive(true);

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
        swipeIndex = -1;

        //set unfocused
        cancelButton.SetActive(true);

        //does not matter
        detailButton.SetActive(false);
        matchButton.SetActive(false);
        unmatchButton.SetActive(false);
    }
}
