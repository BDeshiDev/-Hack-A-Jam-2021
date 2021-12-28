using Core.Combat.Entities;
using UnityEngine;

namespace Core.Combat.Visuals
{
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
}