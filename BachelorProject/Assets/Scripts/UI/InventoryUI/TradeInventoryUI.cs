using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeInventoryUI : MonoBehaviour
{
    [HideInInspector]public HeroSlot[] heroSlots;
    [HideInInspector]public InventoryUI inventory;

    [SerializeField] GameObject slotParent;

    private TradeSlot[] tradeSlots;
    private HeroSlot draggedSlot;


    public void InitTradeHub()
    {
        tradeSlots = slotParent.GetComponentsInChildren<TradeSlot>();

        //set up events:
        for (int i = 0; i < tradeSlots.Length; i++)
        {
            tradeSlots[i].OnDropEvent += Drop;

            tradeSlots[i].slotID = i;
        }

        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].OnBeginDragEvent += BeginDrag;

            heroSlots[i].slotID = i;
        }
    }


    private void BeginDrag(HeroSlot heroSlot)
    {
        draggedSlot = heroSlot;
    }

    private void Drop(TradeSlot tradeSlot)
    {
        if(draggedSlot != null && draggedSlot.playerHero != null && draggedSlot.isFull && draggedSlot.playerHero.status == HeroStatus.Idle)
        {
            Debug.Log("dotrade");


            if (!tradeSlot.isFull)
            {
                Debug.Log("dotrade empty");

                heroSlots[draggedSlot.slotID].changeStatus(HeroStatus.Trading);
                tradeSlot.originalSlotReferenceID = draggedSlot.slotID;

                tradeSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero),DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity);// rarity is the ID
                tradeSlot.showHero();
                return;
            }

            if (draggedSlot.slotID != tradeSlot.originalSlotReferenceID)
            {
                Debug.Log("dotrade full");

                heroSlots[tradeSlot.originalSlotReferenceID].changeStatus(HeroStatus.Idle);

                heroSlots[draggedSlot.slotID].changeStatus(HeroStatus.Trading);
                tradeSlot.originalSlotReferenceID = draggedSlot.slotID;

                tradeSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity);
                tradeSlot.showHero();
            }

        }
    }


    //public funcs
    public void ResetTrade()
    {
        foreach (TradeSlot slot in tradeSlots)
        {
            slot.removeHero();
            slot.hideHero();
        }
    }

    public void AssignHeroToSlot(PlayerHero hero, int ID)
    {
        Debug.Log("assigning " + ID);


        tradeSlots[ID].originalSlotReferenceID = hero.invIndex;
        tradeSlots[ID].updateHero(hero, inventory.CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity);
        tradeSlots[ID].showHero();


        //update status here
        //
        //

    }
}
