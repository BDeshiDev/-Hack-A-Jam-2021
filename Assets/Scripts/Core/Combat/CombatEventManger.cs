using bdeshi.utility;
using BDeshi.Utility;

namespace Core.Combat
{
    public class CombatEventManger: MonoBehaviourLazySingleton<CombatEventManger>
    {
        public SafeEvent OnSuccessFullDodge = new SafeEvent();
        public SafeEvent<EnemyEntity> OnEnemyDefeated = new SafeEvent<EnemyEntity>();
        public SafeEvent<EnemyEntity> OnEnemyHypnotized = new SafeEvent<EnemyEntity>();
        public SafeEvent<Spawner> OnWaveCompleted = new SafeEvent<Spawner> ();

        protected override void PlayModeEnterCleanupInternal()
        {
            base.PlayModeEnterCleanupInternal();
            OnSuccessFullDodge.clear();
            OnEnemyDefeated.clear();
            OnEnemyHypnotized.clear();
            OnWaveCompleted.clear();    
        }
    }
}