using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using TMPro;
using UnityEngine;

public class GameTestText : MonoBehaviour
{
    int a = 5;
    private FiniteTimer timer = new FiniteTimer(0, .6f);
    private TextMeshProUGUI t;

    private void Awake()
    {
        t = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!GameStateManager.Instance.IsPaused)
        {
            if(timer.tryCompleteTimer(Time.deltaTime))
            {
                a++;
                t.text = a.ToString();
                timer.reset();
            }
        }
    }
}
