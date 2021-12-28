using System;
using UnityEngine;

public class BlobEye: MonoBehaviour
{
    public float maxDistance = .75f;
    private SpriteRenderer spriter;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    public void togglePupil(bool shouldShowPupil)
    {
        spriter.enabled = shouldShowPupil;
    }

    public void ChangeLookDir(Vector2 dir)
    {
        transform.localPosition = dir * maxDistance;
    }
}