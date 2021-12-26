using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat;
using DG.Tweening;
using UnityEngine;

public class EnemyHealthGaugeView : MonoBehaviour
{
    public HealthComponent healthComponent;
    public SpriteRenderer Fillspriter;
    public SpriteRenderer backGroundSpriter;
    public SpriteRenderer flashSpriter;
    
    private EnemyEntity enemyEntity;
    private MaterialPropertyBlock propBlock;
    private static readonly int FillHeight = Shader.PropertyToID("FillHeight");
    private HypnoComponent hypnoComponent;
    
    public Color berserkFillColor = Color.yellow;
    public Color berserkColor = Color.yellow;
    public Color berserkRangeColor = Color.red;
    public Color normalColor = Color.blue;
    public Color lowHealthColor  = Color.blue;
    
    //too finicky for dotween
    [SerializeField] FiniteTimer berserkTransitionFlashTimer = new FiniteTimer(.6f);
    [SerializeField] float berserkTransitionFlashThresholdMax = .6f;
    [SerializeField] float berserkTransitionFlashThresholdMMin = .2f;
    
    
    public void updateHealthGaugeFill(ResourceComponent resourceComponent)
    {
        if (hypnoComponent.IsBerserked)
        {
            setFillHeight(1- enemyEntity.berserkTimer.Ratio);
        }
        else if(hypnoComponent.IsInBerserkRange)
        {
            flashSpriter.gameObject.SetActive(true);

            doBerserktransitionFlash();
        }
        else
        {
            Fillspriter.color = Color.Lerp(lowHealthColor,normalColor, healthComponent.Ratio);
            setFillHeight(1);
        }
    }

    private void doBerserktransitionFlash()
    {
        float flashThresold = Mathf.Lerp(berserkTransitionFlashThresholdMMin,
            berserkTransitionFlashThresholdMax,
            enemyEntity.berserkTransitionTimer.Ratio);

        berserkTransitionFlashTimer.updateTimer(Time.deltaTime);
        berserkTransitionFlashTimer.maxValue = flashThresold;

        Color flashEndColor = berserkRangeColor;
        flashEndColor.a = 0;
        flashSpriter.color = Color.Lerp(berserkRangeColor, flashEndColor, berserkTransitionFlashTimer.Ratio);

        if (berserkTransitionFlashTimer.isComplete)
            berserkTransitionFlashTimer.reset();

        setFillHeight(1);
    }

    public void setFillHeight(float normalizedHeight)
    {

        Fillspriter.GetPropertyBlock(propBlock);
        
        propBlock.SetFloat(FillHeight, normalizedHeight);
        
        
        Fillspriter.SetPropertyBlock(propBlock);
    }


    #region Setup

    private void Awake()
    {
        healthComponent = GetComponentInParent<HealthComponent>();
        hypnoComponent = healthComponent.GetComponent<HypnoComponent>();
        enemyEntity = healthComponent.GetComponent<EnemyEntity>();
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
            hypnoComponent.Hypnotized += HandleNormalState;
            hypnoComponent.HypnosisRecovered += HandleNormalState;
        }
    }
    
    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.RatioChanged -= updateHealthGaugeFill;
            hypnoComponent.Berserked -= handleBerserked;
            hypnoComponent.Hypnotized -= HandleNormalState;
            hypnoComponent.HypnosisRecovered -= HandleNormalState;
        }
    }

    #endregion

    private void HandleNormalState(HypnoComponent obj)
    {
        flashSpriter.gameObject.SetActive(false);
    }

    private void handleBerserked(HypnoComponent obj)
    {
        Fillspriter.color = berserkColor;
        
        setFillHeight(1);

        flashSpriter.gameObject.SetActive(false);
            
        backGroundSpriter.color = berserkColor;
        Fillspriter.color = berserkFillColor;
    }


}
