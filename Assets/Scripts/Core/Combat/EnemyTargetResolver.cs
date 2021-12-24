using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat.Enemies;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Core.Combat
{
    public class EnemyTargetResolver : TargetResolverComponent
    {
        [SerializeField] private TargettingInfo normalTargettingInfo;
        [SerializeField] private TargettingInfo hypnotizedTargettingInfo;
        [SerializeField] private TargettingInfo berserkTargettingInfo;
        
        public TargettingState state;
        public Transform player;


        public EnemyEntity mostAggroedTarget = null;
        public Dictionary<EnemyEntity, float> aggroTracker = new Dictionary<EnemyEntity, float>();
        private float aggroGainRate = .33f;

        [SerializeField]FiniteTimer randomDirTimer = new FiniteTimer(1.85f);
        Vector3 lastRandomDir = Vector3.zero;
        
        
        //not worth a fsm
        public void handleHypnosis()
        {
            state = TargettingState.Hypnotized;
            targettingInfo = hypnotizedTargettingInfo;
        }

        public void handleBerserk()
        {
            state = TargettingState.Berserk;
            targettingInfo = berserkTargettingInfo;
        }

        public void handleNormalState()
        {
            state = TargettingState.Normal;
            mostAggroedTarget = null;
            targettingInfo = normalTargettingInfo;
        }

        // public void updateAggro()
        // {
        //     if (mostAggroedTarget != null && mostAggroedTarget.IsHypnotized)
        //     {
        //         mostAggroedTarget = null;
        //     }
        //
        //     foreach (var agroTracker in aggroTracker)
        //     {
        //         
        //     }
        // }

        private void Update()
        {
            randomDirTimer.safeUpdateTimer(Time.deltaTime);
        }


        private void Awake()
        {
            player = GameObject.FindWithTag("Player").transform;
            
            
            randomDirTimer.complete();
            handleNormalState();
        }

        /// <summary>
        /// getTargetPos based on current target state.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>value can be random</returns>
        public override Vector3 getTargetPos()
        {
            switch (state)
            {
                case TargettingState.Normal:
                    return player.transform.position;
                case TargettingState.Hypnotized:
                {
                    var e = EnemyTracker.getLowesHpNormalEnemy();
                    if (e != null)
                    {
                        return e.transform.position;
                    }

                    return getRandomDir();
                }
                case TargettingState.Berserk:
                {
                    return getRandomDir();
                }
            }

            return getRandomDir();

        }

        public Vector2 getRandomDir()
        {
            if (randomDirTimer.isComplete)
            {
                randomDirTimer.reset();
                lastRandomDir = Random.onUnitSphere;
            }

            return lastRandomDir;
        }


        //     private void OnTriggerEnter(Collider other)
        //     {
        //         if (state == TargettingState.Hypnotized)
        //         {
        //             var e = other.GetComponent<EnemyEntity>();
        //             if (e != null && !e.IsHypnotized)
        //             {
        //                 aggroTracker[e] = 1;
        //             }
        //         }
        //     }
        //
        //     private void OnTriggerExit(Collider other)
        //     {
        //         if (state == TargettingState.Hypnotized)
        //         {
        //             var e = other.GetComponent<EnemyEntity>();
        //             if (e != null )
        //             {
        //                 aggroTracker.Remove(e);
        //             }
        //         }
        //     }
        // }

        public enum TargettingState
        {
            Normal,
            Hypnotized,
            Berserk,
        }
    }
}