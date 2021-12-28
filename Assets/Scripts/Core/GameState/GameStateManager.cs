using System;
using System.Collections;
using BDeshi.BTSM;
using BDeshi.Utility;
using UnityEngine;

namespace Core
{
    public class GameStateManager : MonoBehaviourSingletonPersistent<GameStateManager>
    {
        [SerializeField] FSMRunner runner;
        public bool IsPaused =>isPaused;
        [SerializeField] bool isPaused = false;
        public EventDrivenStateMachine<Event> fsm { get; private set; }

        public static State initialState;

        //need to reference these
        public GamePlayState gamePlayState;
        public GameOverState gameoverState;
        // public GameState inGameOptionMenuState;
        // public GameState optionMenuState;

        public event Action Paused;
        public event Action UnPaused;
        public event Action GameplaySceneRefresh;
    

        protected override void initialize()
        {


            if (initialState == null)
                initialState = gamePlayState;
        
            fsm = new EventDrivenStateMachine<GameStateManager.Event>(initialState ?? gamePlayState);
        
            fsm.addEventTransition(gamePlayState, Event.Gameover, gameoverState);
            fsm.addEventHandler(gameoverState , Event.PlayGame, restartGameplayLevel);
            fsm.addEventHandler(gamePlayState, Event.TutorialComplete,() => enterGameplayLevel(gamePlayState.levelSceneName));
        
            runner = gameObject.AddComponent<FSMRunner>();
            runner.fsm = fsm;
            // runner.Initialize(fsm, false);
        }
    
        private void Start()
        {
            if (!willGetDestroyed)
            {
                if (initialState != null)
                    runner.fsm.startingState = initialState;
                runner.Initialize(fsm, false);
            }
        }

        public void pause()
        {
            if (!IsPaused)
            {
                isPaused = true;
                Paused?.Invoke();
            }
        }
    
        public void unPause()
        {
            if (IsPaused)
            {
                isPaused = false;
                UnPaused?.Invoke();
            }
        }

        public static bool setInitialState(State s)
        {

            if(Instance.fsm.curState != null)
                return false;
        
            initialState = s;
        
            Debug.Log($"GameState initial set: {s}, currently {Instance.fsm.curState}" );

        
            return true;
        }

        public void InvokeGameplaySceneChanged()
        {
            StartCoroutine(doInvokeGameplaySceneChanged());
        }
        //unity requires 1 frame delay before checking loaded scene
        IEnumerator doInvokeGameplaySceneChanged()
        {
            yield return null;
            GameplaySceneRefresh?.Invoke();
        }

        public void handleEvent(Event e)
        {
            if(Instance != null)
            {
                Debug.Log($"{fsm.curState} handle {e} ");
                Instance.fsm.handleEvent(e);
                Debug.Log($"{fsm.curState} now ");
            }
        }

        //only two levels needed for the jam so this is sufficient
        public void enterGameplayLevel(string levelName)
        {
            if (levelName == gamePlayState.levelSceneName)
            {
                return;
            }

            gamePlayState.levelSceneName = levelName;

            fsm.transitionTo(gamePlayState, true, true);
        }
    
        public void restartGameplayLevel()
        {
            Debug.Log(gamePlayState, gamePlayState);
            fsm.transitionTo(gamePlayState, true, true);
        }
    
        public enum Event
        {
            PlayGame,
            Gameover,
            TutorialComplete,
        }

  
    }
}