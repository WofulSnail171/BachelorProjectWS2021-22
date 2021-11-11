using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCancelActions : MonoBehaviour
{
    [SerializeField] Button yes;
    [SerializeField] Button no;

    private void Start()
    {
       yes.GetComponent<Button>().onClick.AddListener(() => { ClickedYes(); });
       no.GetComponent<Button>().onClick.AddListener(() => { ClickedNo(); });
    }

    private void ClickedYes()
    {

    }


    private void ClickedNo()
    {
        UIEnablerManager.Instance.DisableElement("DungeonCancel", true);

    }
}
