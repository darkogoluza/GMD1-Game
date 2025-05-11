namespace AI.Core
{
    public abstract class BehaviorNode : IBehaviorNode
    {
        public abstract NodeStatus Tick();
    }
}
