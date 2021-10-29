using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class TradeSlot : MonoBehaviour, IDropHandler
{
    [HideInInspector] public bool isFull = false;
    [HideInInspector] public PlayerHero playerHero;

    [HideInInspector] public int slotID;
    [HideInInspector] public int originalSlotReferenceID;


    [SerializeField] private GameObject heroCard;
    [SerializeField] private GameObject disabledCard;

    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject[] rarityGraphics;
    [SerializeField] TMP_Text heroName;
    [SerializeField] Image portrait;

    //events
    public event Action <TradeSlot> OnDropEvent;


    public void updateHero(PlayerHero hero, Sprite sprite, int rarity)
    {
        playerHero = hero;
        isFull = true;

        portrait.sprite = sprite;
        heroName.text = hero.heroId;

        int spacing = -130;

        foreach (GameObject gameObject in rarityGraphics)
        {
            gameObject.SetActive(false);
        }

        for (int i = 0; i < rarity; i++)
        {
            spacing += 20;
            rarityGraphics[i].SetActive(true);
        }

        rarityGroup.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
    }

    public void showHero()
    {
        disabledCard.SetActive(false);
        heroCard.SetActive(true);
    }

    //public funcs
    public void hideHero()
    {
        heroCard.SetActive(false);
        disabledCard.SetActive(true);
    }

    public void removeHero()
    {
        isFull = false;
        playerHero = null;
    }


    public void OnDrop(PointerEventData pointerEventData)
    {
        if (pointerEventData != null)
        {
            OnDropEvent(this);
        }
    }
}
