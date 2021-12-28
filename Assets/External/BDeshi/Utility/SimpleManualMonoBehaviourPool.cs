using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;



namespace BDeshi.Utility
{
    /// <summary>
    /// Simple pool with normal instantiation automated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualMonoBehaviourPool<T> where T : MonoBehaviour
    {
        private List<T> pool;
        protected T prefab;
        private Transform spawnParent;

        public SimpleManualMonoBehaviourPool(T prefab, int initialCount, Transform spawnParent = null)
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

            return item;
        }

        public void ensurePoolHasAtleast(int count)
        {
            for (int i = pool.Count; i <= count; i++)
            {
                createAndAddToPool();
            }
        }

        public void returnItem(T item)
        {
            item.gameObject.SetActive(false);
            pool.Add(item);
        }
    }



}