using System.Collections.Generic;
using System.Linq;

namespace AI.Core
{
    public class ParallelNode : IBehaviorNode
    {
        private readonly List<IBehaviorNode> _children;
        private readonly bool _succeedOnAny;
        private readonly bool _failOnAny;

        public ParallelNode(bool succeedOnAny, bool failOnAny, params IBehaviorNode[] children)
        {
            _succeedOnAny = succeedOnAny;
            _failOnAny = failOnAny;
            _children = children.ToList();
        }

        public NodeStatus Tick()
        {
            bool anySuccess = false;
            bool anyFailure = false;

            foreach (var child in _children)
            {
                var result = child.Tick();

                if (result == NodeStatus.Success) anySuccess = true;
                else if (result == NodeStatus.Failure) anyFailure = true;
            }

            if (_failOnAny && anyFailure) return NodeStatus.Failure;
            if (_succeedOnAny && anySuccess) return NodeStatus.Success;

            return NodeStatus.Failure;
        } 
    }
}
