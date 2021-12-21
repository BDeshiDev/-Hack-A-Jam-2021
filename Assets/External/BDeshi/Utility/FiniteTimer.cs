using UnityEngine;

namespace BDeshi.Utility
{
    [System.Serializable]
    public struct FiniteTimer
    {
        public float timer;
        public float maxValue;

        public void init(float maxval, float startVal = 0)
        {
            timer = startVal;
            maxValue = maxval;
        }

        public FiniteTimer(float timerStart, float maxVal, bool completed = false)
        {
            timer = completed ? maxVal : timerStart;
            maxValue = maxVal;
        }

        public FiniteTimer(float maxVal = 3, bool completed = false)
        {
            timer = completed ? maxVal : 0;
            maxValue = maxVal;
        }




        public void updateTimer(float delta)
        {
            timer += delta;
        }

        public bool tryCompleteTimer(float delta)
        {
            return tryCompleteTimer(delta, out var r);
        }

        public bool tryCompleteTimer(float delta, out float remainder)
        {
            remainder = delta;
            if (isComplete)
                return true;
            timer += delta;
            if (timer > maxValue)
            {
                remainder = timer - maxValue;
                return true;
            }

            remainder = 0;
            return false;
        }

        public void clampedUpdateTimer(float delta)
        {
            timer = Mathf.Clamp(timer + delta, 0, maxValue);
        }

        public void safeUpdateTimer(float delta)
        {
            if (timer < maxValue)
                timer += delta;
        }

        public void reset()
        {
            timer = 0;
        }

        public void resetByFractionOfMax(float frac)
        {
            timer = Mathf.Max(0, timer - frac * maxValue);
        }

        public void reset(float newMax)
        {
            maxValue = newMax;
            reset();
        }

        public void cyclicReset()
        {
            if (Ratio > 1)
                timer = maxValue * (Ratio - 1);
            else
                reset();
        }

        public void complete()
        {
            timer = maxValue;
        }

        public bool isComplete => timer >= maxValue;

        public bool exceedsRatio(float ratioToExceed)
        {
            return Ratio >= ratioToExceed;
        }

        public float Ratio => Mathf.Clamp01(timer / maxValue);

        public float ReverseRatio => 1 - Ratio;
    }
}