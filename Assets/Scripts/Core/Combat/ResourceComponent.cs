﻿using System;
using UnityEngine;

namespace Core.Combat
{
    public class ResourceComponent : MonoBehaviour
    {
        public float Max => max;
        [SerializeField] float max = 100;
        public float Cur => cur;
        [SerializeField] float cur;

        public float Ratio => ((float)cur) / max;

        public bool IsFull => cur>= max;
        public bool IsEmpty => cur <= 0;

        public event Action<ResourceComponent> Emptied;
        public event Action<ResourceComponent> Capped;
        public event Action<ResourceComponent> RatioChanged;
        

        public void fullyRestore()
        {
            if (IsEmpty)
            {
                cur = max;
            }
            else {
                cur = max;
            }

            RatioChanged?.Invoke(this);
        }

        public void reduceAmount(float amount)
        {
            modifyAmount(-amount);
        }

        /// <summary>
        /// MOdify resource amount clamped by max and min properly
        /// </summary>
        /// <param name="changeAmount">USE -VE IF IT'S DAMAGE </param>
        public void modifyAmount(float changeAmount)
        {
            if(changeAmount < 0)
            {
                if (cur < Mathf.Abs(changeAmount))
                {
                    if(cur > 0)
                    {
                        cur = 0;
                        Emptied?.Invoke(this);
                    }
                }
                else
                {
                    cur += changeAmount;
                }
            }
            else
            {
                if (cur < max)
                {
                    Debug.Log(cur);
                    cur += changeAmount;
                    if (cur > max)
                    {
                        cur = max;
                        Capped?.Invoke(this);
                    }
                }
            }

            RatioChanged?.Invoke(this);
        }
    }
}