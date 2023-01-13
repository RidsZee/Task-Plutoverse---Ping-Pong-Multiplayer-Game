using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    const string Tag_Paddle = "Paddle";
    const string Tag_PlayerScreenEdge = "PlayerScreenEdge";
    const string Tag_VerticleScrenEdge = "VerticleScreenEdge";

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Tag_Paddle))
        {
            ActionsContainer.OnBallCollideWithPaddle?.Invoke(collision.gameObject.GetComponent<PlayerData>().PlayerIdentity, collision.contacts[0].normal);
        }
        else if (collision.gameObject.CompareTag(Tag_PlayerScreenEdge))
        {
            ActionsContainer.OnBallMissed?.Invoke(collision.gameObject.GetComponent<PlayerData>().PlayerIdentity);
        }
        else if (collision.gameObject.CompareTag(Tag_VerticleScrenEdge))
        {
            ActionsContainer.OnBallCollideWithScreenEdge?.Invoke(collision.contacts[0].normal);
        }
    }
}