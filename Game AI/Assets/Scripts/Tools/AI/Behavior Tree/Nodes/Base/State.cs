using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    /// <summary>
    /// The state of a node.
    /// </summary>
    public enum State
    {
        Failure = 0,
        Running = 1,
        Succes  = 2,
    }
}
