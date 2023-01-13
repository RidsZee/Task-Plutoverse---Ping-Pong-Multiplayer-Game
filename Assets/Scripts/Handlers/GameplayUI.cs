using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    string Player1Name;
    string Player2Name;

    [SerializeField] TextMeshProUGUI T_Player1Name;
    [SerializeField] TextMeshProUGUI T_Player2Name;

    [SerializeField] TextMeshProUGUI T_Player1Score;
    [SerializeField] TextMeshProUGUI T_Player2Score;

    [SerializeField] GameObject PopupGameFinish;
    [SerializeField] TextMeshProUGUI T_Player1;
    [SerializeField] TextMeshProUGUI T_Player2;

    void OnEnable()
    {
        ActionsContainer.OnPlayerNameSynced += RPC_Response_OnNameReceived;
        ActionsContainer.OnScoreUpdated += OnScoreUpdated;
        ActionsContainer.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        ActionsContainer.OnPlayerNameSynced -= RPC_Response_OnNameReceived;
        ActionsContainer.OnScoreUpdated -= OnScoreUpdated;
        ActionsContainer.OnGameOver -= OnGameOver;
    }

    void Start()
    {
        PopupGameFinish.SetActive(false);
        OnScoreUpdated();
    }

    void RPC_Response_OnNameReceived(PlayerInfo.PlayerIdentity _player, string _name)
    {
        if(_player == PlayerInfo.PlayerIdentity.Player1)
        {
            Player1Name = _name;
            T_Player1Name.text = Player1Name;
        }
        else
        {
            Player2Name = _name;
            T_Player2Name.text = Player2Name;
        }
    }

    void OnScoreUpdated()
    {
        T_Player1Score.text = ScoreManager.Instance.Player1Score.ToString();
        T_Player2Score.text = ScoreManager.Instance.Player2Score.ToString();
    }

    void OnGameOver(PlayerInfo.PlayerIdentity _player)
    {
        PopupGameFinish.SetActive(true);
        T_Player1.gameObject.SetActive(false);
        T_Player2.gameObject.SetActive(false);

        if(_player == PlayerInfo.PlayerIdentity.Player1)
        {
            T_Player1.gameObject.SetActive(true);
            T_Player1.text = Player1Name + " won!";
        }
        else
        {
            T_Player2.gameObject.SetActive(true);
            T_Player2.text = Player2Name + " won!";
        }
    }
}
