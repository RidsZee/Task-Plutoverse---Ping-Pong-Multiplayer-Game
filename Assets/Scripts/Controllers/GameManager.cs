using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PaddleBounceType
    {
        Reflective,
        Random
    }

    public static GameManager Instance;

    public GameConfiguration GameConfig;
    public PlayerInfo.PlayerIdentity CurrentPlayerTurn;
    public PaddleBounceType PaddleBounce;

    public Transform Y_Depth;
    public Transform Player1Paddle;
    public Transform Player2Paddle;
    public Camera MainCamera;
    public Rigidbody Ball;
    public Transform CenterLine;

    public float ObjectZThickness = 0.5f;

    [SerializeField]
    BoxCollider[] Player1Colliders;

    [SerializeField]
    BoxCollider[] Player2Colliders;

    [HideInInspector] public float PaddlePosMinZ;
    [HideInInspector] public float PaddlePosMaxZ;

    float SpawnPosMaxY;
    float SpawnPosMinY;

    float CurrentBallForce;
    Vector3 CurrentBallDirection;

    Vector3 WorldPoint_RightTop;
    Vector3 WorldPoint_LeftBottom;
    Vector3 WorldPoint_LeftTop;
    Vector3 WorldPoint_RightBottom;
    Vector3 WorldPoint_Center;

    Animator CamShakeAnimator;
    Animator Anim_Paddle1;
    Animator Anim_Paddle2;

    bool isGameOver;

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ActionsContainer.OnBallCollideWithScreenEdge += OnBallCollideWithVerticleEdge;
        ActionsContainer.OnBallCollideWithPaddle += OnBallCollideWithPaddle;
        ActionsContainer.OnBallMissed += OnBallMissed;
        ActionsContainer.OnGameStart += RPC_Response_OnGamestarted;
        ActionsContainer.OnBallShoot += RPC_Received_ShootBall;
        ActionsContainer.OnPaddleBounce += RPC_Receive_PaddleBounceReceived;
        ActionsContainer.RPC_OnBallMissed += RPC_Received_OnBallMissed;
        ActionsContainer.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        ActionsContainer.OnBallCollideWithScreenEdge -= OnBallCollideWithVerticleEdge;
        ActionsContainer.OnBallCollideWithPaddle -= OnBallCollideWithPaddle;
        ActionsContainer.OnBallMissed -= OnBallMissed;
        ActionsContainer.OnGameStart -= RPC_Response_OnGamestarted;
        ActionsContainer.OnBallShoot -= RPC_Received_ShootBall;
        ActionsContainer.OnPaddleBounce -= RPC_Receive_PaddleBounceReceived;
        ActionsContainer.RPC_OnBallMissed -= RPC_Received_OnBallMissed;
        ActionsContainer.OnGameOver -= OnGameOver;
    }

    void Start()
    {
        InitializeScene();
        Ball.gameObject.SetActive(false);
        CurrentPlayerTurn = PlayerInfo.PlayerIdentity.Player1;

        if (PlayerInfo.Instance.MyIdentity == PlayerInfo.PlayerIdentity.Player1)
        {
            foreach (BoxCollider boxCollider in Player2Colliders)
            {
                boxCollider.enabled = false;
            }

            InputManager.Instance.AssignMyPaddle(Player1Paddle, Player2Paddle);
        }
        else
        {
            foreach (BoxCollider boxCollider in Player1Colliders)
            {
                boxCollider.enabled = false;
            }

            InputManager.Instance.AssignMyPaddle(Player2Paddle, Player1Paddle);
        }

        RPCManager.Instance.SendRPC_SCeneLoaded(PlayerInfo.Instance.MyIdentity);
        RPCManager.Instance.SendRPC_PlayerName(PlayerInfo.Instance.MyIdentity, PhotonNetworkManager.Instance.PlayerName);

        CamShakeAnimator = MainCamera.GetComponent<Animator>();
        Anim_Paddle1 = Player1Paddle.GetComponent<Animator>();
        Anim_Paddle2 = Player2Paddle.GetComponent<Animator>();
    }

    void InitializeScene()
    {
        // Initialize screen coordinates

        Vector3 screenPoint_RightTop = Vector3.zero;
        screenPoint_RightTop.x = Screen.width;
        screenPoint_RightTop.y = Screen.height;
        screenPoint_RightTop.z = (Y_Depth.position.y - MainCamera.transform.position.y);
        WorldPoint_RightTop = MainCamera.ScreenToWorldPoint(screenPoint_RightTop);
        
        Vector3 screenPoint_LeftBottom = Vector3.zero;
        screenPoint_LeftBottom.x = 0;
        screenPoint_LeftBottom.y = 0;
        screenPoint_LeftBottom.z = (Y_Depth.position.y - MainCamera.transform.position.y);
        WorldPoint_LeftBottom = MainCamera.ScreenToWorldPoint(screenPoint_LeftBottom);

        WorldPoint_LeftTop = WorldPoint_RightTop;
        WorldPoint_LeftTop.x = WorldPoint_LeftBottom.x;

        WorldPoint_RightBottom = WorldPoint_LeftBottom;
        WorldPoint_RightBottom.x = WorldPoint_RightTop.x;

        WorldPoint_Center.x = WorldPoint_LeftBottom.x + ((WorldPoint_RightBottom.x - WorldPoint_LeftBottom.x) * 0.5f);
        WorldPoint_Center.z = WorldPoint_LeftBottom.z + (Vector3.Distance(WorldPoint_RightTop, WorldPoint_RightBottom) * 0.5f);
        WorldPoint_Center.y = Y_Depth.position.y;

        SpawnPosMaxY = WorldPoint_RightTop.z - GameConfig.CenterLineScreenMargin;
        SpawnPosMinY = WorldPoint_RightBottom.z + GameConfig.CenterLineScreenMargin;

        // Paddle Scale

        Vector3 paddleScale = Vector3.one;
        paddleScale.x = GameConfig.PaddleWidth;
        paddleScale.z = GameConfig.PaddleHeight;
        paddleScale.y = ObjectZThickness;

        Player1Paddle.localScale = paddleScale;
        Player2Paddle.localScale = paddleScale;


        // Paddle Position

        Vector3 PaddlePosition = Vector3.zero;
        PaddlePosition.x = WorldPoint_LeftBottom.x + GameConfig.PaddleScreenDistance + (GameConfig.PaddleWidth * 0.5f);
        PaddlePosition.z = WorldPoint_LeftBottom.z + (Vector3.Distance(WorldPoint_LeftTop, WorldPoint_LeftBottom) * 0.5f);
        PaddlePosition.y = Y_Depth.position.y;
        Player1Paddle.position = PaddlePosition;

        PaddlePosition.x = WorldPoint_RightBottom.x - GameConfig.PaddleScreenDistance - (GameConfig.PaddleWidth * 0.5f);
        Player2Paddle.position = PaddlePosition;

        
        // Define paddle movement range

        PaddlePosMinZ = WorldPoint_LeftBottom.z + (GameConfig.PaddleHeight * 0.5f);
        PaddlePosMaxZ = WorldPoint_LeftTop.z - (GameConfig.PaddleHeight * 0.5f);

        
        // Initialize Ball

        Ball.transform.localScale = Vector3.one * GameConfig.BallScale;


        // Place and scale CenterLine

        CenterLine.position = WorldPoint_Center;

        Vector3 CenterLineScale = Vector3.one;
        CenterLineScale.x = GameConfig.CenterlineThickness;
        CenterLineScale.z = (Vector3.Distance(WorldPoint_RightTop, WorldPoint_RightBottom) - (GameConfig.CenterLineScreenMargin * 2));
        CenterLineScale.y = GameConfig.BallScale;
        CenterLine.localScale = CenterLineScale;
    }


    void RPC_Response_OnGamestarted()
    {
        if(PlayerInfo.Instance.MyIdentity == PlayerInfo.PlayerIdentity.Player1)
        {
            ShootBall();
        }
    }

    void ShootBall()
    {
        if(isGameOver)
        {
            return;
        }

        CurrentBallDirection = RandomDataGenerator.Instance.GetRandomSpawnDirection(CurrentPlayerTurn);
        CurrentBallForce = RandomDataGenerator.Instance.GetRandomSpawnForce();

        Ball.gameObject.SetActive(true);
        Ball.transform.position = RandomDataGenerator.Instance.GetRandomSpawnPosition(WorldPoint_Center, WorldPoint_RightBottom.z, WorldPoint_RightTop.z);

        Ball.velocity = CurrentBallDirection * CurrentBallForce;

        RPCManager.Instance.SendRPC_ShootBall(Ball.transform.position, CurrentBallDirection, CurrentBallForce);
    }

    void OnBallCollideWithVerticleEdge(Vector3 _normal)
    {
        Ball.velocity = Vector3.zero;
        CurrentBallDirection = Vector3.Reflect(CurrentBallDirection, _normal);
        Ball.velocity = CurrentBallDirection * CurrentBallForce;
    }

    void OnBallCollideWithPaddle(PlayerInfo.PlayerIdentity _player, Vector3 _normal)
    {
        CurrentBallForce = RandomDataGenerator.Instance.GetRandomBounceForce();
        
        if(PaddleBounce == PaddleBounceType.Reflective)
        {
            CurrentBallDirection = Vector3.Reflect(CurrentBallDirection, _normal);
        }
        else
        {
            CurrentBallDirection = RandomDataGenerator.Instance.GetRandomBounceDirection(CurrentPlayerTurn);
        }
        
        Ball.velocity = Vector3.zero;
        Ball.velocity = CurrentBallDirection * CurrentBallForce;

        RPCManager.Instance.SendRPC_PaddleBounce(_player, Ball.transform.position, CurrentBallDirection, CurrentBallForce);

        if(_player == PlayerInfo.PlayerIdentity.Player1)
        {
            CurrentPlayerTurn = PlayerInfo.PlayerIdentity.Player2;
            Anim_Paddle1.SetTrigger("glow");
        }
        else
        {
            CurrentPlayerTurn = PlayerInfo.PlayerIdentity.Player1;
            Anim_Paddle2.SetTrigger("glow");
        }
    }

    void OnBallMissed(PlayerInfo.PlayerIdentity _player)
    {
        Ball.gameObject.SetActive(false);
        Invoke(nameof(ShootBall), RandomDataGenerator.Instance.GetRandomSpawnDelay());

        RPCManager.Instance.SendRPC_BallMissed(_player);

        if(_player == PlayerInfo.PlayerIdentity.Player1)
        {
            ScoreManager.Instance.Player2Score++;
            RPCManager.Instance.SendRPC_SyncScore(PlayerInfo.PlayerIdentity.Player2, ScoreManager.Instance.Player2Score);
        }
        else
        {
            ScoreManager.Instance.Player1Score++;
            RPCManager.Instance.SendRPC_SyncScore(PlayerInfo.PlayerIdentity.Player1, ScoreManager.Instance.Player1Score);
        }

        CamShakeAnimator.SetTrigger("shake");
    }

    void RPC_Received_ShootBall(Vector3 _position, Vector3 _direction, float _force)
    {
        Ball.gameObject.SetActive(true);
        Ball.transform.position = _position;

        CurrentBallForce = _force;
        CurrentBallDirection = _direction;

        Ball.velocity = _direction * _force;
    }

    void RPC_Received_OnBallMissed(PlayerInfo.PlayerIdentity _player)
    {
        CamShakeAnimator.SetTrigger("shake");
        Ball.gameObject.SetActive(false);
    }

    void RPC_Receive_PaddleBounceReceived(PlayerInfo.PlayerIdentity _player, Vector3 _ballPosition, Vector3 _direction, float _force)
    {
        Ball.velocity = Vector3.zero;
        Ball.transform.position = _ballPosition;
        Ball.velocity = _direction * _force;

        CurrentBallDirection = _direction;
        CurrentBallForce = _force;

        if (_player == PlayerInfo.PlayerIdentity.Player1)
        {
            CurrentPlayerTurn = PlayerInfo.PlayerIdentity.Player2;
            Anim_Paddle1.SetTrigger("glow");
        }
        else
        {
            CurrentPlayerTurn = PlayerInfo.PlayerIdentity.Player1;
            Anim_Paddle2.SetTrigger("glow");
        }
    }

    void OnGameOver(PlayerInfo.PlayerIdentity _player)
    {
        isGameOver = true;

        Ball.gameObject.SetActive(false);
        PhotonNetworkManager.Instance.Call_LeaveRoom();

        Invoke(nameof(GoBackToMainMenu), 6.0f);
    }

    void GoBackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}