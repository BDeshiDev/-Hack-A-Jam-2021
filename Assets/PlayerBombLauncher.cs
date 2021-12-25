using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using UnityEngine;

public class PlayerBombLauncher : MonoBehaviour
{
    public FiniteTimer bombRechargeTimer = new FiniteTimer(0, 6);
    public PlayerBomb bomb;
    public event Action<float> BombChargeUpdated;
    private void Awake()
    {
        bombRechargeTimer.complete();
        BombChargeUpdated?.Invoke(bombRechargeTimer.Ratio);
    }
    public void handleBombLaunchAttempt()
    {
        if (bombRechargeTimer.isComplete)
        {
            bombRechargeTimer.reset();
            
            bomb.launchBomb(transform.position);
        }
    }

    private void Update()
    {
        if (!bombRechargeTimer.isComplete)
        {
            bombRechargeTimer.updateTimer(Time.deltaTime);
            
            BombChargeUpdated?.Invoke(bombRechargeTimer.Ratio);
        }

    }
}
