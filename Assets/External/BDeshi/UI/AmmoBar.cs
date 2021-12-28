using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat;
using Core.Combat.Shooting;
using UnityEngine;
using UnityEngine.UI;

namespace BDeshi.UI
{
    public class AmmoBar : MonoBehaviour
    {
        [SerializeField] Image ammoClipPrefab;
        [SerializeField] SimpleManualMonoBehaviourPool<Image> ammoClipPool;
        [SerializeField] List<Image> ammoClipList;
        [SerializeField] private float ammoWidth = 10;
        void Awake()
        {
            ammoClipPool = new SimpleManualMonoBehaviourPool<Image>(ammoClipPrefab,1, transform);
        }

        public void init(AmmoComponent ammoComp)
        {
            HorizontalLayoutGroup layoutgroup = GetComponent<HorizontalLayoutGroup>();
            RectTransform rect = GetComponent<RectTransform>();
            ammoWidth = calcAmmoWidth(rect.sizeDelta.x, layoutgroup.spacing,  ammoComp.MaxAmmo);
        }

        public void updateFromRatio(AmmoComponent r)
        {
            int targetCount = r.CurAmmo;
            targetCount = targetCount > 0 ? targetCount : 0;

            displayAmmo(targetCount);
        }
        
        
        public void displayAmmo(int count)
        {
            if (count > ammoClipList.Count)
            {
                for (int i = ammoClipList.Count; i < count; i++)
                {
                    var a = ammoClipPool.getItem();
                    a.rectTransform.sizeDelta = new Vector2(ammoWidth, a.rectTransform.sizeDelta.y);
                    ammoClipList.Add(a);
                    a.transform.SetParent(transform);
                }
            }
            else if (count < ammoClipList.Count)
            {
                for (int i = ammoClipList.Count - 1; i >= count; i--)
                {
                    ammoClipPool.returnItem(ammoClipList[i]);
                    ammoClipList.RemoveAt(i);
                }
            }
        }
        
        public float calcAmmoWidth(float ammoBarWidth,float ammoGap, int maxCount)
        {
            float ammoGapTotal = ((maxCount > 0) ? (maxCount -1) : 0) * ammoGap;
            return (ammoBarWidth - ammoGapTotal) / maxCount;
        }

    }
}