using System.Collections;
using System.Collections.Generic;
using BDeshi.BTSM;
using bdeshi.utility;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

public class GameStateManager : MonoBehaviourLazySingleton<GameStateManager>
{
    [SerializeField] FSMRunner runner;
    public bool IsPaused= false;
    public EventDrivenStateMachine<Event> fsm { get; private set; }
    public static State initialState;

    //need to reference these
    public GamePlayState gamePlayState;
    public GameOverState gameoverState;
    // public GameState inGameOptionMenuState;
    // public GameState optionMenuState;

    
    protected override void initialize()
    {
        base.initialize();
        
        gameoverState.AsChildOf(gamePlayState);

        if (initialState == null)
            initialState = gamePlayState;
        
        fsm = new EventDrivenStateMachine<GameStateManager.Event>(initialState);
        
        fsm.addEventTransition(gamePlayState, Event.EndGame, gameoverState);
        fsm.addEventHandler(gameoverState , Event.PlayGame, restartGamepLayLevel);
        
        fsm.addEventHandler(gamePlayState, Event.TutorialComplete,() => enterGameplayLevel(gamePlayState.levelSceneName));
        
        
        runner = gameObject.AddComponent<FSMRunner>();
        runner.Initialize(fsm, false);
    }
    
    public static bool setInitialState(State s)
    {
        
        if(Instance.fsm == null || Instance.fsm.curState != null)
            return false;
        
        initialState = s;
        Instance.initialize();
        
        return true;
    }
    


    public void handleEvent(Event e)
    {
        if(Instance != null)
            Instance.fsm.handleEvent(e);
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
    
    public void restartGamepLayLevel()
    {
        fsm.transitionTo(gamePlayState, true, true);
    }
    
    public enum Event
    {
        PlayGame,
        EndGame,
        TutorialComplete,
    }

  
}