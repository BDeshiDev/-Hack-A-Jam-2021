using BDeshi.Utility;
using Core.Combat.CombatResources;
using Core.Combat.Entities.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Combat.Targetting
{
    public class EnemyTargetResolver : TargetResolverComponent
    {
        [SerializeField] private TargettingInfo normalTargettingInfo;
        [SerializeField] private TargettingInfo hypnotizedTargettingInfo;
        [SerializeField] private TargettingInfo berserkTargettingInfo;


        public Transform player;
        public Transform lastTarget = null;
        private float aggroGainRate = .33f;

        [SerializeField]FiniteTimer randomDirTimer = new FiniteTimer(1.85f);
        Vector3 lastTargetPoint = Vector3.zero;
        private HypnoComponent hypnoComponent;
        private EnemyEntity self;


        //not worth a fsm
        public void handleHypnosis()
        {
            targettingInfo = hypnotizedTargettingInfo;
        }

        public void handleBerserk()
        {
            targettingInfo = berserkTargettingInfo;
        }

        public void handleNormalState()
        {
            targettingInfo = normalTargettingInfo;
        }
        

        public void initialize()
        {
            randomDirTimer.complete();
            handleNormalState();
            
            player = GameObject.FindWithTag("Player").transform;
            // aggroTracker.Add(player);
        }

        private void Update()
        {
            randomDirTimer.safeUpdateTimer(Time.deltaTime);
        }


        private void Awake()
        {

            self = GetComponent<EnemyEntity>();
        }

        private void Start()
        {
            hypnoComponent = self.HypnoComponent;
            
            initialize();
        }

        /// <summary>
        /// getTargetPos based on current target state.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>value can be random</returns>
        public override Vector3 getTargetPos()
        {
            switch (hypnoComponent.CurState)
            {
                case HypnosisState.Normal:
                    if(player == null)
                        return setTargetToRandomPoint();
                    return setTarget(player.transform);
                case HypnosisState.Hypnotized:
                {
                    var e = EnemyTracker.getLowesHpNormalEnemy();
                    if (e != null)
                    {
                        return setTarget(e.transform);
                    }

                    return setTargetToRandomPoint();
                }
                case HypnosisState.Berserk:
                {
                    if (Random.Range(0,2) != 0)
                    {
                        if (randomDirTimer.isComplete)
                        {
                            randomDirTimer.reset();
                            var e = EnemyTracker.getRandomEnemy(self);
                            if (e != null)
                            {
                                return setTarget(e.transform);
                            }

                        }else if (lastTarget != null)
                        {
                            return lastTarget.position;
                        }

                    }
                    else
                    {
                        if(player != null)
                            return setTarget(player.transform);
                    }


                    return setTargetToRandomPoint();
                }
            }

            return setTargetToRandomPoint();
        }

        private Vector3 setTarget(Transform t)
        {
            lastTarget = t;
            lastTargetPoint = t.position;
            return lastTargetPoint;
        }
        
        private Vector3 setTargetToRandomPoint()
        {
            
            if (randomDirTimer.isComplete)
            {
                lastTarget = null;
                randomDirTimer.reset();
                lastTargetPoint = transform.position + Random.onUnitSphere * 1000;
            }
            return lastTargetPoint;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = lastTarget == null ? Color.red : Color.green;

            if(lastTarget != null)
                Gizmos.DrawWireSphere( lastTarget.position , 1.5f);
            Gizmos.DrawLine(transform.position, lastTargetPoint);
        }
        
    }
}