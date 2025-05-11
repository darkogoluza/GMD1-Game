using System.Collections.Generic;
using AI.Core;
using UnityEngine;

public enum TargetSelectionMode
{
    Closest,
    Farthest,
    LowestHp,
    HighestHp
}

namespace AI
{
    public class SearchForTargetNode : IBehaviorNode
    {
        private readonly AIAgent _agent;
        private readonly float _searchRadius;
        private readonly Collider2D[] _resultsBuffer;
        private readonly int _targetLayerMask;
        private readonly TargetSelectionMode _selectionMode;

        public SearchForTargetNode(AIAgent agent, float searchRadius, LayerMask targetLayerMask, TargetSelectionMode selectionMode, int maxTargets = 10)
        {
            _agent = agent;
            _searchRadius = searchRadius;
            _targetLayerMask = targetLayerMask;
            _resultsBuffer = new Collider2D[maxTargets];
            _selectionMode = selectionMode;
        }

        public NodeStatus Tick()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(
                _agent.transform.position,
                _searchRadius,
                _resultsBuffer,
                _targetLayerMask
            );

            if (hitCount == 0)
            {
                return NodeStatus.Failure;
            }

            List<Transform> potentialTargets = new List<Transform>();
        
            for (int i = 0; i < hitCount; i++)
            {
                var hit = _resultsBuffer[i];
                potentialTargets.Add(hit.transform);
            }

            if (potentialTargets.Count == 0)
            {
                return NodeStatus.Failure;
            }

            Transform selectedTarget = null;

            switch (_selectionMode)
            {
                case TargetSelectionMode.Closest:
                    selectedTarget = GetClosestTarget(potentialTargets);
                    break;

                case TargetSelectionMode.Farthest:
                    selectedTarget = GetFarthestTarget(potentialTargets);
                    break;

                case TargetSelectionMode.LowestHp:
                    selectedTarget = GetTargetWithLowestHP(potentialTargets);
                    break;

                case TargetSelectionMode.HighestHp:
                    selectedTarget = GetTargetWithHighestHP(potentialTargets);
                    break;
            }

            if (selectedTarget != null)
            {
                _agent.SetTarget(selectedTarget);
                return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        } 

        private Transform GetClosestTarget(List<Transform> targets)
        {
            Transform closestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var target in targets)
            {
                float distance = Vector2.Distance(_agent.transform.position, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        private Transform GetFarthestTarget(List<Transform> targets)
        {
            Transform farthestTarget = null;
            float farthestDistance = 0f;

            foreach (var target in targets)
            {
                float distance = Vector2.Distance(_agent.transform.position, target.position);
                if (distance > farthestDistance)
                {
                    farthestDistance = distance;
                    farthestTarget = target;
                }
            }

            return farthestTarget;
        }

        private Transform GetTargetWithLowestHP(List<Transform> targets)
        {
            Transform targetWithLowestHP = null;
            float lowestHP = float.MaxValue;

            foreach (var target in targets)
            {
                var health = target.GetComponent<IDamageable>();
                if (health != null && health.GetHealth() < lowestHP)
                {
                    lowestHP = health.GetHealth();
                    targetWithLowestHP = target;
                }
            }

            return targetWithLowestHP;
        }

        private Transform GetTargetWithHighestHP(List<Transform> targets)
        {
            Transform targetWithHighestHP = null;
            float highestHP = 0f;

            foreach (var target in targets)
            {
                var health = target.GetComponent<IDamageable>();
                if (health != null && health.GetHealth() > highestHP)
                {
                    highestHP = health.GetHealth();
                    targetWithHighestHP = target;
                }
            }

            return targetWithHighestHP;
        }
    }
}
