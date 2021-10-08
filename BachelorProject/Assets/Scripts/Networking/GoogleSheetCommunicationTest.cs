using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

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
            _webRequest = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec");
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
            PlayerInfo result = new PlayerInfo();
            result = JsonUtility.FromJson<PlayerInfo>(_webRequest.downloadHandler.text);
            Imported = true;
            outputTextfield.text = "";
            foreach (var item in result.result)
            {
                outputTextfield.text += item.ToString() + "\n";
                Debug.Log(item);
            }
            _webRequest = null;
            Imported = true;
        }
    }

    private void PostInfos()
    {
        //PlayerInfo newInfo = new PlayerInfo { result = new int[] { 123, 2, 3, 4 } };
        SendInfo newInfo = new SendInfo { input = inputTextField.text };
        var JsonPackage = JsonUtility.ToJson(newInfo);
        _webRequest = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec", JsonPackage);
        _webRequest.SendWebRequest();

        Exported = false;
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public int[] result;
    }

    [System.Serializable]
    public class SendInfo
    {
        public string input;
    }
}
