using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoogleSheetCommunicationTest : MonoBehaviour
{
    public GameObject localSaveScreen;
    public TMP_Text localSaveTextfield;
    public Button localSaveYes;
    public Button localSaveNo;
    public void LocalSaveScreenInit()
    {
        localSaveScreen.SetActive(true);
        localSaveYes.interactable = false;
        localSaveNo.interactable = false;

        DatabaseManager._instance.LoadLocalSave();
        if(DatabaseManager._instance.activePlayerData != null)
        {
            localSaveTextfield.text = "Savefile found! Continue as " + DatabaseManager._instance.activePlayerData.playerId + "?";
            localSaveYes.interactable = true;
            localSaveNo.interactable = true;
        }
        else
        {
            localSaveTextfield.text = "No Savefile found! Continue with signUp or signIn";
            //signupin screen init
            LocalSaveScreenExit();
            SignUpInScreenInit();
        }
    }

    public void LocalSaveScreenExit()
    {
        localSaveScreen.SetActive(false);
    }

    public void OnLocalSaveYes()
    {
        //automatically proceed to signIn up with local save data
        LocalSaveScreenExit();
        SignUpInScreenInit();
        nameTextField.text = DatabaseManager._instance.activePlayerData.playerId;
        pwTextField.text = DatabaseManager._instance.activePlayerData.password;
        OnSignInButton();
    }
    public void OnLocalSaveNo()
    {
        //signUpIn init
        LocalSaveScreenExit();
        SignUpInScreenInit();
    }

    public GameObject signUpInScreen;
    public TMP_Text outputTextfield;
    public TMP_InputField nameTextField;
    public TMP_InputField pwTextField;
    public TMP_InputField requestTextField;

    public Button signInButton;
    public Button signUpButton;

    string PasswordSend;
    string PlayerIdSend;

    public void SignUpInScreenInit()
    {
        signUpInScreen.SetActive(true);
        nameTextField.interactable = true;
        pwTextField.interactable = true;
        signInButton.interactable = true;
        signUpButton.interactable = true;
    }

    public void OnSignUpButton()
    {
        if(nameTextField.text == "" || pwTextField.text == "")
        {
            outputTextfield.text = "Enter Username and Password";
            return;
        }
        PlayerIdSend = nameTextField.text;
        PasswordSend = pwTextField.text;
        nameTextField.interactable = false;
        pwTextField.interactable = false;
        signInButton.interactable = false;
        signUpButton.interactable = false;

        outputTextfield.text = "Getting Data from Server...";
        Request requestType = Request.SignUp;
        LoginInfo playerLogin = new LoginInfo { playerId = PlayerIdSend, password = PasswordSend };
        ServerCommunicationManager._instance.GetInfo(requestType, JsonUtility.ToJson(playerLogin));
    }

    public void OnSignInButton()
    {
        if (nameTextField.text == "" || pwTextField.text == "" || nameTextField.text == "Error")
        {
            outputTextfield.text = "Enter Username and Password";
            return;
        }
        PlayerIdSend = nameTextField.text;
        PasswordSend = pwTextField.text;
        nameTextField.interactable = false;
        pwTextField.interactable = false;
        signInButton.interactable = false;
        signUpButton.interactable = false;

        outputTextfield.text = "Getting Data from Server...";
        Request requestType = Request.SignIn;
        LoginInfo playerLogin = new LoginInfo { playerId = PlayerIdSend, password = PasswordSend };
        ServerCommunicationManager._instance.GetInfo(requestType, JsonUtility.ToJson(playerLogin));
    }



    public void OnTrySignUp(PlayerData _playerData)
    {
        if(_playerData.playerId == "Error")
        {
            outputTextfield.text = "Username already Taken";
            SignUpInScreenInit();
        }
        else
        {
            outputTextfield.text = "SignUp successful";
            DatabaseManager._instance.activePlayerData = _playerData;
            DeleventSystem.eventDataDownloaded += FirstSignUp;
            DatabaseManager._instance.SaveGameDataLocally();
            ServerCommunicationManager._instance.GetInfo(Request.DownloadHeroList);
            ServerCommunicationManager._instance.GetInfo(Request.DownloadEventData);
            ServerCommunicationManager._instance.GetInfo(Request.PushPlayerData, JsonUtility.ToJson(DatabaseManager._instance.activePlayerData), FinishedLogIn);
        }
    }

    public void OnTrySignIn(PlayerData _playerData)
    {
        if (_playerData.playerId == "Error")
        {
            outputTextfield.text = "Username or password incorrect";
            SignUpInScreenInit();
        }
        else
        {
            outputTextfield.text = "SignIn successful";
            DatabaseManager._instance.activePlayerData = _playerData;
            DatabaseManager._instance.SaveGameDataLocally();
            ServerCommunicationManager._instance.GetInfo(Request.DownloadHeroList);
            ServerCommunicationManager._instance.GetInfo(Request.DownloadEventData, "",FinishedLogIn);
            //ServerCommunicationManager._instance.GetInfo(Request.PushPlayerData, JsonUtility.ToJson(DatabaseManager._instance.activePlayerData), FinishedLogIn);

        }
    }

    public void FirstSignUp()
    {
        DatabaseManager._instance.activePlayerData.inventory.Add(HeroCreator.GetRandomHeroOfRarity(3));
        DatabaseManager._instance.activePlayerData.inventory.Add(HeroCreator.GetRandomHeroOfRarity(2));
        DatabaseManager._instance.activePlayerData.inventory.Add(HeroCreator.GetRandomHeroOfRarity(2));
        DatabaseManager._instance.activePlayerData.inventory.Add(HeroCreator.GetRandomHeroOfRarity(1));
        
        DatabaseManager._instance.SaveGameDataLocally();
        ServerCommunicationManager._instance.GetInfo(Request.PushPlayerData, JsonUtility.ToJson(DatabaseManager._instance.activePlayerData));
        DeleventSystem.eventDataDownloaded -= FirstSignUp;
    }

    private void OnEnable()
    {
        DeleventSystem.trySignIn += OnTrySignIn;
        DeleventSystem.trySignUp += OnTrySignUp;
        
    }

    private void OnDisable()
    {
        DeleventSystem.trySignIn -= OnTrySignIn;
        DeleventSystem.trySignUp -= OnTrySignUp;
    }

    public void FinishedLogIn()
    {
        Debug.Log("Finished Login");
        SceneManager.LoadScene(1);
    }
    void Start()
    {
        LocalSaveScreenInit();
        //PostInfos();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FetchData()
    {
        Request requestType = (Request)Int32.Parse(requestTextField.text);
        LoginInfo playerLogin = new LoginInfo { playerId = nameTextField.text, password = pwTextField.text };
        ServerCommunicationManager._instance.GetInfo(requestType, JsonUtility.ToJson(playerLogin));
    }

    public void FetchDataOld()
    {
        /*
        if (_webRequest == null)
        {
            outputTextfield.text = "Fetching Data...";
            HeroData axel = new HeroData
            {
                heroID = "axel",
                rarity = 1,
                texture = "none",
                status = HeroStatus.Idle,
                physical = new Stat
                {
                    defaultVal = 0,
                    minVal = 0,
                    maxVal = 0,
                    defaultPot = 0,
                    maxPot = 0
                },
                magical = new Stat
                {
                    defaultVal = 0,
                    minVal = 0,
                    maxVal = 0,
                    defaultPot = 0,
                    maxPot = 0
                },
                social = new Stat
                {
                    defaultVal = 0,
                    minVal = 0,
                    maxVal = 0,
                    defaultPot = 0,
                    maxPot = 0
                },
                buff = MapNode.Star,
                debuff = MapNode.Rectangle,
                pathAffinity = PathType.Black
            };
            PlayerData newPlayer = new PlayerData
            {
                name = "Rika",
                password = "123",
                date = "16.10.2021",
                profileDescription = "im rika. The great and mighty",
                mtDoomCounter = 0,
                tradeCounter = 0,
                lastDungeonRun = "16.10.2021",
                currentDungeonRun = "forrest",

                inventory = new HeroData[]
                {
                    axel,
                    axel,
                    axel,
                    axel,
                    axel,
                    axel,
                    axel,
                    axel
                }
            };
            SendInfo newInfo = new SendInfo { input = inputTextField.text };
            var JsonPackage = JsonUtility.ToJson(newPlayer);
            string parameters = "?data=" + JsonPackage;
            _webRequest = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec" + parameters);
            _webRequest.SendWebRequest();
            Imported = false;
        }
        */
    }

    public void SaveData()
    {
        DatabaseManager._instance.SaveGameDataLocally();
    }

    public void LoadData()
    {
        DatabaseManager._instance.LoadLocalSave();
    }

    private void CheckForImportRequestEnd()
    {
        /*
        if (_webRequest != null && _webRequest.isDone)
        {
            PlayerInfo thing = new PlayerInfo();
            //Debug.Log(_webRequest.downloadHandler.text);
            string textMessage = _webRequest.downloadHandler.text;
            string requestMarker = textMessage.Substring(0, 1);
            textMessage = textMessage.Remove(0, requestMarker.Length);
            //textMessage = textMessage.Remove(textMessage.Length - 1, 1);
            //Request requestType = (Request)Int32.Parse(requestMarker);

            IncomingHeroData DefaultHeroList = new IncomingHeroData();
            DefaultHeroList = JsonUtility.FromJson<IncomingHeroData>(textMessage);
            
            switch (requestType)
            {
                case Request.SignUp:
                    break;
                case Request.SignIn:
                    break;
                case Request.GetPlayerData:
                    break;
                case Request.DownloadHeroList:
                    DefaultHeroList = JsonUtility.FromJson<IncomingHeroData>(textMessage);
                    break;
                default:
                    break;
            }
            
            string[] splittedMessage = textMessage.Split('{');
            foreach (var item in splittedMessage)
            {
                Debug.Log(item);
            }
            //thing = JsonUtility.FromJson<PlayerInfo>(_webRequest.downloadHandler.text);
            //Imported = true;
            //outputTextfield.text = "";
            //foreach (var item in result.result)
            //{
            //    outputTextfield.text += item.ToString() + "\n";
            //    Debug.Log(item);
            //}
            _webRequest = null;
            Imported = true;
        }
        */
    }


    //12833 characters can be send
    public void PostInfos()
    {
        ServerCommunicationManager._instance.PostInfo(Request.DownloadHeroList, "");
        /*
        //PlayerInfo newInfo = new PlayerInfo { result = new int[] { 123, 2, 3, 4 } };
        SendInfo newInfo = new SendInfo { input = inputTextField.text };
        int numOfHeroData = Int32.Parse(inputTextField.text);
        HeroData axel = new HeroData
        {
            heroID = "axel",
            rarity = 1,
            texture = "none",
            status = HeroStatus.Idle,
            physical = new Stat
            {
                defaultVal = 0,
                minVal = 0,
                maxVal = 0,
                defaultPot = 0,
                maxPot = 0
            },
            magical = new Stat
            {
                defaultVal = 0,
                minVal = 0,
                maxVal = 0,
                defaultPot = 0,
                maxPot = 0
            },
            social = new Stat
            {
                defaultVal = 0,
                minVal = 0,
                maxVal = 0,
                defaultPot = 0,
                maxPot = 0
            },
            buff = MapNode.Star,
            debuff = MapNode.Rectangle,
            pathAffinity = PathType.Black
        };
        PlayerData newPlayer = new PlayerData
        {
            name = "Benedikt",
            password = "321",
            date = System.DateTime.Now.ToString(),
            //signUpDate = System.DateTime.Parse( System.DateTime.Now.ToString()),
            profileDescription = "",
            mtDoomCounter = 0,
            tradeCounter = 0,
            lastDungeonRun = "16.10.2021",
            currentDungeonRun = "forrest",

            inventory = new HeroData[numOfHeroData]
        };
        for (int i = 0; i < newPlayer.inventory.Length; i++)
        {
            axel.heroID = i.ToString() + ":Axel";
            newPlayer.inventory[i] = axel;
        }
        */

    }    
}
