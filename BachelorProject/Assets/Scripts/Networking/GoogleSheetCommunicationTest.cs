using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetCommunicationTest : MonoBehaviour
{
    private UnityWebRequest _webRequest;
    private bool Imported = false;

    // Start is called before the first frame update
    void Start()
    {
        //_webRequest = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec");
        //_webRequest.SendWebRequest();
        PostInfos();


    }

    // Update is called once per frame
    void Update()
    {
        if (_webRequest != null && _webRequest.isDone && Imported)
            CheckForImportRequestEnd();
    }

    private void CheckForImportRequestEnd()
    {
        if (_webRequest != null && _webRequest.isDone)
        {
            PlayerInfo result = new PlayerInfo();
            result = JsonUtility.FromJson<PlayerInfo>(_webRequest.downloadHandler.text);
            Imported = true;
            foreach (var item in result.result)
            {
                Debug.Log(item);
            }
        }
    }

    private void PostInfos()
    {
        PlayerInfo newInfo = new PlayerInfo { result = new int[] { 123, 2, 3, 4 } };
        var JsonPackage = JsonUtility.ToJson(newInfo);
        _webRequest = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec", JsonPackage);
        _webRequest.SendWebRequest();
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public int[] result;
    }
}
