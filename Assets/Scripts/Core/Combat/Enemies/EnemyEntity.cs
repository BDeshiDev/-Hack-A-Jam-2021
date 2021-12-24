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
        [SerializeField] protected HypnoComponent hypnoComponent;
        public EnemyTargetResolver Targetter => targetter;
        public bool IsHypnotized => hypnoComponent.IsHypnotized;

        [SerializeField] protected EnemyTargetResolver targetter;
        [SerializeField] private FiniteTimer berserkTimer = new FiniteTimer(0,6f);
        /// <summary>
        /// Health and hypnosis are modified
        /// </summary>
        /// <param name="damage"></param>
        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            float hypnoDamage = hypnoComponent.calcHypnoDamage(damage);

            hypnoComponent.modifyAmount(hypnoDamage);
        }

        protected virtual void Update()
        {
            if (hypnoComponent.IsBerserked)
            {
                if (berserkTimer.tryCompleteTimer(Time.deltaTime))
                {
                    actuallyDie();
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            hypnoComponent = GetComponent<HypnoComponent>();
            spriter = GetComponent<SpriteRenderer>();
            targetter = GetComponent<EnemyTargetResolver>();
        }

        private void OnEnable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Hypnotized += OnHypnotized;
                hypnoComponent.HypnosisRecovered += HypnosisRecovered;
                hypnoComponent.Berserked += HandleBerserked;

            }

            if (healthComponent != null)
            {
                healthComponent.Emptied += handleDeath;
            }

            EnemyTracker.addNewActiveEnemy(this);
        }



        private void OnDisable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Hypnotized -= OnHypnotized;
                hypnoComponent.HypnosisRecovered -= HypnosisRecovered;
                hypnoComponent.Berserked -= HandleBerserked;
            }
            EnemyTracker.removeInactiveEnemy(this);
        }
        
        private void handleDeath(ResourceComponent obj)
        {
            if (hypnoComponent.hypnoDOTActive)
            {
                hypnoComponent.forceBerserkState();
            }
            else
            {
                actuallyDie();
            }
        }

        void actuallyDie()
        {
            Debug.Log(gameObject + " has died " + gameObject);
            Destroy(gameObject);
        }

        protected void HypnosisRecovered()
        {
            targetter.handleNormalState();

            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
        }


        protected virtual void HandleBerserked()
        {
            Debug.Log("berserked  " +gameObject , gameObject);

            targetter.handleBerserk();
            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
        }

        protected void OnHypnotized()
        {
            Debug.Log("hypnotised " +gameObject , gameObject);
            
            targetter.handleHypnosis();
            targetter.gameObject.layer = targetter.TargettingInfo.HypnotizedLayer.LayerIndex;
        }
    }
    
    
}