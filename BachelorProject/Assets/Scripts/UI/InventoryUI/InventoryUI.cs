using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class InventoryUI : MonoBehaviour
{
    #region vars
    [SerializeField] Transform slotParent;
    [SerializeField] HeroSlot[] heroSlots;
    [Space]
    [SerializeField] DragHero draggableHero;
    [SerializeField] private float dragSpeed;
    [Space]
    [SerializeField] Sprite fallbackImage;

    private Dictionary<string, Sprite> defaultImageDict = new Dictionary<string, Sprite>(); //this does not exist yet

    private HeroSlot draggedSlot;
    private int draggedSlotIndex;
    private Vector3 originalPosition;

    bool isDragging = false;
    #endregion



    //Init
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
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

        if (heroSlots[hero.invIndex].isFull)
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

        AssignHeroToSlot(hero, hero.invIndex);
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    private void AssignHeroToSlot(PlayerHero hero, int ID)
    {
        heroSlots[ID].showHero();
        heroSlots[ID].updateHero(hero, CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity);


        //assign ID to the invIndex in Database
        //
        //

    }

    //DragDrop
    private void BeginDrag (HeroSlot heroSlot)
    {
        if(heroSlot.isFull && heroSlot != null && heroSlot.playerHero != null && DatabaseManager._instance !=null && DatabaseManager._instance.defaultHeroData != null && DatabaseManager._instance.defaultHeroData.defaultHeroDictionary != null)
        {
            //where we started
            isDragging = true;

            draggedSlot = heroSlot;
            originalPosition = heroSlot.transform.position;
            draggableHero.gameObject.SetActive(true);


            //set drag represetnation to the item in the slot
            draggableHero.updateDragHero(heroSlot.playerHero, CheckForSprite(heroSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroSlot.playerHero.heroId].rarity);


            //show dragable hero
            draggableHero.transform.position = Input.mousePosition;
            heroSlot.hideHero();
        }
    }

    private void EndDrag(HeroSlot heroSlot)
    {
        draggableHero.gameObject.SetActive(false);
        heroSlot.showHero();
        isDragging = false;
    }

    private void CancelDrag(HeroSlot heroSlot)
    {
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
        StartCoroutine(DoDrop(heroSlot));
    }




    //public funcs
    //probably will be removed later
    public void NewDataAssign()
    {
        InitInventoryUI();
    }


    //helper checks
    private Sprite CheckForSprite(PlayerHero hero)
    {
        if (defaultImageDict.ContainsKey(hero.heroId))
            return defaultImageDict[hero.heroId];

        return fallbackImage;
    }

    //shitty work around
    IEnumerator DoDrop(HeroSlot heroSlot)
    {
        while(isDragging)
        {
            yield return new WaitForFixedUpdate();
        }

        //switch two full fields
        if (heroSlot.isFull && draggedSlot.isFull && heroSlot != draggedSlot)
        {
            PlayerHero temphero = draggedSlot.playerHero;

            AssignHeroToSlot(heroSlot.playerHero, draggedSlot.slotID);
            AssignHeroToSlot(temphero, heroSlot.slotID);
           
        }

        else if (!heroSlot.isFull && draggedSlot.isFull && heroSlot != draggedSlot)
        {

            PlayerHero temphero = draggedSlot.playerHero;

            draggedSlot.removeHero();
            draggedSlot.hideHero();

            AssignHeroToSlot(temphero, heroSlot.slotID);
        }


        else
            Debug.Log("drop not executed");
    }
}
