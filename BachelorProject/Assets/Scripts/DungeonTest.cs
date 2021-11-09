using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DungeonTest : MonoBehaviour
{
    public GameObject dungeonScreenUI;
    public TMP_Text outputTextfield;
    public TMP_InputField dungeonNumField;
    public int dungeonNum;
    public TMP_InputField autoSpeed;

    public Button startButton;
    public Button stepButton;
    public Button autoButton;
    public Button uiToggleButton;

    public bool AutoPlay = false;
    public int AutoPlaySpeed = 100;
    public int CurrentStep = 0;

    void OnSceneInit()
    {
        dungeonScreenUI.SetActive(true);
        dungeonNumField.interactable = true;
        autoSpeed.interactable = false;
        startButton.interactable = false;
        stepButton.interactable = false;
        autoButton.interactable = false;
        uiToggleButton.interactable = true;
        if(DatabaseManager._instance != null)
        {
            string temp = "";
            foreach (var item in DatabaseManager._instance.dungeonData.dailyDungeons)
            {
                temp += item.questName + " | ";
                temp += item.layoutId + " | ";
                temp += item.dailySeed.ToString() + "\n";
            }
            outputTextfield.text = temp;
        }
    }

    public void OnStartButton()
    {
        dungeonScreenUI.SetActive(true);
        dungeonNumField.interactable = false;
        autoSpeed.interactable = false;
        startButton.interactable = false;
        stepButton.interactable = true;
        autoButton.interactable = true;
        uiToggleButton.interactable = true;
        AutoPlay = false;

        //create the dungeon run gogogogo!
        
        DatabaseManager._instance.dungeonData.currentRun = DungeonManager._instance.CreateDungeonRun(dungeonNum);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(true);
        DungeonManager._instance.CalculateRun(0);
        UploadDungeonData dataDungeon = new UploadDungeonData { dungeonData = DatabaseManager._instance.dungeonData, playerInfo = new LoginInfo { playerId = "Rika", password = "pw" } };
        ServerCommunicationManager._instance.GetInfo(Request.PushDungeonData, JsonUtility.ToJson(dataDungeon), OnDataPushed);
    }

    public void OnDataPushed()
    {
        ServerCommunicationManager._instance.GetInfo(Request.DownloadDungeonData, JsonUtility.ToJson(new LoginInfo { playerId = "Rika", password = "pw" }));
    }

    public void OnAutoPlayToggle()
    {
        stepButton.interactable = AutoPlay;
        autoSpeed.interactable = !AutoPlay;
        AutoPlay = !AutoPlay;
        StartCoroutine(AutoplayRoutine(.5f));
    }

    IEnumerator AutoplayRoutine(float waitTime)
    {
        while (AutoPlay)
        {
            yield return new WaitForSeconds((float)AutoPlaySpeed / 100.0f);
            //CurrentStep++;
            //DungeonManager._instance.CalculateRun(CurrentStep);
            DungeonManager._instance.NextStepRun();
        }
    }

    public void OnAutoPlayValueChanged()
    {
        try
        {
            AutoPlaySpeed = Int32.Parse(autoSpeed.text);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{autoSpeed.text}'");
        }
    }

    public void OnDungeonNumChanged()
    {
        try
        {
            dungeonNum = Int32.Parse(dungeonNumField.text);
            if(dungeonNum <= DatabaseManager._instance.dungeonData.dailyDungeons.Length && dungeonNum > 0)
            {
                startButton.interactable = true;
            }
            else
                startButton.interactable = false;
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{dungeonNumField.text}'");
            startButton.interactable = false;
        }
    }

    public void OnStepButton()
    {
        DungeonManager._instance.NextStepRun();
    }

    public void OnUiToggleButton()
    {
        dungeonScreenUI.SetActive(!dungeonScreenUI.activeSelf);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnSceneInit();
        if (ServerCommunicationManager._instance == null || DatabaseManager._instance == null)
            SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (DungeonManager._instance == null)
            return;
        if (DungeonManager._instance.currentCalcRun != null && DungeonManager._instance.currentCalcRun.dungeonLogArr != null && DungeonManager._instance.currentCalcRun.dungeonLogArr.Length > 0)
            outputTextfield.text = DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry;
    }
}
