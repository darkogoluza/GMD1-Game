using System;
using AI.Core;
using UnityEngine;

namespace AI
{
    public class ShootAtTargetNode : IBehaviorNode
    {
        private readonly Func<Transform> _getTarget;
        private readonly Transform _agentTransform;
        private readonly IWeapon _weapon;
        private readonly LayerMask _obstacleMask;

        private bool _isFiring;

        public ShootAtTargetNode(Func<Transform> getTarget, Transform agentTransform, IWeapon weapon,
            LayerMask obstacleMask)
        {
            _getTarget = getTarget;
            _agentTransform = agentTransform;
            _weapon = weapon;
            _obstacleMask = obstacleMask;
        }

        public NodeStatus Tick()
        {
            var target = _getTarget();
            if (target == null)
            {
                StopFiring();
                return NodeStatus.Failure;
            }

            Vector2 origin = _agentTransform.position;
            Vector2 destination = target.position;

            var hit = Physics2D.Linecast(origin, destination, _obstacleMask);

            if (hit.collider != null)
            {
                StopFiring();
                return NodeStatus.Failure;
            }

            StartFiring();
            return NodeStatus.Success;
        }

        private void StartFiring()
        {
            if (!_isFiring)
            {
                _isFiring = true;
                _weapon.StartFire();
            }
        }

        public void StopFiring()
        {
            if (_isFiring)
            {
                _isFiring = false;
                _weapon.EndFire();
            }
        }
    }
}
