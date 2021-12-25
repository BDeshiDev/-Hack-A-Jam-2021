using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BDeshi.Utility
{
    public class AutoMonobehaviourPool<T> where T : MonoBehaviour, AutoPoolable<T>
    {
        private List<T> pool;
        private HashSet<T> loaned = new HashSet<T>();
        
        protected T prefab;
        private Transform spawnParent;

        public AutoMonobehaviourPool(T prefab, int initialCount, Transform spawnParent = null)
        {
            this.prefab = prefab;
            this.spawnParent = spawnParent;
            pool = new List<T>();
            while (initialCount > 0)
            {
                initialCount--;
                createAndAddToPool();
            }
        }

        T createItem()
        {
            T item = default(T);
                        
            if (spawnParent != null)
            {
                item = Object.Instantiate(this.prefab, spawnParent,false);
            }
            else
            {
                item = Object.Instantiate(this.prefab);
            }


            return item;
        }


        void createAndAddToPool()
        {
            var item = createItem();
            
            item.gameObject.SetActive(false);
            pool.Add(item);
        }

        public T getItem()
        {
            T item = null;
            if (pool.Count > 0)
            {
                item = pool[pool.Count -1];
                pool.RemoveAt(pool.Count - 1);
                item.gameObject.SetActive(true);
            }
            else
            {
                item = createItem();
            }
            item.ReturnCallback += handleNormalReturn;
            item.initialize();
            
            loaned.Add(item);
            
            return item;
        }

        public void ensurePoolHasAtleast(int count)
        {
            for (int i = pool.Count; i <= count; i++)
            {
                createAndAddToPool();
            }
        }

        public void returnAll()
        {
            foreach (var item in loaned)
            {
                handleReturnInternal(item);
            }
            
            loaned.Clear();
        }


        void handleNormalReturn(T item)
        {
            handleReturnInternal(item);
            loaned.Remove(item);
        }

        void handleReturnInternal(T item)
        {
            item.gameObject.SetActive(false);
            item.ReturnCallback -= handleNormalReturn;

            pool.Add(item);
        }
    }
    
    public interface AutoPoolable<T>
    {
        void initialize();
        void forceReturn();
        event Action<T> ReturnCallback;
    }
}