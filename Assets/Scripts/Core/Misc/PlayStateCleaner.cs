using bdeshi.levelloading;
using Core.Combat;
using Core.Combat.Enemies;
using Core.Input;
using UnityEditor;
using UnityEngine;

namespace Core.Misc
{
    /// <summary>
    /// Unity playmode with no domain reload/scene reload can cause problems with static stuff
    /// But it's too nice to give up.
    /// Call cleanup funcs here for now.
    /// TODO: Make an attribute to fetch and call automatically
    /// </summary>
    [InitializeOnLoad]
    public static class PlayStateCleaner
    {
        static PlayStateCleaner()
        {
            EditorApplication.playModeStateChanged += ModeChanged;
        }

        static void ModeChanged(PlayModeStateChange playModeState)
        {
            if (playModeState == PlayModeStateChange.ExitingPlayMode)
            {
                ManagerLoadEnsurer.loadedManager = false;
                InputManager.PlayModeExitCleanUp();

                TipDispenser.cleanup();
            }
            else if (playModeState == PlayModeStateChange.ExitingEditMode)
            {
                EnemyTracker.PlayModeEnterCleanup();
                GameplayPoolManager.PlayModeEnterCleanup();
                // GameStateManager.PlayModeEnterCleanup();
                
                CombatEventManger.PlayModeEnterCleanup();
            }
        }
    }
}