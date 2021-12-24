using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BDeshi.Utility
{
    public class AutoMonobehaviourPool<T> where T : MonoBehaviour, AutoPoolable<T>
    {
        private List<T> pool;
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
            if (spawnParent != null)
            {
                return Object.Instantiate(this.prefab, spawnParent,false);
            }

            return Object.Instantiate(this.prefab);
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
            item.ReturnCallback += returnItem;
            item.initialize();
            
            return item;
        }

        public void ensurePoolHasAtleast(int count)
        {
            for (int i = pool.Count; i <= count; i++)
            {
                createAndAddToPool();
            }
        }

        void returnItem(T item)
        {
            item.gameObject.SetActive(false);
            item.ReturnCallback -= returnItem;

            pool.Add(item);
        }
    }
    
    public interface AutoPoolable<T>
    {
        void initialize();
        event Action<T> ReturnCallback;
    }
}