using BDeshi.BTSM;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class GameOverState : MonoBehaviourStateBase
    {
        [SerializeField] private CanvasGroup gameOverImageContainer;
        [FormerlySerializedAs("fadeInTime")] [SerializeField] float fadeTime = .33f;
        private Tween tween;
        public Tween  createTween()
        {
            var t = DOTween.Sequence()
                .Append(gameOverImageContainer.DOFade(1,fadeTime ))
                .SetUpdate(true)
                .SetAutoKill(false)
                .SetRecyclable(true);
            t.onComplete += ()=>gameOverImageContainer.interactable = true;

            return t;
        }

        void startTWeen()
        {
            if (tween == null)
            {
                tween = createTween();
            }
            else
            {
                tween.Restart();
            }
        }

        public override void EnterState()
        {
            startTWeen();
            
            GameStateManager.Instance.pause();
            Time.timeScale = 0;
        }

        public override void Tick()
        {
        
        }

        public override void ExitState()
        {
            if(tween.IsPlaying())
            {
                tween.Complete();
            }
            gameOverImageContainer.interactable = false;
            gameOverImageContainer.DOFade(0,fadeTime);
            
            GameStateManager.Instance.unPause();
            Time.timeScale = 1;
        }
    }
}