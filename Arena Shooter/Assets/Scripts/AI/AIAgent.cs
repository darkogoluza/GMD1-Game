using System.Linq;
using AI.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        [Header("Follow Parameters")] [SerializeField]
        private float stopDistance = 3f;

        [SerializeField] private float retreatDistance = 1.5f;

        [Header("Search Parameters")] [SerializeField]
        private LayerMask targetLayer;

        [SerializeField] private TargetSelectionMode targetSelectionMode;
        [SerializeField] private float searchRadius = 5f;

        [Header("Shoot Parameters")] [SerializeField]
        private LayerMask[] shootTargetLayers;

        [SerializeField] private LayerMask obsticleLayerMask;

        private Transform _target;
        private NavMeshAgent _agent;
        private IBehaviorNode _root;
        [CanBeNull] private IWeapon _weapon;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void SetUpWeapon(IWeapon weapon)
        {
            _weapon = weapon;
            _weapon?.SetTargetMasks(shootTargetLayers);
            _weapon?.SetIsBotFlag();
        }

        private void Start()
        {
            _root = CreateBehaviorTree();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

            AIManager.Instance?.Register(this);

            transform.rotation = Quaternion.identity;
        }

        public void SetNewTargetLayerMasks(LayerMask[] newTargetLayerMasks)
        {
            shootTargetLayers = newTargetLayerMasks;

            targetLayer = 0;

            foreach (var mask in newTargetLayerMasks)
            {
                targetLayer |= mask;
            }
        }

        public void TickBehaviorTree()
        {
            _root?.Tick();
        }

        public void SetDestination(Vector3 targetPosition)
        {
            _agent.isStopped = false;
            _agent.SetDestination(targetPosition);
        }

        public void StopMoving()
        {
            _agent.isStopped = true;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private IBehaviorNode CreateBehaviorTree()
        {
            var searchNode = new SearchForTargetNode(this, searchRadius, targetLayer, targetSelectionMode);
            var follow = new FollowTargetNode(this, () => _target, stopDistance, retreatDistance);
            var lookAtNode = new LookAtTargetNode(this, () => _target);
            var shootNode = new ShootAtTargetNode(
                () => _target,
                transform,
                _weapon,
                obsticleLayerMask
            );
            var reloadNode = new ReloadNode(_weapon);
            var checkAmmoNode = new CheckAmmoNode(() => _weapon.CheckAmmo());

            var wanderNode = new WanderNode(_agent, GetWanderPoints(GameObject.FindWithTag("WanderPoints").transform));

            var hasTargetNode = new HasTargetNode(() => _target);

            var combatLogic = new SequenceNode(
                onFailure: () => shootNode.StopFiring(),
                lookAtNode,
                follow,
                new ParallelNode(
                    succeedOnAny: true,
                    failOnAny: false,
                    shootNode,
                    new SequenceNode(
                        checkAmmoNode,
                        reloadNode
                    )
                )
            );

            var root = new SelectorNode(
                new SequenceNode(
                    searchNode,
                    new SequenceNode(hasTargetNode, combatLogic)
                ),
                wanderNode
            );
            
            return root;
        }

        private void OnDestroy()
        {
            AIManager.Instance?.Unregister(this);
        }

        private Vector3[] GetWanderPoints(Transform wanderPointsParent)
        {
            return wanderPointsParent.Cast<Transform>()
                .Select(t => t.position)
                .ToArray();
        }
    }
}
