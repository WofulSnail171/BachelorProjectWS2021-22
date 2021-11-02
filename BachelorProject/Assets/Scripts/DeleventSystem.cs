using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleventSystem : MonoBehaviour
{
    public delegate void SimpleEvent();
    public static SimpleEvent eventDataDownloaded;

    public delegate void MessageEvent(string _message);

    public delegate void PlayerDataEvent(PlayerData _playerData);
    public static PlayerDataEvent trySignUp;
    public static PlayerDataEvent trySignIn;
}
