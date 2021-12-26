using BDeshi.BTSM;
using Core.Combat;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core
{
    public class GameOverState : MonoBehaviourStateBase
    {
        [SerializeField] private CanvasGroup gameOverImageContainer;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [FormerlySerializedAs("fadeInTime")] [SerializeField] float fadeTime = .33f;
        private Tween tween;
        [SerializeField] private ScoreViewer scoreViewer;
        [SerializeField] private CanvasGroup rectTransformButton;
        public Tween  createTween()
        {

            var t = DOTween.Sequence()
                .Append(gameOverImageContainer.DOFade(1, fadeTime).SetEase(Ease.OutCubic))
                .Append(gameOverText.transform.DOScale(Vector3.one, fadeTime))
                .Append(gameOverText.transform.DOPunchScale(1.5f * Vector3.one, fadeTime))
                .SetUpdate(true)
                .SetAutoKill(false)
                .SetRecyclable(true);
          scoreViewer
                .appendTo(t)
                .Append(
                    DOTween.Sequence()
                        .Append(rectTransformButton.DOFade(1, fadeTime).SetEase(Ease.OutCubic))
                        .Insert(0, rectTransformButton.transform.DOScale(Vector3.one, fadeTime))
                    )
                .OnComplete(() => {
                    gameOverImageContainer.interactable = true;
                    //for some reason, timescale = 0 doesn't work with dotween even if SetUpdate(true) is used.
                    //so set it after
                });
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
            gameOverImageContainer.interactable = false;
            gameOverText.transform.localScale = Vector3.zero;
            
            gameOverImageContainer.alpha = 0;
            rectTransformButton.alpha = 0;
            rectTransformButton.transform.localScale = Vector3.zero;

            startTWeen();
            
            GameStateManager.Instance.pause();
            Time.timeScale = 0;
            
            #if UNITY_EDITOR
                tween.Complete();
            #endif
        }

        public override void Tick()
        {
        
        }

        public override void ExitState()
        {
            if(tween.IsPlaying())
            {
                tween.Complete(false);
            }
            gameOverImageContainer.interactable = false;
            gameOverImageContainer.DOFade(0,fadeTime);
            
            GameStateManager.Instance.unPause();
            Time.timeScale = 1;
        }
    }
}