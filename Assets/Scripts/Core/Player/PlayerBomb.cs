using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriter;
    [SerializeField] private float scaleTime = 1.2f;
    [SerializeField] private HitBox bombHitbox;
    private Tween tween;
    public Vector3 endSize = Vector3.one;

    public UnityEvent bombLaunched;

    public float flashPreiod = .5f;
    public float flashCount = 4;

    private void Start()
    {
        transform.parent = null;
    }

    public void launchBomb(Vector3 point)
    {
        gameObject.SetActive(true);
        
        transform.position = point;
        transform.localScale = Vector3.zero;
        bombHitbox.startDetection();
        
        bombLaunched.Invoke();
        
        createOrRestartTween();
    }

    Tween  createTween()
    {
        var part1Time = scaleTime * .7f;
        var part2Time = scaleTime * .2f;
        var part3Time = scaleTime * .6f;
        var t = DOTween.Sequence()
            .Append(
                transform.DOScale(endSize * .5f, part1Time)
                .SetEase(Ease.Flash, flashCount, flashPreiod)
                )
            .Append(transform.DOScale(endSize * 1.2f, part2Time)
                .SetEase(Ease.OutCubic))
            .Insert(0, spriter.DOFade(1, part1Time))
            .Append(transform.DOScale(0, part3Time))
            .SetAutoKill(false)
            .SetRecyclable(true);
        t.onComplete += cleanup;

        return t;
    }


    void createOrRestartTween()
    {
        if (tween == null)
        {
            tween = createTween();
        }else {
            tween.Restart();
        }
    }

    public void cleanup()
    {
        bombHitbox.stopDetection();
        gameObject.SetActive(false);
    }


}
