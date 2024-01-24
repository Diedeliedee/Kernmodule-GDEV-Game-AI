using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joeri.Tools.AI.BehaviorTree;

public class RegisterWeaponInInventory : LeafNode
{
    public override State OnUpdate()
    {
        var weaponMemory = board.Get<WeaponMemory>();

        //  Fail the state if the closest weapon pickup is somehow false.
        if (weaponMemory.closestWeaponPickup == null) return State.Failure;

        //  Pick up the weapon if we can grab the weapon.
        weaponMemory.weapon = weaponMemory.closestWeaponPickup.Pickup();
        weaponMemory.hasWeapon = true;
        return State.Succes;
    }
}
