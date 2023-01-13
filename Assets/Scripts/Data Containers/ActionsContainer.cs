using UnityEngine;
using System;

public class ActionsContainer
{
    public static Action<PlayerInfo.PlayerIdentity> OnPlayerSideSwitch;
    public static Action<PlayerInfo.PlayerIdentity, Vector3> OnBallCollideWithPaddle;
    public static Action<PlayerInfo.PlayerIdentity> OnBallMissed;
    public static Action<Vector3> OnBallCollideWithScreenEdge;
    public static Action OnScoreUpdated;
    public static Action<PlayerInfo.PlayerIdentity> OnGameOver;

    // Network

    public static Action OnGameStart;
    public static Action<PlayerInfo.PlayerIdentity, string> OnPlayerNameSynced;
    public static Action<PlayerInfo.PlayerIdentity, int> OnPlayerScoreReceived;
    public static Action<Vector3, Vector3, float> OnBallShoot;
    public static Action<PlayerInfo.PlayerIdentity, Vector3, Vector3, float> OnPaddleBounce;
    public static Action<PlayerInfo.PlayerIdentity> RPC_OnBallMissed;
    public static Action<int, float> OnSyncPaddleInput;
    public static Action<string> OnUpdateNetworkStatus;
}