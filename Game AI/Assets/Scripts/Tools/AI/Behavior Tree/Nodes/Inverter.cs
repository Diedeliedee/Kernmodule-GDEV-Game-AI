using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Inverter : Node
    {
        public Inverter(Node child) : base(child) { }

        public override State Evaluate()
        {
            //  Evaluate the only child it has (sad), and invert it's result.
            switch (children[0].Evaluate())
            {
                case State.Failure: return State.Succes;
                case State.Running: return State.Running;
                case State.Succes:  return State.Failure;
            }

            //  Return failure if the switch case somehow fails.
            return State.Failure;
        }
    }
}
