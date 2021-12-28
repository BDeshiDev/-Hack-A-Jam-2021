using System.Collections;
using BDeshi.BTSM;
using BDeshi.Utility;
using Core.Combat;
using Core.Combat.Enemies;
using Core.Combat.Powerups;
using Core.Input;
using Core.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Tutorial
{
    public class TutorialRunner:MonoBehaviour
    {
        private HypnoPlayer player;
        private PlayerBombLauncher bomber;
        private Arena arena;

        public BombPowerup powerup;
        private bool powerUpDespawned = false;

        [SerializeField] private EnemyEntity enemyPrefab;
        [SerializeField] private SummoningCircle summoningCirclePrefab;
        private EnemyEntity enemy1;
        private EnemyEntity enemy2;
        
        [SerializeField] private float hypnoTextShowTime = 5f;
        [SerializeField] private TextMeshPro titleDropText1;
        [SerializeField] private TextMeshPro titleDropText2;
        [SerializeField] private TextMeshPro titleDropText3;
        [SerializeField] private TextMeshPro titleDropText4;
        public UnityEvent TitleDropped;
        [SerializeField]  TextMeshPro text;
        private float titleDropWaitTime = 1f;
        private float hypnoFightShowTime = 4;
        private float welcomeTime = .8f;


        private void Start()
        {
            player = GameObject.FindWithTag("Player")
                                .GetComponent<HypnoPlayer>();
            bomber = player.GetComponent<PlayerBombLauncher>();
            arena = FindObjectOfType<Arena>();
            
            StartCoroutine(doTutorial());
        }
        
        public IEnumerator doTutorial()
        {
            yield return StartCoroutine(showControls());
            yield return StartCoroutine(showEnemyMechanics());

            yield return StartCoroutine(doTitleDrop());
            
            GameStateManager.Instance.enterGameplayLevel(GamePlayState.GamePlayScene);
        }
        
        public IEnumerator showEnemyMechanics()
        {
            player.CanDie = false;
            //stuff other than the mechanics tutorial aren't complex enough to require fsm
            ModularState enemy1SpawnState = new ModularState(
                () =>
                {
                    showText("Your attacks can hypnotize enemies.");
                    
                    var summoningCircle = GameplayPoolManager.Instance.summoningCircles.get(summoningCirclePrefab);
                    summoningCircle.startSummon(enemyPrefab,
                        arena.findSafeSpawnSpot(),
                        (e) => { enemy1 = e;  });
                });
            
            //at this state, enemy1 is hypnotized
            FiniteTimer hypnoFightShowTimer = new FiniteTimer(0, this.hypnoFightShowTime);
            ModularState enemy2SpawnState = new ModularState(
                () =>
                {
                    hypnoFightShowTimer.reset();

                    var summoningCircle = GameplayPoolManager.Instance.summoningCircles.get(summoningCirclePrefab);
                    summoningCircle.startSummon(enemyPrefab,
                        arena.findSafeSpawnSpot(), 
                        e =>
                        {
                            enemy2 = e;
                            showText("Hypnotized enemies will temporarily fight for you");
                        });
                },
                () => hypnoFightShowTimer.updateTimer(Time.deltaTime)
            );
            //
            FiniteTimer migraineShowTimer = new FiniteTimer(hypnoTextShowTime);
            ModularState migraineState = new ModularState(
                () =>
                {
                    migraineShowTimer.reset();
                    showText("Hypnotized enemies take damage over time from migraine.");
                },
                () => migraineShowTimer.updateTimer(Time.deltaTime)
                );
            ModularState berserkState = new ModularState(
                () =>
                {
                    showText("Enemies go berserk at low health");
                }
            );
            

            StateMachine fsm = new StateMachine(enemy1SpawnState);
            fsm.addTransition(
                enemy1SpawnState,
                enemy2SpawnState,
                () => enemy1 != null && enemy1.IsHypnotized);
            
            fsm.addTransition(enemy2SpawnState, migraineState, 
                () =>
                        (
                            enemy2 != null &&
                            (enemy1.HealthComponent.IsEmpty || enemy2.HealthComponent.IsEmpty)
                        )||
                        hypnoFightShowTimer.isComplete
                        ) ;
            
            fsm.addTransition(migraineState, berserkState, 
                () => 
                         enemy1.HypnoComponent.IsInBerserkRange ||
                         enemy1.HypnoComponent.IsBerserked ||
                         enemy2.HypnoComponent.IsInBerserkRange ||
                         enemy2.HypnoComponent.IsBerserked
            ) ;
            
            // fsm.addTransition(berserkState,  migraineState, 
            //     () =>
            //         (!enemy1.HypnoComponent.IsInBerserkRange && !enemy2.HypnoComponent.IsInBerserkRange) &&
            //         enemy1.HealthComponent.IsEmpty ||
            //         enemy2.HealthComponent.IsEmpty
            // ) ;

            
            fsm.enter();
            //both not dead
            while (true)
            {
                fsm.Tick();
                yield return null;

                if (enemy1 != null && enemy2 != null &&
                    enemy1.TrulyDead && enemy2.TrulyDead)
                {
                    break;
                }
            }
        }
        

        public IEnumerator showControls()
        {
            bomber.forceRemoveBOmb();
            showText("WASD To Move");
            
            while (!InputManager.IsMoveInputActive)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(1.5f);
            
            showText("LShift to \n Dash");
            
            while (!InputManager.dashButton.isHeld)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(3f);
            
            showText("LMB to \n Shoot .");
            
            
            while (!InputManager.meleeButton.isHeld)
            {
                yield return null;
            }
            yield return new WaitForSeconds(4f);
            
            
            
            showText("Pick up the powerup");
            
            CombatEventManger.Instance.OnPowerUpDeSpawned.add(gameObject, handlePowerUpDespawned);
            
            while(true)
            {
                powerUpDespawned = false;
                powerup.gameObject.SetActive(true);
                
                while (!powerUpDespawned)
                {
                    yield return null;
                }
                powerup.gameObject.SetActive(false);
            
                if (bomber.HasBomb)
                {
            
                    break;
                }
            
                Debug.Log(powerup.gameObject.activeSelf, powerup.gameObject);
            }
            yield return new WaitForSeconds(.5f);
            
            showText("RMB to use the bomb");
            
            while (bomber.HasBomb)
            {
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }

        public IEnumerator doTitleDrop()
        {
            text.gameObject.SetActive(false);
            
            
            yield return showText("Welcome", titleDropText1).WaitForCompletion();
            yield return showText("To", titleDropText2).WaitForCompletion();
            yield return new WaitForSeconds(welcomeTime);


            titleDropText1.text = titleDropText2.text = "";
            TitleDropped.Invoke();
            yield return showText("YOUR BULLETS.", titleDropText3).WaitForCompletion();
            yield return new WaitForSeconds(.6f);

            yield return showText("OUR HELL.", titleDropText4).WaitForCompletion();
            

            
            yield return new WaitForSeconds(titleDropWaitTime);
        }
        
        private void handlePowerUpDespawned(Powerup obj)
        {
            powerUpDespawned = true;
        }

        private Tween showText(string s, TextMeshPro tmp, float showTime = .2f)
        {
            return DOTween.Sequence()
                .Append(
                    tmp.DOFade(0, .2f)
                        .OnComplete(() => tmp.text = s)
                )
                .Append(tmp.DOFade(1, .2f))
                ;
        }
        
        private void showText(string s)
        {
            showText(s, text);
        }
    }
}