using System;
using System.Collections.Generic;
using BDeshi.BTSM;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayState : MonoBehaviourStateBase
{
    //referecning scenes by string is sufficient for the jam
    public const String GamePlayScene = "GamePlayScene" ;
    public const String TutorialScene =  "TutorialScene" ;
    
    public String levelSceneName = "";

    public CanvasGroup gameplayHudGroup;

    public void setInitiallevelSceneName(string levelName)
    {
        if (string.IsNullOrEmpty(levelSceneName))
            levelSceneName = levelName;
    }

    public override void EnterState()
    {
        Debug.Log(levelSceneName + " loaded level ");

        gameplayHudGroup.alpha = 1;
        
        SceneManager.LoadScene(levelSceneName);
        GameStateManager.Instance.InvokeGameplaySceneChanged();
    }

    public override void Tick()
    {

    }

    public override void ExitState()
    {
        gameplayHudGroup.alpha = 0;

    }
}