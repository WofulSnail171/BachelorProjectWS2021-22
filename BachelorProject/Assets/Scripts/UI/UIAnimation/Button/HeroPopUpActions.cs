using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPopUpActions : MonoBehaviour
{
    [SerializeField] GameObject ReleaseButton;
    [SerializeField] GameObject CloseButton;

    [SerializeField] HubButtonActions hub;

    private void OnEnable()
    {
        if (hub.currentHubFocus == HubState.HeroHub && !hub.isRewarding)
            ReleaseButton.SetActive(true);

        else
            ReleaseButton.SetActive(false);
    }

    private void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(() => { CloseHeroPopUp(); });
    }

    public void CloseHeroPopUp()
    {
        UIEnablerManager.Instance.DisableElement("HeroCard", true);
    }

}
