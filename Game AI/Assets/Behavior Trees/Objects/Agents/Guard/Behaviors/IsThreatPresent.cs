using UnityEngine;
using Joeri.Tools.AI.BehaviorTree;

public class IsThreatPresent : LeafNode
{
    public override State OnUpdate()
    {
        if (board.Get<ThreatMemory>().hasSeenThreat) return State.Succes;
        return State.Failure;
    }
}
