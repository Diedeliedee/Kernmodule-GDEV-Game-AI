using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joeri.Tools.AI.BehaviorTree;

public class DamagePlayer : LeafNode
{
    public override State OnUpdate()
    {
        return State.Succes;
    }
}
