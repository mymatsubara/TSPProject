using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FPSMovement))]
public class PlayerCharacter : MonoBehaviour
{
    private FPSMovement _movementControl;

    public int health = 5;
    public int score = 0;
    public float baseSpeed = 6.0f;

    private float _speedMultiplier = 1;
    private float _speed;

    private void Awake()
    {
        Messenger<float>.AddListener(Event.SpeedMultiplierChanged, ChangePlayerSpeedMulitplier);
    }

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(Event.SpeedMultiplierChanged, ChangePlayerSpeedMulitplier);
    }


    // Start is called before the first frame update
    void Start()
    {
        _movementControl = GetComponent<FPSMovement>();
        ChangePlayerSpeedMulitplier(_speedMultiplier);
        
        Messenger<int>.Broadcast(Event.ScoreChanged, score);
    }

    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
    }

    public void ChangePlayerSpeedMulitplier(float multiplier)
    {
        _speedMultiplier = multiplier;
        _speed = _speedMultiplier * baseSpeed;
        _movementControl.speed = _speed;
    }

    public void IncrementScore(int points)
    {
        score += points;
        Messenger<int>.Broadcast(Event.ScoreChanged, score);
    }
}
