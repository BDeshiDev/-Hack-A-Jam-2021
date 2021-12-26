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
    public Vector2 baseSize = Vector2.one * 2;

    private IEnumerator lastSummon = null;
    // private Tween t;
    // public void startSummon(EnemyEntity prefab, Vector3 pos, Action<EnemyEntity> spawnCallback)
    // {
    //     transform.position = pos;
    //     transform.localScale = Vector3.zero;
    //     var c = spriter1.color;
    //     c.a = 0;
    //     spriter1.color = spriter2.color = c;
    //     
    //     
    //     
    //     t = DOTween.Sequence()
    //             .Append(transform.DOScale(Vector3.one*1.3f, animDuration*.8f))
    //             .Append(transform.DOScale(Vector3.one, animDuration * .2f))
    //             .Insert(0,spriter1.DOFade(1,animDuration))
    //             .Insert(0,spriter2.DOFade(1,animDuration))
    //             .OnComplete(() =>
    //             {
    //                 Debug.Log("anim 1 "+ Time.timeScale, gameObject);
    //
    //                 var e = GameplayPoolManager.Instance.enemyPool.get(prefab);
    //                 e.transform.position = transform.position;
    //                 Debug.Log("anim 2 "+ Time.timeScale, gameObject);
    //
    //                 spawnCallback.Invoke(e);
    //                 Debug.Log("anim 3 "+ Time.timeScale, gameObject);
    //
    //                 NormalReturnCallback?.Invoke(this);
    //             })
    //         ;
    //     // lastSummon = summon(prefab, pos, spawnCallback);
    //     // StartCoroutine(lastSummon);
    // }
    //
    //
    // //won't be used by anything else for the jam, so modularity not necessary
    // IEnumerator summon(EnemyEntity prefab, Vector3 pos, Action<EnemyEntity> spawnCallback)
    // {
    //     transform.position = pos;
    //     transform.localScale = Vector3.zero;
    //     var c = spriter1.color;
    //     c.a = 0;
    //     spriter1.color = spriter2.color = c;
    //     Tween t = DOTween.Sequence()
    //             .Append(transform.DOScale(Vector3.one*1.3f, animDuration*.8f))
    //             .Append(transform.DOScale(Vector3.one, animDuration * .2f))
    //             .Insert(0,spriter1.DOFade(1,animDuration))
    //             .Insert(0,spriter2.DOFade(1,animDuration))
    //             .OnComplete(() =>
    //             {
    //
    //                 var e = GameplayPoolManager.Instance.enemyPool.get(prefab);
    //                 e.transform.position = transform.position;
    //
    //                 spawnCallback.Invoke(e);
    //     
    //                 NormalReturnCallback?.Invoke(this);
    //             })
    //         ;
    //     yield return t.WaitForCompletion();
    //     yield return null;
    //
    //
    //     lastSummon = null;
    // }
    //
    //
    //
    // public void initialize()
    // {
    //     
    // }
    //
    // public void handleForceReturn()
    // {
    //     // if (lastSummon != null)
    //     // {
    //     //     StopCoroutine(lastSummon);
    //     // }
    //     // lastSummon = null;
    //
    //     if (t != null && t.IsPlaying())
    //     {
    //         t.Kill();
    //     }
    //
    //     t = null;
    // }
    
    public void startSummon(EnemyEntity prefab, Vector3 pos, Action<EnemyEntity> spawnCallback)
    {
        transform.position = pos;
        transform.localScale = Vector3.zero;
        var c = spriter1.color;
        c.a = 0;
        spriter1.color = spriter2.color = c;
        lastSummon = summon(prefab, pos, spawnCallback);
        StartCoroutine(lastSummon);
        Debug.Log($"a === {gameObject.activeSelf}", gameObject);
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
                .Append(transform.DOScale(baseSize *1.3f, animDuration*.8f))
                .Append(transform.DOScale(baseSize, animDuration * .2f))
                .Insert(0,spriter1.DOFade(1,animDuration))
                .Insert(0,spriter2.DOFade(1,animDuration))
                .SetUpdate(true)
                .OnComplete(() => { 
                    var e = GameplayPoolManager.Instance.enemyPool.get(prefab);
                    e.transform.position = transform.position;

                    spawnCallback.Invoke(e);
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
