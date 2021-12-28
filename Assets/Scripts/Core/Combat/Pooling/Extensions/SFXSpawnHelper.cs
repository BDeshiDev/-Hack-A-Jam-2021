using UnityEngine;

namespace Core.Combat.Pooling.Extensions
{
    public class SFXSpawnHelper : MonoBehaviour
    {
        [SerializeField] PoolableSFXPlayer prefab;
        
        
        public void  spawn(){
            if(prefab != null)
            {
                var particles = GameplayPoolManager.Instance
                    .sfxPool
                    .get(prefab);
            }
        }
    }
}