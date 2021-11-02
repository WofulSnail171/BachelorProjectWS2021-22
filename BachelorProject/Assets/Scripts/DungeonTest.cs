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
    public TMP_InputField dungeonNum;
    public TMP_InputField autoSpeed;

    public Button startButton;
    public Button stepButton;
    public Button autoButton;
    public Button uiToggleButton;

    public bool AutoPlay = false;
    public int AutoPlaySpeed = 1;

    void OnSceneInit()
    {
        dungeonScreenUI.SetActive(true);
        dungeonNum.interactable = true;
        autoSpeed.interactable = false;
        startButton.interactable = true;
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
        dungeonNum.interactable = false;
        autoSpeed.interactable = false;
        startButton.interactable = false;
        stepButton.interactable = true;
        autoButton.interactable = true;
        uiToggleButton.interactable = true;
        AutoPlay = false;
    }

    public void OnAutoPlayToggle()
    {
        stepButton.interactable = AutoPlay;
        autoSpeed.interactable = !AutoPlay;
        AutoPlay = !AutoPlay;
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
        Debug.Log(autoSpeed.text);
    }

    public void OnStepButton()
    {

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
        
    }
}
