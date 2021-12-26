using bdeshi.utility;
using BDeshi.Utility;
using Core.Combat.Powerups;

namespace Core.Combat
{
    public class CombatEventManger: MonoBehaviourLazySingleton<CombatEventManger>
    {
        public SafeEvent OnSuccessFullDodge = new SafeEvent();
        public SafeEvent<EnemyEntity> OnEnemyDefeated = new SafeEvent<EnemyEntity>();
        public SafeEvent<EnemyEntity> OnEnemyHypnotized = new SafeEvent<EnemyEntity>();
        public SafeEvent<Spawner> OnWaveCompleted = new SafeEvent<Spawner> ();
        public SafeEvent<Powerup> OnPowerUpSpawned = new SafeEvent<Powerup>();
        public SafeEvent<Powerup> OnPowerUpDeSpawned = new SafeEvent<Powerup>();

        protected override void PlayModeEnterCleanupInternal()
        {
            base.PlayModeEnterCleanupInternal();
            OnSuccessFullDodge.clear();
            OnEnemyDefeated.clear();
            OnEnemyHypnotized.clear();
            OnWaveCompleted.clear();  
            OnPowerUpSpawned.clear();
            OnPowerUpDeSpawned.clear();
        }
    }
}