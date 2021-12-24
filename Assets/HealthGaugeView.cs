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
    private HypnoComponent hypnoComponent;
    
    public Color berserkColor = Color.yellow;
    public Color berserkRangeColor = Color.red;
    public Color normalColor = Color.blue;
    
    public void updateHealthGaugeFill(ResourceComponent resourceComponent)
    {
        Fillspriter.color = hypnoComponent.IsBerserked
                            ? Color.yellow 
                            : hypnoComponent.IsInBerserkRange
                                ? berserkColor
                                : normalColor;
        Fillspriter.GetPropertyBlock(propBlock);
        
        // propBlock.SetColor("SOMECOLOR", );
        propBlock.SetFloat("FillHeight",hypnoComponent.IsBerserked? 1 : resourceComponent.Ratio );

        
        Fillspriter.SetPropertyBlock(propBlock);
    }
    

    private void Awake()
    {
        healthComponent = GetComponentInParent<HealthComponent>();
        hypnoComponent = healthComponent.GetComponent<HypnoComponent>();
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
            hypnoComponent.Berserked += handleBerserked;
        }
    }

    private void handleBerserked()
    {
        Fillspriter.color = berserkColor;
    }

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.RatioChanged -= updateHealthGaugeFill;
            hypnoComponent.Berserked -= handleBerserked;
        }
    }
}
