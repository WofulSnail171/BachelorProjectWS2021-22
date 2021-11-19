using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrowthCard : MonoBehaviour
{
    #region vars

    [SerializeField] TMP_Text Name;
    [Space]
    [SerializeField] TMP_Text physicalStatText;
    [SerializeField] TMP_Text magicalStatText;
    [SerializeField] TMP_Text socialStatText;
    [Space]
    [SerializeField] TMP_Text physicalGrowthText;
    [SerializeField] TMP_Text magicalGrowthText;
    [SerializeField] TMP_Text socialGrowthText;
    [Space]
    [SerializeField] Image physicalStatBar;
    [SerializeField] Image magicalStatBar;
    [SerializeField] Image socialStatBar;
    [Space]
    [SerializeField] Image physicalPotentialStatBar;
    [SerializeField] Image magicalPotentialStatBar;
    [SerializeField] Image socialPotentialStatBar;
    [Space]
    [SerializeField] GameObject[] Rarity;


    private DefaultHero defaultHero;

    private string physicalPotential;
    private string magicalPotential;
    private string socialPotential;


    private float max = 999;

    #endregion

    public void UpdateGrowth(int physical, int magical, int social)
    {
        if (physical < 0)
            physicalGrowthText.text = $"-{physical}";

        else
            physicalGrowthText.text = $"+{physical}";



        if (magical < 0)
            magicalGrowthText.text = $"-{magical}";

        else
            magicalGrowthText.text = $"+{magical}";



        if (social < 0)
            socialGrowthText.text = $"-{social}";

        else
            socialGrowthText.text = $"+{social}";
    }

    public void UpdateHero(PlayerHero hero)
    {
        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];

        Name.text = hero.heroId;



        //stats
        //check if potential is maxed
        
        CheckPotential(hero);

        //text
        physicalStatText.text = $"{hero.pVal} / {hero.pPot} ({physicalPotential})";
        magicalStatText.text = $"{hero.mVal} / {hero.mPot} ({magicalPotential})";
        socialStatText.text = $"{hero.sVal} / {hero.sPot} ({socialPotential})";

        //set actual stat



        physicalStatBar.fillAmount = hero.pVal / max;
        magicalStatBar.fillAmount = hero.mVal / max;
        socialStatBar.fillAmount = hero.sVal / max;



        //set rarity
        foreach(GameObject rarityStar in Rarity)
        {
            rarityStar.SetActive(false);
        }

        for (int i = 0; i <= defaultHero.rarity - 1; i++)
        {
            Rarity[i].SetActive(true);
        }
    }

    private void CheckPotential(PlayerHero hero)
    {
        //physical
        if (hero.pPot >= defaultHero.pMaxPot)
        {
            hero.pPot = defaultHero.pMaxPot;
            physicalPotential = "max";

            //bar
            physicalPotentialStatBar.fillAmount = 1 - defaultHero.pMaxPot / max;
        }

        else
        {
            physicalPotential = hero.pPot.ToString();

            //bar
            physicalPotentialStatBar.fillAmount = 1 - hero.pPot / max;

        }


        //magical
        if (hero.mPot >= defaultHero.mMaxPot)
        {
            hero.mPot = defaultHero.mMaxPot;
            magicalPotential = "max";

            //bar
            magicalPotentialStatBar.fillAmount = 1 - defaultHero.mMaxPot / max;
        }

        else
        {
            magicalPotential = hero.mPot.ToString();

            //bar
            magicalPotentialStatBar.fillAmount = 1 - hero.mPot / max;
        }


        //social
        if (hero.sPot >= defaultHero.sMaxPot)
        {
            hero.sPot = defaultHero.sMaxPot;
            socialPotential = "max";

            //bar
            socialPotentialStatBar.fillAmount = 1 - defaultHero.sMaxPot / max;
        }

        else
        {
            socialPotential = hero.sPot.ToString();

            //bar
            socialPotentialStatBar.fillAmount = 1 - hero.sPot / max;
        }
    }


}
