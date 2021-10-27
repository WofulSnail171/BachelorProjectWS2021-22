using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] Transform slotParent;
    [SerializeField] HeroSlot[] heroSlots;
    [SerializeField] List<PlayerHero> playerHeroes;
    [Space]
    [SerializeField] Sprite fallbackImage;

    private Dictionary<string, Sprite> defaultImageDict = new Dictionary<string, Sprite>(); //this does not exist yet

    private void Awake()
    {
        if(slotParent != null)
        {
            heroSlots = slotParent.GetComponentsInChildren<HeroSlot>();
        }
    }

    private void Start()
    {
        ServerCommunicationManager._instance.GetInfo(Request.DownloadHeroList, JsonUtility.ToJson(new LoginInfo { playerId = "Sarah", password = "123" }));

    }

    private void RefreshHeroes()
    {
        //check if default heroes exist
        if(DatabaseManager._instance != null && DatabaseManager._instance.defaultHeroData != null && DatabaseManager._instance.defaultHeroData.defaultHeroDictionary != null)
        {

            //check if heroes exist
            if (DatabaseManager._instance.activePlayerData != null && DatabaseManager._instance.activePlayerData.inventory != null)
            {
                playerHeroes.Clear();

                foreach (PlayerHero hero in DatabaseManager._instance.activePlayerData.inventory)
                {
                    AddHero(hero);
                }
            }
        }
    }

    private void RefreshInventoryUI()
    {
        if (DatabaseManager._instance != null && DatabaseManager._instance.activePlayerData != null && DatabaseManager._instance.activePlayerData.inventory != null && heroSlots != null)
        {     
            //reset
            for (int i = 0; i < heroSlots.Length; i++ )
            {
                heroSlots[i].hideHero();
            }

            //assign
            foreach(PlayerHero hero in playerHeroes)
            {
                AssignHeroToSlot(hero);
            }
        }
    }

    private void AssignHeroToSlot(PlayerHero hero)
    {
        if(hero.invIndex > heroSlots.Length)
        {
            Debug.Log("trying to show hero which is in slot that is bigger than the inventory size");
            return;
        }

        if (!heroSlots[hero.invIndex].showHero())
        {
            Debug.Log("trying to show hero which is already shown or assign more heroes to one slot");
            return;
        }

        if (!DatabaseManager._instance.defaultHeroData.defaultHeroDictionary.ContainsKey(hero.heroId))
        {
            heroSlots[hero.invIndex].hideHero();
            Debug.Log("trying to show hero that does not exist");
            return;
        }


        if (!defaultImageDict.ContainsKey(hero.heroId))
        {
            heroSlots[hero.invIndex].updateHero(DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity, hero.heroId, fallbackImage,hero.status);
            return;
        }

        heroSlots[hero.invIndex].updateHero(DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity, hero.heroId, defaultImageDict[hero.heroId],hero.status);
    }

    private bool AddHero(PlayerHero hero)
    {
        if (IsFull())
        {
            Debug.Log("inventory is full");
            return false; 
        }

        playerHeroes.Add(hero);
        return true;
    }



    //public funcs
    public bool IsFull ()
    {
        return playerHeroes.Count >= heroSlots.Length;
    }
    

    public void NewDataAssign()
    {

        //pull data or something here
        //
        //

        RefreshHeroes();
        RefreshInventoryUI();
    }
}
