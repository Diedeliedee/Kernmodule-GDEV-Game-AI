using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
