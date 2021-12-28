using System.Collections;
using BDeshi.Utility;
using Core.Combat;
using Core.Combat.Powerups;
using Core.Input;
using Core.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.Misc
{
    public class Tutorial:MonoBehaviour
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
        [SerializeField] private TextMeshPro titleDropTextLeft;
        [SerializeField] private TextMeshPro titleDropTextRight;
        [SerializeField]  TextMeshPro text;
        private float titleDropWaitTime = 1f;

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
            //do enemy portion
            var summoningCircle = GameplayPoolManager.Instance.summoningCircles.get(summoningCirclePrefab);
            yield return StartCoroutine(
                    summoningCircle.summon(enemyPrefab,
                                        arena.findSafeSpawnSpot(),
                                        (e) => enemy1 = e)
                    );
            showText("Your attacks can hypnotize enemies.");

            while (!enemy1.IsHypnotized)
            {
                yield return null;
            }
            
            summoningCircle = GameplayPoolManager.Instance.summoningCircles.get(summoningCirclePrefab);
            yield return StartCoroutine(
                summoningCircle.summon(enemyPrefab,
                    arena.findSafeSpawnSpot(),
                    (e) => enemy2 = e)
            );
            showText("Hypnotized enemies will temporarily fight for you.");
            FiniteTimer hypnoFightTextShowTimer = new FiniteTimer(0, hypnoTextShowTime);
            //both not dead
            while (!enemy1.HealthComponent.IsEmpty &&
                   !enemy2.HealthComponent.IsEmpty)
            {
                
                // we've show the hypnoFightText long enough or 
                // any one of them dead
                // and not berserk, because that takes priority
                if (!hypnoFightTextShowTimer.isComplete ||
                    (enemy1.HealthComponent.IsEmpty ||
                     enemy2.HealthComponent.IsEmpty) && 
                    !(
                        enemy1.HypnoComponent.IsBerserked || 
                        enemy2.HypnoComponent.IsBerserked
                      )
                    )
                {
                    showText("Hypnotized enemies take damage over time from migraine.");
                    hypnoFightTextShowTimer.safeUpdateTimer(Time.deltaTime);
                }
                else
                {
                    //any one going to turn into or is berserk
                    if ((enemy1.HypnoComponent.IsInBerserkRange ||
                         enemy1.HypnoComponent.IsBerserked) ||
                        (enemy2.HypnoComponent.IsInBerserkRange ||
                         enemy2.HypnoComponent.IsBerserked))
                    {
                        showText("Enemies go berserk at low health");
                    }
                }
                yield return null;
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
            
            showText("LMB to \n Shoot hypnotic \"Fireballs\".");
            
            
            while (!InputManager.meleeButton.isHeld)
            {
                yield return null;
            }
            yield return new WaitForSeconds(3f);
            
            showText("They reload over time.");
            
            yield return new WaitForSeconds(2f);
            
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
            yield return showText("Welcome", titleDropTextLeft);
            yield return showText("To", titleDropTextRight);

            titleDropTextLeft.text = titleDropTextRight.text = "";
            
            yield return showText("YOUR BULLETS.", titleDropTextLeft);
            yield return showText("OUR HELL.", titleDropTextRight);
            
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