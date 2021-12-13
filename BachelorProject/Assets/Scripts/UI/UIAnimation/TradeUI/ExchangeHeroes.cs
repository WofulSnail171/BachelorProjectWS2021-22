using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExchangeHeroes : MonoBehaviour
{
    #region vars
    [HideInInspector] public int currentMatch;
    [HideInInspector] public int totalMatch;
    [HideInInspector] public bool allMatch;
    [HideInInspector] public List<Match> allMatchList;
    [HideInInspector] public List<int[]> allMatchListPotential;


    [SerializeField] GameObject givenCard;
    [SerializeField] GameObject newCard;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI userText;
    [Space]
    [SerializeField] GameObject nextExchange;
    [SerializeField] GameObject FinishExchange;
    [SerializeField] GameObject FinishTrade;
    [SerializeField] float animSpeed;
    #endregion

    private void OnEnable()
    {
        statusText.text = "";

        FinishExchange.SetActive(false);
        FinishTrade.SetActive(false);
        nextExchange.SetActive(false);

        ResetAndAnimate();
    }

    private void EnableButton()
    {
        currentMatch++;

        if(allMatch && totalMatch >= currentMatch && totalMatch != 0)
        {
            FinishExchange.SetActive(false);
            FinishTrade.SetActive(true);
            nextExchange.SetActive(false);
        }

        else if(totalMatch >= currentMatch && totalMatch != 0)
        {
            FinishExchange.SetActive(true);
            FinishTrade.SetActive(false);
            nextExchange.SetActive(false);
        }

        else
        {
            FinishExchange.SetActive(false);
            FinishTrade.SetActive(false);
            nextExchange.SetActive(true);
        }
    }

    private void ResetAndAnimate()
    {
        //init
        givenCard.transform.localScale = new Vector3(1, 1, 1);
        newCard.transform.localScale = new Vector3(0,0,0);

        givenCard.SetActive(true);
        newCard.SetActive(false);


        //update cards according to match
        givenCard.GetComponent<ExchangeCardAnim>().UpdateHero(allMatchList[currentMatch].ownHero);
        newCard.GetComponent<ExchangeCardNew>().UpdateHero(allMatchList[currentMatch].matchedOffer.heroId);

        //update match text
        userText.text = $"Found a match with {allMatchList[currentMatch].matchedOffer.available}.";
        statusText.text = $"You say goodbye to {allMatchList[currentMatch].ownHero.heroId}.";

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(animSpeed);

        //remove old card
        LeanTween.scale(givenCard, new Vector3(0, 0, 0), animSpeed);

        yield return new WaitForSeconds(animSpeed*1.5f);

        //get new card
        statusText.text = $"You welcome {allMatchList[currentMatch].matchedOffer.heroId}.";

        givenCard.SetActive(false);
        newCard.SetActive(true);

        newCard.transform.localScale = new Vector3(0, 0, 0);
        givenCard.transform.localScale = new Vector3(1, 1, 1);

        LeanTween.scale(newCard, new Vector3(1,1,1), animSpeed);

        yield return new WaitForSeconds(animSpeed * 1.5f);


        //check if potential growth

        bool pot = false;

        foreach (int buff in allMatchListPotential[currentMatch])
        { 
            if(buff > 0)
            {
                pot = true;
                break;
            }
        }

        if (pot)
        {
            statusText.text = $"{allMatchList[currentMatch].matchedOffer.heroId} unlocked {allMatchListPotential[currentMatch][0]} physical, {allMatchListPotential[currentMatch][1]} magical and {allMatchListPotential[currentMatch][2]} social potential.";
        }

        else
        {
            statusText.text = "";
        }

        EnableButton();
    }

}
