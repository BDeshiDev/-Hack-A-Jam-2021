using System;
using System.Collections;
using System.Collections.Generic;
using Core.Combat;
using UnityEngine;

public class BlobEyeAnimator : MonoBehaviour
{
    [SerializeField] private BlobEye e1;
    [SerializeField] private BlobEye e2;
    private BlobEntity blobEntity;
    private void Awake()
    {
        blobEntity = GetComponentInParent<BlobEntity>();
    }

    private void Update()
    {
        e1.ChangeLookDir(blobEntity.LastLookDir);
        e2.ChangeLookDir(blobEntity.LastLookDir);
    }
}