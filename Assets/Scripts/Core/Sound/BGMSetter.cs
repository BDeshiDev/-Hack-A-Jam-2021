using UnityEngine;

namespace Core.Sound
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