namespace Joeri.Tools.AI.BehaviorTree
{
    public abstract class Node : INode
    {
        public Node parent { get; private set; }
        protected Patterns.FittedBlackboard board { get; private set; }

        public abstract State Evaluate();

        /// <summary>
        /// Called if the branch the node is a part of is aborted.
        /// </summary>
        public abstract void OnAbort();

        /// <summary>
        /// Sets the passed in node as the this node's parent.
        /// </summary>
        public void AttachParent(Node _parent)
        {
            parent = _parent;
        }

        /// <summary>
        /// Attaches the passed in blackboard to the Node. And depending on overrides, can pass it down to the Node's children too.
        /// </summary>
        public virtual void PassBlackboard(Patterns.FittedBlackboard _blackboard)
        {
            board = _blackboard;
        }
    }
}
