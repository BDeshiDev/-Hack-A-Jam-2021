using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Player;
using UnityEngine;

public class PlayerBombLauncher : MonoBehaviour
{
    public PlayerBomb bomb;
    public event Action<bool> BombChargeToggled;
    public bool HasBomb { get; private set; } = true;

    public void handleBombLaunchAttempt()
    {
        if (HasBomb)
        {
            HasBomb = false;
            
            bomb.launchBomb(transform.position);
            BombChargeToggled?.Invoke(false);
        }
    }



    void addBomb()
    {
        HasBomb = true;
        BombChargeToggled?.Invoke(true);
    }
}
