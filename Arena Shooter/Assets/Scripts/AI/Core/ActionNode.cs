using System;

namespace AI.Core
{
    public class ActionNode : BehaviorNode
    {
        private readonly Func<NodeStatus> _action;

        public ActionNode(Func<NodeStatus> action)
        {
            _action = action;
        }

        public override NodeStatus Tick()
        {
            return _action();
        }
    }
}
