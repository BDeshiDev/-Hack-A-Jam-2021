using UnityEngine;

namespace BDeshi.Utility
{
    public abstract class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
                
                initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected abstract void initialize();
        
    }
}