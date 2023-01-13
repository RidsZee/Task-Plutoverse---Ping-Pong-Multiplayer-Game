using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TMP_InputField IF_PlayerName;
    [SerializeField] Button ButtonConnect;
    [SerializeField] TextMeshProUGUI T_Status;

    void OnEnable()
    {
        ActionsContainer.OnUpdateNetworkStatus += ReceiveNetworkUpdate;
    }

    void OnDisable()
    {
        ActionsContainer.OnUpdateNetworkStatus -= ReceiveNetworkUpdate;
    }

    private void Start()
    {
        if(!Photon.Pun.PhotonNetwork.IsConnected)
        {
            PhotonNetworkManager.Instance.Call_ConnectToServer();
        }
        else
        {
            T_Status.text = "Connected..";
        }
    }

    void ReceiveNetworkUpdate(string _status)
    {
        T_Status.text = _status;
    }

    public void OnPlayerNameEntered()
    {
        IF_PlayerName.text = IF_PlayerName.text.Trim();

        if (IF_PlayerName.text.Length != 0)
        {
            ButtonConnect.interactable = false;
            PhotonNetworkManager.Instance.PlayerName = IF_PlayerName.text;
            IF_PlayerName.interactable = false;

            PhotonNetworkManager.Instance.Call_JoinRoom();
        }
        else
        {
            Debug.Log("Empty name is not valid");
        }
    }
}