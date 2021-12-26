using UnityEngine;

namespace Core.Combat
{
    public class Arena: MonoBehaviour
    {
        [SerializeField] Vector2 spawnPadding = 2f * Vector2.one;
        [SerializeField] Vector2 spawnRange;
        
        
        private void Awake()
        {
            spawnRange = new Vector2(
                (transform.localScale.x - spawnPadding.x),
                (transform.localScale.y - spawnPadding.y)
            );
        }

        public Vector2 findSafeSpawnSpot()
        {
            Vector2 randomFactor = (Random.insideUnitCircle) * .5f;
            Vector3 point = new Vector2(
                spawnRange.x * (randomFactor.x),
                spawnRange.y * (randomFactor.y)
            );

            return transform.position + point;
        }

        public Vector2 getCornerInDirection(Vector2 dir)
        {
            return new Vector2(spawnRange.x * Mathf.Sign(dir.x), spawnRange.y * Mathf.Sign(dir.y)) *.5f;
        }
        
        
    }
}