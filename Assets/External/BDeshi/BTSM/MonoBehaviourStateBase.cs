﻿using System;
using UnityEngine;

namespace BDeshi.BTSM
{
    /// <summary>
    /// C# default interface issues so this is a copy of statebase with monobehaviour inheritance
    /// </summary>
    public abstract class MonoBehaviourStateBase: MonoBehaviour,State
    {
        public abstract void EnterState();
        public abstract void Tick();
        public abstract void ExitState();
        public string Prefix { get; set; }
        public string FullStateName => Prefix +"_"+ GetParentChainName();
        public State Parent { get; set; }
               
        public string Name => GetType().Name;

        public State AsChildOf(State p)
        {
            Parent = p;
            return this;
        }

        public String GetParentChainName()
        {
            string chain = Name;
            var p = Parent;
            while (p != null)
            {
                chain = p.Name + "." + chain;
                p = p.Parent;
            }
            return chain;
        }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}