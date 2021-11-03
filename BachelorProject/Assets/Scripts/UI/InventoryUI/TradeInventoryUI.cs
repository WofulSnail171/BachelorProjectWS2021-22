using UnityEngine;

public class TradeInventoryUI : MonoBehaviour
{
    #region vars
    [HideInInspector]public HeroSlot[] heroSlots;
    [HideInInspector]public InventoryUI inventory;

    [SerializeField] GameObject slotParent;
    [SerializeField] GameObject completeParent;

    private TradeSlot[] tradeSlots;
    private HeroSlot draggedSlot;
    #endregion

    //action funcs
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    private void BeginDrag(HeroSlot heroSlot)
    {
        draggedSlot = heroSlot;
    }

    private void Drop(TradeSlot tradeSlot)
    {
        if (draggedSlot != null && draggedSlot.playerHero != null && draggedSlot.playerHero.status == HeroStatus.Idle)
        {
            //assign to empty
            if (tradeSlot.playerHero == null)
            {
                //assign
                PlayerHero temphero = draggedSlot.playerHero;
                int tempID = draggedSlot.slotID;

                AssignHeroToSlot(temphero, tradeSlot.slotID, tempID);


                //update original
                draggedSlot.changeStatus(HeroStatus.Trading);
                draggedSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity, tradeSlot.slotID, -1);

                return;
            }

            //switcheroo
            if (draggedSlot.slotID != tradeSlot.originalSlotReferenceID)
            {
                //assign
                PlayerHero temphero = draggedSlot.playerHero;
                int tempID = draggedSlot.slotID;
                int originalID = tradeSlot.originalSlotReferenceID;

                AssignHeroToSlot(temphero, tradeSlot.slotID, tempID);


                //update original
                draggedSlot.changeStatus(HeroStatus.Trading);
                draggedSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity, tradeSlot.slotID, -1);

                heroSlots[originalID].changeStatus(HeroStatus.Idle);
            }

        }
    }

    private void RemoveHeroFromSlot(TradeSlot tradeSlot)
    {
        if (tradeSlot.originalSlotReferenceID >= 0)
        {
            heroSlots[tradeSlot.originalSlotReferenceID].tradeReferenceID = -1;
            heroSlots[tradeSlot.originalSlotReferenceID].changeStatus(HeroStatus.Idle);
        }
    }




    //public funcs
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    //init
    public void ResetTrade()
    {
        foreach (TradeSlot slot in tradeSlots)
        {
            slot.removeHero();
            slot.hideHero();
        }
    }

    public void InitTradeHub()
    {
        tradeSlots = slotParent.GetComponentsInChildren<TradeSlot>();

        //set up events:
        for (int i = 0; i < tradeSlots.Length; i++)
        {
            tradeSlots[i].OnDropEvent += Drop;
            tradeSlots[i].OnRemoveEvent += RemoveHeroFromSlot;

            tradeSlots[i].slotID = i;
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
        if (ID >= 0 && ID < tradeSlots.Length)
            tradeSlots[ID].originalSlotReferenceID = referenceID;
    }//helper

    public void AssignHeroToSlot(PlayerHero hero, int ID, int referenceID)
    {
        tradeSlots[ID].updateHero(hero, inventory.CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity, referenceID);
        tradeSlots[ID].showHero();
    }



    //button events
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    public void RemoveAllHeroesFromTrade()
    {
        foreach (HeroSlot slot in heroSlots)
        {
            if (slot != null && slot.playerHero != null && slot.playerHero.status == HeroStatus.Trading)
                slot.changeStatus(HeroStatus.Idle);
        }

        foreach (TradeSlot slot in tradeSlots)
        {
            slot.hideHero();
            slot.removeHero();

            RemoveHeroFromSlot(slot);
        }

        UIEnablerManager.Instance.DisableElement("TradeSelect"); //hide UI
    }

    public void ConfirmAllHeroesForTrade()
    {
        bool anySlotIsFull = false;

        foreach (TradeSlot slot in tradeSlots)
        {
            if (slot.playerHero != null)
            {
                anySlotIsFull = true;
                break;
            }
        }

        if (anySlotIsFull)
        {
            Debug.Log("do transfer logic");

            UIEnablerManager.Instance.DisableElement("TradeSelect"); //hide UI

            //transmit the data here and update the database
            //
            //
        }
    }
}
