using System;
using System.Collections.Generic;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShieldSan.Input
{
    /// <summary>
    /// You may also assume that this can be safely stored in fields
    /// as this is not serialized
    /// </summary>
    public class InputButtonSlot
    {
        private List<SafeAction> onPerformedCallbacks = new List<SafeAction>();
        private List<SafeAction> OnCancelledCallbacks = new List<SafeAction>();
            
        public bool isHeld { get; private set; }

        public void addPerformedCallback(GameObject go, Action a)
        {
            onPerformedCallbacks.Add(new SafeAction(go ,a ));
        }
            
        public void addCancelledCallback(GameObject go, Action a)
        {
            OnCancelledCallbacks.Add(new SafeAction(go ,a ));
        }

        public void bind(InputActionReference iar)
        {
            if(iar == null)
                return;
            iar.action.performed += ActionOnperformed;
            iar.action.canceled += ActionOncanceled;
        }

        public void unBind(InputActionReference iar)
        {               
            if(iar == null)
                return;
            iar.action.performed -= ActionOnperformed;
            iar.action.canceled -= ActionOncanceled;
        }

        private void ActionOncanceled(InputAction.CallbackContext obj)
        {
            isHeld = false;
            for (int i = OnCancelledCallbacks.Count -1 ; i >=0; i--)
            {
                if (OnCancelledCallbacks[i].go == null)
                {
                    OnCancelledCallbacks.removeAndSwapToLast(i);
                }
                else
                {
                    OnCancelledCallbacks[i].action.Invoke();
                }
            }
        }

        private void ActionOnperformed(InputAction.CallbackContext obj)
        {
            isHeld = true;
            for (int i = onPerformedCallbacks.Count -1 ; i >=0; i--)
            {
                if (onPerformedCallbacks[i].go == null)
                {
                    onPerformedCallbacks.removeAndSwapToLast(i);
                }
                else
                {
                    onPerformedCallbacks[i].action.Invoke();
                }
            }
        }
        
        public void cleanup()
        {
            isHeld = false;
            onPerformedCallbacks.Clear();
            OnCancelledCallbacks.Clear();
        }
    }
}