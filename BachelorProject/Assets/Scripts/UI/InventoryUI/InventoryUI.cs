using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class InventoryUI : MonoBehaviour
{
    #region vars
    [SerializeField] TradeInventoryUI tradeInventory;
    [SerializeField] ExploreInventoryUI exploreInventory;
    [SerializeField] GameObject slotParent;
    [HideInInspector] HeroSlot[] heroSlots;

    [Space]
    [SerializeField] DragHero draggableHero;
    [Space]
    [SerializeField] Sprite fallbackImage;

    private Dictionary<string, Sprite> defaultImageDict = new Dictionary<string, Sprite>(); //this does not exist yet

    private HeroSlot draggedSlot;
    private Vector3 originalPosition;

    private bool isDragging = false;

    private ScrollRect scroll;

    private int amountInTrade = 0;
    private int amountInDungeon = 0;
    #endregion



    //Init
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        heroSlots = slotParent.GetComponentsInChildren<HeroSlot>();
        //init tred inventory
        tradeInventory.inventory = this;
        tradeInventory.heroSlots = heroSlots;

        tradeInventory.InitTradeHub();
        tradeInventory.ResetTrade();

        //init explore inventory
        exploreInventory.inventory = this;
        exploreInventory.heroSlots = heroSlots;

        exploreInventory.InitExploreHub();
        exploreInventory.ResetExplore();

        //get scroll
        scroll = GetComponent<ScrollRect>();

        //set up events:
        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].OnBeginDragEvent += BeginDrag;
            heroSlots[i].OnCancelDragEvent += CancelDrag;
            heroSlots[i].OnDragEvent += Drag;
            heroSlots[i].OnEndDragEvent += EndDrag;
            heroSlots[i].OnDropEvent += Drop;

            heroSlots[i].slotID = i;
        }
    }

    private void Start()
    {
        ServerCommunicationManager._instance.GetInfo(Request.DownloadHeroList, JsonUtility.ToJson(new LoginInfo { playerId = "Sarah", password = "123" }));
    }

    private void InitInventoryUI()
    {
        if (DatabaseManager._instance != null && DatabaseManager._instance.activePlayerData != null && DatabaseManager._instance.activePlayerData.inventory != null && heroSlots != null)
        {     
            //reset
            for (int i = 0; i < heroSlots.Length; i++ )
            {
                heroSlots[i].removeHero();
                heroSlots[i].hideHero();
            }

            //assign
            foreach(PlayerHero hero in DatabaseManager._instance.activePlayerData.inventory)
            {
                InitAssignHeroToSlot(hero);
            }
        }
    }

    private void InitAssignHeroToSlot(PlayerHero hero)
    {
        //catch exeptions
        if(hero.invIndex > heroSlots.Length)
        {
            Debug.Log("trying to show hero which is in slot that is bigger than the inventory size");
            return;
        }

        if (heroSlots[hero.invIndex].playerHero != null)
        {
            Debug.Log("trying to show hero which is already shown or assign more heroes to one slot");
            return;
        }

        if (!DatabaseManager._instance.defaultHeroData.defaultHeroDictionary.ContainsKey(hero.heroId))
        {
            heroSlots[hero.invIndex].removeHero();
            heroSlots[hero.invIndex].hideHero();
            Debug.Log("trying to show hero that does not exist");
            return;
        }

        AssignHeroToSlot(hero, hero.invIndex, -1,-1);//default is -1

        
        if(hero.status == HeroStatus.Trading)
        {
            amountInTrade += 1;

            if (amountInTrade > 4)
            {
                Debug.Log("trying to trade too many heroes");
                return;
            }

            tradeInventory.AssignHeroToSlot(hero, amountInTrade - 1, hero.invIndex);
            heroSlots[hero.invIndex].tradeReferenceID = amountInTrade-1;

            exploreInventory.UpdateReference(hero.invIndex, -1);
            tradeInventory.UpdateReference(hero.invIndex, amountInTrade - 1);
        }

        if (hero.status == HeroStatus.Exploring)
        {
            amountInDungeon += 1;

            if (amountInDungeon > 4)
            {
                Debug.Log("trying to explore too many heroes");
                return;
            }

            exploreInventory.AssignHeroToSlot(hero, amountInDungeon - 1,hero.invIndex);
            heroSlots[hero.invIndex].exploreReferenceID = amountInDungeon - 1;

            tradeInventory.UpdateReference(hero.invIndex, -1);
            exploreInventory.UpdateReference(hero.invIndex, amountInDungeon - 1);
        }
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    private void AssignHeroToSlot(PlayerHero hero, int ID, int tradeID, int exploreID)
    {
        heroSlots[ID].showHero();
        heroSlots[ID].updateHero(hero, CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity,tradeID,exploreID);

        Debug.Log("trade ref: " + tradeID);
        Debug.Log("explore ref: " + exploreID);

        if(heroSlots[ID].playerHero.status == HeroStatus.Trading)
        {
            tradeInventory.UpdateReference(ID, heroSlots[ID].tradeReferenceID);
        }

        if(heroSlots[ID].playerHero.status == HeroStatus.Exploring)
        {
            exploreInventory.UpdateReference( ID, heroSlots[ID].exploreReferenceID);
        }
    }

    //DragDrop
    private void BeginDrag (HeroSlot heroSlot)
    {
        if(heroSlot != null && heroSlot.playerHero != null && DatabaseManager._instance !=null && DatabaseManager._instance.defaultHeroData != null && DatabaseManager._instance.defaultHeroData.defaultHeroDictionary != null)
        {
            //deactivate scrolling
            scroll.vertical = false;

            //where we started
            isDragging = true;

            draggedSlot = heroSlot;

            Debug.Log(heroSlot.tradeReferenceID);
            Debug.Log(heroSlot.exploreReferenceID);

            draggableHero.gameObject.SetActive(true);


            //set drag represetnation to the item in the slot
            draggableHero.updateDragHero(heroSlot.playerHero, CheckForSprite(heroSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroSlot.playerHero.heroId].rarity);


            //show dragable hero
            draggableHero.transform.position = Input.mousePosition;
            heroSlot.hideHero();

            Debug.Log(draggedSlot.tradeReferenceID);
            Debug.Log(draggedSlot.exploreReferenceID);
        }
    }

    private void EndDrag(HeroSlot heroSlot)
    {
        //activate scrolling
        scroll.vertical = true;


        draggableHero.gameObject.SetActive(false);
        heroSlot.showHero();
        isDragging = false;
    }

    private void CancelDrag(HeroSlot heroSlot)
    {
        //activate scrolling
        scroll.vertical = true;

        Debug.Log("cancel not assigned yet");
        heroSlot.showHero();
        isDragging = false;
    }

    private void Drag(HeroSlot heroSlot)
    {
        draggableHero.transform.position = Input.mousePosition;
    }

    private void Drop(HeroSlot heroSlot)
    {
        if (draggedSlot != null && draggedSlot.playerHero != null)
            StartCoroutine(DoDrop(heroSlot));
    }




    //public funcs
    public void NewDataAssign()
    {
        InitInventoryUI();
    }//probably will be outdated


    //helper checks
    public Sprite CheckForSprite(PlayerHero hero)
    {
        if (defaultImageDict.ContainsKey(hero.heroId))
            return defaultImageDict[hero.heroId];

        return fallbackImage;
    }

    //shitty work around coroutine
    IEnumerator DoDrop(HeroSlot heroSlot)
    {
        while(isDragging)
        {
            yield return null;
        }

        //switch two full fields
        if (heroSlot.playerHero != null && heroSlot != draggedSlot)
        {
            PlayerHero temphero = draggedSlot.playerHero;
            int tempTradeID = draggedSlot.tradeReferenceID;
            int tempExploreID = draggedSlot.exploreReferenceID;

            Debug.Log(tempTradeID);

            AssignHeroToSlot(heroSlot.playerHero, draggedSlot.slotID, heroSlot.tradeReferenceID, heroSlot.exploreReferenceID);
            AssignHeroToSlot(temphero, heroSlot.slotID, tempTradeID,tempExploreID);

            //update references
            tradeInventory.UpdateReference(draggedSlot.slotID, tempTradeID);
            tradeInventory.UpdateReference(heroSlot.slotID, heroSlot.tradeReferenceID);

            exploreInventory.UpdateReference(draggedSlot.slotID, tempExploreID);
            exploreInventory.UpdateReference(heroSlot.slotID, heroSlot.exploreReferenceID);
        }

        //assign to empty
        else if (heroSlot.playerHero == null && heroSlot != draggedSlot)
        {
            Debug.Log(draggedSlot.tradeReferenceID);


            PlayerHero temphero = draggedSlot.playerHero;
            int tempTradeID = draggedSlot.tradeReferenceID;
            int tempExploreID = draggedSlot.exploreReferenceID;

            draggedSlot.removeHero();
            draggedSlot.hideHero();

            Debug.Log(tempTradeID);

            AssignHeroToSlot(temphero, heroSlot.slotID,tempTradeID,tempExploreID);

            tradeInventory.UpdateReference(draggedSlot.slotID, -1);
            tradeInventory.UpdateReference(heroSlot.slotID, heroSlot.tradeReferenceID);

            exploreInventory.UpdateReference(draggedSlot.slotID, -1);
            exploreInventory.UpdateReference(heroSlot.slotID, heroSlot.exploreReferenceID);
        }
    }
}