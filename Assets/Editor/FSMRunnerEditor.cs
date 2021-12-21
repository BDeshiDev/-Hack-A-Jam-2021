using System;
using System.Collections.Generic;
using BDeshi.BTSM;
using UnityEngine;


namespace Editor
{
    using UnityEditor;
    [CustomEditor(typeof(FSMRunner))]
    public class FSMRunnerEditor: Editor
    {
        private List<bool> toggleStatus = new List<bool>();
        private const int maxRecursion = 128;
        private void OnEnable()
        {
            toggleStatus.Clear();
        }

        void drawTransition(Transition transition)
        {
            GUI.color = transition.TakenLastTime ? Color.green : Color.red;
            EditorGUILayout.LabelField("-->"+(transition.SuccessState == null? "?": transition.SuccessState.FullStateName));
        }

        void drawBT(State s)
        {
            BTWrapperState root = s as BTWrapperState;
            if (root != null)
            {
                drawNode(root.BTRoot);
            }
        }

        void drawNode(BtNodeBase n, int depth = 0)
        {
            if(n == null)
                return;
            depth++;
            if(depth >= maxRecursion)
                return;
            EditorGUILayout.LabelField(n.Name);
            GUI.color = n.LastStatus == BTStatus.Success
                ? Color.green
                : n.LastStatus == BTStatus.Running
                    ? Color.yellow
                    : Color.red; 
                
            var decorator = n as BTDecorator;
            if (decorator != null)
            {
                EditorGUI.indentLevel++;
                foreach (var child in decorator.GetChildren)
                {   
                    
                    drawNode(child, depth);
                }
                EditorGUI.indentLevel--;

            }
        }

        public override void OnInspectorGUI()
        {
            var runner = target as FSMRunner;
            var fsm = runner.fsm;
            if(fsm == null)
                return;
            EditorGUILayout.LabelField("CUR: " + (fsm.curState == null ? "None" : fsm.curState.FullStateName));

            var oldCol = GUI.color;
            drawBT(fsm.curState);
            foreach (var transition in fsm.activeTransitions)
            {
                drawTransition(transition);
            }

            int i = 0;
            foreach (var p in fsm.transitions)
            {
                bool wasToggled = false;
                if (i < toggleStatus.Count)
                {
                    wasToggled = toggleStatus[i];
                }
                else
                {
                    toggleStatus.Add(false);
                }

                toggleStatus[i] = 
                EditorGUILayout.BeginFoldoutHeaderGroup(wasToggled, p.Key.FullStateName);

                if (toggleStatus[i])
                {
                    drawBT(p.Key);
                    foreach (var transition in p.Value)
                    {
                        drawTransition(transition);
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

    }
}