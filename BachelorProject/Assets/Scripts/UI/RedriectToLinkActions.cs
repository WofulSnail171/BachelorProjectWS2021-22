using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedriectToLinkActions : MonoBehaviour
{
    [SerializeField] GameObject LinkButton;
    [SerializeField] GameObject InfoButton;
    [SerializeField] GameObject LaterButton;
    [SerializeField] GameObject ThanksButton;
    [SerializeField] string URL;

    private void Start()
    {
        LinkButton.GetComponent<Button>().onClick.AddListener(() => { OpenLink(); });
        LaterButton.GetComponent<Button>().onClick.AddListener(() => { Close(); });
        ThanksButton.GetComponent<Button>().onClick.AddListener(() => { Close(); });
    }

    private void OnEnable()
    {
        ThanksButton.SetActive(false);
        LaterButton.SetActive(true);
    }

    private void OpenLink()
    {
        Application.OpenURL(URL);

        InfoButton.SetActive(false);
        LaterButton.SetActive(false);
        ThanksButton.SetActive(true);
    }

    private void Close()
    {
        UIEnablerManager.Instance.DisableElement("LinkInfo", true);
    }
}
