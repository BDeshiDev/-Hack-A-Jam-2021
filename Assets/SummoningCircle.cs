using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core;
using Core.Combat;
using DG.Tweening;
using UnityEngine;

public class SummoningCircle : MonoBehaviour, AutoPoolable<SummoningCircle>
{
    public float animDuration = .6f;
    public SpriteRenderer spriter1;
    public SpriteRenderer spriter2;
    
    
    public void startSummon(EnemyEntity prefab, Vector3 pos, Action<EnemyEntity> spawnCallback)
    {
        StartCoroutine(summon(prefab, pos, spawnCallback));
    }
    
    
    //won't be used by anything else for the jam, so modularity not necessary
    IEnumerator summon(EnemyEntity prefab, Vector3 pos, Action<EnemyEntity> spawnCallback)
    {
        transform.position = pos;
        transform.localScale = Vector3.zero;
        var c = spriter1.color;
        c.a = 0;
        spriter1.color = spriter2.color = c;

        Tween t = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one*1.3f, animDuration*.8f))
                .Append(transform.DOScale(Vector3.one, animDuration * .2f))
            .Insert(0,spriter1.DOFade(1,animDuration))
            .Insert(0,spriter2.DOFade(1,animDuration))
            ;
        yield return t.WaitForCompletion();
        
        var e = GameplayPoolManager.Instance.enemyPool.get(prefab);
        e.transform.position = transform.position;
        
        spawnCallback.Invoke(e);
        
        NormalReturnCallback?.Invoke(this);
    }

    public void initialize()
    {
        
    }

    public void handleForceReturn()
    {
        StopAllCoroutines();
    }

    public event Action<SummoningCircle> NormalReturnCallback;
}
