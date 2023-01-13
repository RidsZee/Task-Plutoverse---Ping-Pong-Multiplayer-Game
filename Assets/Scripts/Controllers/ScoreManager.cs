using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    public int Player1Score;
    public int Player2Score;

    public int ScoreToWin;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        ActionsContainer.OnPlayerScoreReceived += RPC_Response_OnScoreReceived;
    }

    void OnDisable()
    {
        ActionsContainer.OnPlayerScoreReceived -= RPC_Response_OnScoreReceived;
    }

    void RPC_Response_OnScoreReceived(PlayerInfo.PlayerIdentity _player, int _score)
    {
        if (_player == PlayerInfo.PlayerIdentity.Player1)
        {
            Player1Score = _score;
        }
        else
        {
            Player2Score = _score;
        }

        ActionsContainer.OnScoreUpdated?.Invoke();

        if(Player1Score >= ScoreToWin)
        {
            ActionsContainer.OnGameOver?.Invoke(PlayerInfo.PlayerIdentity.Player1);
        }
        else if(Player2Score >= ScoreToWin)
        {
            ActionsContainer.OnGameOver?.Invoke(PlayerInfo.PlayerIdentity.Player2);
        }
    }
}
