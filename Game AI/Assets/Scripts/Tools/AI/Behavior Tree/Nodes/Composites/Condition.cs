using UnityEngine;
using UnityEngine.Rendering;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Condition : CompositeNode
    {
        private int m_index = 0;

        private readonly System.Func<bool> m_condition = null;

        public Condition(System.Func<bool> _condition, Node _if, Node _else) : base(_if, _else)
        {
            m_condition = _condition;
        }

        public override State Evaluate()
        {
            if (m_condition())  m_index = 0;
            else                m_index = 1;
            return children[m_index].Evaluate();
        }

        public override void OnDraw(Vector3 _center)
        {
            children[m_index].OnDraw(_center);
        }
    }
}
