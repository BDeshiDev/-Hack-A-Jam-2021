using UnityEngine;

namespace Core.Misc.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptables/StaticEventHelper", fileName = "StaticEventHelper")]
    public class StaticEventHelper : ScriptableObject
    {
        public void setGameoverStateInitial()
        {
            GameStateManager.setInitialState(GameStateManager.Instance.gameoverState);
        }

        
        public void setTutorialLevelInitial()
        {
            GameStateManager.Instance.gamePlayState.setInitiallevelSceneName(GamePlayState.TutorialScene);
            GameStateManager.setInitialState(GameStateManager.Instance.gamePlayState);
        }
        
        public void restartLevel()
        {
            GameStateManager.Instance.handleEvent(GameStateManager.Event.PlayGame);
        }

        
        public void setNormalLevelInitial()
        {
            GameStateManager.Instance.gamePlayState.setInitiallevelSceneName(GamePlayState.GamePlayScene);
            GameStateManager.setInitialState(GameStateManager.Instance.gamePlayState);
        }


    }
}
