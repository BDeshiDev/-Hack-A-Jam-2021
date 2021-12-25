using System;
using UnityEngine;

namespace Core.Combat
{
    public class AmmoComponent : MonoBehaviour 
    {
        [SerializeField] int curAmmo = 0;
        [SerializeField] int maxAmmo = 6;

        public bool IsEmpty => curAmmo <= 0;
        public int CurAmmo => curAmmo;
        public int MaxAmmo => maxAmmo;
        
        public event Action<AmmoComponent> onAmountChanged;
        public event Action<AmmoComponent> onEmptied;


        void Awake()
        {
            reload();
        }

        public bool tryUse(int amount )
        {
            if (curAmmo >= amount)
            {
                applyChange(-amount);
                return true;
            }

            return false;
        }

        public void applyChange(int change)
        {
            var oldCur = curAmmo;
            curAmmo = Mathf.Clamp(curAmmo + change, 0, maxAmmo);
            if (curAmmo != oldCur)
            {
                onAmountChanged?.Invoke(this);
                if (oldCur <= Mathf.Abs(change))
                {
                    onEmptied?.Invoke(this);
                }
            }

            // Debug.Log(curAmmo + " " + oldCur + " " + change);
        }


        public void reload(int ammo)
        {
            curAmmo = Mathf.Clamp(curAmmo + ammo, 0 , maxAmmo );
            Debug.Log(curAmmo + "-- " + ammo );
            onAmountChanged?.Invoke(this);
        }

        public void reload()
        {
            curAmmo = maxAmmo;

            onAmountChanged?.Invoke(this);
        }

        public void forceEmpty()
        {
            applyChange(-curAmmo);
        }
    }
}