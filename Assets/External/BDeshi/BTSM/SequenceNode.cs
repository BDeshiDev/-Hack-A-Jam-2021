using System.Collections.Generic;
using UnityEngine;

namespace BDeshi.BTSM
{
    /// <summary>
    /// Continue running one by one until one fail, then it itself fails.
    /// </summary>
    public class SequenceNode : BTMultiDecorator
    {
        [SerializeField] List<BtNodeBase> children;
        [SerializeField] private int curIndex;
        public override IEnumerable<BtNodeBase> GetChildren => children;
        public override void addChild(BtNodeBase child)
        {
            children.Add(child);
        }

        public SequenceNode(List<BtNodeBase> children)
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
                return BTStatus.Success;
            }
            else
            {
                var childResult = children[curIndex].Tick();
                if (childResult == BTStatus.Fail)
                    return BTStatus.Fail;
                else if (childResult == BTStatus.Success)
                {

                    children[curIndex].Exit();

                    curIndex++;
                    if (curIndex <= (children.Count - 1))
                    {
                        children[curIndex].Enter();
                    }
                }
            }

            return BTStatus.Running;
        }

        public override void Exit()
        {
            if (curIndex < children.Count  && curIndex >= 0)
            {
                children[curIndex].Exit();
            }
        }

        public override string Typename => "Sequence";
    }
}