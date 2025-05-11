using System;
using AI.Core;
using UnityEngine;

namespace AI
{
    public class FollowTargetNode : BehaviorNode
    {
        private readonly AIAgent _agent;
        private readonly Func<Transform> _getTarget;
        private readonly float _stopDistance;
        private readonly float _retreatDistance;

        public FollowTargetNode(AIAgent agent, Func<Transform> getTarget, float stopDistance, float retreatDistance)
        {
            _agent = agent;
            _getTarget = getTarget;
            _stopDistance = stopDistance;
            _retreatDistance = retreatDistance;
        }

        public override NodeStatus Tick()
        {
            if (_getTarget() == null)
                return NodeStatus.Failure;
            
            float distance = Vector3.Distance(_agent.transform.position, _getTarget().position);

            if (distance < _retreatDistance)
            {
                // Move away from player
                Vector3 dir = (_agent.transform.position - _getTarget().position).normalized;
                Vector3 retreatPos = _agent.transform.position + dir * 2f;
                _agent.SetDestination(retreatPos);
                return NodeStatus.Failure;
            }
            else if (distance > _stopDistance)
            {
                // Move toward player
                _agent.SetDestination(_getTarget().position);
                return NodeStatus.Failure;
            }

            // At safe distance
            _agent.StopMoving();
            return NodeStatus.Success;
        } 
    }
}
