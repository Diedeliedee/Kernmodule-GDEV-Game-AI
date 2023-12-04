namespace Joeri.Tools.AI.BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        public Node[] children { get; private set; }

        public CompositeNode(params Node[] _children)
        {
            children = _children;           //  Attach children to the node.
            foreach (var node in children)  //  Attach this node as the childrens' parent.
            {
                node.AttachParent(this);
            }
        }

        public override void OnAbort()
        {
            for (int i = 0; i < children.Length; i++) children[i].OnAbort();
        }

        public override void PassBlackboard(Patterns.Blackboard _blackboard)
        {
            base.PassBlackboard(_blackboard);
            for (int i = 0; i < children.Length; i++)
            {
                children[i].PassBlackboard(_blackboard);
            }
        }
    }
}
