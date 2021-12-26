using System;
using BDeshi.Utility;
using Core.Player;
using UnityEngine;
using UnityEngine.Events;

public abstract class Powerup: MonoBehaviour, AutoPoolable<Powerup>
{
    public FiniteTimer durationTimer = new FiniteTimer(9);
    public UnityEvent OnPowerUpSpawned;
    void Update()
    {
        if (durationTimer.tryCompleteTimer(Time.deltaTime))
        {
            NormalReturn();
        }
    }
    

    private void NormalReturn()
    {
        NormalReturnCallback?.Invoke(this);
    }

    public void initialize()
    {
        durationTimer.reset();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var p = other.GetComponent<HypnoPlayer>();
        if (p != null)
        {
            doPowerUpThing(p);
            NormalReturn();
        }
    }

    protected abstract void doPowerUpThing(HypnoPlayer hypnoPlayer);


    public void handleForceReturn()
    {
        
    }

    public event Action<Powerup> NormalReturnCallback;
}