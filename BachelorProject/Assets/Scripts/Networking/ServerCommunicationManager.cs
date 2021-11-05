using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ServerCommunicationManager : MonoBehaviour
{
    public static ServerCommunicationManager _instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
    }
    private string _URL = "https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec";

    private List<WebRequestInstance> webRequestQueue = new List<WebRequestInstance>();
    private UnityWebRequest _webRequest;
    private bool Imported = true;
    private Request lastGetInfo;
    public Request LastGetInfo { get { return lastGetInfo; } }
    private string lastMessage;
    public string LastMessage { get { return lastMessage; } }
    private bool Exported = true;

    // Update is called once per frame
    void Update()
    {
        if (_webRequest != null && _webRequest.isDone && !Imported)
            CheckForImportRequestEnd();
        if (_webRequest != null && _webRequest.isDone && !Exported)
        {
            _webRequest = null;
            Exported = true;
        }
        if(_webRequest == null && webRequestQueue.Count > 0)
        {
            PopRequestQueue();
        }
    }

    private void CheckForImportRequestEnd()
    {
        if (_webRequest != null && _webRequest.isDone)
        {
            ExecuteImport();
            
        }
    }

    private void ExecuteImport()
    {
        if (_webRequest == null || !_webRequest.isDone)
            return;
        string message = _webRequest.downloadHandler.text;
        //ToDO! more length for the requestmarker!!! maybe check where the first non numeric symbol is ('{' maybe?)
        string requestMarker = message.Substring(0, 1);
        lastMessage = message.Remove(0, requestMarker.Length);
        int requestTypeInt;
        if (!Int32.TryParse(requestMarker, out requestTypeInt))
            return;
        Request requestType = (Request)Int32.Parse(requestMarker);
        _webRequest = null;
        Imported = true;
        Debug.Log(lastMessage);
        switch (requestType)
        {
            case Request.Error:
                ErrorHandling(lastMessage);
                break;
            case Request.SignUp:
                //start game
                //create new userprofil with data given
                //tutorials + first hero
                //DatabaseManager._instance.UpdateActivePlayerFromServer(lastMessage);
                DeleventSystem.trySignUp(JsonUtility.FromJson<PlayerData>(lastMessage));
                break;
            case Request.SignIn:
                //get user profil
                //if time stamp on online profile is newer than local discard local data and reapply online data
                DeleventSystem.trySignIn(JsonUtility.FromJson<PlayerData>(lastMessage));
                break;
            case Request.GetPlayerData:
                // generically download a specified user profile for diverse use cases
                break;
            case Request.DownloadHeroList:
                //download the default hero list and replace local copy
                DatabaseManager._instance.UpdateDefaultHeroListFromServer(lastMessage);
                break;
            case Request.DownloadEventData:
                //download the default hero list and replace local copy
                DatabaseManager._instance.UpdateEventDataFromServer(lastMessage);
                if(DeleventSystem.eventDataDownloaded != null)
                    DeleventSystem.eventDataDownloaded();
                break;
            case Request.PushPlayerData:
                break;
            default:
                break;
        }
        WebRequestInstance _temp = webRequestQueue[0];
        webRequestQueue.RemoveAt(0); //in case one of the functions throw an error i want to ´still remove the message
        if (_temp.simpleEvent != null)
            _temp.simpleEvent();
        if (_temp.messageEvent != null)
            _temp.messageEvent(lastMessage);
    }

    private void ErrorHandling(string _message)
    {
        Debug.LogWarning(_message);
        switch (LastGetInfo)
        {
            case Request.Error:
                break;
            case Request.SignUp:
                DeleventSystem.trySignUp(new PlayerData { playerId = "Error", password = _message});
                break;
            case Request.SignIn:
                DeleventSystem.trySignIn(new PlayerData { playerId = "Error", password = _message});
                break;
            case Request.GetPlayerData:
                break;
            case Request.DownloadHeroList:
                break;
            case Request.DownloadEventData:
                break;
            case Request.PushPlayerData:
                break;
            default:
                break;
        }
    }

    public void PopRequestQueue()
    {
        WebRequestInstance _temp = webRequestQueue[0];
        _webRequest = _temp.request;
        _webRequest.SendWebRequest();
        Debug.Log(_temp.requestType);
        if (_temp.waitForAnswer)
        {
            lastGetInfo = _temp.requestType;
            Imported = false;
        }
        else
        {
            Exported = false;
            webRequestQueue.RemoveAt(0);
        }
    }

    public void GetInfo(Request _request, string _message = "", DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        if (_webRequest != null)
        {
            return;
        }
        ServerRequest newRequest = new ServerRequest { request = _request, jsonData = _message};
        var JsonPackage = JsonUtility.ToJson(newRequest);
        string parameters = "?data=" + JsonPackage;
        //check for message length?
        WebRequestInstance _temp = new WebRequestInstance{ request = UnityWebRequest.Get(_URL + parameters), requestType = _request, waitForAnswer = true, simpleEvent = _simpleEvent, messageEvent = _messageEvent};
        webRequestQueue.Add(_temp);        
    }

    public void PostInfo(Request _request, string _message = "")
    {
        if(_webRequest != null)
        {
            return;
        }
        ServerRequest newRequest = new ServerRequest { request = Request.DownloadHeroList, jsonData = _message };
        var JsonPackage = JsonUtility.ToJson(newRequest);
        //check for message length?
        WWWForm form = new WWWForm();
        form.AddField("data", JsonPackage);
        WebRequestInstance _temp = new WebRequestInstance { request = UnityWebRequest.Post(_URL, form), requestType = _request, waitForAnswer = false };
        webRequestQueue.Add(_temp);
    }
}

[System.Serializable]
public class ServerRequest
{
    public Request request;
    public string jsonData;
}

public enum Request
{
    Error,
    SignUp,
    SignIn,
    GetPlayerData,
    DownloadHeroList,
    DownloadEventData,
    PushPlayerData
}

//for the request queue:
public struct WebRequestInstance
{
    public UnityWebRequest request;
    public Request requestType;
    public bool waitForAnswer;
    public DeleventSystem.SimpleEvent simpleEvent;
    public DeleventSystem.MessageEvent messageEvent;
}
