namespace Joeri.Tools.AI.BehaviorTree
{
    public class Condition : CompositeNode
    {
        private readonly System.Func<bool> m_condition = null;

        public Condition(System.Func<bool> _condition, Node _if, Node _else) : base(_if, _else)
        {
            m_condition = _condition;
        }

        public override State Evaluate()
        {
            if (m_condition())  return children[0].Evaluate();
                                return children[1].Evaluate();
        }
    }
}
