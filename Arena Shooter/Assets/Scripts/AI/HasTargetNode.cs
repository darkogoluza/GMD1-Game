using System;
using AI.Core;
using UnityEngine;

namespace AI
{
    public class HasTargetNode : IBehaviorNode
    {
        private readonly Func<Transform> _getTarget;

        public HasTargetNode(Func<Transform> getTarget)
        {
            _getTarget = getTarget;
        }

        public NodeStatus Tick()
        {
            var target = _getTarget();
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        } 
    }
}
