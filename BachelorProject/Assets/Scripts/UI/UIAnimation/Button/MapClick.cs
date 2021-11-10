using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapClick : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => { MapClicked(); });
    }

    private void MapClicked()
    {
        //go to the map instance
        //
        //
        //enable dungeon detail select footer
        UIEnablerManager.Instance.SwitchElements("DungeonMapSelect", "DungeonDetailSelect", true);

        //disable background
        UIEnablerManager.Instance.DisableCanvas();

    }
}
