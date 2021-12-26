using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Player;
using UnityEngine;

public class PlayerBombLauncher : MonoBehaviour
{
    public FiniteTimer bombRechargeTimer = new FiniteTimer(0, 6);
    public float dodgeBombChargeRecoverAmount = 1;
    
    public PlayerBomb bomb;
    public event Action<float> BombChargeUpdated;
    private HypnoPlayer player;

    #region Setup

    private void Awake()
    {
        player = GetComponent<HypnoPlayer>();
        
        bombRechargeTimer.complete();
        BombChargeUpdated?.Invoke(bombRechargeTimer.Ratio);
    }

    private void OnEnable()
    {
        player.SuccessfullyDodged += handleSuccessfulDodge;
    }
    
    private void OnDisable()
    {
        player.SuccessfullyDodged -= handleSuccessfulDodge;
    }

    #endregion

    private void handleSuccessfulDodge()
    {
        bombRechargeTimer.updateTimer(dodgeBombChargeRecoverAmount);
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
            updateRechargeTimer(Time.deltaTime);
        }

    }

    void updateRechargeTimer(float amount)
    {
        bombRechargeTimer.updateTimer(amount);
        BombChargeUpdated?.Invoke(bombRechargeTimer.Ratio);
    }
}
