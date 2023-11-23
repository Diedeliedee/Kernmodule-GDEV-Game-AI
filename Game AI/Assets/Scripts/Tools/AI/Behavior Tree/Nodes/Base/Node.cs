namespace Joeri.Tools.AI.BehaviorTree
{
    public abstract class Node : INode
    {
        public Node parent { get; private set; }

        public abstract State Evaluate();

        /// <summary>
        /// Sets the passed in node as the this node's parent.
        /// </summary>
        public void AttachParent(Node _parent)
        {
            parent = _parent;
        }
    }
}
