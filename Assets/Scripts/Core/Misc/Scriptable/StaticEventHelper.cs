using UnityEngine;

namespace Core.Misc.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptables/StaticEventHelper", fileName = "StaticEventHelper")]
    public class StaticEventHelper : ScriptableObject
    {
        public void PauseGameEvent()
        {
            GameStateManager.Instance.handleEvent(GameStateManager.Event.PauseToggle);
        }

        public void GoToTitleEvent()
        {
            GameStateManager.Instance.handleEvent(GameStateManager.Event.GoToTitle);
        }
        
        public void ViewOptionsEvent()
        {
            GameStateManager.Instance.handleEvent(GameStateManager.Event.ViewOptions);
        }
        
          
        public void PlayGameEvent()
        {
            GameStateManager.Instance.handleEvent(GameStateManager.Event.PlayGame);
        }
        
        public void setPauseStateInitial()
        {
            GameStateManager.setInitialState(GameStateManager.pauseMenuState);
        }

        public void setTitleStateInitial()
        {
            GameStateManager.setInitialState(GameStateManager.titleMenuState);
        }
        
        public void setViewOptionsStateInitial()
        {
            GameStateManager.setInitialState(GameStateManager.optionMenuState);
        }
        
        
        public void setGamplayStateInitial()
        {
            GameStateManager.setInitialState(GameStateManager.gamePlayState);
        }



    }
}
