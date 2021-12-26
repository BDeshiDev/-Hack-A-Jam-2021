using System;
using System.Collections;
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
        public TextMeshPro text;

        public BombPowerup powerup;
        private bool powerUpDespawned = false;
        private void Awake()
        {
            player = GameObject.FindWithTag("Player")
                                .GetComponent<HypnoPlayer>();
            bomber = player.GetComponent<PlayerBombLauncher>();

            StartCoroutine(doTutorial());
        }

  


        public IEnumerator doTutorial()
        {
            bomber.forceRemoveBOmb();
            showTutorialText("WASD To Move");
            
            while (!InputManager.IsMoveInputActive)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(1.5f);
            
            showTutorialText("LShift to \n Dash");
            
            while (!InputManager.dashButton.isHeld)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(3f);
            
            showTutorialText("LMB to \n Shoot hypnotic \"Fireballs\".");
            
            
            while (!InputManager.meleeButton.isHeld)
            {
                yield return null;
            }
            yield return new WaitForSeconds(3f);
            
            showTutorialText("They reload over time.");
            
            yield return new WaitForSeconds(2f);
            
            showTutorialText("Pick up the powerup");
            
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
            
            showTutorialText("RMB to use the bomb");
            
            while (bomber.HasBomb)
            {
                yield return null;
            }
            yield return new WaitForSeconds(2f);

            showTutorialText("Good luck.");
            
            yield return new WaitForSeconds(1f);
            
            GameStateManager.Instance.enterGameplayLevel(GamePlayState.GamePlayScene);
        }

        private void handlePowerUpDespawned(Powerup obj)
        {
            powerUpDespawned = true;
        }

        private void showTutorialText(string s)
        {
            text.DOFade(0, .2f).WaitForCompletion();
            text.text = s;
            text.DOFade(1, .2f).WaitForCompletion();
        }
    }
}