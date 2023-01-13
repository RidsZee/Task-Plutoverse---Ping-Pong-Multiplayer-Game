using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDataGenerator : MonoBehaviour
{
    public static RandomDataGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public Vector3 GetRandomSpawnPosition(Vector3 _centerPoint, float _min, float _max)
    {
        Vector3 ReturnPosition = Vector3.zero;
        ReturnPosition = _centerPoint;
        ReturnPosition.z = Random.Range(_min, _max);

        return ReturnPosition;
    }

    public Vector3 GetRandomBounceDirection(PlayerInfo.PlayerIdentity _currentPlayer)
    {
        Vector3 ReturnDirection = Vector3.zero;

        if (_currentPlayer == PlayerInfo.PlayerIdentity.Player1)
        {
            ReturnDirection.z = Random.Range(-0.4f, 0.4f);

            if (ReturnDirection.z > 1)
            {
                ReturnDirection.x = 1.0f - ReturnDirection.x;
            }
            else
            {
                ReturnDirection.x = 1.0f + ReturnDirection.x;
            }
        }
        else
        {
            ReturnDirection.z = Random.Range(-0.4f, 0.4f);

            if (ReturnDirection.z > 1)
            {
                ReturnDirection.x = 1.0f - ReturnDirection.x;
            }
            else
            {
                ReturnDirection.x = 1.0f + ReturnDirection.x;
            }

            ReturnDirection.x *= -1;
        }

        return ReturnDirection;
    }

    public Vector3 GetRandomSpawnDirection(PlayerInfo.PlayerIdentity _currentPlayer)
    {
        Vector3 ReturnDirection = Vector3.zero;

        if (_currentPlayer == PlayerInfo.PlayerIdentity.Player1)
        {
            ReturnDirection.z = Random.Range(0.2f, 0.5f);
            
            if(Random.Range(1, 11) > 5)
            {
                ReturnDirection.z *= -1;
            }
            
            if(ReturnDirection.z > 1)
            {
                ReturnDirection.x = 1.0f - ReturnDirection.z;
            }
            else
            {
                ReturnDirection.x = 1.0f + ReturnDirection.z;
            }
            
            ReturnDirection.x *= -1;
        }
        else
        {
            ReturnDirection.z = Random.Range(-0.4f, 0.4f);

            if (ReturnDirection.z > 1)
            {
                ReturnDirection.x = 1.0f - ReturnDirection.z;
            }
            else
            {
                ReturnDirection.x = 1.0f + ReturnDirection.z;
            }
        }

        return ReturnDirection;
    }

    public float GetRandomBounceForce()
    {
        return Random.Range(GameManager.Instance.GameConfig.BallForceMin, GameManager.Instance.GameConfig.BallForceMax);
    }

    public float GetRandomSpawnForce()
    {
        return Random.Range(GameManager.Instance.GameConfig.SpawnForceMin, GameManager.Instance.GameConfig.SpawnForceMax);
    }

    public float GetRandomSpawnDelay()
    {
        return Random.Range(GameManager.Instance.GameConfig.SpawnDelayMin, GameManager.Instance.GameConfig.SpawnDelayMax);
    }
}