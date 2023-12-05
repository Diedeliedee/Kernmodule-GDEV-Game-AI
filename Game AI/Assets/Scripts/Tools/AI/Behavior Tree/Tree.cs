namespace Joeri.Tools.AI.BehaviorTree
{
    public class BehaviorTree
    {
        private Node m_root = null;

        public BehaviorTree(Node rootNode)
        {
            m_root = rootNode;
        }

        public void Tick()
        {
            m_root?.Evaluate();
        }

        public void PassBlackboard(Patterns.FittedBlackboard _blackboard)
        {
            m_root.PassBlackboard(_blackboard);
        }
    }
}
