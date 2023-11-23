﻿namespace Joeri.Tools.AI.BehaviorTree
{
    public abstract class DecoratorNode : Node
    {
        public Node child { get; private set; }

        public DecoratorNode(Node _child)
        {
            child = _child;             //  Attach child to the node.
            _child.AttachParent(this);  //  Attach this node as the children's parent.
        }
    }
}