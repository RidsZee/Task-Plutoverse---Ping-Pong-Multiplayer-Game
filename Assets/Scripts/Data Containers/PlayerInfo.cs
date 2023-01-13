using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public enum PlayerIdentity
    {
        None,
        Player1,
        Player2
    }

    public PlayerIdentity MyIdentity;

    public static PlayerInfo Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetPlayerIdentity(PlayerIdentity _identity)
    {
        MyIdentity = _identity;
    }
}