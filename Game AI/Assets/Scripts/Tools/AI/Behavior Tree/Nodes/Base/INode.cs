using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    public interface INode
    {
        /// <returns>The state that the evaluation resulted in.</returns>
        public State Evaluate();
    }
}
