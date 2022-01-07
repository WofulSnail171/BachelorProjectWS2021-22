using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MottomMenuActions : MonoBehaviour
{
    [SerializeField] GameObject OpenButton;
    [SerializeField] GameObject CreditsButton;
    [SerializeField] GameObject HelpButton;

    [SerializeField] string URL;

    private void Start()
    {
        OpenButton.GetComponent<Button>().onClick.AddListener(() => { OpenDex(); });
        CreditsButton.GetComponent<Button>().onClick.AddListener(() => { OpenCredit(); });
        HelpButton.GetComponent<Button>().onClick.AddListener(() => { HelpLink(); });
    }

    private void OpenDex()
    {
        UIEnablerManager.Instance.EnableElement("Dex", true);
    }

    private void OpenCredit()
    {
        UIEnablerManager.Instance.EnableElement("Credits", true);
    }

    private void HelpLink()
    {
        Application.OpenURL(URL);
    }
}
