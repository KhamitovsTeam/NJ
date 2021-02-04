using System.Collections.Generic;

namespace Chip
{
    public enum WeaponType
    {
        Pistol = 0
    }

    public class Weapons
    {
        public static Dictionary<WeaponType, Weapon> List = new Dictionary<WeaponType, Weapon>();

        private static Weapon Add(Weapon weapon)
        {
            if (!List.ContainsKey(weapon.Type))
                List[weapon.Type] = new Weapon();
            List[weapon.Type] = weapon;
            return weapon;
        }
    }
}