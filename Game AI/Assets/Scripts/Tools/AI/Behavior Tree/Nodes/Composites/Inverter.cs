namespace Joeri.Tools.AI.BehaviorTree
{
    public class Inverter : DecoratorNode
    {
        public Inverter(Node _child) : base(_child) { }

        public override State Evaluate()
        {
            //  Evaluate the only child it has (sad), and invert it's result.
            switch (child.Evaluate())
            {
                default:            return State.Succes;
                case State.Running: return State.Running;
                case State.Succes:  return State.Failure;
            }
        }
    }
}
