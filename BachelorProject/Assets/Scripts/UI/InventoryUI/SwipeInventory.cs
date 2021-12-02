using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInventory : MonoBehaviour
{
    #region vars
    [SerializeField] TradeInventoryUI tradeInventory;
    [SerializeField] GameObject slotParent;
    [HideInInspector] public SwipeSlot[] swipeSlots;

    private PlayerHero matchHero;

    #endregion


    private void Awake()
    {
        swipeSlots = slotParent.GetComponentsInChildren<SwipeSlot>();
    }

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
                swipeSlots[slot.slotID].showHero();
            }
        }
    }

    private void ResetSwipeHeroes()
    {
         foreach (SwipeSlot slot in swipeSlots)
        {
            slot.hideHero();
            slot.unmatchHero();
        }    
    }


    //drag


    //highlight
    private void Click(int index)
    {
        foreach (SwipeSlot heroSlot in swipeSlots)
        {
            if (heroSlot.playerHero != null)
                heroSlot.disableHighlight();
        }

        swipeSlots[index].enableHighlight();
        matchHero = swipeSlots[index].playerHero;

    
    }
}
