using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HeroSlot : MonoBehaviour ,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    #region vars
    //basics
    [SerializeField] private bool isFull = false;
    [SerializeField] private GameObject heroCard;

    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject [] rarityGraphics;
    [SerializeField] TMP_Text heroName;
    [SerializeField] Image portrait;

    [SerializeField] GameObject TradeStatusUI;
    [SerializeField] GameObject DungeonStatusUI;
 
    //drag and drop
    [SerializeField] private float speed;
    [SerializeField] private GameObject scroll;

    Vector3 originalPosition;
    bool isDragging = false;
    #endregion



    //base funcs
    //------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        if (isFull)
        {
            heroCard.SetActive(true);
        }
        else
        {
            heroCard.SetActive(false);
        }
    }

    public bool showHero()
    {
        if (isFull)
        {
            return false; 
        }

        isFull = true;
        heroCard.SetActive(true);
        return true;
    }

    public void hideHero()
    {
        isFull = false;
        heroCard.SetActive(false);
    }


    public void updateHero(int rarity, string name, Sprite sprite,HeroStatus status)
    {
        portrait.sprite = sprite;
        heroName.text = name;

        int spacing = -130;

        foreach (GameObject gameObject in rarityGraphics)
        {
            gameObject.SetActive(false);
        }

        for(int i = 0; i < rarity; i ++)
        {
            spacing += 20;
            rarityGraphics[i].SetActive(true);
        }

        rarityGroup.GetComponent<HorizontalLayoutGroup>().spacing = spacing;


        switch(status)
        {
            case HeroStatus.Trading:
                TradeStatusUI.SetActive(true);
                DungeonStatusUI.SetActive(false);
                break;
            case HeroStatus.Exploring:
                TradeStatusUI.SetActive(false);
                DungeonStatusUI.SetActive(true);
                break;
            default:
                TradeStatusUI.SetActive(false);
                DungeonStatusUI.SetActive(false);
                break;
        }

    }





    //drag funcs
    //-------------------------------------------------------------------------------------------------------------------------------------------
    public void OnDrag(PointerEventData pointerEventData)
    {
        if (Input.touchCount == 1 && isDragging)
        {
            heroCard.transform.position = Input.mousePosition;
            heroCard.transform.position = Vector3.MoveTowards(heroCard.transform.position, Input.mousePosition, speed * Time.deltaTime);
        }

        else if (isDragging)
        {
            StartCoroutine(ResetDrag());
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (Input.touchCount == 1)
        {
            scroll.GetComponent<ScrollRect>().vertical = false;
            originalPosition = heroCard.transform.position;
            isDragging = true;
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (isDragging)
        { StartCoroutine(ResetDrag()); }
    }


    //coroutine
    IEnumerator ResetDrag()
    {
        heroCard.transform.position = originalPosition;
        yield return new WaitForSeconds(0.1f);
        isDragging = false; 
        scroll.GetComponent<ScrollRect>().vertical = true;
    }

}
