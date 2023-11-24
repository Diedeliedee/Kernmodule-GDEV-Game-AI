namespace Joeri.Tools.AI.BehaviorTree
{
    public class BehaviorTree
    {
        private INode m_root = null;

        public BehaviorTree(INode rootNode)
        {
            m_root = rootNode;
        }

        public void Tick()
        {
            m_root?.Evaluate();
        }
    }
}
