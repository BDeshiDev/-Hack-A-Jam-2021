using System;
using System.Collections;
using System.Collections.Generic;
using Core.Input;
using UnityEngine;
using Random = UnityEngine.Random;

public class SFXPlayer : MonoBehaviour
{
    [Range(-3,3)]
    public float pitchMin = 1;
    
    [Range(-3,3)]
    public float pitchMax = 1;
    [SerializeField] private AudioSource source;
    [SerializeField] private bool debug = false;
    [SerializeField] private bool playOnEnable = false;
    public void play()
    {
        source.Play();
    }
    
    public void playRandomized()
    {
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();
    }

    private void Start()
    {
# if UNITY_EDITOR
        if(debug)
            InputManager.debugButton1.addPerformedCallback(gameObject, playRandomized);
# endif
    }

    private void OnEnable()
    {
        if(playOnEnable)
            playRandomized();
    }
}
