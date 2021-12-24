using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.BTSM;
using bdeshi.utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviourLazySingleton<GameStateManager>
{
    [SerializeField] FSMRunner runner;
    public static bool isPaused { get; private set; } = false;
    public EventDrivenStateMachine<Event> fsm { get; private set; }
    public static State initialState;

    public static StateBase titleMenuState { get; } = new TitleMenuState();
    public static StateBase optionMenuState { get; } = new GameState("Options Menu");
    public static StateBase gamePlayState { get; } = new GamePlayState();
    public static StateBase pauseMenuState { get; } = new GamePausedState("PauseMenu");
    public static StateBase inGameOptionMenuState  { get; }= new GameState("Options Menu");
    
    
    protected override void initialize()
    {
        base.initialize();
        
        optionMenuState.AsChildOf(titleMenuState);
        pauseMenuState.AsChildOf(gamePlayState);
        inGameOptionMenuState.AsChildOf(pauseMenuState);

        if (initialState == null)
            initialState = gamePlayState;
        
        fsm = new EventDrivenStateMachine<GameStateManager.Event>(initialState);
        fsm.addEventTransition(titleMenuState, Event.ViewOptions, optionMenuState);
        
        fsm.addEventTransition(gamePlayState, Event.PauseToggle, pauseMenuState);
        
        fsm.addEventTransition(pauseMenuState, Event.PauseToggle, gamePlayState);
        fsm.addEventTransition(pauseMenuState, Event.ViewOptions, inGameOptionMenuState);
        fsm.addEventTransition(optionMenuState, Event.ViewOptions, pauseMenuState);
        
        fsm.addGlobalEventTransition(Event.PlayGame, gamePlayState);
        fsm.addGlobalEventTransition(Event.GoToTitle, titleMenuState);
        
        runner = gameObject.AddComponent<FSMRunner>();
        runner.Initialize(fsm, false);
    }
    
    public static void setInitialState(State s)
    {
        initialState = s;
        
        if(Instance.fsm.curState != null)
            return;
        
        Instance.initialize();
    }



    public void handleEvent(Event e)
    {
        if(Instance != null)
            Instance.fsm.handleEvent(e);
    }
    
    
    
    public enum Event
    {
        PauseToggle,
        PlayGame,
        ViewOptions,
        GoToTitle,
    }

    public class GameState : StateBase
    {
        public GameState()
        {
        }

        public GameState(string prefix)
        {
            this.Prefix = prefix;
        }

        public override void EnterState()
        {
            Debug.Log($"Enter {FullStateName}", GameStateManager.Instance);
        }

        public override void Tick()
        {
            
        }

        public override void ExitState()
        {
            
        }
    }
    
    public class GamePausedState: GameState
    {
        public GamePausedState(String prefix) : base(prefix)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("paused");
            isPaused = true;
        }

        public override void ExitState()
        {
            base.ExitState();
            isPaused = false;
        }
    }
    
}

public class GamePlayState : StateBase
{
    public override void EnterState()
    {
        SceneManager.LoadScene("GamePlayScene");
        Debug.Log("VAR");
    }

    public override void Tick()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

public class TitleMenuState : GameStateManager.GameState
{
    public override void EnterState()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public override void Tick()
    {
        
    }

    public override void ExitState()
    {
        
    }
}


