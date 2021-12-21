using System.Collections;
using System.Collections.Generic;
using bdeshi.utility;
using BDeshi.Utility;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviourSingletonPersistent<MusicManager>
{
    [SerializeField] private AudioSource source1;
    [SerializeField] private AudioSource source2;
    [SerializeField] private AudioSource curSource;
    [SerializeField] private AudioSource otherSource;
    public AudioMixerGroup bgmGroup;

    [SerializeField] FiniteTimer fadeOutTimer = new FiniteTimer(0, .33f);
    [SerializeField] FiniteTimer fadeInTimer = new FiniteTimer(0, .33f);


    public bool muteBGMInEditor = true;

    [SerializeField] private AudioClip curTrack;
    private IEnumerator CustomTrackCoroutine = null;
    private IEnumerator VolumeLerpCoroutine = null;
    
    protected override void initialize()
    {
        source1 = gameObject.AddComponent<AudioSource>();
        source2 = gameObject.AddComponent<AudioSource>();
        source1.outputAudioMixerGroup = source2.outputAudioMixerGroup = bgmGroup;
        source1.loop = source2.loop = true;
        curSource = source1;
        otherSource = source2;
#if UNITY_EDITOR
        //protect my sanity and the sanctity of my ears
        if(muteBGMInEditor)
        {
            source1.mute = true;
            source2.mute = true;
        }
#endif
    }



    public void setPitch(float pitch)
    {
        curSource.pitch = pitch;
    }

    public void setVolume(float volume)
    {
        curSource.volume = volume;
    }
    
    private IEnumerator fadeVolume(float finalVolume,float fadeTime)
    {
        float startVolume = curSource.volume;
        FiniteTimer timer = new FiniteTimer(0, fadeTime);
        while (!timer.isComplete)
        {
            timer.updateTimer(Time.deltaTime);
            curSource.volume = Mathf.Lerp(startVolume, finalVolume, timer.Ratio);
            yield return null;
        }

        curSource.volume = finalVolume;
        VolumeLerpCoroutine = null;
    }

    public void doFadeVolume(float finalVolume, float fadeTime)
    {
        if(VolumeLerpCoroutine != null)
            StopCoroutine(VolumeLerpCoroutine);

        VolumeLerpCoroutine = fadeVolume(finalVolume, fadeTime);
        StartCoroutine(VolumeLerpCoroutine);
    }



    public void playClip(AudioSource source, AudioClip clip,bool shouldLoop)
    {
        curSource.clip = clip;
        curSource.loop = shouldLoop;
        curSource.Play();
    }

    public void fadeInFadeOutTrack([CanBeNull] AudioClip trackToFadeIn)
    {
        StartCoroutine(doFadeInFadeOutTrack(trackToFadeIn));
    }

    public IEnumerator doFadeInFadeOutTrack([CanBeNull]AudioClip trackToFadeIn)
    {
        fadeInTimer.reset();
        fadeOutTimer.reset();
        if (curSource.clip == null)
        {
            fadeOutTimer.complete();
        }
        
        otherSource.clip = trackToFadeIn;
        
        if (otherSource.clip == null)
        {
            fadeInTimer.complete();
        }
        else
        {
            otherSource.Play();
        }

        while (true)
        {
            fadeInTimer.updateTimer(Time.deltaTime);
            fadeOutTimer.updateTimer(Time.deltaTime);
                
            curSource.volume = Mathf.Lerp(1, 0, fadeOutTimer.Ratio);
            otherSource.volume = Mathf.Lerp(0, 1, fadeInTimer.Ratio);
            
            if (fadeInTimer.isComplete && fadeOutTimer.isComplete)
                break;
            yield return null;
        }
        
        curSource.Stop();
        curSource.clip = null;
        (curSource, otherSource) = (otherSource, curSource);

#if UNITY_EDITOR
        if (muteBGMInEditor)
        {
            curSource.volume = 0;
            Debug.LogWarning("volume muted IN EDITOR ONLY ");
        }
#endif
    }
}