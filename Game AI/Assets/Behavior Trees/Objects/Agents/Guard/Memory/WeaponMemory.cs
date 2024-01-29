using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class WeaponMemory
{
    //  Memory about weapon's in the environment.
    public WeaponPickup closestWeaponPickup = null;

    //  Could be located a separate combat class for the guard. For the sake of convenience, it's in the weapon memory.
    public Weapon weapon = null;
    public bool hasWeapon = false;

    public void RegisterWeapon()
    {
        weapon = closestWeaponPickup.Pickup();
        hasWeapon = true;
    }
}
