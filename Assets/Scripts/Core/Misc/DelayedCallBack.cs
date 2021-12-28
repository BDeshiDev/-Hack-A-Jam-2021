using BDeshi.Utility;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Core.Misc
{
    public class DelayedCallBack : MonoBehaviour
    {
        private FiniteTimer timer = new FiniteTimer();
        private float range = .6f;
        public UnityEvent callback;
        private void Awake()
        {
            timer = new FiniteTimer(Random.Range(0,range));
        }

        private void OnEnable()
        {
            timer.reset();
        }

        void Update()
        {
            if (!timer.isComplete)
            {
                if (timer.tryCompleteTimer(Time.deltaTime))
                    callback.Invoke();
            }
        }
    }
}
