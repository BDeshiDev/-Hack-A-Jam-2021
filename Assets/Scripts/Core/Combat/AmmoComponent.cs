using System;
using UnityEngine;

namespace Core.Combat
{
    public class AmmoComponent : MonoBehaviour 
    {
        [SerializeField] int curAmmo = 0;
        [SerializeField] int maxAmmo = 6;
        [SerializeField] int maxOverFlow = 5;

        public bool IsEmpty => curAmmo <= 0;
        public bool IsFull => curAmmo >= maxAmmo;
        public int CurAmmo => curAmmo;
        public int MaxAmmo => maxAmmo;
        public int MaxOverFlow=> maxOverFlow;
        
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
            curAmmo = clipAmmo(curAmmo + change);
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

        public int clipAmmo(int totalAmmoAmount)
        {
            return Mathf.Clamp(totalAmmoAmount, 0 , maxAmmo + maxOverFlow );
        }


        public void reload(int ammo)
        {
            curAmmo = clipAmmo(curAmmo + ammo);
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