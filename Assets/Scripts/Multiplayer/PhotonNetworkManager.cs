/// <Sumery>
/// This class is responsible for:
/// 1. Multiplayer connection, room and RPC management
/// 2. Processing game states connected with multiplayer activities
/// 3. Game logic for switching player sides
/// </Summery>

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    #region Variables

    public static PhotonNetworkManager Instance;
    public bool isMaster;
    [SerializeField] int RoomFailedAttempt;

    public bool PlayerSceneLoaded;
    public bool OpponentSceneLoaded;

    public string PlayerName;

    #endregion


    #region Initialization

    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    #endregion


    #region Server Connectivity

    public void Call_ConnectToServer()
    {
        ActionsContainer.OnUpdateNetworkStatus?.Invoke("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ActionsContainer.OnUpdateNetworkStatus?.Invoke("Connected..");
        Debug.Log("Connected to server");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (RoomFailedAttempt > 0)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
            RoomFailedAttempt--;
        }
    }

    public void Call_JoinRoom()
    {
        ActionsContainer.OnUpdateNetworkStatus?.Invoke("Joining match..");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        isMaster = PhotonNetwork.IsMasterClient;

        if(isMaster)
        {
            PlayerInfo.Instance.SetPlayerIdentity(PlayerInfo.PlayerIdentity.Player1);
        }
        else
        {
            PlayerInfo.Instance.SetPlayerIdentity(PlayerInfo.PlayerIdentity.Player2);
        }

        if(!isMaster && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        else
        {
            ActionsContainer.OnUpdateNetworkStatus?.Invoke("Room joined.. waiting for opponent!");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (isMaster && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    public void Call_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
       
    }

    public void OnUpdateSceneLoaded(PlayerInfo.PlayerIdentity _player)
    {
        if(_player == PlayerInfo.Instance.MyIdentity)
        {
            PlayerSceneLoaded = true;
        }
        else
        {
            OpponentSceneLoaded = true;
        }

        if(PlayerSceneLoaded && OpponentSceneLoaded && isMaster)
        {
            RPCManager.Instance.SendRPC_GameStart();
        }
    }

    #endregion
}