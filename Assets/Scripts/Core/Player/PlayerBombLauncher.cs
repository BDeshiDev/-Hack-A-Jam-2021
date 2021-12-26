using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Player;
using UnityEngine;

public class PlayerBombLauncher : MonoBehaviour
{
    [SerializeField] PlayerBomb bomb;
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



    public void addBomb()
    {
        HasBomb = true;
        BombChargeToggled?.Invoke(true);
    }
}
