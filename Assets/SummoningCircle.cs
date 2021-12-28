using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core;
using Core.Combat;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SummoningCircle : MonoBehaviour, AutoPoolable<SummoningCircle>
{
    public float animDuration = .6f;
    public SpriteRenderer spriter1;
    public SpriteRenderer spriter2;
    public Vector2 baseSize = Vector2.one * 2;

    private IEnumerator lastSummon = null;
    private FiniteTimer delayTimer;
    public UnityEvent spawnStartCallback;

    private void Awake()
    {
        delayTimer = new FiniteTimer(Random.Range(.2f, 1f));
    }

    public void startSummon(EnemyEntity prefab, Vector3 pos, Action<EnemyEntity> spawnCallback)
    {
        lastSummon = summon(prefab, pos, spawnCallback);
        StartCoroutine(lastSummon);
    }
    
    
    //won't be used by anything else for the jam, so modularity not necessary
    public IEnumerator summon(EnemyEntity prefab,
                        Vector3 pos,
                        [CanBeNull] Action<EnemyEntity> spawnCallback)
    {
        transform.position = pos;
        transform.localScale = Vector3.zero;
        var c = spriter1.color;
        c.a = 0;
        spriter1.color = spriter2.color = c;
        lastSummon = summon(prefab, pos, spawnCallback);
        delayTimer.reset();

        while (!delayTimer.isComplete)
        {
            delayTimer.updateTimer(Time.deltaTime);
            yield return null;
        }
        spawnStartCallback.Invoke();
        var part1Duration = animDuration*.4f;
        var part2Duration = animDuration * .6f;
        Tween t = DOTween.Sequence()
                .Append(transform.DOScale(baseSize *2f, part1Duration))
                .Append(transform.DOScale(baseSize, part2Duration))
                .Insert(0,spriter1.DOFade(1,part1Duration))
                .Insert(0,spriter2.DOFade(1,part1Duration))
                .SetUpdate(true)
                .OnComplete(() => { 
                    var e = GameplayPoolManager.Instance.enemyPool.get(prefab);
                    e.transform.position = transform.position;

                    spawnCallback?.Invoke(e);
                })
            ;
        yield return t.WaitForCompletion();

        NormalReturnCallback?.Invoke(this);
        lastSummon = null;
    }

    

    public void initialize()
    {
        
    }

    public void handleForceReturn()
    {
        if (lastSummon != null)
        {
            StopCoroutine(lastSummon);
        }
        lastSummon = null;
    }


    public event Action<SummoningCircle> NormalReturnCallback;
}
