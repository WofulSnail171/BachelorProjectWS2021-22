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
    [SerializeField] Rect PopUp;
    [Space]
    [SerializeField] GameObject nextExchange;
    [SerializeField] GameObject FinishExchange;
    [SerializeField] GameObject FinishTrade;
    [SerializeField] float animSpeed;
    #endregion

    private void OnEnable()
    {
        FinishExchange.SetActive(false);
        FinishTrade.SetActive(false);
        nextExchange.SetActive(false);

        ResetAndAnimate();
    }

    private void EnableButton()
    {
        if(allMatch && totalMatch == currentMatch && totalMatch != 0)
        {
            FinishExchange.SetActive(false);
            FinishTrade.SetActive(true);
            nextExchange.SetActive(false);
        }

        else if(totalMatch == currentMatch && totalMatch != 0)
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
        givenCard.SetActive(true);
        newCard.SetActive(false);

        givenCard.transform.position = new Vector3(0, 0, 0);
        newCard.transform.position = new Vector3(PopUp.width, 0, 0);

        //update cards according to match
        givenCard.GetComponent<ExchangeCardAnim>().UpdateHero(allMatchList[currentMatch].ownHero);
        newCard.GetComponent<ExchangeCardNew>().UpdateHero(allMatchList[currentMatch].matchedOffer.heroId);

        //update match text
        userText.text = $"Found a match with {allMatchList[currentMatch].matchedOffer.available}.";
        userText.text = $"You say goodbye to {allMatchList[currentMatch].ownHero.heroId}.";

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(animSpeed);

        //remove old card
        LeanTween.moveX(givenCard, givenCard.transform.position.x + PopUp.width,animSpeed);

        yield return new WaitForSeconds(animSpeed*1.5f);

        //get new card
        userText.text = $"You welcome {allMatchList[currentMatch].matchedOffer.heroId}.";

        givenCard.transform.position = new Vector3(0, 0, 0);
        givenCard.SetActive(false);
        newCard.SetActive(true);

        LeanTween.moveX(newCard, 0 + PopUp.width, animSpeed);

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
