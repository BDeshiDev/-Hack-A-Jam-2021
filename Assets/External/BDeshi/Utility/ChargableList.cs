using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDeshi.Utility
{
    [Serializable]
    public class ChargableList<TItem> : IEnumerable<TItem>
    {
        [SerializeField] private int chargeIndex;
        [SerializeField] private List<ChargableListSlot> items = new List<ChargableListSlot>();
        [SerializeField] private bool ChargeComplete => chargeIndex >= (items.Count) ||
                                                        ( items.Count > 0 
                                                        && items[chargeIndex].chargeTimer.isComplete);

        [SerializeField] private bool hasMoreChargeLevels => (items.Count -1 ) > chargeIndex;

        public void updateCharge(float amount, Func<TItem, TItem,bool > levelIncreasePermissionFunc = null)
        {
            if (ChargeComplete)
            {
                return;
            }

            if (items[chargeIndex].chargeTimer.tryCompleteTimer(amount))
            {
                if (hasMoreChargeLevels &&
                    (levelIncreasePermissionFunc == null
                     || levelIncreasePermissionFunc.Invoke(items[chargeIndex].item, items[chargeIndex + 1].item)))
                {
                    chargeIndex++;
                    if (chargeIndex < items.Count)
                    {
                        items[chargeIndex].chargeTimer.reset();
                    }
                }
            }

        }

        public TItem getCurrentItem()
        {
            if (items.Count <= 0)
                return default(TItem);
            return items[chargeIndex].item;
        }

        public void resetChargeLevel()
        {
            chargeIndex = 0;
            if (chargeIndex < items.Count)
            {
                items[chargeIndex].chargeTimer.reset();
            }
        }

        public TItem getItemAndReset()
        {
            var result =  getCurrentItem();
            resetChargeLevel();
            return result;
        }
        [Serializable]
        public class ChargableListSlot
        {
            [SerializeField] public TItem item;
            [SerializeField] public FiniteTimer chargeTimer = new FiniteTimer(0.5f);
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var chargableListSlot in items)
            {
                yield return chargableListSlot.item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}