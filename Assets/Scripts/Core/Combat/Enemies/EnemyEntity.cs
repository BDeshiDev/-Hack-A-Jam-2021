using System;
using BDeshi.Utility;
using Core.Combat.Enemies;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public abstract class EnemyEntity: BlobEntity, AutoPoolable<EnemyEntity>
    {
        public HypnoComponent HypnoComponent { get; private set; }
        public override TargetResolverComponent TargetResolverComponent => targetter;
        public bool IsHypnotized => HypnoComponent.IsHypnotized;

        [SerializeField] protected EnemyTargetResolver targetter;
        [SerializeField] protected FiniteTimer attackCoolDown = new FiniteTimer(0, 2f);
        public FiniteTimer berserkTransitionTimer = new FiniteTimer(0,6f);
        public FiniteTimer berserkTimer = new FiniteTimer(0,6.5f);

        public float berserkCoolDownMultiplier = .5f;
        public float berserkHypnoDamageConversionFactor = .08f;
        public float normalCoolDownDuration = 2f;

        public float TimeSpentHypnotized { get; private set; } = 0;
        
        public UnityEvent SelfBerserkedEvent;
        public UnityEvent SelfHypnotizedEvent;

        public bool TrulyDead => healthComponent.IsEmpty && berserkTimer.isComplete;
        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            float hypnoDamage = HypnoComponent.calcHypnoDamage(damage);

            HypnoComponent.modifyAmount(hypnoDamage);

            //in berserk state, make berserk last less when hit
            if (HypnoComponent.IsBerserked)
            {
                berserkTimer.updateTimer(damage.hypnoDamage * berserkHypnoDamageConversionFactor);
            }
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
            attackCoolDown.safeUpdateTimer(Time.deltaTime);


            if (HypnoComponent.IsHypnotized)
            {
                TimeSpentHypnotized += Time.deltaTime;
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
        
        protected void handleDeath(ResourceComponent healthComp)
        {
            if (HypnoComponent.wasHypnotizedBefore)
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
            invokeDeathEvent();
            CombatEventManger.Instance.OnEnemyDefeated.Invoke(this);
            normalReturn();
            // Destroy(gameObject);
        }

        protected void HypnosisRecovered(HypnoComponent obj)
        {
            targetter.handleNormalState();

            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
            
            CombatEventManger.Instance.OnEnemyHypnosisRecovery.Invoke(this);
        }


        protected virtual void HandleBerserked(HypnoComponent obj)
        {
            healthComponent.forceEmpty();
            targetter.handleBerserk();
            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
            
            attackCoolDown.reset( attackCoolDown.maxValue * berserkCoolDownMultiplier);
            fsm.handleEvent(Events.Berserk);
            
            CombatEventManger.Instance.OnEnemyBerserked.Invoke(this);
            
            SelfBerserkedEvent.Invoke();
        }

        protected void OnHypnotized(HypnoComponent obj)
        {
            targetter.handleHypnosis();
            targetter.gameObject.layer = targetter.TargettingInfo.HypnotizedLayer.LayerIndex;
            
            CombatEventManger.Instance.OnEnemyHypnotized.Invoke(this);
            
            SelfHypnotizedEvent.Invoke();
        }


        public virtual void initialize()
        {
            HypnoComponent.initialize();
            healthComponent.fullyRestore();
            
            targetter.initialize();
            berserkTimer.reset();
            berserkTransitionTimer.reset();

            initializeFSM();
            
            attackCoolDown.reset(normalCoolDownDuration);

            TimeSpentHypnotized = 0;
        }

        public void handleForceReturn()
        {
            if (fsm != null && fsm.curState != null)
            {
                fsm.exitCurState();
            }
        }

        void normalReturn()
        {
            handleForceReturn();
            NormalReturnCallback?.Invoke(this);
        }


        public event Action<EnemyEntity> NormalReturnCallback;
    }
    
    
}