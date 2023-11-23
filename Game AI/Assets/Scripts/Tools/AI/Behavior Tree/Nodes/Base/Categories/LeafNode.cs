namespace Joeri.Tools.AI.BehaviorTree
{
    public abstract class LeafNode : Node
    {
        public virtual void OnEnter() { }

        public virtual void OnUpdate() { }

        public virtual void OnExit() { }
    }
}
