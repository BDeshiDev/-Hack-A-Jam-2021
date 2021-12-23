using System;
using BDeshi.BTSM;
using BDeshi.Utility.Extensions;
using Core.Physics;
using UnityEngine;

namespace Core.Combat
{
    public abstract class EnemyEntity: CombatEntity
    {
        [SerializeField] protected HypnoComponent hypnoComponent;
        public MoveComponent moveComponent { get; private set; }
        [SerializeField] protected bool isHypnotized = false;
        [SerializeField] private SpriteRenderer spriter;
        /// <summary>
        /// at t = T% hypnodamage taken at T%/100 health and so on
        /// </summary>
        public AnimationCurve hypnoDamageVsHealthCurve = AnimationCurve.EaseInOut(0,1,1,0);
        public float hypnoDamageHealthScalingFactor = 5;
        public float hypnoRecoveryPerHealthDamage = .5f;

        public FSMRunner fsmRunner;
        public abstract StateMachine createFSM();
        public Transform target;
        public Transform aimer;

        /// <summary>
        /// Health and hypnosis are modified
        /// </summary>
        /// <param name="damage"></param>
        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            float hypnoDamage = calcHypnoDamage(damage);
            hypnoComponent.modifyAmount(hypnoDamage);
        }

        public float calcHypnoDamage(DamageInfo damage)
        {
            return ( 1 + damage.hypnoDamage * hypnoDamageVsHealthCurve.Evaluate(healthComponent.Ratio))
                   * hypnoDamageHealthScalingFactor
                   - (isHypnotized? 1 : 0) * hypnoRecoveryPerHealthDamage;
        }

        public void lookAlong(Vector3 dir)
        {
            aimer.allignToDir(dir);
        }

        protected override void Awake()
        {
            base.Awake();
            hypnoComponent = GetComponent<HypnoComponent>();
            spriter = GetComponent<SpriteRenderer>();
            fsmRunner = GetComponent<FSMRunner>();
            moveComponent = GetComponent<MoveComponent>();
            
            target = GameObject.FindWithTag("Player").transform;

        }

        private void Start()
        {
            fsmRunner.Initialize(createFSM());
        }


        private void OnEnable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Capped += OnHypnotized;
                hypnoComponent.Emptied += HypnosisRecovered;
            }
        }
        
        private void OnDisable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Capped -= OnHypnotized;
                hypnoComponent.Emptied -= HypnosisRecovered;
            }
        }

        protected void HypnosisRecovered(ResourceComponent obj)
        {
            if (isHypnotized)
            {
                Debug.Log("revcover hypnotise " +gameObject , gameObject);
            }
            spriter.color = Color.yellow;
            isHypnotized = false;
        }

        protected void OnHypnotized(ResourceComponent obj)
        {
            isHypnotized = true;
            Debug.Log("hypnotised " +gameObject , gameObject);
            
            spriter.color = Color.cyan;

        }
    }
    
    
}