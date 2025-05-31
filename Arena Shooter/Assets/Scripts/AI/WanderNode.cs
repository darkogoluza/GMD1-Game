using AI.Core;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class WanderNode : IBehaviorNode
    {
        private readonly NavMeshAgent _agent;
        private readonly Vector3[] _wanderPoints;
        private Vector3 _currentTarget;
        private float _threshold = 2f;

        private float _waitTime = 6f;
        private float _waitTimer = 0f;
        private bool _waiting = true;

        public WanderNode(NavMeshAgent agent, Vector3[] wanderPoints)
        {
            _agent = agent;
            _wanderPoints = wanderPoints;
        }

        public NodeStatus Tick()
        {
            if (_wanderPoints.Length == 0)
                return NodeStatus.Failure;

            if (ReachedTarget())
            {
                if (!_waiting)
                {
                    _waiting = true;
                    _waitTimer = _waitTime;
                }

                _waitTimer -= 1f / AIManager.Instance.tickRate;

                if (_waitTimer <= 0f)
                {
                    _currentTarget = _wanderPoints[Random.Range(0, _wanderPoints.Length)];
                    _agent.SetDestination(_currentTarget);
                    _waiting = false;
                }

                return NodeStatus.Success;
            }
            
            LookTowardsMovementDirection();

            return NodeStatus.Success;
        }

        private bool ReachedTarget()
        {
            return !_agent.pathPending && _agent.remainingDistance <= _threshold;
        }
        
        private void LookTowardsMovementDirection()
        {
            Vector3 velocity = _agent.velocity;

            if (velocity.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
                _agent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            } 
        }
    }
}