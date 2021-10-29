using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ServerCommunicationManager : MonoBehaviour
{
    public static ServerCommunicationManager _instance;
    // Start is called before the first frame update
    public InventoryUI InventoryUI;




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
        lastGetInfo = requestType;
        switch (requestType)
        {
            case Request.Error:
                ErrorHandling(lastMessage);
                break;
            case Request.SignUp:
                //start game
                //create new userprofil with data given
                //tutorials + first hero
                DatabaseManager._instance.UpdateActivePlayerFromServer(lastMessage);
                _webRequest = null;
                Imported = true;
                InventoryUI.NewDataAssign();
                break;
            case Request.SignIn:
                //get user profil
                //if time stamp on online profile is newer than local discard local data and reapply online data
                DatabaseManager._instance.UpdateActivePlayerFromServer(lastMessage);
                _webRequest = null;
                Imported = true;
                InventoryUI.NewDataAssign();
                break;
            case Request.GetPlayerData:
                // generically download a specified user profile for diverse use cases
                break;
            case Request.DownloadHeroList:
                //download the default hero list and replace local copy
                DatabaseManager._instance.UpdateDefaultHeroListFromServer(lastMessage);
                _webRequest = null;
                Imported = true;
                ServerCommunicationManager._instance.GetInfo(Request.SignIn, JsonUtility.ToJson(new LoginInfo { playerId = "Sarah", password = "EvenSaferPassword" }));
                break;
            case Request.DownloadEventData:
                //download the default hero list and replace local copy
                DatabaseManager._instance.UpdateEventDataFromServer(lastMessage);
                break;
            default:
                break;
        }
    }

    private void ErrorHandling(string _message)
    {
        Debug.LogError(_message);
        switch (LastGetInfo)
        {
            case Request.Error:
                break;
            case Request.SignUp:
                break;
            case Request.SignIn:
                break;
            case Request.GetPlayerData:
                break;
            case Request.DownloadHeroList:
                break;
            case Request.DownloadEventData:
                break;
            default:
                break;
        }
    }

    public void GetInfo(Request _request, string _message = "")
    {
        if (_webRequest != null)
        {
            return;
        }
        ServerRequest newRequest = new ServerRequest { request = _request, jsonData = _message};
        var JsonPackage = JsonUtility.ToJson(newRequest);
        string parameters = "?data=" + JsonPackage;
        //check for message length?
        _webRequest = UnityWebRequest.Get(_URL + parameters);
        _webRequest.SendWebRequest();
        lastGetInfo = _request;
        Imported = false;
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
        _webRequest = UnityWebRequest.Post(_URL, form);
        _webRequest.SendWebRequest();
        Exported = false;
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
    DownloadEventData
}
