using System;
using BDeshi.Utility;
using Core.Combat.Enemies;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public abstract class EnemyEntity: BlobEntity
    {
        public HypnoComponent HypnoComponent { get; private set; }

        public EnemyTargetResolver Targetter => targetter;
        public bool IsHypnotized => HypnoComponent.IsHypnotized;

        [SerializeField] protected EnemyTargetResolver targetter;
        [SerializeField] private FiniteTimer berserkTimer = new FiniteTimer(0,6f);
        [SerializeField] private FiniteTimer berserkTransitionTimer = new FiniteTimer(0,6f);
        /// <summary>
        /// Health and hypnosis are modified
        /// </summary>
        /// <param name="damage"></param>
        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            float hypnoDamage = HypnoComponent.calcHypnoDamage(damage);

            HypnoComponent.modifyAmount(hypnoDamage);
        }

        protected virtual void Update()
        {

            if (HypnoComponent.IsBerserked)
            {
                if (berserkTimer.tryCompleteTimer(Time.deltaTime))
                {
                    actuallyDie();
                }
            }else if(HypnoComponent.IsInBerserkRange)
            {
                if (berserkTransitionTimer.isComplete)
                {
                    HypnoComponent.forceBerserkState();
                }
                else
                {
                    berserkTransitionTimer.updateTimer(Time.deltaTime);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            HypnoComponent = GetComponent<HypnoComponent>();
            spriter = GetComponent<SpriteRenderer>();
            targetter = GetComponent<EnemyTargetResolver>();
        }

        private void OnEnable()
        {
            if(HypnoComponent != null)
            {
                HypnoComponent.Hypnotized += OnHypnotized;
                HypnoComponent.HypnosisRecovered += HypnosisRecovered;
                HypnoComponent.Berserked += HandleBerserked;

            }

            if (healthComponent != null)
            {
                healthComponent.Emptied += handleDeath;
            }

            EnemyTracker.addNewActiveEnemy(this);
        }



        private void OnDisable()
        {
            if(HypnoComponent != null)
            {
                HypnoComponent.Hypnotized -= OnHypnotized;
                HypnoComponent.HypnosisRecovered -= HypnosisRecovered;
                HypnoComponent.Berserked -= HandleBerserked;
            }
            EnemyTracker.removeInactiveEnemy(this);
        }
        
        private void handleDeath(ResourceComponent obj)
        {
            if (HypnoComponent.hypnoDOTActive)
            {
                HypnoComponent.forceBerserkState();
            }
            else
            {
                actuallyDie();
            }
        }

        void actuallyDie()
        {
            Destroy(gameObject);
        }

        protected void HypnosisRecovered()
        {
            targetter.handleNormalState();

            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
        }


        protected virtual void HandleBerserked()
        {

            targetter.handleBerserk();
            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
        }

        protected void OnHypnotized()
        {
            
            targetter.handleHypnosis();
            targetter.gameObject.layer = targetter.TargettingInfo.HypnotizedLayer.LayerIndex;
        }
    }
    
    
}