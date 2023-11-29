namespace Joeri.Tools.AI.BehaviorTree
{
    public class Selector : CompositeNode
    {
        private int m_lastIndex = 0;

        public Selector(params Node[] _children) : base(_children) { }

        public override State Evaluate()
        {
            //  Check node states of the children.
            for (int i = 0; i < children.Length; i++)
            {
                switch (children[i].Evaluate())
                {
                    //  If the current node has failed, move on to evaluate the next.
                    case State.Failure: continue;

                    // If the current child is running, the selector is still running too.
                    case State.Running: return State.Running;

                    //  If the current child has been a succes, the selector has been a succes too.
                    case State.Succes:
                        if (i < m_lastIndex) children[m_lastIndex].OnAbort();
                        m_lastIndex = i;
                        return State.Succes;
                }
            }

            //  If no children are running, none can be selected, and the selector has failed.
            return State.Failure;
        }

        public override void OnAbort()
        {
            children[m_lastIndex].OnAbort();
            m_lastIndex = 0;
        }
    }
}
