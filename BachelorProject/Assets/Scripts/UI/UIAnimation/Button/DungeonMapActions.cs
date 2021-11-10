using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMapActions : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject detailButton;
    [SerializeField] GameObject mapContainer;

    [SerializeField] ScrollSnapRect scrollSnap;

    private Button[] mapButtons;
    #endregion

    
    
    //init
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        mapButtons = mapContainer.GetComponentsInChildren<Button>();

        cancelButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
        selectButton.GetComponent<Button>().onClick.AddListener(() => { ClickedSelect(); });
    }


    //click checks
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedCancel()
    {
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);
        UIEnablerManager.Instance.EnableElement("General", true);
        UIEnablerManager.Instance.SwitchElements( "DungeonMapSelect", "HeroHub", true);
    }

    private void ClickedSelect()
    {
        //change camera to the generated dungeon
        int i = scrollSnap.GetNearestPage();

        mapButtons[i].onClick.Invoke();
    }
}
