using System.Collections.Generic;
using System.Linq;

namespace AI.Core
{
    public class SelectorNode : BehaviorNode
    {
        private readonly List<IBehaviorNode> _children;

        public SelectorNode(params IBehaviorNode[] children)
        {
            _children = children.ToList();
        }

        public override NodeStatus Tick()
        {
            foreach (var child in _children)
            {
                var result = child.Tick();
                if (result == NodeStatus.Success)
                    return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        }
    }
}
