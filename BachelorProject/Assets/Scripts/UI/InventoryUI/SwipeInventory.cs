using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInventory : MonoBehaviour
{
    #region vars
    [SerializeField] TradeInventoryUI tradeInventory;
    [HideInInspector] public SwipeSlot[] swipeSlots { get; private set; }
    #endregion

    private void OnEnable()
    {
        ResetSwipeHeroes();
        UpdateSwipeHeroes();
    }

    private void UpdateSwipeHeroes()
    {
        foreach (TradeSlot slot in tradeInventory.tradeSlots)
        {
            if (slot.playerHero != null)
            {
                swipeSlots[slot.slotID].updateHero(slot.playerHero, slot.portrait.sprite, slot.slotrarity,slot.originalSlotReferenceID);
                swipeSlots[slot.slotID].slotID = slot.slotID;
            }
        }
    }

    private void ResetSwipeHeroes()
    {
         foreach (SwipeSlot slot in swipeSlots)
        {
            slot.hideHero();
            slot.IsMatched = false;
        }    
    }


    //drag
}
