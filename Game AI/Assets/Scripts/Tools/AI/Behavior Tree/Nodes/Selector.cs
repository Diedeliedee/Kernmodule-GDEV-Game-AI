using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Selector : Node
    {
        public Selector(params Node[] children) : base(children) { }

        public override State Evaluate()
        {
            //  Check node states of the children.
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    //  If the current node has failed, move on to evaluate the next.
                    case State.Failure:
                        continue;

                    // If the current child is running, the selector is still running too.
                    case State.Running:
                        return RetrieveState(State.Running);

                    //  If the current child has been a succes, the selector has been a succes too.
                    case State.Succes:
                        return RetrieveState(State.Succes);
                }
            }

            //  If no children are running, none can be selected, and the selector has failed.
            return RetrieveState(State.Failure);
        }
    }
}
