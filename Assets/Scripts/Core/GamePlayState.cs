using System;
using BDeshi.BTSM;
using UnityEngine.SceneManagement;

public class GamePlayState : MonoBehaviourStateBase
{
    //referecning scenes by string is sufficient for the jam
    public const String GamePlayScene = "GamePlayScene" ;
    public const String TutorialScene =  "TutorialScene" ;
    
    public String levelSceneName = "";

    public void setInitiallevelSceneName(string name)
    {
        if (string.IsNullOrEmpty(levelSceneName))
            levelSceneName = name;
    }

    public override void EnterState()
    {
        SceneManager.LoadScene(levelSceneName);
    }

    public override void Tick()
    {
        
    }

    public override void ExitState()
    {
        
    }
}