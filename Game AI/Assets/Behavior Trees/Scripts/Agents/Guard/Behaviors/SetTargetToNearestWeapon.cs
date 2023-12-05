using UnityEngine;
using Joeri.Tools.AI.BehaviorTree;

/// <summary>
/// Finds the nearest weapon, and sets that as the agent's target.
/// This could be converted to a class with a generic variable as well, as long as that variable is a MonoBehaviour.
/// </summary>
public class SetTargetToNearestWeapon : LeafNode
{
    public override State OnUpdate()
    {
        //  Temporary omnipotent knowledge of where the weapon is.
        var weapon = Object.FindObjectOfType<WeaponPickup>();

        if (weapon != null)
        {
            board.Get<WeaponMemory>().instance = weapon;
            board.Get<TargetMemory>().SetTarget(weapon.transform.position);
            return State.Succes;
        }
        return State.Failure;
    }
}
