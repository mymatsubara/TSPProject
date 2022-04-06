using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public int health { get; private set; }
    public int maxHealth { get; private set; }

    public void Startup()
    {
        Debug.Log("Player manager starting...");

        health = 50;
        maxHealth = 100;

        status = ManagerStatus.Started;
    }

    public void IncrementHealth(int value)
    {
        health = Mathf.Clamp(health + value, maxHealth, 0);
        Debug.Log("Health: " + health + "/" + maxHealth);
    }
}
