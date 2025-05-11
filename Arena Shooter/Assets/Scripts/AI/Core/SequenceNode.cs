using System.Collections.Generic;
using System.Linq;

namespace AI.Core
{
    public class SequenceNode : IBehaviorNode
    {
        private readonly IBehaviorNode[] _children;
        private readonly System.Action _onFailure;

        public SequenceNode(params IBehaviorNode[] children)
        {
            _children = children;
            _onFailure = null;
        }

        public SequenceNode(System.Action onFailure, params IBehaviorNode[] children)
        {
            _children = children;
            _onFailure = onFailure;
        }

        public NodeStatus Tick()
        {
            foreach (var child in _children)
            {
                var result = child.Tick();

                if (result == NodeStatus.Failure)
                {
                    _onFailure?.Invoke();
                    return NodeStatus.Failure;
                }
            }

            return NodeStatus.Success;
        }
    } 
}
