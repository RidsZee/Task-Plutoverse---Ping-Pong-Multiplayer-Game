using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] float PaddleSpeed;

    Transform myPaddle;
    Transform OpponentPaddle;

    int MovementDirection;
    int OpponentPaddleDirection;
    int LastPlayerDirection;

    Vector3 newPosition;
    public bool AutoShootForTest;

    bool isGameOver;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        ActionsContainer.OnSyncPaddleInput += SyncOpponentPaddle;
        ActionsContainer.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        ActionsContainer.OnSyncPaddleInput -= SyncOpponentPaddle;
        ActionsContainer.OnGameOver -= OnGameOver;
    }

    private void Start()
    {
        if(AutoShootForTest)
        {
            AutoShootForTest = PhotonNetworkManager.Instance.isMaster;
        }
    }

    public void AssignMyPaddle(Transform _myPaddle, Transform _opponentPaddle)
    {
        myPaddle = _myPaddle;
        OpponentPaddle = _opponentPaddle;
    }

    void SyncOpponentPaddle(int _direction, float _position)
    {
        OpponentPaddleDirection = _direction;

        Vector3 temp = OpponentPaddle.position;
        temp.z = _position;
        OpponentPaddle.position = temp;
    }

    void Update()
    {
        if(!isGameOver)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MovementDirection = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                MovementDirection = -1;
            }
            else
            {
                MovementDirection = 0;
            }

            myPaddle.Translate(myPaddle.forward * PaddleSpeed * MovementDirection * Time.deltaTime);

            if (myPaddle.transform.position.z > GameManager.Instance.PaddlePosMaxZ)
            {
                newPosition = myPaddle.position;
                newPosition.z = GameManager.Instance.PaddlePosMaxZ;
                myPaddle.transform.position = newPosition;
            }
            else if (myPaddle.transform.position.z < GameManager.Instance.PaddlePosMinZ)
            {
                newPosition = myPaddle.position;
                newPosition.z = GameManager.Instance.PaddlePosMinZ;
                myPaddle.transform.position = newPosition;
            }

            if (LastPlayerDirection != MovementDirection)
            {
                LastPlayerDirection = MovementDirection;
                RPCManager.Instance.SendRPC_SyncPaddlePosition(MovementDirection, myPaddle.position.z);
            }

            OpponentPaddle.Translate(OpponentPaddle.forward * PaddleSpeed * OpponentPaddleDirection * Time.deltaTime);

            if (OpponentPaddle.transform.position.z > GameManager.Instance.PaddlePosMaxZ)
            {
                newPosition = OpponentPaddle.position;
                newPosition.z = GameManager.Instance.PaddlePosMaxZ;
                OpponentPaddle.transform.position = newPosition;
            }
            else if (OpponentPaddle.transform.position.z < GameManager.Instance.PaddlePosMinZ)
            {
                newPosition = OpponentPaddle.position;
                newPosition.z = GameManager.Instance.PaddlePosMinZ;
                OpponentPaddle.transform.position = newPosition;
            }
        }

    }

    void OnGameOver(PlayerInfo.PlayerIdentity _player)
    {
        isGameOver = true;
    }
}