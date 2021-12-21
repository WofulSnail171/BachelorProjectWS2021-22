using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormsManager : MonoBehaviour
{
    public static FormsManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        StartCoroutine(AutoplayRoutine());
    }

    IEnumerator AutoplayRoutine()
    {
        while (DatabaseManager._instance.activePlayerData.answeredForms.Count < DatabaseManager._instance.globalData.formData.Count)
        {
            TryDisplayForm();
            yield return new WaitForSeconds(4);
        }
    }

    public void TryDisplayForm()
    {
        if(RedriectToLinkActions.displayText != "inactive")
        {
            return;
        }
        foreach (var item in DatabaseManager._instance.globalData.formData)
        {
            if (!DatabaseManager._instance.activePlayerData.answeredForms.Contains(item.title)) //+condition is met! ToDo
            {
                //Do the Routine for the ui
                RedriectToLinkActions.displayText = item.message;
                RedriectToLinkActions.formName = item.title;
                RedriectToLinkActions.URL = item.link.Replace("SomeUserName", DatabaseManager._instance.activePlayerData.playerId);
                UIEnablerManager.Instance.EnableElement("InfoAvailable", true);
                break;
            }
        }
    }

}
