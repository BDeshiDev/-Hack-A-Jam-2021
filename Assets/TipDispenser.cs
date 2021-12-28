using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
//Due to the time extension, I had the time to actually make a tutorial
// The remaining mechanics are really meant for the player to figure out on their own
// so this is no longer used.

/// <summary>
/// Shows a random string tip
/// No time to set up proper tutorial.
/// And the mechanics are hard to show directly
/// This'll have to do.
/// </summary>
public class TipDispenser : MonoBehaviour
{
    //could've been a sinlgeton but it's simpler to hook up events this way
    public static bool firstRun = true;
    public static List<string> tips = new List<string>()
    {
        "The brighter an enemy is, \n the more health they have.",
        "The enemy's hat color shows their health \n even when they are hypnotized.",
        "The bomb deals a lot of damage and blocks projectiles.",
        "You can dash through bullets.",
        "Enemies will will recover from hypnosis over time.",
        "Hypnotized Enemies will take damage over time",
        "Enemies can be hypnotized more quickly at lower health.",
    };

    static int tipIndex =0;

    public UnityEvent TipShown;
    public CanvasGroup tipTextCanvasGroup;
    public TextMeshProUGUI tipHeaderText;
    public TextMeshProUGUI tipText;
    public float tipShowDuration = 1.6f;
    public static float tipFadeTime = .4f;
    private float  minfadeTime = .2f;

    private void Awake()
    {
        showTip();
    }

    void showTip()
    {
        if (firstRun)
        {
            firstRun = false;
            invokeTipShown();
        }
        else
        {
            actuallyShowTip();
        }
    }
    void actuallyShowTip()
    {
        if (tipIndex < tips.Count)
        {
            actuallyShowTip(tips[tipIndex]);

            tipIndex++;
            if (tipIndex >= tips.Count)
            {
                tipIndex = 0;
                tips.Shuffle();
                tipFadeTime = minfadeTime;
            }
            
        }
    }
    void actuallyShowTip(string tip)
    {
        tipText.text = tip;

        tipTextCanvasGroup.alpha = 1;
        tipText.alpha = tipHeaderText.alpha =0;
        
        //hacky but sufficient for now
        Time.timeScale = 0;

        DOTween.Sequence()
            
            .Append(tipText.DOFade(1, tipFadeTime)
                .SetEase(Ease.InQuad))
            .Insert(0,
                tipHeaderText.DOFade(1, tipFadeTime*.8f)
                .SetEase(Ease.InQuad))
            .Insert(tipFadeTime + tipShowDuration,
                tipTextCanvasGroup.DOFade(0, tipFadeTime)
                .SetEase(Ease.InOutQuad)
            )
            .SetUpdate(true)
            .OnComplete(invokeTipShown);
    }

    void invokeTipShown()
    {
        //hacky but sufficient for now
        Time.timeScale = 1;
        TipShown.Invoke();
    }

    public static void cleanup()
    {
        firstRun = true;
        tipIndex = 0;
    }

}
