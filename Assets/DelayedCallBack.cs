using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class DelayedCallBack : MonoBehaviour
{
    private FiniteTimer timer = new FiniteTimer();
    private float range = .6f;
    public UnityEvent callback;
    private void Awake()
    {
        timer = new FiniteTimer(Random.Range(0,range));
    }

    private void OnEnable()
    {
        timer.reset();
    }

    void Update()
    {
        if (!timer.isComplete)
        {
            if (timer.tryCompleteTimer(Time.deltaTime))
                callback.Invoke();
        }
    }
}
