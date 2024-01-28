using UnityEngine;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Selector : CompositeNode
    {
        private int m_index = 0;

        public Selector(params Node[] _children) : base(_children) { }

        public override State Evaluate()
        {
            //  Check node states of the children.
            for (m_index = 0; m_index < children.Length; m_index++)
            {
                switch (children[m_index].Evaluate())
                {
                    //  If the current node has failed, move on to evaluate the next.
                    case State.Failure:
                        children[m_index].OnAbort();
                        continue;

                    // If the current child is running, the selector is still running too.
                    case State.Running: return State.Running;

                    //  If the current child has been a succes, the selector has been a succes too.
                    case State.Succes: return State.Succes;
                }
            }

            //  If no children are running, none can be selected and the selector has failed.
            return State.Failure;
        }

        public override void OnAbort()
        {
            children[m_index].OnAbort();
        }

        public override void OnDraw(Vector3 _center)
        {
            children[m_index].OnDraw(_center);
        }
    }
}
