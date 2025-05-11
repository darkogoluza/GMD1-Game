using AI.Core;
using UnityEngine;

namespace AI
{
    public class LookAtTargetNode : IBehaviorNode
    {
        private readonly AIAgent _agent;
        private readonly System.Func<Transform> _getTarget;
        private const float Offset = 90;

        public LookAtTargetNode(AIAgent agent, System.Func<Transform> getTarget)
        {
            _agent = agent;
            _getTarget = getTarget;
        }

        public NodeStatus Tick()
        {
            var target = _getTarget?.Invoke();
            if (target == null)
                return NodeStatus.Failure;

            Vector2 direction = (target.position - _agent.transform.position).normalized;

            if (direction.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _agent.transform.rotation = Quaternion.Euler(0, 0, angle + Offset);
                return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        }
    } 
}
