using System;

namespace AI.Core
{
    public class ConditionNode : BehaviorNode
    {
        private readonly Func<bool> _condition;

        public ConditionNode(Func<bool> condition)
        {
            _condition = condition;
        }

        public override NodeStatus Tick()
        {
            return _condition() ? NodeStatus.Success : NodeStatus.Failure;
        } 
    }
}
