using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPullUI : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject ReleaseButton;
    [SerializeField] GameObject DiscardButton;
    [SerializeField] GameObject ContinueButton;

    [SerializeField] GameObject ReleaseText;
    

    [SerializeField] InventoryUI inventory;
    [SerializeField] float animSpeed;

    //for update


    #endregion

    private void OnEnable()
    {
        UpdatePulledHero();


        if (DatabaseManager._instance.activePlayerData.inventory.Count <= inventory.heroSlots.Length)
        {
            UIEnablerManager.Instance.EnableElement("HeroPullRelease", false);

            ReleaseText.SetActive(true);
            
            ReleaseText.transform.localScale = new Vector3(1, 0, 1);

            LeanTween.scaleY(ReleaseText, 1, animSpeed).setEaseOutBounce();

        }

        else
        {
            ReleaseText.SetActive(false);
            UIEnablerManager.Instance.EnableElement("HeroPullContinue", false);
        }
    }

    private void UpdatePulledHero()
    {

    }
}
