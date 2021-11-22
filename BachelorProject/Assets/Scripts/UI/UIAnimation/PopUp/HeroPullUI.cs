using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroPullUI : MonoBehaviour
{
    #region vars

    [SerializeField] GameObject ReleaseText;

    [SerializeField] GameObject ReleaseButton;
    [SerializeField] GameObject ContinueButton;
    [SerializeField] GameObject card;
    
    

    [SerializeField] InventoryUI inventory;
    [SerializeField] float animSpeed;
    [Space]
    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] GameObject [] rarity;

    DefaultHero defaultHero;


    [SerializeField]UpdateHeroCard heroCard;
    //for update


    #endregion
    private void Start()
    {
        card.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
    }

    private void OnEnable()
    {
        
        UpdatePulledHero(DungeonManager._instance.rewardHero);


        if (DatabaseManager._instance.activePlayerData.inventory.Count >= inventory.heroSlots.Length)
        {
            ContinueButton.SetActive(false);
            ReleaseButton.SetActive(true);

            ReleaseText.SetActive(true);
            
            ReleaseText.transform.localScale = new Vector3(1, 0, 1);

            LeanTween.scaleY(ReleaseText, 1, animSpeed).setEaseOutBounce();

        }

        else
        {
            ReleaseText.SetActive(false);
            ContinueButton.SetActive(true);
            ReleaseButton.SetActive(false);
        }
    }

    private void UpdatePulledHero(PlayerHero hero)
    {
        //pop up
        heroCard.UpdateHero(hero);

        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];


        //set rarity
        foreach (GameObject rarityStar in rarity)
        {
            rarityStar.SetActive(false);
        }

        for (int i = 0; i <= defaultHero.rarity - 1; i++)
        {
            rarity[i].SetActive(true);
        }

        //texts
        name.text = hero.heroId;


        //image
    }

    private void DoubleClick(int i)
    {
        UIEnablerManager.Instance.EnableElement("HeroCard", true);
    }
}
