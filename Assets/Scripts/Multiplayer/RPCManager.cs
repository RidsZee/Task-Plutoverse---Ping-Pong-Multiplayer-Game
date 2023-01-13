/// <Sumery>
/// This class is responsible for:
/// 1. Sending all RPC calls and raising associated Action Events
/// </Summery>

using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class RPCManager : MonoBehaviour
{
    #region Variables

    public static RPCManager Instance;
    PhotonView photonView;

    #endregion


    #region Initialization

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    #endregion



    #region RPC Start the Game

    public void SendRPC_GameStart()
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_GameStart), RpcTarget.AllViaServer);
    }

    [PunRPC]
    void RPC_GameStart()
    {
        ActionsContainer.OnGameStart?.Invoke();
    }

    #endregion



    #region RPC Sync Player Name

    public void SendRPC_PlayerName(PlayerInfo.PlayerIdentity _player, string _playerName)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_PlayerName), RpcTarget.AllViaServer, _player, _playerName);
    }

    [PunRPC]
    void RPC_PlayerName(PlayerInfo.PlayerIdentity _player, string _playerName)
    {
        ActionsContainer.OnPlayerNameSynced?.Invoke(_player, _playerName);
    }

    #endregion



    #region RPC Sync Scene Loaded

    public void SendRPC_SCeneLoaded(PlayerInfo.PlayerIdentity _player)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_ScenLoaded), RpcTarget.AllViaServer, _player);
    }

    [PunRPC]
    void RPC_ScenLoaded(PlayerInfo.PlayerIdentity _player)
    {
        PhotonNetworkManager.Instance.OnUpdateSceneLoaded(_player);
    }

    #endregion



    #region RPC Sync Score

    public void SendRPC_SyncScore(PlayerInfo.PlayerIdentity _player, int _score)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_SyncScore), RpcTarget.AllViaServer, _player, _score);
    }

    [PunRPC]
    void RPC_SyncScore(PlayerInfo.PlayerIdentity _player, int _score)
    {
        ActionsContainer.OnPlayerScoreReceived?.Invoke(_player, _score);
    }

    #endregion



    #region RPC Shoot Ball

    public void SendRPC_ShootBall(Vector3 _position, Vector3 _direction, float _force)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_ShootBall), RpcTarget.Others, _position, _direction, _force);
    }

    [PunRPC]
    void RPC_ShootBall(Vector3 _position, Vector3 _direction, float _force)
    {
        ActionsContainer.OnBallShoot?.Invoke(_position, _direction, _force);
    }

    #endregion



    #region RPC Paddle Bounce

    public void SendRPC_PaddleBounce(PlayerInfo.PlayerIdentity _player, Vector3 _position, Vector3 _direction, float _force)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_PaddleBounce), RpcTarget.Others, _player, _position, _direction, _force);
    }

    [PunRPC]
    void RPC_PaddleBounce(PlayerInfo.PlayerIdentity _player, Vector3 _position, Vector3 _direction, float _force)
    {
        ActionsContainer.OnPaddleBounce?.Invoke(_player, _position, _direction, _force);
    }

    #endregion



    #region RPC Ball Missed

    public void SendRPC_BallMissed(PlayerInfo.PlayerIdentity _player)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_BallMissed), RpcTarget.Others, _player);
    }

    [PunRPC]
    void RPC_BallMissed(PlayerInfo.PlayerIdentity _player)
    {
        ActionsContainer.RPC_OnBallMissed?.Invoke(_player);
    }

    #endregion



    #region RPC Paddle Position Sync

    public void SendRPC_SyncPaddlePosition(int _direction, float _position)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(RPC_PaddlePosition), RpcTarget.Others, _direction, _position);
    }

    [PunRPC]
    void RPC_PaddlePosition(int _direction, float _position)
    {
        ActionsContainer.OnSyncPaddleInput?.Invoke(_direction, _position);
    }

    #endregion
}