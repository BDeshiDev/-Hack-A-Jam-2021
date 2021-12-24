using System;
using System.Collections;
using System.Collections.Generic;
using Core.Combat;
using UnityEngine;

public class HealthGaugeView : MonoBehaviour
{
    public HealthComponent healthComponent;
    public SpriteRenderer Fillspriter;
    private MaterialPropertyBlock propBlock;
    private static readonly int FillHeight = Shader.PropertyToID("FillHeight");

    public void updateHealthGaugeFill(ResourceComponent resourceComponent)
    {
        Fillspriter.GetPropertyBlock(propBlock);
        
        // propBlock.SetColor("SOMECOLOR", );
        propBlock.SetFloat("FillHeight",resourceComponent.Ratio );

        
        Fillspriter.SetPropertyBlock(propBlock);
    }
    

    private void Awake()
    {
        healthComponent = GetComponentInParent<HealthComponent>();
        propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        updateHealthGaugeFill(healthComponent);
    }
    


    private void OnEnable()
    {
        if (healthComponent != null)
        {
            healthComponent.RatioChanged += updateHealthGaugeFill;
        }
    }

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.RatioChanged -= updateHealthGaugeFill;
        }
    }
}
