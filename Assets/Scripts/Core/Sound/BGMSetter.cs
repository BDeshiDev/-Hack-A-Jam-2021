using System;
using UnityEngine;

namespace Core.Misc
{
    public class BGMSetter : MonoBehaviour
    {
        public AudioClip clip;
        public bool shouldRestartIfSame = false;
        private void Start()
        {
            setBGM();
        }

        public void setBGM()
        {
            MusicManager.Instance.playClip(clip,true, shouldRestartIfSame);
        }
    }
}