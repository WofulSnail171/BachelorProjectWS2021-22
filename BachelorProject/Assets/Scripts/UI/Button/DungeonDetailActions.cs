using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonDetailActions : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject confirmButton;
    #endregion

    private void Start()
    {
        cancelButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
        confirmButton.GetComponent<Button>().onClick.AddListener(() => { ClickedConfirm(); });
    }


    private void ClickedCancel()
    {
        //go back
        UIEnablerManager.Instance.SwitchElements( "DungeonDetailSelect", "DungeonMapSelect", true);

        //enable background
        UIEnablerManager.Instance.EnableCanvas();
    }

    private void ClickedConfirm()
    {
        //go to hero selection
        //save the selected map in data
        UIEnablerManager.Instance.SwitchElements("DungeonDetailSelect", "DungeonHeroSelect", true);
        UIEnablerManager.Instance.EnableElement("HeroHub", true);
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);

        //enable background
        UIEnablerManager.Instance.EnableCanvas();

    }
}
