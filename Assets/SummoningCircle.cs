using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class SummoningCircle : MonoBehaviour, AutoPoolable<SummoningCircle>
{
    public float animDuration = .6f;
    public SpriteRenderer spriter1;
    public SpriteRenderer spriter2;
    
    //won't be used by anything else for the jam, so modularity not necessary
    public IEnumerator summon(Vector3 position, EnemyEntity prefab, List<EnemyEntity> spawned)
    {
        transform.position = position;
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
        // Debug.Log(t == null);
        yield return t.WaitForCompletion();
        var e = PoolManager.Instance.enemyPool.get(prefab);
        e.transform.position = position;
        Spawner.Instance.trackEnemy(e);
        spawned.Add(e);
        
        ReturnCallback?.Invoke(this);
    }

    public void initialize()
    {
        
    }

    public event Action<SummoningCircle> ReturnCallback;
}
