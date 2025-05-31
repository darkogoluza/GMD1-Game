using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIManager : MonoBehaviour
    {
        [SerializeField] public int tickRate = 5; // 0.2 times a second

        public static AIManager Instance => _instance;

        private static AIManager _instance;
        private float _tickTimer = 0f;
        private readonly List<AIAgent> _agents = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Register(AIAgent agent)
        {
            if (!_agents.Contains(agent))
                _agents.Add(agent);
        }

        public void Unregister(AIAgent agent)
        {
            _agents.Remove(agent);
        }

        private void Update()
        {
            _tickTimer += Time.deltaTime;

            if (_tickTimer >= (1f / tickRate))
            {
                foreach (var agent in _agents)
                {
                    agent.TickBehaviorTree();
                }

                _tickTimer = 0f;
            }
        }
    }
}
