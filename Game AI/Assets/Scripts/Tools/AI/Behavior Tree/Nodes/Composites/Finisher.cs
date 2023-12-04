using UnityEditor;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Finisher : CompositeNode
    {
        private int m_index = -1;

        public Finisher(params Node[] _children) : base(_children) { }

        public override State Evaluate()
        {
            if (m_index >= 0)
            {
                switch (children[m_index].Evaluate())
                {
                    case State.Failure:

                }
            }

            for (int i = 0; i < children.Length; i++)
            {
                switch (children[i].Evaluate())
                {
                    case State.Failure: continue;
                    case State.Running: return State.Running;
                    case State.Succes: return State.Succes;
                }
            }
            return State.Failure;
        }

        public override void OnAbort()
        {
            children[m_index].OnAbort();
            m_index = -1;
        }
    }
}
