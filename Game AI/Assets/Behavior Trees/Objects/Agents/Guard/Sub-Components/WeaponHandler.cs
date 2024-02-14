namespace GameAI.BehaviorSystem
{
    public class WeaponHandler
    {
        //  Could be located a separate combat class for the guard. For the sake of convenience, it's in the weapon memory.
        public Weapon weapon = null;
        public bool hasWeapon = false;

        public void RegisterWeapon(Weapon _weapon)
        {
            weapon = _weapon;
            hasWeapon = true;
        }
    }
}