using System;
using BDeshi.Utility;
using UnityEngine;

public class PoolableSFXPlayer : SFXPlayer, AutoPoolable<PoolableSFXPlayer>
{
    private bool shouldPool = false;
    [SerializeField] FiniteTimer timer;

    //this is not called on awake, so normal ones won't get pooled
    public void initialize()
    {
        shouldPool = true;
        timer.reset(source.clip.length);
    }

    public void handleForceReturn()
    {
        
    }

    private void Update()
    {
        if (timer.tryCompleteTimer(Time.deltaTime))
        {
            NormalReturnCallback?.Invoke(this);
        }
    }

    public event Action<PoolableSFXPlayer> NormalReturnCallback;
}