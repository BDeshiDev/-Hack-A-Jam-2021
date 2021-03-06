using System;
using BDeshi.Utility;
using UnityEngine;

namespace Core.Misc
{
    public class ParticleHelper : MonoBehaviour, AutoPoolable<ParticleHelper>
    {
        [SerializeField] private ParticleSystem particleSystem;

        public void ForcePlay()
        {
            if(particleSystem.isPlaying)
                particleSystem.Stop();
            particleSystem.Play();
        }

        public void setColor(Color c)
        {
            var m = particleSystem.main;
            m.startColor = c;
        }

        public void initialize()
        {
            ForcePlay();
        }
    
        public void OnParticleSystemStopped()
        {
            NormalReturnCallback?.Invoke(this);
        }

        public void handleForceReturn()
        {
        
        }

        public event Action<ParticleHelper> NormalReturnCallback;
    }
}
