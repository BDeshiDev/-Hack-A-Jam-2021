﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;



namespace BDeshi.Utility
{
    /// <summary>
    /// manual pool for POCOs
    /// Creation is done through the Func<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualPool<T>
    {
        [SerializeField] private List<T> pool;//no stack cuz now the whole pool class is serializable!
        private Func<T> CreationMethod;

        public SimpleManualPool(Func<T> creationMethod)
        {
            CreationMethod = creationMethod;
            pool = new List<T>();
        }

        void createAndAddToPool()
        {
            var item = CreationMethod();

            pool.Add(item);
        }

        public T getItem()
        {
            T item = default;
            if (pool.Count > 0)
            {
                item = pool[pool.Count -1];
                pool.RemoveAt(pool.Count - 1);
            }
            else
            {
                item = CreationMethod();
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
            pool.Add(item);
        }
    }
}