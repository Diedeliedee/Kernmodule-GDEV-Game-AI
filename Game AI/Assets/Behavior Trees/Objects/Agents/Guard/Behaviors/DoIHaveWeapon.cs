using UnityEngine;
using Joeri.Tools.AI.BehaviorTree;

public class DoIHaveWeapon : LeafNode
{
    public override State OnUpdate()
    {
        if (board.Get<WeaponMemory>().hasWeapon) return State.Succes;
        return State.Failure;
    }
}
