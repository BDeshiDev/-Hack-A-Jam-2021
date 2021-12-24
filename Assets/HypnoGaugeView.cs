using System;
using System.Collections;
using System.Collections.Generic;
using Core.Combat;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HypnoGaugeView : MonoBehaviour
{
    public HypnoComponent hypnoComponent;
    public SpriteRenderer hypnoGaugeFillSpriter;
    private MaterialPropertyBlock hypnGaugeFillPropertyBlock;
    private static readonly int FillHeight = Shader.PropertyToID("FillHeight");
    public Color hypnotizedColor;
    public Color normalColor;

    public void updateHypnoGaugeFill(ResourceComponent resourceComponent)
    {
        hypnoGaugeFillSpriter.GetPropertyBlock(hypnGaugeFillPropertyBlock);
        // Debug.Log("fill hips " + resourceComponent.Ratio);
        // hypnGaugeFillPropertyBlock.SetColor("SOMECOLOR", );
        hypnGaugeFillPropertyBlock.SetFloat("FillHeight",resourceComponent.Ratio );

        
        hypnoGaugeFillSpriter.SetPropertyBlock(hypnGaugeFillPropertyBlock);
    }

    private void Awake()
    {
        hypnoComponent = GetComponentInParent<HypnoComponent>();
        hypnGaugeFillPropertyBlock = new MaterialPropertyBlock();
        hypnoGaugeFillSpriter.color = normalColor;
    
        
        handleHypnosisRecovery();
    }

    private void Start()
    {
        updateHypnoGaugeFill(hypnoComponent);
    }

    private void Update()
    {
        updateHypnoGaugeFill(hypnoComponent);
    }


    private void OnEnable()
    {
        if (hypnoComponent != null)
        {
            hypnoComponent.RatioChanged += updateHypnoGaugeFill;
            hypnoComponent.Hypnotized += handleHypnotized;
            hypnoComponent.HypnosisRecovered += handleHypnosisRecovery;
        }
    }

    private void handleHypnosisRecovery()
    {
        hypnoGaugeFillSpriter.color = normalColor;
    }

    private void handleHypnotized()
    {
        hypnoGaugeFillSpriter.color = hypnotizedColor;
    }

    private void OnDisable()
    {
        if (hypnoComponent != null)
        {
            hypnoComponent.RatioChanged -= updateHypnoGaugeFill;
            hypnoComponent.Hypnotized -= handleHypnotized;
            hypnoComponent.HypnosisRecovered -= handleHypnosisRecovery;
        }
    }
}
