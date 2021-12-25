using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriter;
    [SerializeField] private float scaleTime = 1.2f;
    [SerializeField] private HitBox bombHitbox;
    private Tween tween;
    public Vector3 endSize = Vector3.one;

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
        
        createOrRestartTween();
    }

    Tween  createTween()
    {
        var part1Time = scaleTime * .9f;
        var part2Time = scaleTime * .1f;
        var t = DOTween.Sequence()
            .Append(transform.DOScale(endSize * 1.5f, part1Time))
            .Insert(0, spriter.DOFade(1, part1Time))
            .Append(transform.DOScale(endSize, part2Time))
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
