using BDeshi.BTSM;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class GameOverState : MonoBehaviourStateBase
    {
        [SerializeField] private CanvasGroup gameOverImageContainer;
        [SerializeField] float fadeInTime = .33f;
        private Tween tween;
        public Tween  createTween()
        {
            var t = DOTween.Sequence()
                .Append(gameOverImageContainer.DOFade(1,fadeInTime ))
                .SetUpdate(true)
                .SetAutoKill(false);
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
            
            GameStateManager.Instance.IsPaused = true;
            Time.timeScale = 0;
        }

        public override void Tick()
        {
        
        }

        public override void ExitState()
        {
            if(tween.IsPlaying())
                tween.Rewind();
            
            GameStateManager.Instance.IsPaused = true;
            Time.timeScale = 1;
        }
    }
}