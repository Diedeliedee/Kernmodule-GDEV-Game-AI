using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence(params Node[] children) : base(children) { }

        public override State Evaluate()
        {
            var anyChildIsRunning = false;

            //  Check node states of the children.
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    //  If any node has failed, the sequence is broken.
                    case State.Failure:
                        return RetrieveState(State.Failure);

                    // If any child is running, the sequence is still running too.
                    case State.Running:
                        anyChildIsRunning = true;
                        continue;
                }
            }

            //  If no children are running, and none has failed, the sequence is a succes.
            return RetrieveState(anyChildIsRunning ? State.Running : State.Succes);
        }
    }
}
