using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Create Game Config")]
public class GameConfiguration : ScriptableObject
{
    [Space]
    [Range(0.1f, 1.0f)]
    public float BallScale;

    [Space]
    [Range(0.2f, 3.0f)]
    public float PaddleHeight;

    [Space]
    [Range(0.01f, 1.0f)]
    public float PaddleWidth;

    [Space]
    [Range(0, 1.0f)]
    public float PaddleScreenDistance;

    [Space]
    [Range(0, 2.0f)]
    public float CenterLineScreenMargin;

    [Space]
    [Range(0.01f, 1.0f)]
    public float CenterlineThickness;

    [Space]
    [Range(1.0f, 10.0f)]
    public float SpawnForceMin;

    [Space]
    [Range(1.0f, 10.0f)]
    public float SpawnForceMax;

    [Space]
    [Range(1.0f, 10.0f)]
    public float BallForceMin;

    [Space]
    [Range(1.0f, 10.0f)]
    public float BallForceMax;

    [Space]
    [Range(0.1f, 5.0f)]
    public float SpawnDelayMin;

    [Space]
    [Range(0.1f, 5.0f)]
    public float SpawnDelayMax;
}