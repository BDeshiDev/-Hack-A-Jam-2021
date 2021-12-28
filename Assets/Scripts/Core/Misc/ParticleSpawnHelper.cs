using Core.Combat.Pooling;
using UnityEngine;

namespace Core.Misc
{
    public class ParticleSpawnHelper : MonoBehaviour
    {
        [SerializeField]ParticleHelper prefab;
        
        
        public void  spawn(){
            if(prefab != null)
            {
                var particles = GameplayPoolManager.Instance.particlePool
                    .get(prefab);
                particles.transform.position = transform.position;
                particles.ForcePlay();
            }
        }
    }
}