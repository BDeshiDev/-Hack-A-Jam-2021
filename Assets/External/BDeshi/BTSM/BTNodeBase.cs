using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;

namespace BDeshi.BTSM
{
    /// <summary>
    /// Keep on trying nodes until one succeeds, then it itself will stop and succeed
    /// </summary>
    public class Selector : BTMultiDecorator
    {
        [SerializeField] List<BtNodeBase> children;
        [SerializeField] private int curIndex;
        
        public override IEnumerable<BtNodeBase> GetChildren => children;
        public override void addChild(BtNodeBase child)
        {
            children.Add(child);
        }


        public Selector(List<BtNodeBase> children)
        {
            this.children = children;
        }



        public override void Enter()
        {
            curIndex = 0;

            if (curIndex >= children.Count || curIndex < 0)
            {
                return;
            }
            else
            {
                children[curIndex].Enter();
            }
        }


        protected override BTStatus InternalTick()
        {
            if (curIndex >= children.Count || curIndex < 0)
            {
                return BTStatus.Fail;
            }
            else
            {
                var childResult = children[curIndex].Tick();
                if (childResult == BTStatus.Fail)
                {
                    children[curIndex].Exit();

                    curIndex++;
                    if (curIndex <= (children.Count - 1))
                    {
                        children[curIndex].Enter();
                    }
                }

                return BTStatus.Running;
                
            }

            return BTStatus.Running;
        }

        public override void Exit()
        {
            if (curIndex >= children.Count || curIndex < 0)
            {
                children[curIndex].Exit();
            }
        }

        public override string Typename => "Selector";
    }

    public abstract class BTMultiDecorator: BTDecorator
    {
        public abstract void addChild(BtNodeBase child);
    }

    public abstract class BTDecorator : BtNodeBase
    {
        public abstract IEnumerable<BtNodeBase> GetChildren { get; }
    }

    public abstract class BTSingleDecorator : BTDecorator
    {
        
        protected BtNodeBase child;
        public override  IEnumerable<BtNodeBase> GetChildren => getChildWrapper();

        IEnumerable<BtNodeBase> getChildWrapper()
        {
            yield return child;
        }

        protected BTSingleDecorator(BtNodeBase child)
        {
            this.child = child;
        }
    }

    public class FailTillComplete : BTSingleDecorator
    {
        public override string Typename => "FailTIllComplete";

        public override void Enter()
        {
            
        }

        protected override BTStatus InternalTick()
        {
            var result = child.Tick();
            return result == BTStatus.Success ? BTStatus.Fail : BTStatus.Success;
        }

        public override void Exit()
        {
            
        }

        public FailTillComplete(BtNodeBase child) : base(child)
        {
        }
    }

    public class OverrideStatus : BTSingleDecorator
    {
        
        public override void Enter()
        {

        }

        protected override BTStatus InternalTick()
        {
            var result = child.Tick();
            return result == BTStatus.Success ? BTStatus.Fail : BTStatus.Success;
        }

        public override void Exit()
        {

        }

        public override string Typename => "Override";

        public OverrideStatus(BtNodeBase child) : base(child)
        {
        }
    }


    /// <summary>
    /// Run all children every tick
    /// Succeed on first on succeeding, or when all do.
    /// If the later, the children may or may not restart
    /// </summary>
    public class Parallel: BTMultiDecorator
    {
        public override IEnumerable<BtNodeBase> GetChildren => children;
        private List<BtNodeBase> children;
        private List<BtNodeBase> activeList;
        private bool allMustSucceed;
        /// <summary>
        /// Will it restart children that have succeded is allMustSucceed = true?
        /// I swear that is not what it sounds like.
        /// </summary>
        private bool repeatSuccessfullChildren;

        public Parallel(List<BtNodeBase> children, bool allMustSucceed, bool repeatSuccessfullChildren)
        {
            this.children = children;
            this.allMustSucceed = allMustSucceed;
            this.repeatSuccessfullChildren = repeatSuccessfullChildren;
        }

        public override void Enter()
        {
            if(repeatSuccessfullChildren)
                activeList = new List<BtNodeBase>(children);
            else
            {
                activeList = children;
            }
        }

        protected override BTStatus InternalTick()
        {
            bool anySuccess = false;
            bool allSuccess = true;
            for(int i = 0; i < children.Count; i++)
            {
                var btNodeBase = children[i];
                if (btNodeBase.Tick() == BTStatus.Success)
                {
                    anySuccess = true;
                    if (!repeatSuccessfullChildren && allMustSucceed)
                    {
                        activeList.removeAndSwapToLast(i);
                    }
                }
                else
                {
                    allSuccess = false;
                }
            }

            if ((allMustSucceed && allSuccess) || (!allMustSucceed && anySuccess))
                return BTStatus.Success;
            return BTStatus.Running;
        }

        public override string Typename => "Parallel";

        public override void Exit()
        {
            
        }

        public override void addChild(BtNodeBase child)
        {
            children.Add(child);
        }
    }

    /// <summary>
    /// Repeats child regardless of result, itself returning running
    /// After N times, return success
    /// If n < 0, keep ticking,
    /// </summary>
    public class Repeat:BTSingleDecorator
    {
        private int n = 0;
        private int c = 0;
        public override string Typename => "Repeat";

        public Repeat(int n, BtNodeBase child) : base(child)
        {
            this.n = n;
        }

        public Repeat(BtNodeBase child) : base(child)
        {
            this.n = -1;
        }

        public override void Enter()
        {
            c = n;
            child.Enter();
        }

        protected override BTStatus InternalTick()
        {
            if (c == 0)
            {
                return BTStatus.Success;
            }

            var r = child.Tick();
            if (r == BTStatus.Success || r == BTStatus.Fail)
            {
                child.Exit();

                if (c > 0)
                {
                    c--;
                    if (c != 0)
                    {
                        child.Enter();
                    }
                }
                else 
                {
                    child.Enter();
                }

                return BTStatus.Running;
            }

            return BTStatus.Running;
        }

        public override void Exit()
        {
            
        }
    }

    public class EnterExitDecorator: BTSingleDecorator
    {
        public Action OnEnter;
        public Action OnExit;
        public override string Typename => "EnterExitDeco";

        public EnterExitDecorator(BtNodeBase child, Action onEnter, Action onExit = null) : base(child)
        {
            OnEnter = onEnter;
            OnExit = onExit;
        }

        public override void Enter()
        {
            OnEnter?.Invoke();
            child.Enter();
        }

        protected override BTStatus InternalTick()
        {
            return child.Tick();
        }

        public override void Exit()
        {
            OnExit?.Invoke();
            child.Exit();
        }
    }

    public abstract class BtNodeBase
    {
        public abstract void Enter();

        /// <summary>
        /// NOT VIRTUAL OR ABSTRACT. DO NOT OVERRIDE.
        /// </summary>
        /// <returns></returns>
        public BTStatus Tick()
        {
            lastStatus = InternalTick();
            return lastStatus;
        }

        /// <summary>
        /// To allow caching status onto lastStatus
        /// </summary>
        /// <returns></returns>
        protected abstract BTStatus InternalTick();

        public BTStatus LastStatus => lastStatus;
        public abstract void Exit();

        protected BTStatus lastStatus;
        public abstract string Typename { get; }
        public string Prefix;
        public string Name => Prefix +"_"+ Typename;

        public BtNodeBase setPrefix(string prefix)
        {
            Prefix = prefix;
            return this;
        }
    }

    public enum BTStatus{
        Running,Success,Fail
    }
}
