using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

public class GoogleSheetCommunicationTest : MonoBehaviour
{
    private UnityWebRequest _webRequest;
    private bool Imported = true;
    private bool Exported = true;
    public TMP_Text outputTextfield;
    public TMP_InputField inputTextField;

    // Start is called before the first frame update
    void Start()
    {
        
        //PostInfos();


    }

    // Update is called once per frame
    void Update()
    {
        if (_webRequest != null && _webRequest.isDone && !Imported)
            CheckForImportRequestEnd();
        if(_webRequest != null && _webRequest.isDone && !Exported)
        {
            _webRequest = null;
            Exported = true;
        }
    }

    public void FetchData()
    {
        if(_webRequest == null)
        {
            outputTextfield.text = "Fetching Data...";
            ServerRequest newRequest = new ServerRequest { request = Request.DownloadHeroList };
            var JsonPackage = JsonUtility.ToJson(newRequest);
            string parameters = "?data=" + JsonPackage;
            _webRequest = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec" + parameters);
            _webRequest.SendWebRequest();
            Imported = false;
        }
    }

    public void FetchDataOld()
    {
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
    }

    public void WriteData()
    {
        if (_webRequest == null)
        {
            PostInfos();
        }
    }

    private void CheckForImportRequestEnd()
    {
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
            /*
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
            */
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
    }


    //12833 characters can be send
    private void PostInfos()
    {
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
            signUpDate = System.DateTime.Parse( System.DateTime.Now.ToString()),
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
        var JsonPackage = JsonUtility.ToJson(newPlayer);
        Debug.Log(JsonPackage.Length);


        //var JsonPackage = JsonUtility.ToJson(newInfo);
        string manualJSON = "\"input\":\"test?\",\"someNumber\":0,\"aFloat\":0.30000001192092898,\"pInfo\":{\"result\":[0,1,2,3,4]}";

        WWWForm form = new WWWForm();
        form.AddField("data", JsonPackage);
        _webRequest = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec", form);
        //https://script.google.com/macros/s/AKfycbxPMPxys3BNhNip71uoGyMPoF5Ag68qlGc6HKWxNAg/dev  https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec
        //https://script.google.com/macros/s/AKfycbxPMPxys3BNhNip71uoGyMPoF5Ag68qlGc6HKWxNAg/dev
        _webRequest.SendWebRequest();

        Exported = false;
    }

    [System.Serializable]
    public class ServerRequest
    {
        public Request request;
        public string JsonData;
    }

    public enum Request
    {
        SignUp,
        SignIn,
        GetPlayerData,
        DownloadHeroList
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public string[] result;
    }

    [System.Serializable]
    public class SendInfo
    {
        public string input;
        public int someNumber = 0;
        public float aFloat = 0.3f;
        public PlayerInfo pInfo = new PlayerInfo() { result = new string[] { "0","1", "2", "3", "4" } };
    }

    [System.Serializable]
    public class SheetRequest
    {
        public string requType = "none";
        public string sendInfoJson = "";
    }

    [System.Serializable]
    public class LogInInfo
    {
        public string username = "name";
        public string password = "pw";
    }
}
