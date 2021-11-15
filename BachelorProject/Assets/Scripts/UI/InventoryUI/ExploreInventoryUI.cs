using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreInventoryUI : MonoBehaviour
{
    #region vars
    [HideInInspector] public HeroSlot[] heroSlots;
    [HideInInspector] public InventoryUI inventory;

    [SerializeField] GameObject slotParent;
    [SerializeField] GameObject completeParent;

    private ExploreSlot[] exploreSlots;
    private HeroSlot draggedSlot;
    #endregion


    //action funcs
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    private void BeginDrag(HeroSlot heroSlot)
    {
        draggedSlot = heroSlot;
    }

    private void Drop(ExploreSlot exploreSlot)
    {
        if (draggedSlot != null && draggedSlot.playerHero != null && draggedSlot.playerHero.status == HeroStatus.Idle)
        {
            //assign to empty
            if (exploreSlot.playerHero == null)
            {
                //assign
                PlayerHero temphero = draggedSlot.playerHero;
                int tempID = draggedSlot.slotID;

                AssignHeroToSlot(temphero, exploreSlot.slotID,tempID);


                //update original
                draggedSlot.changeStatus(HeroStatus.Exploring);
                draggedSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity, -1, exploreSlot.slotID);

                return;
            }

            //switcheroo
            if (draggedSlot.slotID != exploreSlot.originalSlotReferenceID)
            {
                PlayerHero temphero = draggedSlot.playerHero;
                int tempID = draggedSlot.slotID;
                int originalID = exploreSlot.originalSlotReferenceID;

                AssignHeroToSlot(temphero, exploreSlot.slotID, tempID);


                //update original
                draggedSlot.changeStatus(HeroStatus.Exploring);
                draggedSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity, -1, exploreSlot.slotID);

                heroSlots[originalID].changeStatus(HeroStatus.Idle);
            }

        }
    }

    private void RemoveHeroFromSlot(ExploreSlot exploreSlot)
    {
        if (exploreSlot.originalSlotReferenceID >= 0)
        {
            heroSlots[exploreSlot.originalSlotReferenceID].exploreReferenceID = -1;
            heroSlots[exploreSlot.originalSlotReferenceID].changeStatus(HeroStatus.Idle);
        }
    }



    //public funcs
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    //init
    public void ResetExplore()
    {
        foreach (ExploreSlot slot in exploreSlots)
        {
            slot.removeHero();
            slot.hideHero();
        }
    }

    public void InitExploreHub()
    {
        exploreSlots = slotParent.GetComponentsInChildren<ExploreSlot>();

        //set up events:
        for (int i = 0; i < exploreSlots.Length; i++)
        {
            exploreSlots[i].OnDropEvent += Drop;
            exploreSlots[i].OnRemoveEvent += RemoveHeroFromSlot;

            exploreSlots[i].slotID = i;
            exploreSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().heroReference = i;
            exploreSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
        }

        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].OnBeginDragEvent += BeginDrag;

            heroSlots[i].slotID = i;
        }
    }


    //inventory assignment
    public void UpdateReference(int referenceID, int ID)
    {
        if(ID >= 0 && ID < exploreSlots.Length)
            exploreSlots[ID].originalSlotReferenceID = referenceID;
    }//helper

    public void AssignHeroToSlot(PlayerHero hero, int ID, int referenceID)
    {
        exploreSlots[ID].updateHero(hero, inventory.CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity, referenceID);
        exploreSlots[ID].showHero();
    }



    //button events
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    public void RemoveAllHeroesFromExplore()
    {
        foreach (HeroSlot slot in heroSlots)
        {
            if(slot != null && slot.playerHero != null && slot.playerHero.status == HeroStatus.Exploring)
                slot.changeStatus(HeroStatus.Idle);
        }

        foreach (ExploreSlot slot in exploreSlots)
        {
            slot.hideHero();
            slot.removeHero();

            RemoveHeroFromSlot(slot);
        }
    }

    public bool ConfirmAllHeroesForExplore()
    {
        bool anySlotIsFull = false;

        foreach (ExploreSlot slot in exploreSlots)
        {
            if (slot.playerHero != null)
            {
                anySlotIsFull = true;
                break;
            }

            Debug.Log("there are no heroes selected");
        }

        if (anySlotIsFull)
        {
            Debug.Log("do transfer logic");
            //transmit the exploredata here and update the databas
            //
            //

            return true;
        }

        return false;
    }


    private void DoubleClick(int index)
    {
        RemoveHeroFromSlot(exploreSlots[index]);
        exploreSlots[index].removeHero();
        exploreSlots[index].hideHero();
    }
}
